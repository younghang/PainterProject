/*
 * Created by SharpDevelop.
 * User: DELL
 * Date: 2021/9/11
 * Time: 23:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Painter.View.CanvasView
{
	partial class WinFormCanvas
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusMsg;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.旋转ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtRotateAngle = new System.Windows.Forms.ToolStripTextBox();
            this.ConfirmRotate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.绘制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.直线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.矩形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.圆ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.椭圆ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.轨迹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文字ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ReadFromFile = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsPNG = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsDAT = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 729);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 14, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1200, 31);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusMsg
            // 
            this.statusMsg.Name = "statusMsg";
            this.statusMsg.Size = new System.Drawing.Size(22, 24);
            this.statusMsg.Text = "...";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.旋转ToolStripMenuItem,
            this.toolStripSeparator2,
            this.绘制ToolStripMenuItem,
            this.toolStripSeparator3,
            this.ReadFromFile,
            this.保存ToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(117, 142);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(113, 6);
            // 
            // 旋转ToolStripMenuItem
            // 
            this.旋转ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtRotateAngle,
            this.ConfirmRotate});
            this.旋转ToolStripMenuItem.Name = "旋转ToolStripMenuItem";
            this.旋转ToolStripMenuItem.Size = new System.Drawing.Size(116, 30);
            this.旋转ToolStripMenuItem.Text = "旋转";
            // 
            // txtRotateAngle
            // 
            this.txtRotateAngle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtRotateAngle.Name = "txtRotateAngle";
            this.txtRotateAngle.Size = new System.Drawing.Size(100, 30);
            this.txtRotateAngle.Text = "90";
            // 
            // ConfirmRotate
            // 
            this.ConfirmRotate.Name = "ConfirmRotate";
            this.ConfirmRotate.Size = new System.Drawing.Size(270, 34);
            this.ConfirmRotate.Text = "确定";
            this.ConfirmRotate.Click += new System.EventHandler(this.ConfirmRotate_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(113, 6);
            // 
            // 绘制ToolStripMenuItem
            // 
            this.绘制ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.直线ToolStripMenuItem,
            this.矩形ToolStripMenuItem,
            this.圆ToolStripMenuItem,
            this.椭圆ToolStripMenuItem,
            this.轨迹ToolStripMenuItem,
            this.文字ToolStripMenuItem});
            this.绘制ToolStripMenuItem.Name = "绘制ToolStripMenuItem";
            this.绘制ToolStripMenuItem.Size = new System.Drawing.Size(116, 30);
            this.绘制ToolStripMenuItem.Text = "绘制";
            // 
            // 直线ToolStripMenuItem
            // 
            this.直线ToolStripMenuItem.Checked = true;
            this.直线ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.直线ToolStripMenuItem.Name = "直线ToolStripMenuItem";
            this.直线ToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.直线ToolStripMenuItem.Text = "直线";
            // 
            // 矩形ToolStripMenuItem
            // 
            this.矩形ToolStripMenuItem.Name = "矩形ToolStripMenuItem";
            this.矩形ToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.矩形ToolStripMenuItem.Text = "矩形";
            // 
            // 圆ToolStripMenuItem
            // 
            this.圆ToolStripMenuItem.Name = "圆ToolStripMenuItem";
            this.圆ToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.圆ToolStripMenuItem.Text = "圆";
            // 
            // 椭圆ToolStripMenuItem
            // 
            this.椭圆ToolStripMenuItem.Name = "椭圆ToolStripMenuItem";
            this.椭圆ToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.椭圆ToolStripMenuItem.Text = "椭圆";
            // 
            // 轨迹ToolStripMenuItem
            // 
            this.轨迹ToolStripMenuItem.Name = "轨迹ToolStripMenuItem";
            this.轨迹ToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.轨迹ToolStripMenuItem.Text = "轨迹";
            // 
            // 文字ToolStripMenuItem
            // 
            this.文字ToolStripMenuItem.Name = "文字ToolStripMenuItem";
            this.文字ToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.文字ToolStripMenuItem.Text = "文字";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(113, 6);
            // 
            // ReadFromFile
            // 
            this.ReadFromFile.Name = "ReadFromFile";
            this.ReadFromFile.Size = new System.Drawing.Size(116, 30);
            this.ReadFromFile.Text = "读取";
            this.ReadFromFile.Click += new System.EventHandler(this.ReadFromFile_Click);
            // 
            // 保存ToolStripMenuItem1
            // 
            this.保存ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveAsPNG,
            this.SaveAsDAT});
            this.保存ToolStripMenuItem1.Name = "保存ToolStripMenuItem1";
            this.保存ToolStripMenuItem1.Size = new System.Drawing.Size(116, 30);
            this.保存ToolStripMenuItem1.Text = "保存";
            // 
            // SaveAsPNG
            // 
            this.SaveAsPNG.Name = "SaveAsPNG";
            this.SaveAsPNG.Size = new System.Drawing.Size(270, 34);
            this.SaveAsPNG.Text = "PNG";
            this.SaveAsPNG.Click += new System.EventHandler(this.SaveAsPNG_Click);
            // 
            // SaveAsDAT
            // 
            this.SaveAsDAT.Name = "SaveAsDAT";
            this.SaveAsDAT.Size = new System.Drawing.Size(270, 34);
            this.SaveAsDAT.Text = "二进制文件";
            this.SaveAsDAT.Click += new System.EventHandler(this.SaveAsDAT_Click);
            // 
            // WinFormCanvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.statusStrip1);
            this.ForeColor = System.Drawing.SystemColors.Highlight;
            this.Name = "WinFormCanvas";
            this.Size = new System.Drawing.Size(1200, 760);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 旋转ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox txtRotateAngle;
        private System.Windows.Forms.ToolStripMenuItem ConfirmRotate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 直线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 矩形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 圆ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 椭圆ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 轨迹ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文字ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ReadFromFile;
        private System.Windows.Forms.ToolStripMenuItem SaveAsPNG;
        private System.Windows.Forms.ToolStripMenuItem SaveAsDAT;
    }
}
