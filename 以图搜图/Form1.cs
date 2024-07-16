﻿using Masuit.Tools.Logging;
using Masuit.Tools.Media;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Masuit.Tools.Files.FileDetector;
using Masuit.Tools.Systems;
using Image = SixLabors.ImageSharp.Image;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using System.Runtime.InteropServices;
using System;

namespace 以图搜图;

public partial class Form1 : Form
{
  public Form1()
  {
    InitializeComponent();
    CheckForIllegalCrossThreadCalls = false;
  }

  private async void Form1_Load(object sender, EventArgs e)
  {
    lbIndexCount.Text = "正在加载索引...";
    if (File.Exists("index.json"))
    {
      _index = await JsonSerializer.DeserializeAsync<ConcurrentDictionary<string, ulong[]>>(File.OpenRead("index.json")).ConfigureAwait(false);
    }
    if (File.Exists("frame_index.json"))
    {
      _frameIndex = await JsonSerializer.DeserializeAsync<ConcurrentDictionary<string, List<ulong[]>>>(File.OpenRead("frame_index.json")).ConfigureAwait(false);
    }

    if (_index.Count + _frameIndex.Count > 0)
    {
      lbIndexCount.Text = _index.Count + _frameIndex.Count + "文件";
    }
    else
    {
      lbIndexCount.Text = "请先创建索引";
    }
  }

  private void btnDirectory_Click(object sender, EventArgs e)
  {
    using var dialog = new FolderBrowserDialog();
    var result = dialog.ShowDialog();
    if (result == DialogResult.OK)
    {
      txtDirectory.Text = dialog.SelectedPath;
    }
  }

  private void btnPic_Click(object sender, EventArgs e)
  {
    using var dialog = new OpenFileDialog();
    if (dialog.ShowDialog() == DialogResult.OK)
    {
      txtPic.Text = dialog.FileName;
    }
  }

  private ConcurrentDictionary<string, ulong[]> _index = new();
  private ConcurrentDictionary<string, List<ulong[]>> _frameIndex = new();
  private static readonly ReaderWriterLockSlim ReaderWriterLock = new();
  private bool _removingInvalidIndex;
  public bool IndexRunning { get; set; }

