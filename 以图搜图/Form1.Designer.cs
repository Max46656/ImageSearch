namespace 以图搜图
{
  partial class Form1
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      Font SmallFont = new Font("Microsoft JhengHei", 12);
      Font MediumFont = new Font("Microsoft JhengHei", 16);
      Font LargeFont = new Font("Microsoft JhengHei", 20);
      this.Font = new Font("Microsoft JhengHei", 9F, FontStyle.Regular, GraphicsUnit.Point);
      this.BackColor = Color.White;
      this.ForeColor = Color.Black;
      label1 = new Label();
      txtDirectory = new TextBox();
      btnDirectory = new Button();
      btnPic = new Button();
      txtPic = new TextBox();
      label2 = new Label();
      btnSearch = new Button();
      btnIndex = new Button();
      picSource = new PictureBox();
      picDest = new PictureBox();
      label3 = new Label();
      dgvResult = new DataGridView();
      //以下新增
      dgvResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      dgvResult.MultiSelect = true;
      //
      dgvContextMenuStrip = new ContextMenuStrip(components);
      打开所在文件夹 = new ToolStripMenuItem();
      label4 = new Label();
      lbIndexCount = new Label();
      label5 = new Label();
      lbElpased = new Label();
      lblDestInfo = new Label();
      lbSrcInfo = new Label();
      lbSpeed = new Label();
      numLike = new NumericUpDown();
      cbRotate = new CheckBox();
      cbFlip = new CheckBox();
      label6 = new Label();
      lblProcess = new Label();
      cbRemoveInvalidIndex = new CheckBox();
      lblGithub = new LinkLabel();
      buttonClipSearch = new Button();
      ((System.ComponentModel.ISupportInitialize)picSource).BeginInit();
      ((System.ComponentModel.ISupportInitialize)picDest).BeginInit();
      ((System.ComponentModel.ISupportInitialize)dgvResult).BeginInit();
      dgvContextMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)numLike).BeginInit();
      SuspendLayout();
      //
      // label1
      //
      label1.AutoSize = true;
      label1.Location = new Point(12, 9);
      label1.Name = "label1";
      label1.Size = new Size(95, 30);
      label1.TabIndex = 0;
      label1.Text = "添加搜索文件夹:";
      label1.Font = SmallFont;
      label1.TextAlign = ContentAlignment.MiddleLeft;
      //
      // txtDirectory
      //
      txtDirectory.AllowDrop = true;
      txtDirectory.Location = new Point(113, 6);
      txtDirectory.Name = "txtDirectory";
      txtDirectory.Size = new Size(504, 33);
      txtDirectory.TabIndex = 1;
      txtDirectory.DragDrop += txtDirectory_DragDrop;
      txtDirectory.DragEnter += txtDirectory_DragEnter;
      txtDirectory.Font = SmallFont;
      //
      // btnDirectory
      //
      btnDirectory.Location = new Point(620, 6);
      btnDirectory.Name = "btnDirectory";
      btnDirectory.Size = new Size(64, 33);
      btnDirectory.TabIndex = 2;
      btnDirectory.Text = "选择";
      btnDirectory.UseVisualStyleBackColor = true;
      btnDirectory.Click += btnDirectory_Click;
      btnDirectory.Font = SmallFont;
      //
      // btnPic
      //
      btnPic.Location = new Point(620, 49);
      btnPic.Name = "btnPic";
      btnPic.Size = new Size(64, 33);
      btnPic.TabIndex = 5;
      btnPic.Text = "选择";
      btnPic.UseVisualStyleBackColor = true;
      btnPic.Click += btnPic_Click;
      btnPic.Font = SmallFont;
      //
      // txtPic
      //
      txtPic.AllowDrop = true;
      txtPic.Location = new Point(113, 52);
      txtPic.Name = "txtPic";
      txtPic.Size = new Size(504, 33);
      txtPic.TabIndex = 0;
      txtPic.DragDrop += txtPic_DragDrop;
      txtPic.DragEnter += txtPic_DragEnter;
      //
      // label2
      //
      label2.AutoSize = true;
      label2.Location = new Point(13, 52);
      label2.Name = "label2";
      label2.Size = new Size(95, 30);
      label2.TabIndex = 3;
      label2.Text = "用于检索的图片:";
      label2.Font = SmallFont;
      //
      // btnSearch
      //
      btnSearch.AutoSize = true;
      btnSearch.Location = new Point(550, 97);
      btnSearch.Name = "btnSearch";
      btnSearch.Size = new Size(130, 60);
      btnSearch.TabIndex = 50;
      btnSearch.Text = "搜索";
      btnSearch.UseVisualStyleBackColor = true;
      btnSearch.Click += btnSearch_Click;
      btnSearch.Font = LargeFont;
      //
      // btnIndex
      //
      btnIndex.Location = new Point(690, 6);
      btnIndex.Name = "btnIndex";
      btnIndex.Size = new Size(90, 33);
      btnIndex.TabIndex = 7;
      btnIndex.Text = "更新索引";
      btnIndex.UseVisualStyleBackColor = true;
      btnIndex.Font = SmallFont;
      btnIndex.Click += btnIndex_Click;
      //
      // picSource
      //
      picSource.Location = new Point(818, 90);
      picSource.Name = "picSource";
      picSource.Size = new Size(800, 500);
      picSource.SizeMode = PictureBoxSizeMode.Zoom;
      picSource.TabIndex = 9;
      picSource.TabStop = false;
      picSource.LoadCompleted += picSource_LoadCompleted;
      picSource.DoubleClick += picSource_Click;
      //
      // lbSrcInfo
      //
      lbSrcInfo.AutoSize = true;
      lbSrcInfo.Location = new Point(935, 60);
      lbSrcInfo.Name = "lbSrcInfo";
      lbSrcInfo.Size = new Size(0, 30);
      lbSrcInfo.TabIndex = 19;
      //
      // picDest
      //
      picDest.Location = new Point(818, 650);
      picDest.Name = "picDest";
      picDest.Size = new Size(800, 500);
      picDest.SizeMode = PictureBoxSizeMode.Zoom;
      picDest.TabIndex = 10;
      picDest.TabStop = false;
      picDest.LoadCompleted += picDest_LoadCompleted;
      picDest.DoubleClick += picDest_Click;
      //
      // lblDestInfo
      //
      lblDestInfo.AutoSize = true;
      lblDestInfo.Location = new Point(935, 620);
      lblDestInfo.Name = "lblDestInfo";
      lblDestInfo.Size = new Size(0, 30);
      lblDestInfo.TabIndex = 18;
      //
      // label3
      //
      label3.AutoSize = true;
      label3.Location = new Point(13, 90);
      label3.Name = "label3";
      label3.Size = new Size(80, 30);
      label3.TabIndex = 11;
      label3.Text = "查找相似度：";
      label3.Font = SmallFont;
      //
      // dgvResult
      //
      dgvResult.AllowUserToAddRows = false;
      dgvResult.AllowUserToDeleteRows = false;
      dgvResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      dgvResult.BackgroundColor = SystemColors.Control;
      dgvResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dgvResult.Location = new Point(12, 200);
      dgvResult.Name = "dgvResult";
      dgvResult.ReadOnly = true;
      dgvResult.Size = new Size(800, 950);
      dgvResult.TabIndex = 13;
      dgvResult.Font = SmallFont;
      dgvResult.CellClick += dgvResult_CellClick;
      dgvResult.CellContentClick += dgvResult_CellContentClick;
      dgvResult.CellDoubleClick += dgvResult_CellDoubleClick;
      dgvResult.CellMouseDown += dgvResult_CellMouseDown;
      dgvResult.KeyDown += dgvResult_KeyDown;
      dgvResult.KeyUp += dgvResult_KeyUp;
      //
      // dgvContextMenuStrip
      //
      dgvContextMenuStrip.Items.AddRange(new ToolStripItem[] { 打开所在文件夹
  });
      dgvContextMenuStrip.Name = "dgvContextMenuStrip";
      dgvContextMenuStrip.Size = new Size(161, 26);
      //
      // 打开所在文件夹
      //
      打开所在文件夹.Name = "打开所在文件夹";
      打开所在文件夹.Size = new Size(160, 22);
      打开所在文件夹.Text = "打开所在文件夹";
      打开所在文件夹.Font = SmallFont;
      打开所在文件夹.Click += 打开所在文件夹_Click;
      //
      // label4
      //
      label4.AutoSize = true;
      label4.Location = new Point(697, 62);
      label4.Name = "label4";
      label4.Size = new Size(80, 30);
      label4.TabIndex = 14;
      label4.Text = "索引总数量：";
      label4.Font = SmallFont;
      //
      // lbIndexCount
      //
      lbIndexCount.AutoSize = true;
      lbIndexCount.Location = new Point(782, 62);
      lbIndexCount.Name = "lbIndexCount";
      lbIndexCount.Size = new Size(0, 30);
      lbIndexCount.TabIndex = 15;
      //
      // label5
      //
      label5.AutoSize = true;
      label5.Location = new Point(697, 98);
      label5.Name = "label5";
      label5.Size = new Size(68, 30);
      label5.TabIndex = 16;
      label5.Text = "搜索耗时：";
      label5.Font = SmallFont;
      //
      // lbElpased
      //
      lbElpased.AutoSize = true;
      lbElpased.Location = new Point(780, 99);
      lbElpased.Name = "lbElpased";
      lbElpased.Size = new Size(0, 30);
      lbElpased.TabIndex = 17;
      //
      // lbSpeed
      //
      lbSpeed.AutoSize = true;
      lbSpeed.Location = new Point(697, 128);
      lbSpeed.Name = "lbSpeed";
      lbSpeed.Size = new Size(16, 30);
      lbSpeed.TabIndex = 20;
      lbSpeed.Text = "  ";
      lbSpeed.Font = SmallFont;
      //
      // numLike
      //
      numLike.Location = new Point(114, 93);
      numLike.Minimum = new decimal(new int[] { 70, 0, 0, 0 });
      numLike.Name = "numLike";
      numLike.Size = new Size(45, 33);
      numLike.TabIndex = 21;
      numLike.Font = SmallFont;
      numLike.Value = new decimal(new int[] { 80, 0, 0, 0 });
      //
      // cbRotate
      //
      cbRotate.AutoSize = true;
      cbRotate.Checked = true;
      cbRotate.CheckState = CheckState.Checked;
      cbRotate.Location = new Point(171, 99);
      cbRotate.Name = "cbRotate";
      cbRotate.Size = new Size(87, 21);
      cbRotate.TabIndex = 22;
      cbRotate.Text = "查找已旋转";
      cbRotate.UseVisualStyleBackColor = true;
      cbRotate.Font = SmallFont;
      //
      // cbFlip
      //
      cbFlip.AutoSize = true;
      cbFlip.Location = new Point(275, 99);
      cbFlip.Name = "cbFlip";
      cbFlip.Size = new Size(87, 21);
      cbFlip.TabIndex = 23;
      cbFlip.Text = "查找已翻转";
      cbFlip.UseVisualStyleBackColor = true;
      cbFlip.Font = SmallFont;
      //
      // label6
      //
      label6.AutoSize = true;
      label6.Location = new Point(31, 1160);
      label6.Name = "label6";
      label6.Size = new Size(68, 30);
      label6.TabIndex = 25;
      label6.Text = "项目地址：";
      label6.Font = SmallFont;
      //
      // lblGithub
      //
      lblGithub.AutoSize = true;
      lblGithub.Location = new Point(113, 1160);
      lblGithub.Name = "lblGithub";
      lblGithub.Size = new Size(227, 30);
      lblGithub.TabIndex = 29;
      lblGithub.TabStop = true;
      lblGithub.Text = "https://github.com/ldqk/ImageSearch";
      lblGithub.LinkClicked += lblGithub_LinkClicked;
      //
      // lblProcess
      //
      lblProcess.AutoSize = true;
      lblProcess.Location = new Point(793, 8);
      lblProcess.Name = "lblProcess";
      lblProcess.Size = new Size(12, 30);
      lblProcess.TabIndex = 27;
      lblProcess.Text = " ";
      //
      // cbRemoveInvalidIndex
      //
      cbRemoveInvalidIndex.AutoSize = true;
      cbRemoveInvalidIndex.Location = new Point(785, 9);
      cbRemoveInvalidIndex.Name = "cbRemoveInvalidIndex";
      cbRemoveInvalidIndex.Size = new Size(99, 21);
      cbRemoveInvalidIndex.TabIndex = 28;
      cbRemoveInvalidIndex.Text = "移除无效索引";
      cbRemoveInvalidIndex.UseVisualStyleBackColor = true;
      cbRemoveInvalidIndex.Font = SmallFont;
      //
      // buttonClipSearch
      //
      buttonClipSearch.Location = new Point(400, 97);
      buttonClipSearch.Name = "buttonClipSearch";
      buttonClipSearch.Size = new Size(126, 33);
      buttonClipSearch.TabIndex = 30;
      buttonClipSearch.Text = "从剪切板搜索";
      buttonClipSearch.UseVisualStyleBackColor = true;
      buttonClipSearch.Click += buttonClipSearch_Click;
      buttonClipSearch.Font = SmallFont;

      // 圖片批次處理功能

      //
      //multiSelectLabel
      //
      multiSelectLabel = new Label();
      multiSelectLabel.AutoSize = true;
      multiSelectLabel.Location = new Point(13, 140);
      multiSelectLabel.Name = "multiSelectLabel";
      multiSelectLabel.Size = new Size(60, 30);
      multiSelectLabel.TabIndex = 25;
      multiSelectLabel.Text = "已选取图片：";
      multiSelectLabel.Font = SmallFont;
      //
      // btnOpenSelected
      //
      btnOpenSelected = new Button();
      btnOpenSelected.Location = new Point(115, 138);
      btnOpenSelected.Name = "btnOpenSelected";
      btnOpenSelected.Size = new Size(51, 33);
      btnOpenSelected.TabIndex = 31;
      btnOpenSelected.Text = "开启";
      btnOpenSelected.UseVisualStyleBackColor = true;
      btnOpenSelected.Click += btnOpenSelected_Click;
      btnOpenSelected.Font = SmallFont;
      //
      //btnDeleteSelected
      //
      btnDeleteSelected = new Button();
      btnDeleteSelected.Location = new Point(175, 138);
      btnDeleteSelected.Name = "btnDeleteSelected";
      btnDeleteSelected.Size = new Size(51, 33);
      btnDeleteSelected.TabIndex = 32;
      btnDeleteSelected.Text = "删除";
      btnDeleteSelected.UseVisualStyleBackColor = true;
      btnDeleteSelected.Click += btnDeleteSelected_Click;
      btnDeleteSelected.Font = SmallFont;

      //
      // Form1
      //
      AutoScaleDimensions = new SizeF(7F, 17F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1600, 1200);
      Controls.Add(buttonClipSearch);
      Controls.Add(lblGithub);
      Controls.Add(cbRemoveInvalidIndex);
      Controls.Add(lblProcess);
      Controls.Add(label6);
      Controls.Add(cbFlip);
      Controls.Add(cbRotate);
      Controls.Add(numLike);
      Controls.Add(lbSpeed);
      Controls.Add(lbSrcInfo);
      Controls.Add(lblDestInfo);
      Controls.Add(lbElpased);
      Controls.Add(label5);
      Controls.Add(lbIndexCount);
      Controls.Add(label4);
      Controls.Add(dgvResult);
      Controls.Add(label3);
      Controls.Add(picDest);
      Controls.Add(picSource);
      Controls.Add(btnIndex);
      Controls.Add(btnSearch);
      Controls.Add(btnPic);
      Controls.Add(txtPic);
      Controls.Add(label2);
      Controls.Add(btnDirectory);
      Controls.Add(txtDirectory);
      Controls.Add(label1);
      Controls.Add(multiSelectLabel);
      Controls.Add(btnOpenSelected);
      Controls.Add(btnDeleteSelected);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      Icon = (Icon)resources.GetObject("$this.Icon");
      MaximizeBox = false;
      Name = "Form1";
      StartPosition = FormStartPosition.CenterScreen;
      Text = "本地以图搜图小工具 by 懒得勤快 (评估版本)";
      FormClosing += Form1_FormClosing;
      Load += Form1_Load;
      ((System.ComponentModel.ISupportInitialize)picSource).EndInit();
      ((System.ComponentModel.ISupportInitialize)picDest).EndInit();
      ((System.ComponentModel.ISupportInitialize)dgvResult).EndInit();
      dgvContextMenuStrip.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)numLike).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label label1;
    private TextBox txtDirectory;
    private Button btnDirectory;
    private Button btnPic;
    private TextBox txtPic;
    private Label label2;
    private Button btnSearch;
    private Button btnIndex;
    private PictureBox picSource;
    private PictureBox picDest;
    private Label label3;
    private DataGridView dgvResult;
    private Label label4;
    private Label lbIndexCount;
    private Label label5;
    private Label lbElpased;
    private Label lblDestInfo;
    private Label lbSrcInfo;
    private Label lbSpeed;
    private NumericUpDown numLike;
    private CheckBox cbRotate;
    private CheckBox cbFlip;
    private Label label6;
    private Label lblProcess;
    private CheckBox cbRemoveInvalidIndex;
    private LinkLabel lblGithub;
    private Button buttonClipSearch;
    private ContextMenuStrip dgvContextMenuStrip;
    private ToolStripMenuItem 打开所在文件夹;
    private Label multiSelectLabel;
    private Button btnOpenSelected;
    private Button btnDeleteSelected;
  }
}