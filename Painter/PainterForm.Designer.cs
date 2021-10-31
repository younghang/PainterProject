namespace Painter
{
    partial class PainterForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PainterForm));
            this.listGCode = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.OpenCmd = new System.Windows.Forms.ToolStripButton();
            this.SettingFrmCmd = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusASpeed = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusSingleCutLength = new System.Windows.Forms.ToolStripStatusLabel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckWait = new System.Windows.Forms.CheckBox();
            this.ckXAxisUp = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblLaserStatus = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.winFormUC1 = new Painter.WinFormDisplay.WinFormUC();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listGCode
            // 
            this.listGCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listGCode.FormattingEnabled = true;
            this.listGCode.ItemHeight = 12;
            this.listGCode.Location = new System.Drawing.Point(0, 0);
            this.listGCode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.listGCode.Name = "listGCode";
            this.listGCode.Size = new System.Drawing.Size(261, 920);
            this.listGCode.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenCmd,
            this.SettingFrmCmd});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1185, 27);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "Setting";
            // 
            // OpenCmd
            // 
            this.OpenCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenCmd.Image = global::Painter.Properties.Resources.loadFile;
            this.OpenCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenCmd.Name = "OpenCmd";
            this.OpenCmd.Size = new System.Drawing.Size(24, 24);
            this.OpenCmd.Text = "打开文件";
            this.OpenCmd.Click += new System.EventHandler(this.OpenCmd_Click);
            // 
            // SettingFrmCmd
            // 
            this.SettingFrmCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SettingFrmCmd.Image = ((System.Drawing.Image)(resources.GetObject("SettingFrmCmd.Image")));
            this.SettingFrmCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SettingFrmCmd.Name = "SettingFrmCmd";
            this.SettingFrmCmd.Size = new System.Drawing.Size(24, 24);
            this.SettingFrmCmd.Text = "设置";
            this.SettingFrmCmd.Click += new System.EventHandler(this.SettingFrmCmdClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMsg,
            this.toolStripStatusASpeed,
            this.toolStripStatusSingleCutLength});
            this.statusStrip1.Location = new System.Drawing.Point(0, 925);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 11, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1185, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusMsg
            // 
            this.statusMsg.Name = "statusMsg";
            this.statusMsg.Size = new System.Drawing.Size(25, 17);
            this.statusMsg.Text = "0.0";
            // 
            // toolStripStatusASpeed
            // 
            this.toolStripStatusASpeed.Name = "toolStripStatusASpeed";
            this.toolStripStatusASpeed.Size = new System.Drawing.Size(52, 17);
            this.toolStripStatusASpeed.Text = "A轴速度";
            // 
            // toolStripStatusSingleCutLength
            // 
            this.toolStripStatusSingleCutLength.Name = "toolStripStatusSingleCutLength";
            this.toolStripStatusSingleCutLength.Size = new System.Drawing.Size(104, 17);
            this.toolStripStatusSingleCutLength.Text = "单个脉冲加工长度";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Location = new System.Drawing.Point(0, 60);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(921, 293);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(1, 5);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(90, 43);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.button2_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.notifyIcon1.BalloonTipText = "关闭";
            this.notifyIcon1.BalloonTipTitle = "关闭窗口";
            this.notifyIcon1.Icon = global::Painter.Properties.Resources.Icon1;
            this.notifyIcon1.Text = "Drawing";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listGCode);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.winFormUC1);
            this.splitContainer1.Size = new System.Drawing.Size(1185, 920);
            this.splitContainer1.SplitterDistance = 261;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckWait);
            this.panel1.Controls.Add(this.ckXAxisUp);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblPosition);
            this.panel1.Controls.Add(this.lblLaserStatus);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 636);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(921, 353);
            this.panel1.TabIndex = 8;
            // 
            // ckWait
            // 
            this.ckWait.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckWait.Location = new System.Drawing.Point(685, 29);
            this.ckWait.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckWait.Name = "ckWait";
            this.ckWait.Size = new System.Drawing.Size(107, 29);
            this.ckWait.TabIndex = 10;
            this.ckWait.Text = "屏蔽等待";
            this.ckWait.UseVisualStyleBackColor = true;
            // 
            // ckXAxisUp
            // 
            this.ckXAxisUp.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckXAxisUp.Location = new System.Drawing.Point(685, 2);
            this.ckXAxisUp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckXAxisUp.Name = "ckXAxisUp";
            this.ckXAxisUp.Size = new System.Drawing.Size(107, 29);
            this.ckXAxisUp.TabIndex = 10;
            this.ckXAxisUp.Text = "Y轴向上";
            this.ckXAxisUp.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(343, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "00:00.000";
            // 
            // lblPosition
            // 
            this.lblPosition.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosition.Location = new System.Drawing.Point(454, 5);
            this.lblPosition.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(208, 43);
            this.lblPosition.TabIndex = 8;
            this.lblPosition.Text = "(1000.00,1000.00)";
            this.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPosition.Click += new System.EventHandler(this.LblPositionClick);
            // 
            // lblLaserStatus
            // 
            this.lblLaserStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblLaserStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLaserStatus.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLaserStatus.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblLaserStatus.Location = new System.Drawing.Point(797, 12);
            this.lblLaserStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLaserStatus.Name = "lblLaserStatus";
            this.lblLaserStatus.Size = new System.Drawing.Size(71, 37);
            this.lblLaserStatus.TabIndex = 8;
            this.lblLaserStatus.Text = "激光开";
            this.lblLaserStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(331, 5);
            this.lblTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(119, 43);
            this.lblTime.TabIndex = 8;
            this.lblTime.Text = "00:00.000";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(203, 5);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(90, 43);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "清除";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClearClick);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(101, 5);
            this.btnStop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(90, 43);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.button3_Click);
            // 
            // winFormUC1
            // 
            this.winFormUC1.BackColor = System.Drawing.Color.White;
            this.winFormUC1.Dock = System.Windows.Forms.DockStyle.Top;
            this.winFormUC1.Location = new System.Drawing.Point(0, 0);
            this.winFormUC1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.winFormUC1.Name = "winFormUC1";
            this.winFormUC1.Size = new System.Drawing.Size(921, 636);
            this.winFormUC1.TabIndex = 7;
            this.winFormUC1.Load += new System.EventHandler(this.winFormUC1_Load);
            // 
            // PainterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 947);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "PainterForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listGCode;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton OpenCmd;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusMsg;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private WinFormDisplay.WinFormUC winFormUC1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ToolStripButton SettingFrmCmd;
        private System.Windows.Forms.CheckBox ckXAxisUp;
        private System.Windows.Forms.Label lblLaserStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusASpeed;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusSingleCutLength;
        private System.Windows.Forms.CheckBox ckWait;
    }
}