  private async void btnIndex_Click(object sender, EventArgs e)
  {
    if (IndexRunning)
    {
      IndexRunning = false;
      btnIndex.Text = "更新索引";
      return;
    }

    if (string.IsNullOrEmpty(txtDirectory.Text))
    {
      MessageBox.Show("请先选择文件夹");
      return;
    }

    if (cbRemoveInvalidIndex.Checked)
    {
      _ = Task.Run(() =>
      {
        _removingInvalidIndex = true;
        foreach (var key in _index.Keys.Except(_index.Keys.GroupBy(x => string.Join('\\', x.Split('\\')[..2])).SelectMany(g => Directory.EnumerateFiles(FindLCP(g.ToArray()), "*", SearchOption.AllDirectories).Where(s => Regex.IsMatch(s, "(jpg|jpeg|png|bmp)$", RegexOptions.IgnoreCase)))).AsParallel().WithDegreeOfParallelism(32).Where(s => !File.Exists(s)))
        {
          _index.TryRemove(key, out _);
        }

        foreach (var key in _frameIndex.Keys.Except(_frameIndex.Keys.GroupBy(x => string.Join('\\', x.Split('\\')[..2])).SelectMany(g => Directory.EnumerateFiles(FindLCP(g.ToArray()), "*.gif", SearchOption.AllDirectories))).AsParallel().WithDegreeOfParallelism(32).Where(s => !File.Exists(s)))
        {
          _frameIndex.TryRemove(key, out _);
        }

        ReaderWriterLock.EnterWriteLock();
        File.WriteAllText("index.json", JsonSerializer.Serialize(_index), Encoding.UTF8);
        File.WriteAllText("frame_index.json", JsonSerializer.Serialize(_frameIndex), Encoding.UTF8);
        ReaderWriterLock.ExitWriteLock();
        _removingInvalidIndex = false;
        lbIndexCount.Text = _index.Count + _frameIndex.Count + "文件";
      }).ConfigureAwait(false);
    }

    IndexRunning = true;
    btnIndex.Text = "停止索引";
    cbRemoveInvalidIndex.Hide();
    var imageHasher = new ImageHasher(new ImageSharpTransformer());
    int? filesCount = null;
    _ = Task.Run(() => filesCount = Directory.EnumerateFiles(txtDirectory.Text, "*", SearchOption.AllDirectories).Except(_index.Keys).Count(s => Regex.IsMatch(s, "(gif|jpg|jpeg|png|bmp)$", RegexOptions.IgnoreCase))).ConfigureAwait(false);
    var local = new ThreadLocal<int>(true);
    await Task.Run(() =>
    {
      var sw = Stopwatch.StartNew();
      long size = 0;
      Directory.EnumerateFiles(txtDirectory.Text, "*", SearchOption.AllDirectories).Except(_index.Keys).Where(s => Regex.IsMatch(s, "(jpg|jpeg|png|bmp)$", RegexOptions.IgnoreCase)).Chunk(Environment.ProcessorCount * 2).AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount * 2).ForAll(g =>
          {
            foreach (var s in g)
            {
              if (IndexRunning)
              {
                if (lblProcess.InvokeRequired)
                {
                  local.Value++;
                  lblProcess.Invoke(() => lblProcess.Text = $"{local.Values.Sum()}/{filesCount}");
                }
                try
                {
                  _index.GetOrAdd(s, _ => imageHasher.DifferenceHash256(s));
                  size += new FileInfo(s).Length;
                }
                catch
                {
                  LogManager.Info(s + "格式不正确");
                }
              }
              else
              {
                break;
              }
            }
          });
      Directory.EnumerateFiles(txtDirectory.Text, "*.gif", SearchOption.AllDirectories).Except(_frameIndex.Keys).Chunk(Environment.ProcessorCount * 2).AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount * 2).ForAll(g =>
          {
            foreach (var s in g)
            {
              if (IndexRunning)
              {
                if (lblProcess.InvokeRequired)
                {
                  local.Value++;
                  lblProcess.Invoke(() => lblProcess.Text = $"{local.Values.Sum()}/{filesCount}");
                }
                try
                {
                  using var gif = Image.Load<Rgba32>(s);
                  for (var i = 0; i < gif.Frames.Count; i++)
                  {
                    using var frame = gif.Frames.ExportFrame(i);
                    var hash = imageHasher.DifferenceHash256(frame);
                    _frameIndex.GetOrAdd(s, _ => []).Add(hash);
                  }

                  size += new FileInfo(s).Length;
                }
                catch
                {
                  LogManager.Info(s + "格式不正确");
                }
              }
              else
              {
                break;
              }
            }
          });
      lbSpeed.Text = $"索引速度: {Math.Round(local.Values.Sum() * 1.0 / sw.Elapsed.TotalSeconds)} items/s({size * 1f / 1048576 / sw.Elapsed.TotalSeconds:N}MB/s)";
      lbIndexCount.Text = _index.Count + _frameIndex.Count + "文件";
      cbRemoveInvalidIndex.Show();
      ReaderWriterLock.EnterWriteLock();
      File.WriteAllText("index.json", JsonSerializer.Serialize(_index), Encoding.UTF8);
      File.WriteAllText("frame_index.json", JsonSerializer.Serialize(_frameIndex), Encoding.UTF8);
      ReaderWriterLock.ExitWriteLock();
      MessageBox.Show("索引创建完成，耗时：" + sw.Elapsed.TotalSeconds + "s");
    }).ConfigureAwait(false);
    IndexRunning = false;
    btnIndex.Text = "更新索引";
  }

  private static string FindLCP(string[] strs)
  {
    if (strs == null || strs.Length == 0) return "";

    // 找到最短的字符串，因为LCP不会超过最短字符串的长度
    string shortest = strs[0];
    for (int i = 1; i < strs.Length; i++)
    {
      if (strs[i].Length < shortest.Length)
        shortest = strs[i];
    }

    // 逐字符比较，直到找到不匹配的字符
    for (int i = 0; i < shortest.Length; i++)
    {
      char c = shortest[i];
      for (int j = 1; j < strs.Length; j++)
      {
        // 如果当前索引越界或字符不匹配，则返回当前LCP
        if (i >= strs[j].Length || strs[j][i] != c)
          return shortest.Substring(0, i);
      }
    }

    // 如果所有字符串共享整个最短字符串，则返回它
    return shortest;
  }

  private void btnSearch_Click(object sender, EventArgs e)
  {
    if (string.IsNullOrEmpty(txtPic.Text))
    {
      MessageBox.Show("请先选择图片");
      return;
    }

    if (_index.Count == 0)
    {
      MessageBox.Show("当前没有任何索引，请先添加文件夹创建索引后再搜索");
      return;
    }

    if (!new FileInfo(txtPic.Text).DetectFiletype().MimeType.StartsWith("image"))
    {
      MessageBox.Show("不是图像文件，无法检索");
      return;
    }

    var sim = (float)numLike.Value / 100;
    var hasher = new ImageHasher();
    var sw = Stopwatch.StartNew();
    var hashs = new ConcurrentBag<ulong[]>();
    var actions = new List<Action>();

    if (txtPic.Text.EndsWith("gif"))
    {
      using (var gif = Image.Load<Rgba32>(txtPic.Text))
      {
        for (var i = 0; i < gif.Frames.Count; i++)
        {
          var frame = gif.Frames.ExportFrame(i);
          actions.Add(() =>
          {
            hashs.Add(frame.DifferenceHash256());
            frame.Dispose();
          });
        }
        Parallel.Invoke(actions.ToArray());
      }
    }
    else
    {
      hashs.Add(hasher.DifferenceHash256(txtPic.Text));
      using (var image = Image.Load<Rgba32>(txtPic.Text))
      {
        if (cbRotate.Checked)
        {
          actions.Add(() =>
          {
            using var clone = image.Clone(c => c.Rotate(90));
            var rotate90 = clone.DifferenceHash256();
            hashs.Add(rotate90);
          });
          actions.Add(() =>
          {
            using var clone = image.Clone(c => c.Rotate(180));
            var rotate180 = clone.DifferenceHash256();
            hashs.Add(rotate180);
          });
          actions.Add(() =>
          {
            using var clone = image.Clone(c => c.Rotate(270));
            var rotate270 = clone.DifferenceHash256();
            hashs.Add(rotate270);
          });
        }

        if (cbFlip.Checked)
        {
          actions.Add(() =>
          {
            using var clone = image.Clone(c => c.Flip(FlipMode.Horizontal));
            var flipH = clone.DifferenceHash256();
            hashs.Add(flipH);
          });
          actions.Add(() =>
          {
            using var clone = image.Clone(c => c.Flip(FlipMode.Vertical));
            var flipV = clone.DifferenceHash256();
            hashs.Add(flipV);
          });
        }
        Parallel.Invoke(actions.ToArray());
      }
    }

    var list = new List<SearchResult>();
    if (txtPic.Text.EndsWith("gif"))
    {
      list.AddRange(_frameIndex.AsParallel().WithDegreeOfParallelism(32).Select(x => new SearchResult
      {
        路径 = x.Key,
        匹配度 = x.Value.SelectMany(h => hashs.Select(hh => ImageHasher.Compare(h, hh))).Where(f => f >= sim).OrderDescending().Take(10).DefaultIfEmpty().Average()
      }).Where(x => x.匹配度 >= sim));
    }
    else
    {
      list.AddRange(_frameIndex.AsParallel().WithDegreeOfParallelism(32).Select(x => new SearchResult
      {
        路径 = x.Key,
        匹配度 = x.Value.SelectMany(h => hashs.Select(hh => ImageHasher.Compare(h, hh))).Max()
      }).Where(x => x.匹配度 >= sim));
      list.AddRange(_index.AsParallel().WithDegreeOfParallelism(32).Select(x => new SearchResult
      {
        路径 = x.Key,
        匹配度 = hashs.Select(h => ImageHasher.Compare(x.Value, h)).Max()
      }).Where(x => x.匹配度 >= sim));
    }

    list = list.OrderByDescending(a => a.匹配度).ToList();
    lbElpased.Text = sw.ElapsedMilliseconds + "ms";
    if (list.Count > 0)
    {
      picSource.ImageLocation = txtPic.Text;
      picSource.Refresh();
    }

    dgvResult.DataSource = list;
    dgvResult.Focus();
  }

  private void dgvResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
  {
  }

  private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
  {
    picDest.ImageLocation = dgvResult.SelectedCells[0].OwningRow.Cells["路径"].Value.ToString();
  }

  private void picSource_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
  {
    lbSrcInfo.Text = $"来源图片： 分辨率：{picSource.Image.Width}x{picSource.Image.Height}，大小：{new FileInfo(picSource.ImageLocation).Length / 1024}KB";
  }

  private void picDest_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
  {
    lblDestInfo.Text = $"选择图片： 分辨率：{picDest.Image.Width}x{picDest.Image.Height}，大小：{new FileInfo(picDest.ImageLocation).Length / 1024}KB";
  }

  private void lblGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
  {
    Process.Start(new ProcessStartInfo { FileName = lblGithub.Text, UseShellExecute = true });
  }

  private void txtDirectory_DragEnter(object sender, DragEventArgs e)
  {
    e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Link : DragDropEffects.None;
  }

  private void txtDirectory_DragDrop(object sender, DragEventArgs e)
  {
    ((TextBox)sender).Text = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
  }

  private void txtPic_DragEnter(object sender, DragEventArgs e)
  {
    e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Link : DragDropEffects.None;
  }

  private void txtPic_DragDrop(object sender, DragEventArgs e)
  {
    ((TextBox)sender).Text = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
  }

  private void buttonClipSearch_Click(object sender, EventArgs e)
  {
    if (Clipboard.ContainsFileDropList())
    {
      txtPic.Text = Clipboard.GetFileDropList()[0];
      btnSearch_Click(sender, e);
      return;
    }

    if (Clipboard.ContainsText())
    {
      var text = Clipboard.GetText();
      if (File.Exists(text))
      {
        btnSearch_Click(sender, e);
      }
      else
      {
        dgvResult.DataSource = null;
        picSource.Image = null;
        picSource.Refresh();
      }
      return;
    }

    if (Clipboard.ContainsImage())
    {
      using var sourceImage = Clipboard.GetImage();
      var filename = Path.Combine(Environment.GetEnvironmentVariable("temp"), SnowFlake.NewId);
      sourceImage.Save(filename, ImageFormat.Jpeg);
      var sim = (float)numLike.Value / 100;
      var hasher = new ImageHasher();
      var sw = Stopwatch.StartNew();
      var hash = hasher.DifferenceHash256(filename);
      var hashs = new ConcurrentBag<ulong[]> { hash };
      using (var image = Image.Load<Rgba32>(filename))
      {
        var actions = new List<Action>();
        if (cbRotate.Checked)
        {
          actions.Add(() =>
          {
            using var clone = image.Clone(c => c.Rotate(90));
            var rotate90 = clone.DifferenceHash256();
            hashs.Add(rotate90);
          });
          actions.Add(() =>
          {
            using var clone = image.Clone(c => c.Rotate(180));
            var rotate180 = clone.DifferenceHash256();
            hashs.Add(rotate180);
          });
          actions.Add(() =>
          {
            using var clone = image.Clone(c => c.Rotate(270));
            var rotate270 = clone.DifferenceHash256();
            hashs.Add(rotate270);
          });
        }

        if (cbFlip.Checked)
        {
          actions.Add(() =>
          {
            using var clone = image.Clone(c => c.Flip(FlipMode.Horizontal));
            var flipH = clone.DifferenceHash256();
            hashs.Add(flipH);
          });
          actions.Add(() =>
          {
            using var clone = image.Clone(c => c.Flip(FlipMode.Vertical));
            var flipV = clone.DifferenceHash256();
            hashs.Add(flipV);
          });
        }
        Parallel.Invoke(actions.ToArray());
      }

      var list = _frameIndex.AsParallel().WithDegreeOfParallelism(32).Select(x => new SearchResult()
      {
        路径 = x.Key,
        匹配度 = x.Value.SelectMany(h => hashs.Select(hh => ImageHasher.Compare(h, hh))).Max()
      }).Where(x => x.匹配度 >= sim).ToList();
      list.AddRange(_index.AsParallel().WithDegreeOfParallelism(32).Select(x => new SearchResult()
      {
        路径 = x.Key,
        匹配度 = hashs.Select(h => ImageHasher.Compare(x.Value, h)).Max()
      }).Where(x => x.匹配度 >= sim));
      list = list.OrderByDescending(a => a.匹配度).ToList();
      lbElpased.Text = sw.ElapsedMilliseconds + "ms";
      if (list.Count > 0)
      {
        picSource.ImageLocation = filename;
        picSource.Refresh();
      }

      dgvResult.DataSource = list;
      Task.Run(() =>
      {
        Thread.Sleep(1000);
        File.Delete(filename);
      }).ContinueWith(_ => 0).ConfigureAwait(false);
    }
  }

  private void dgvResult_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
  {
    if (e.Button == MouseButtons.Right && dgvResult.SelectedCells.Count > 0)
    {
      dgvContextMenuStrip.Show(MousePosition.X, MousePosition.Y);
      return;
    }

    dgvContextMenuStrip.Hide();
  }

  private void 打开所在文件夹_Click(object sender, EventArgs e)
  {
    ExplorerFile(dgvResult.SelectedCells[0].OwningRow.Cells["路径"].Value.ToString());
  }

  private void dgvResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
  {
    Process.Start(new ProcessStartInfo { FileName = dgvResult.SelectedCells[0].OwningRow.Cells["路径"].Value.ToString(), UseShellExecute = true });
  }

  /// <summary>
  /// 打开路径并定位文件...对于@"h:\Bleacher Report - Hardaway with the safe call ??.mp4"这样的，explorer.exe /select,d:xxx不认，用API整它
  /// </summary>
  [DllImport("shell32.dll", ExactSpelling = true)]
  private static extern void ILFree(IntPtr pidlList);

  [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
  private static extern IntPtr ILCreateFromPathW(string pszPath);

  [DllImport("shell32.dll", ExactSpelling = true)]
  private static extern int SHOpenFolderAndSelectItems(IntPtr pidlList, uint cild, IntPtr children, uint dwFlags);

  public void ExplorerFile(string filePath)
  {
    if (!File.Exists(filePath))
    {
      return;
    }

    var pidlList = ILCreateFromPathW(filePath);
    if (pidlList != IntPtr.Zero)
    {
      try
      {
        Marshal.ThrowExceptionForHR(SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0));
      }
      catch
      {
      }
      finally
      {
        ILFree(pidlList);
      }
    }
  }

  protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
  {
    if (keyData == (Keys.Control | Keys.V))
    {
      buttonClipSearch_Click(null, null);
    }
    return base.ProcessCmdKey(ref msg, keyData);
  }

  private void dgvResult_KeyUp(object sender, KeyEventArgs e)
  {
  }

  private void dgvResult_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyCode is Keys.Up or Keys.Down)
    {
      picDest.ImageLocation = dgvResult.SelectedCells[0].OwningRow.Cells["路径"].Value.ToString();
    }
  }

  private void picSource_Click(object sender, EventArgs e)
  {
    Process.Start(new ProcessStartInfo { FileName = picSource.ImageLocation, UseShellExecute = true });
  }

  private void picDest_Click(object sender, EventArgs e)
  {
    Process.Start(new ProcessStartInfo { FileName = picDest.ImageLocation, UseShellExecute = true });
  }

  private void Form1_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (IndexRunning)
    {
      MessageBox.Show("正在索引文件，关闭程序前请先取消");
      e.Cancel = true;
      return;
    }

    if (_removingInvalidIndex)
    {
      MessageBox.Show("正在移除无效的索引文件，请稍后再试");
      e.Cancel = true;
    }
  }
  private void btnOpenSelected_Click(object sender, EventArgs e)
  {
    OpenSelectedImages();
  }

  private void btnDeleteSelected_Click(object sender, EventArgs e)
  {
    DeleteSelectedImages();
  }

  private void OpenSelectedImages()
  {
    foreach (DataGridViewRow row in dgvResult.SelectedRows)
    {
      string filePath = row.Cells["路径"].Value.ToString();
      if (File.Exists(filePath))
      {
        Process.Start(new ProcessStartInfo { FileName = filePath, UseShellExecute = true });
      }
    }
  }
  private void DeleteSelectedImages()
  {
    if (MessageBox.Show("确定要将选取图片移动到回收筒吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
    {
      var itemsToRemove = new List<SearchResult>();
      foreach (DataGridViewRow row in dgvResult.SelectedRows)
      {
        var searchResult = row.DataBoundItem as SearchResult;
        if (searchResult != null)
        {
          string filePath = searchResult.路径;
          if (File.Exists(filePath))
          {
            try
            {
              Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(filePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
              itemsToRemove.Add(searchResult);
            }
            catch (Exception ex)
            {
              MessageBox.Show($"删除图片 {filePath} 时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          }
        }
      }

      // 從數據源中刪除項目
      var dataSource = dgvResult.DataSource as List<SearchResult>;
      if (dataSource != null)
      {
        foreach (var item in itemsToRemove)
        {
          dataSource.Remove(item);
        }
      }

      // 刷新DataGridView
      dgvResult.DataSource = null;
      dgvResult.DataSource = dataSource;
    }
  }
}

public record SearchResult
{
  public string 路径 { get; set; }
  public float 匹配度 { get; set; }
}