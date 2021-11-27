
namespace Painter
{
    partial class GCodeLaserHeadForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Painter.View.CanvasView.WinFormCanvas winFormCanvas;
        private System.Windows.Forms.CheckBox ckWait;
        private System.Windows.Forms.CheckBox ckXAxisUp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblLaserStatus;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusSingleCutLength;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusASpeed;
        private System.Windows.Forms.ToolStripStatusLabel statusMsg;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton SettingFrmCmd;
        private System.Windows.Forms.ToolStripButton OpenCmd;
        private System.Windows.Forms.ToolStrip toolStrip1;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GCodeLaserHeadForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabGCode = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.winFormCanvas = new Painter.View.CanvasView.WinFormCanvas();
            this.tabMsg = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckWait = new System.Windows.Forms.CheckBox();
            this.ckXAxisUp = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblLaserStatus = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnShowChart = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStripStatusSingleCutLength = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusASpeed = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.SettingFrmCmd = new System.Windows.Forms.ToolStripButton();
            this.OpenCmd = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.WatchCmd = new System.Windows.Forms.ToolStripButton();
            this.AddMultiHeadCmd = new System.Windows.Forms.ToolStripButton();
            this.SaveCmd = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabGCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabMsg.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 38);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabGCode);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1862, 1345);
            this.splitContainer1.SplitterDistance = 352;
            this.splitContainer1.TabIndex = 11;
            // 
            // tabGCode
            // 
            this.tabGCode.Controls.Add(this.tabPage3);
            this.tabGCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabGCode.Location = new System.Drawing.Point(0, 0);
            this.tabGCode.Name = "tabGCode";
            this.tabGCode.SelectedIndex = 0;
            this.tabGCode.Size = new System.Drawing.Size(352, 1345);
            this.tabGCode.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.DarkGray;
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage3.Size = new System.Drawing.Size(344, 1313);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Head1";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.winFormCanvas);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabMsg);
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Size = new System.Drawing.Size(1506, 1345);
            this.splitContainer2.SplitterDistance = 749;
            this.splitContainer2.TabIndex = 12;
            // 
            // winFormCanvas
            // 
            this.winFormCanvas.BackColor = System.Drawing.Color.White;
            this.winFormCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.winFormCanvas.ForeColor = System.Drawing.SystemColors.Highlight;
            this.winFormCanvas.Location = new System.Drawing.Point(0, 0);
            this.winFormCanvas.Name = "winFormCanvas";
            this.winFormCanvas.Size = new System.Drawing.Size(1506, 749);
            this.winFormCanvas.TabIndex = 7;
            // 
            // tabMsg
            // 
            this.tabMsg.Controls.Add(this.tabPage1);
            this.tabMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMsg.Location = new System.Drawing.Point(0, 81);
            this.tabMsg.Name = "tabMsg";
            this.tabMsg.SelectedIndex = 0;
            this.tabMsg.Size = new System.Drawing.Size(1506, 511);
            this.tabMsg.TabIndex = 11;
            this.tabMsg.TabStop = false;
            this.tabMsg.SelectedIndexChanged += new System.EventHandler(this.tabMsg_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.DarkGray;
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(1498, 479);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Head1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckWait);
            this.panel1.Controls.Add(this.ckXAxisUp);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblPosition);
            this.panel1.Controls.Add(this.lblLaserStatus);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.btnShowChart);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1506, 81);
            this.panel1.TabIndex = 8;
            // 
            // ckWait
            // 
            this.ckWait.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckWait.Location = new System.Drawing.Point(1029, 45);
            this.ckWait.Name = "ckWait";
            this.ckWait.Size = new System.Drawing.Size(160, 45);
            this.ckWait.TabIndex = 10;
            this.ckWait.Text = "屏蔽等待";
            this.ckWait.UseVisualStyleBackColor = true;
            // 
            // ckXAxisUp
            // 
            this.ckXAxisUp.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckXAxisUp.Location = new System.Drawing.Point(1029, 3);
            this.ckXAxisUp.Name = "ckXAxisUp";
            this.ckXAxisUp.Size = new System.Drawing.Size(160, 45);
            this.ckXAxisUp.TabIndex = 10;
            this.ckXAxisUp.Text = "Y轴向上";
            this.ckXAxisUp.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(514, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 30);
            this.label1.TabIndex = 9;
            this.label1.Text = "00:00.000";
            // 
            // lblPosition
            // 
            this.lblPosition.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosition.Location = new System.Drawing.Point(681, 8);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(312, 64);
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
            this.lblLaserStatus.Location = new System.Drawing.Point(1194, 18);
            this.lblLaserStatus.Name = "lblLaserStatus";
            this.lblLaserStatus.Size = new System.Drawing.Size(106, 53);
            this.lblLaserStatus.TabIndex = 8;
            this.lblLaserStatus.Text = "激光开";
            this.lblLaserStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(498, 8);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(177, 64);
            this.lblTime.TabIndex = 8;
            this.lblTime.Text = "00:00.000";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnShowChart
            // 
            this.btnShowChart.Location = new System.Drawing.Point(1334, 18);
            this.btnShowChart.Name = "btnShowChart";
            this.btnShowChart.Size = new System.Drawing.Size(114, 54);
            this.btnShowChart.TabIndex = 7;
            this.btnShowChart.Text = "显示X坐标";
            this.btnShowChart.UseVisualStyleBackColor = true;
            this.btnShowChart.Click += new System.EventHandler(this.btnShowChart_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(304, 8);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(135, 64);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "清除";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClearClick);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(152, 8);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(135, 64);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStopClick);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(3, 8);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(135, 64);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStartClick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.notifyIcon1.BalloonTipText = "关闭";
            this.notifyIcon1.BalloonTipTitle = "关闭窗口";
            this.notifyIcon1.Text = "Drawing";
            this.notifyIcon1.Visible = true;
            // 
            // toolStripStatusSingleCutLength
            // 
            this.toolStripStatusSingleCutLength.Name = "toolStripStatusSingleCutLength";
            this.toolStripStatusSingleCutLength.Size = new System.Drawing.Size(154, 24);
            this.toolStripStatusSingleCutLength.Text = "单个脉冲加工长度";
            // 
            // toolStripStatusASpeed
            // 
            this.toolStripStatusASpeed.Name = "toolStripStatusASpeed";
            this.toolStripStatusASpeed.Size = new System.Drawing.Size(77, 24);
            this.toolStripStatusASpeed.Text = "A轴速度";
            // 
            // statusMsg
            // 
            this.statusMsg.Name = "statusMsg";
            this.statusMsg.Size = new System.Drawing.Size(36, 24);
            this.statusMsg.Text = "0.0";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMsg,
            this.toolStripStatusASpeed,
            this.toolStripStatusSingleCutLength});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1383);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 15, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1862, 31);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // SettingFrmCmd
            // 
            this.SettingFrmCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SettingFrmCmd.Image = ((System.Drawing.Image)(resources.GetObject("SettingFrmCmd.Image")));
            this.SettingFrmCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SettingFrmCmd.Name = "SettingFrmCmd";
            this.SettingFrmCmd.Size = new System.Drawing.Size(34, 33);
            this.SettingFrmCmd.Text = "设置";
            this.SettingFrmCmd.Click += new System.EventHandler(this.SettingFrmCmdClick);
            // 
            // OpenCmd
            // 
            this.OpenCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenCmd.Image = global::Painter.Properties.Resources.loadFile;
            this.OpenCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenCmd.Name = "OpenCmd";
            this.OpenCmd.Size = new System.Drawing.Size(34, 33);
            this.OpenCmd.Text = "打开中间文件";
            this.OpenCmd.Click += new System.EventHandler(this.OpenCmdClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenCmd,
            this.WatchCmd,
            this.AddMultiHeadCmd,
            this.SaveCmd,
            this.SettingFrmCmd});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1862, 38);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "Setting";
            // 
            // WatchCmd
            // 
            this.WatchCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.WatchCmd.Image = global::Painter.Properties.Resources.preview;
            this.WatchCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WatchCmd.Name = "WatchCmd";
            this.WatchCmd.Size = new System.Drawing.Size(34, 33);
            this.WatchCmd.Text = "打开G代码";
            this.WatchCmd.Click += new System.EventHandler(this.WatchCmd_Click);
            // 
            // AddMultiHeadCmd
            // 
            this.AddMultiHeadCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddMultiHeadCmd.Image = global::Painter.Properties.Resources.multi;
            this.AddMultiHeadCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddMultiHeadCmd.Name = "AddMultiHeadCmd";
            this.AddMultiHeadCmd.Size = new System.Drawing.Size(34, 33);
            this.AddMultiHeadCmd.Text = "打开多个中间文件";
            this.AddMultiHeadCmd.Click += new System.EventHandler(this.AddMultiHeadCmd_Click);
            // 
            // SaveCmd
            // 
            this.SaveCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveCmd.Image = global::Painter.Properties.Resources.save;
            this.SaveCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveCmd.Name = "SaveCmd";
            this.SaveCmd.Size = new System.Drawing.Size(34, 33);
            this.SaveCmd.Text = "保存通用文件";
            this.SaveCmd.Click += new System.EventHandler(this.SaveCmd_Click);
            // 
            // GCodeLaserHeadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1862, 1414);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.ForeColor = System.Drawing.SystemColors.Highlight;
            this.Name = "GCodeLaserHeadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormTestcs";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabGCode.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabMsg.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton AddMultiHeadCmd;
        private System.Windows.Forms.TabControl tabGCode;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabMsg;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnShowChart;
        private System.Windows.Forms.ToolStripButton WatchCmd;
        private System.Windows.Forms.ToolStripButton SaveCmd;
    }
}