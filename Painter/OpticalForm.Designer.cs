
namespace Painter
{
    partial class OpticalForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_Rays = new System.Windows.Forms.TabPage();
            this.groupRayProperties = new System.Windows.Forms.GroupBox();
            this.btnUpdateRay = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.combRayColors = new System.Windows.Forms.ComboBox();
            this.ckColor = new System.Windows.Forms.RadioButton();
            this.ckWave = new System.Windows.Forms.RadioButton();
            this.txtWaveLength = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.txtOriY = new System.Windows.Forms.TextBox();
            this.txtOriX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listRays = new System.Windows.Forms.ListView();
            this.RemoveRay = new System.Windows.Forms.Button();
            this.addRay = new System.Windows.Forms.Button();
            this.tabLens = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.btnTransLen = new System.Windows.Forms.Button();
            this.groupLensProperties = new System.Windows.Forms.GroupBox();
            this.cbMaterialNames = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdMirror = new System.Windows.Forms.RadioButton();
            this.rdArc = new System.Windows.Forms.RadioButton();
            this.rdNegetive = new System.Windows.Forms.RadioButton();
            this.rdPositive = new System.Windows.Forms.RadioButton();
            this.txtLenAngle = new System.Windows.Forms.TextBox();
            this.txtLenOriY = new System.Windows.Forms.TextBox();
            this.txtRadius2 = new System.Windows.Forms.TextBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtThickness = new System.Windows.Forms.TextBox();
            this.txtRadius1 = new System.Windows.Forms.TextBox();
            this.txtLenOriX = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnUpdateLens = new System.Windows.Forms.Button();
            this.listLens = new System.Windows.Forms.ListView();
            this.removeLens = new System.Windows.Forms.Button();
            this.addLens = new System.Windows.Forms.Button();
            this.tabLength = new System.Windows.Forms.TabPage();
            this.btnFocusAlign = new System.Windows.Forms.Button();
            this.lbl_RayImagePoint = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.tabMaterials = new System.Windows.Forms.TabPage();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnDeleteMat = new System.Windows.Forms.Button();
            this.btnUpdateMaterial = new System.Windows.Forms.Button();
            this.txtMatEquation = new System.Windows.Forms.RichTextBox();
            this.txtMatName = new System.Windows.Forms.TextBox();
            this.listMaterials = new System.Windows.Forms.ListView();
            this.btnStopAutoGap = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.txt_AutoDelta = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtAutoCount = new System.Windows.Forms.TextBox();
            this.ckShowAutoAdjust = new System.Windows.Forms.CheckBox();
            this.canvasView1 = new Painter.View.CanvasView.CanvasView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tab_Rays.SuspendLayout();
            this.groupRayProperties.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabLens.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupLensProperties.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabLength.SuspendLayout();
            this.tabMaterials.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.canvasView1);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1419, 987);
            this.splitContainer1.SplitterDistance = 690;
            this.splitContainer1.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1419, 36);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(62, 30);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.保存ToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_Rays);
            this.tabControl1.Controls.Add(this.tabLens);
            this.tabControl1.Controls.Add(this.tabLength);
            this.tabControl1.Controls.Add(this.tabMaterials);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1419, 293);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tab_Rays
            // 
            this.tab_Rays.Controls.Add(this.groupRayProperties);
            this.tab_Rays.Controls.Add(this.listRays);
            this.tab_Rays.Controls.Add(this.RemoveRay);
            this.tab_Rays.Controls.Add(this.addRay);
            this.tab_Rays.Location = new System.Drawing.Point(4, 28);
            this.tab_Rays.Name = "tab_Rays";
            this.tab_Rays.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Rays.Size = new System.Drawing.Size(1411, 261);
            this.tab_Rays.TabIndex = 0;
            this.tab_Rays.Text = "光线";
            this.tab_Rays.UseVisualStyleBackColor = true;
            // 
            // groupRayProperties
            // 
            this.groupRayProperties.Controls.Add(this.btnUpdateRay);
            this.groupRayProperties.Controls.Add(this.groupBox1);
            this.groupRayProperties.Controls.Add(this.txtAngle);
            this.groupRayProperties.Controls.Add(this.txtOriY);
            this.groupRayProperties.Controls.Add(this.txtOriX);
            this.groupRayProperties.Controls.Add(this.label3);
            this.groupRayProperties.Controls.Add(this.label2);
            this.groupRayProperties.Controls.Add(this.label4);
            this.groupRayProperties.Controls.Add(this.label1);
            this.groupRayProperties.Location = new System.Drawing.Point(429, 12);
            this.groupRayProperties.Name = "groupRayProperties";
            this.groupRayProperties.Size = new System.Drawing.Size(777, 243);
            this.groupRayProperties.TabIndex = 2;
            this.groupRayProperties.TabStop = false;
            this.groupRayProperties.Text = "光线属性";
            this.groupRayProperties.Enter += new System.EventHandler(this.groupRayProperties_Enter);
            // 
            // btnUpdateRay
            // 
            this.btnUpdateRay.Location = new System.Drawing.Point(663, 168);
            this.btnUpdateRay.Name = "btnUpdateRay";
            this.btnUpdateRay.Size = new System.Drawing.Size(75, 38);
            this.btnUpdateRay.TabIndex = 12;
            this.btnUpdateRay.Text = "确定";
            this.btnUpdateRay.UseVisualStyleBackColor = true;
            this.btnUpdateRay.Click += new System.EventHandler(this.btnUpdateRay_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.combRayColors);
            this.groupBox1.Controls.Add(this.ckColor);
            this.groupBox1.Controls.Add(this.ckWave);
            this.groupBox1.Controls.Add(this.txtWaveLength);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(62, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 125);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "波长";
            // 
            // combRayColors
            // 
            this.combRayColors.FormattingEnabled = true;
            this.combRayColors.Location = new System.Drawing.Point(278, 81);
            this.combRayColors.Name = "combRayColors";
            this.combRayColors.Size = new System.Drawing.Size(121, 26);
            this.combRayColors.TabIndex = 4;
            // 
            // ckColor
            // 
            this.ckColor.AutoSize = true;
            this.ckColor.Location = new System.Drawing.Point(256, 27);
            this.ckColor.Name = "ckColor";
            this.ckColor.Size = new System.Drawing.Size(69, 22);
            this.ckColor.TabIndex = 3;
            this.ckColor.Text = "颜色";
            this.ckColor.UseVisualStyleBackColor = true;
            // 
            // ckWave
            // 
            this.ckWave.AutoSize = true;
            this.ckWave.Checked = true;
            this.ckWave.Location = new System.Drawing.Point(43, 27);
            this.ckWave.Name = "ckWave";
            this.ckWave.Size = new System.Drawing.Size(69, 22);
            this.ckWave.TabIndex = 3;
            this.ckWave.TabStop = true;
            this.ckWave.Text = "波长";
            this.ckWave.UseVisualStyleBackColor = true;
            // 
            // txtWaveLength
            // 
            this.txtWaveLength.Location = new System.Drawing.Point(96, 81);
            this.txtWaveLength.Name = "txtWaveLength";
            this.txtWaveLength.Size = new System.Drawing.Size(100, 28);
            this.txtWaveLength.TabIndex = 2;
            this.txtWaveLength.Text = "600";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 18);
            this.label5.TabIndex = 1;
            this.label5.Text = "波长";
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(501, 35);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(100, 28);
            this.txtAngle.TabIndex = 8;
            this.txtAngle.Text = "-3";
            // 
            // txtOriY
            // 
            this.txtOriY.Location = new System.Drawing.Point(318, 35);
            this.txtOriY.Name = "txtOriY";
            this.txtOriY.Size = new System.Drawing.Size(100, 28);
            this.txtOriY.TabIndex = 9;
            this.txtOriY.Text = "0";
            // 
            // txtOriX
            // 
            this.txtOriX.Location = new System.Drawing.Point(158, 35);
            this.txtOriX.Name = "txtOriX";
            this.txtOriX.Size = new System.Drawing.Size(100, 28);
            this.txtOriX.TabIndex = 10;
            this.txtOriX.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(295, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Y";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(451, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "角度";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "起点";
            // 
            // listRays
            // 
            this.listRays.HideSelection = false;
            this.listRays.Location = new System.Drawing.Point(141, 12);
            this.listRays.Name = "listRays";
            this.listRays.Size = new System.Drawing.Size(266, 241);
            this.listRays.TabIndex = 1;
            this.listRays.UseCompatibleStateImageBehavior = false;
            this.listRays.View = System.Windows.Forms.View.List;
            this.listRays.SelectedIndexChanged += new System.EventHandler(this.listRays_SelectedIndexChanged);
            // 
            // RemoveRay
            // 
            this.RemoveRay.Location = new System.Drawing.Point(20, 63);
            this.RemoveRay.Name = "RemoveRay";
            this.RemoveRay.Size = new System.Drawing.Size(115, 45);
            this.RemoveRay.TabIndex = 0;
            this.RemoveRay.Text = "移除光线";
            this.RemoveRay.UseVisualStyleBackColor = true;
            this.RemoveRay.Click += new System.EventHandler(this.RemoveRay_Click);
            // 
            // addRay
            // 
            this.addRay.Location = new System.Drawing.Point(20, 12);
            this.addRay.Name = "addRay";
            this.addRay.Size = new System.Drawing.Size(115, 45);
            this.addRay.TabIndex = 0;
            this.addRay.Text = "添加光线";
            this.addRay.UseVisualStyleBackColor = true;
            this.addRay.Click += new System.EventHandler(this.addRay_Click);
            // 
            // tabLens
            // 
            this.tabLens.Controls.Add(this.groupBox3);
            this.tabLens.Controls.Add(this.groupLensProperties);
            this.tabLens.Controls.Add(this.listLens);
            this.tabLens.Controls.Add(this.removeLens);
            this.tabLens.Controls.Add(this.addLens);
            this.tabLens.Location = new System.Drawing.Point(4, 28);
            this.tabLens.Name = "tabLens";
            this.tabLens.Padding = new System.Windows.Forms.Padding(3);
            this.tabLens.Size = new System.Drawing.Size(1411, 261);
            this.tabLens.TabIndex = 1;
            this.tabLens.Text = "透镜";
            this.tabLens.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDown1);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.btnTransLen);
            this.groupBox3.Location = new System.Drawing.Point(1149, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(212, 220);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "位移";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(66, 37);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 28);
            this.numericUpDown1.TabIndex = 25;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(1180, 64);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(17, 18);
            this.label18.TabIndex = 24;
            this.label18.Text = "X";
            this.label18.Click += new System.EventHandler(this.label13_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(27, 39);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(17, 18);
            this.label17.TabIndex = 24;
            this.label17.Text = "X";
            this.label17.Click += new System.EventHandler(this.label13_Click);
            // 
            // btnTransLen
            // 
            this.btnTransLen.Location = new System.Drawing.Point(22, 88);
            this.btnTransLen.Name = "btnTransLen";
            this.btnTransLen.Size = new System.Drawing.Size(184, 28);
            this.btnTransLen.TabIndex = 16;
            this.btnTransLen.Text = "移动";
            this.btnTransLen.UseVisualStyleBackColor = true;
            this.btnTransLen.Click += new System.EventHandler(this.btnTransLen_Click);
            // 
            // groupLensProperties
            // 
            this.groupLensProperties.Controls.Add(this.cbMaterialNames);
            this.groupLensProperties.Controls.Add(this.groupBox2);
            this.groupLensProperties.Controls.Add(this.txtLenAngle);
            this.groupLensProperties.Controls.Add(this.txtLenOriY);
            this.groupLensProperties.Controls.Add(this.txtRadius2);
            this.groupLensProperties.Controls.Add(this.txtHeight);
            this.groupLensProperties.Controls.Add(this.txtThickness);
            this.groupLensProperties.Controls.Add(this.txtRadius1);
            this.groupLensProperties.Controls.Add(this.txtLenOriX);
            this.groupLensProperties.Controls.Add(this.label6);
            this.groupLensProperties.Controls.Add(this.label9);
            this.groupLensProperties.Controls.Add(this.label7);
            this.groupLensProperties.Controls.Add(this.label8);
            this.groupLensProperties.Controls.Add(this.label10);
            this.groupLensProperties.Controls.Add(this.label11);
            this.groupLensProperties.Controls.Add(this.label12);
            this.groupLensProperties.Controls.Add(this.label13);
            this.groupLensProperties.Controls.Add(this.label14);
            this.groupLensProperties.Controls.Add(this.btnUpdateLens);
            this.groupLensProperties.Location = new System.Drawing.Point(423, 10);
            this.groupLensProperties.Name = "groupLensProperties";
            this.groupLensProperties.Size = new System.Drawing.Size(707, 220);
            this.groupLensProperties.TabIndex = 6;
            this.groupLensProperties.TabStop = false;
            this.groupLensProperties.Text = "透镜属性";
            // 
            // cbMaterialNames
            // 
            this.cbMaterialNames.FormattingEnabled = true;
            this.cbMaterialNames.Items.AddRange(new object[] {
            "NBK7"});
            this.cbMaterialNames.Location = new System.Drawing.Point(550, 132);
            this.cbMaterialNames.Name = "cbMaterialNames";
            this.cbMaterialNames.Size = new System.Drawing.Size(121, 26);
            this.cbMaterialNames.TabIndex = 34;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdMirror);
            this.groupBox2.Controls.Add(this.rdArc);
            this.groupBox2.Controls.Add(this.rdNegetive);
            this.groupBox2.Controls.Add(this.rdPositive);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(33, 61);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(647, 63);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "透镜类型";
            // 
            // rdMirror
            // 
            this.rdMirror.AutoSize = true;
            this.rdMirror.Location = new System.Drawing.Point(434, 27);
            this.rdMirror.Name = "rdMirror";
            this.rdMirror.Size = new System.Drawing.Size(87, 22);
            this.rdMirror.TabIndex = 0;
            this.rdMirror.Text = "平面镜";
            this.rdMirror.UseVisualStyleBackColor = true;
            // 
            // rdArc
            // 
            this.rdArc.AutoSize = true;
            this.rdArc.Location = new System.Drawing.Point(278, 27);
            this.rdArc.Name = "rdArc";
            this.rdArc.Size = new System.Drawing.Size(105, 22);
            this.rdArc.TabIndex = 0;
            this.rdArc.Text = "圆弧透镜";
            this.rdArc.UseVisualStyleBackColor = true;
            // 
            // rdNegetive
            // 
            this.rdNegetive.AutoSize = true;
            this.rdNegetive.Location = new System.Drawing.Point(151, 27);
            this.rdNegetive.Name = "rdNegetive";
            this.rdNegetive.Size = new System.Drawing.Size(87, 22);
            this.rdNegetive.TabIndex = 0;
            this.rdNegetive.Text = "凹透镜";
            this.rdNegetive.UseVisualStyleBackColor = true;
            // 
            // rdPositive
            // 
            this.rdPositive.AutoSize = true;
            this.rdPositive.Checked = true;
            this.rdPositive.Location = new System.Drawing.Point(40, 27);
            this.rdPositive.Name = "rdPositive";
            this.rdPositive.Size = new System.Drawing.Size(87, 22);
            this.rdPositive.TabIndex = 0;
            this.rdPositive.TabStop = true;
            this.rdPositive.Text = "凸透镜";
            this.rdPositive.UseVisualStyleBackColor = true;
            // 
            // txtLenAngle
            // 
            this.txtLenAngle.Location = new System.Drawing.Point(499, 27);
            this.txtLenAngle.Name = "txtLenAngle";
            this.txtLenAngle.Size = new System.Drawing.Size(100, 28);
            this.txtLenAngle.TabIndex = 26;
            this.txtLenAngle.Text = "0";
            // 
            // txtLenOriY
            // 
            this.txtLenOriY.Location = new System.Drawing.Point(316, 27);
            this.txtLenOriY.Name = "txtLenOriY";
            this.txtLenOriY.Size = new System.Drawing.Size(100, 28);
            this.txtLenOriY.TabIndex = 27;
            this.txtLenOriY.Text = "0";
            // 
            // txtRadius2
            // 
            this.txtRadius2.Location = new System.Drawing.Point(338, 130);
            this.txtRadius2.Name = "txtRadius2";
            this.txtRadius2.Size = new System.Drawing.Size(100, 28);
            this.txtRadius2.TabIndex = 31;
            this.txtRadius2.Text = "50";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(338, 179);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(100, 28);
            this.txtHeight.TabIndex = 30;
            this.txtHeight.Text = "40";
            // 
            // txtThickness
            // 
            this.txtThickness.Location = new System.Drawing.Point(119, 179);
            this.txtThickness.Name = "txtThickness";
            this.txtThickness.Size = new System.Drawing.Size(100, 28);
            this.txtThickness.TabIndex = 29;
            this.txtThickness.Text = "2";
            // 
            // txtRadius1
            // 
            this.txtRadius1.Location = new System.Drawing.Point(120, 130);
            this.txtRadius1.Name = "txtRadius1";
            this.txtRadius1.Size = new System.Drawing.Size(100, 28);
            this.txtRadius1.TabIndex = 28;
            this.txtRadius1.Text = "50";
            // 
            // txtLenOriX
            // 
            this.txtLenOriX.Location = new System.Drawing.Point(156, 27);
            this.txtLenOriX.Name = "txtLenOriX";
            this.txtLenOriX.Size = new System.Drawing.Size(100, 28);
            this.txtLenOriX.TabIndex = 32;
            this.txtLenOriX.Text = "120";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(248, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 18);
            this.label6.TabIndex = 23;
            this.label6.Text = "半径R2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(248, 184);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 18);
            this.label9.TabIndex = 22;
            this.label9.Text = "高度";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 18);
            this.label7.TabIndex = 21;
            this.label7.Text = "厚度";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(484, 135);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 18);
            this.label8.TabIndex = 20;
            this.label8.Text = "材料";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(30, 135);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 18);
            this.label10.TabIndex = 19;
            this.label10.Text = "半径R1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(293, 32);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 18);
            this.label11.TabIndex = 17;
            this.label11.Text = "Y";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(123, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 18);
            this.label12.TabIndex = 18;
            this.label12.Text = "X";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(449, 32);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 18);
            this.label13.TabIndex = 24;
            this.label13.Text = "角度";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(57, 32);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 18);
            this.label14.TabIndex = 25;
            this.label14.Text = "位置";
            // 
            // btnUpdateLens
            // 
            this.btnUpdateLens.Location = new System.Drawing.Point(487, 179);
            this.btnUpdateLens.Name = "btnUpdateLens";
            this.btnUpdateLens.Size = new System.Drawing.Size(184, 28);
            this.btnUpdateLens.TabIndex = 16;
            this.btnUpdateLens.Text = "确定";
            this.btnUpdateLens.UseVisualStyleBackColor = true;
            this.btnUpdateLens.Click += new System.EventHandler(this.btnUpdateLens_Click);
            // 
            // listLens
            // 
            this.listLens.HideSelection = false;
            this.listLens.Location = new System.Drawing.Point(135, 10);
            this.listLens.Name = "listLens";
            this.listLens.Size = new System.Drawing.Size(272, 220);
            this.listLens.TabIndex = 5;
            this.listLens.UseCompatibleStateImageBehavior = false;
            this.listLens.View = System.Windows.Forms.View.List;
            this.listLens.SelectedIndexChanged += new System.EventHandler(this.listLens_SelectedIndexChanged);
            // 
            // removeLens
            // 
            this.removeLens.Location = new System.Drawing.Point(14, 61);
            this.removeLens.Name = "removeLens";
            this.removeLens.Size = new System.Drawing.Size(115, 45);
            this.removeLens.TabIndex = 3;
            this.removeLens.Text = "移除透镜";
            this.removeLens.UseVisualStyleBackColor = true;
            this.removeLens.Click += new System.EventHandler(this.RemoveLens_Click);
            // 
            // addLens
            // 
            this.addLens.Location = new System.Drawing.Point(14, 10);
            this.addLens.Name = "addLens";
            this.addLens.Size = new System.Drawing.Size(115, 45);
            this.addLens.TabIndex = 4;
            this.addLens.Text = "添加透镜";
            this.addLens.UseVisualStyleBackColor = true;
            this.addLens.Click += new System.EventHandler(this.addLen_Click);
            // 
            // tabLength
            // 
            this.tabLength.Controls.Add(this.ckShowAutoAdjust);
            this.tabLength.Controls.Add(this.txtAutoCount);
            this.tabLength.Controls.Add(this.txt_AutoDelta);
            this.tabLength.Controls.Add(this.btnStopAutoGap);
            this.tabLength.Controls.Add(this.btnFocusAlign);
            this.tabLength.Controls.Add(this.label21);
            this.tabLength.Controls.Add(this.lbl_RayImagePoint);
            this.tabLength.Controls.Add(this.label20);
            this.tabLength.Controls.Add(this.label19);
            this.tabLength.Location = new System.Drawing.Point(4, 28);
            this.tabLength.Name = "tabLength";
            this.tabLength.Size = new System.Drawing.Size(1411, 261);
            this.tabLength.TabIndex = 3;
            this.tabLength.Text = "总长";
            this.tabLength.UseVisualStyleBackColor = true;
            // 
            // btnFocusAlign
            // 
            this.btnFocusAlign.Location = new System.Drawing.Point(29, 179);
            this.btnFocusAlign.Name = "btnFocusAlign";
            this.btnFocusAlign.Size = new System.Drawing.Size(99, 45);
            this.btnFocusAlign.TabIndex = 1;
            this.btnFocusAlign.Text = "开始变焦";
            this.btnFocusAlign.UseVisualStyleBackColor = true;
            this.btnFocusAlign.Click += new System.EventHandler(this.btnFocusAlign_Click);
            // 
            // lbl_RayImagePoint
            // 
            this.lbl_RayImagePoint.AutoSize = true;
            this.lbl_RayImagePoint.Location = new System.Drawing.Point(115, 38);
            this.lbl_RayImagePoint.Name = "lbl_RayImagePoint";
            this.lbl_RayImagePoint.Size = new System.Drawing.Size(35, 18);
            this.lbl_RayImagePoint.TabIndex = 0;
            this.lbl_RayImagePoint.Text = "0.0";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(26, 38);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 18);
            this.label19.TabIndex = 0;
            this.label19.Text = "像点距离";
            // 
            // tabMaterials
            // 
            this.tabMaterials.Controls.Add(this.label16);
            this.tabMaterials.Controls.Add(this.label15);
            this.tabMaterials.Controls.Add(this.btnDeleteMat);
            this.tabMaterials.Controls.Add(this.btnUpdateMaterial);
            this.tabMaterials.Controls.Add(this.txtMatEquation);
            this.tabMaterials.Controls.Add(this.txtMatName);
            this.tabMaterials.Controls.Add(this.listMaterials);
            this.tabMaterials.Location = new System.Drawing.Point(4, 28);
            this.tabMaterials.Name = "tabMaterials";
            this.tabMaterials.Size = new System.Drawing.Size(1411, 261);
            this.tabMaterials.TabIndex = 2;
            this.tabMaterials.Text = "材料";
            this.tabMaterials.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(418, 86);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(134, 18);
            this.label16.TabIndex = 4;
            this.label16.Text = "折射率与波长nm";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(418, 15);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 18);
            this.label15.TabIndex = 4;
            this.label15.Text = "名称";
            // 
            // btnDeleteMat
            // 
            this.btnDeleteMat.Location = new System.Drawing.Point(906, 46);
            this.btnDeleteMat.Name = "btnDeleteMat";
            this.btnDeleteMat.Size = new System.Drawing.Size(130, 41);
            this.btnDeleteMat.TabIndex = 3;
            this.btnDeleteMat.Text = "计算器";
            this.btnDeleteMat.UseVisualStyleBackColor = true;
            this.btnDeleteMat.Click += new System.EventHandler(this.btnDeleteMat_Click);
            // 
            // btnUpdateMaterial
            // 
            this.btnUpdateMaterial.Location = new System.Drawing.Point(746, 46);
            this.btnUpdateMaterial.Name = "btnUpdateMaterial";
            this.btnUpdateMaterial.Size = new System.Drawing.Size(130, 41);
            this.btnUpdateMaterial.TabIndex = 3;
            this.btnUpdateMaterial.Text = "更新";
            this.btnUpdateMaterial.UseVisualStyleBackColor = true;
            this.btnUpdateMaterial.Click += new System.EventHandler(this.btnUpdateMaterial_Click);
            // 
            // txtMatEquation
            // 
            this.txtMatEquation.Location = new System.Drawing.Point(421, 107);
            this.txtMatEquation.Name = "txtMatEquation";
            this.txtMatEquation.Size = new System.Drawing.Size(864, 127);
            this.txtMatEquation.TabIndex = 2;
            this.txtMatEquation.Text = "";
            // 
            // txtMatName
            // 
            this.txtMatName.Location = new System.Drawing.Point(421, 46);
            this.txtMatName.Name = "txtMatName";
            this.txtMatName.Size = new System.Drawing.Size(297, 28);
            this.txtMatName.TabIndex = 1;
            // 
            // listMaterials
            // 
            this.listMaterials.HideSelection = false;
            this.listMaterials.Location = new System.Drawing.Point(8, 15);
            this.listMaterials.Name = "listMaterials";
            this.listMaterials.Size = new System.Drawing.Size(354, 238);
            this.listMaterials.TabIndex = 0;
            this.listMaterials.UseCompatibleStateImageBehavior = false;
            this.listMaterials.View = System.Windows.Forms.View.List;
            this.listMaterials.SelectedIndexChanged += new System.EventHandler(this.listMaterials_SelectedIndexChanged);
            // 
            // btnStopAutoGap
            // 
            this.btnStopAutoGap.Location = new System.Drawing.Point(178, 179);
            this.btnStopAutoGap.Name = "btnStopAutoGap";
            this.btnStopAutoGap.Size = new System.Drawing.Size(99, 45);
            this.btnStopAutoGap.TabIndex = 1;
            this.btnStopAutoGap.Text = "停止";
            this.btnStopAutoGap.UseVisualStyleBackColor = true;
            this.btnStopAutoGap.Click += new System.EventHandler(this.btnStopAutoGap_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(26, 88);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(116, 18);
            this.label20.TabIndex = 0;
            this.label20.Text = "F3移动微距离";
            // 
            // txt_AutoDelta
            // 
            this.txt_AutoDelta.Location = new System.Drawing.Point(148, 85);
            this.txt_AutoDelta.Name = "txt_AutoDelta";
            this.txt_AutoDelta.Size = new System.Drawing.Size(100, 28);
            this.txt_AutoDelta.TabIndex = 2;
            this.txt_AutoDelta.Text = "0.175";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(26, 136);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(98, 18);
            this.label21.TabIndex = 0;
            this.label21.Text = "F3移动次数";
            // 
            // txtAutoCount
            // 
            this.txtAutoCount.Location = new System.Drawing.Point(148, 133);
            this.txtAutoCount.Name = "txtAutoCount";
            this.txtAutoCount.Size = new System.Drawing.Size(100, 28);
            this.txtAutoCount.TabIndex = 2;
            this.txtAutoCount.Text = "200";
            // 
            // ckShowAutoAdjust
            // 
            this.ckShowAutoAdjust.AutoSize = true;
            this.ckShowAutoAdjust.Location = new System.Drawing.Point(211, 37);
            this.ckShowAutoAdjust.Name = "ckShowAutoAdjust";
            this.ckShowAutoAdjust.Size = new System.Drawing.Size(106, 22);
            this.ckShowAutoAdjust.TabIndex = 3;
            this.ckShowAutoAdjust.Text = "实时显示";
            this.ckShowAutoAdjust.UseVisualStyleBackColor = true;
            // 
            // canvasView1
            // 
            this.canvasView1.BackColor = System.Drawing.Color.White;
            this.canvasView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasView1.Location = new System.Drawing.Point(0, 36);
            this.canvasView1.Name = "canvasView1";
            this.canvasView1.Size = new System.Drawing.Size(1419, 654);
            this.canvasView1.TabIndex = 0;
            this.canvasView1.Load += new System.EventHandler(this.canvasView1_Load);
            // 
            // OpticalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1419, 987);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "OpticalForm";
            this.Text = "OpticalForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tab_Rays.ResumeLayout(false);
            this.groupRayProperties.ResumeLayout(false);
            this.groupRayProperties.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabLens.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupLensProperties.ResumeLayout(false);
            this.groupLensProperties.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabLength.ResumeLayout(false);
            this.tabLength.PerformLayout();
            this.tabMaterials.ResumeLayout(false);
            this.tabMaterials.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private View.CanvasView.CanvasView canvasView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_Rays;
        private System.Windows.Forms.GroupBox groupRayProperties;
        private System.Windows.Forms.ListView listRays;
        private System.Windows.Forms.Button RemoveRay;
        private System.Windows.Forms.Button addRay;
        private System.Windows.Forms.TabPage tabLens;
        private System.Windows.Forms.GroupBox groupLensProperties;
        private System.Windows.Forms.ListView listLens;
        private System.Windows.Forms.Button removeLens;
        private System.Windows.Forms.Button addLens;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox combRayColors;
        private System.Windows.Forms.RadioButton ckColor;
        private System.Windows.Forms.RadioButton ckWave;
        private System.Windows.Forms.TextBox txtWaveLength;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.TextBox txtOriY;
        private System.Windows.Forms.TextBox txtOriX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpdateRay;
        private System.Windows.Forms.ComboBox cbMaterialNames;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdMirror;
        private System.Windows.Forms.RadioButton rdArc;
        private System.Windows.Forms.RadioButton rdNegetive;
        private System.Windows.Forms.RadioButton rdPositive;
        private System.Windows.Forms.TextBox txtLenAngle;
        private System.Windows.Forms.TextBox txtLenOriY;
        private System.Windows.Forms.TextBox txtRadius2;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtThickness;
        private System.Windows.Forms.TextBox txtRadius1;
        private System.Windows.Forms.TextBox txtLenOriX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnUpdateLens;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabPage tabMaterials;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnUpdateMaterial;
        private System.Windows.Forms.RichTextBox txtMatEquation;
        private System.Windows.Forms.TextBox txtMatName;
        private System.Windows.Forms.ListView listMaterials;
        private System.Windows.Forms.Button btnDeleteMat;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnTransLen;
        private System.Windows.Forms.TabPage tabLength;
        private System.Windows.Forms.Button btnFocusAlign;
        private System.Windows.Forms.Label lbl_RayImagePoint;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.Button btnStopAutoGap;
        private System.Windows.Forms.TextBox txtAutoCount;
        private System.Windows.Forms.TextBox txt_AutoDelta;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox ckShowAutoAdjust;
    }
}