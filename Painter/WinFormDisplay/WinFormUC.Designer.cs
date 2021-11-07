
using Painter.Properties;

namespace Painter.WinFormDisplay
{ 
    partial class WinFormUC
        {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button button1;
        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
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
            this.button1 = new System.Windows.Forms.Button();
            this.comBoxSelectShape = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.PictureBox();
            this.btnUndo = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtLineWidth = new System.Windows.Forms.NumericUpDown();
            this.lblImage = new System.Windows.Forms.Label();
            this.lblTextFont = new System.Windows.Forms.Label();
            this.comBoxLineCap = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMarginColor = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fillChecked = new System.Windows.Forms.CheckBox();
            this.lblFillColor = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.保存图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.截取区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnRedo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUndo)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineWidth)).BeginInit();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(897, 10);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "清空";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // comBoxSelectShape
            // 
            this.comBoxSelectShape.FormattingEnabled = true;
            this.comBoxSelectShape.Location = new System.Drawing.Point(14, 14);
            this.comBoxSelectShape.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comBoxSelectShape.Name = "comBoxSelectShape";
            this.comBoxSelectShape.Size = new System.Drawing.Size(74, 26);
            this.comBoxSelectShape.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(256, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "框线";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnRedo);
            this.panel1.Controls.Add(this.btnUndo);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.comBoxSelectShape);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1290, 54);
            this.panel1.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1011, 10);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(74, 34);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Load";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnRedo.Image = global::Painter.Properties.Resources.redo;
            this.btnRedo.Location = new System.Drawing.Point(843, 12);
            this.btnRedo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(28, 30);
            this.btnRedo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnRedo.TabIndex = 8;
            this.btnRedo.TabStop = false;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnUndo.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnUndo.Image = global::Painter.Properties.Resources.undo;
            this.btnUndo.Location = new System.Drawing.Point(789, 12);
            this.btnUndo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(28, 30);
            this.btnUndo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnUndo.TabIndex = 8;
            this.btnUndo.TabStop = false;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtLineWidth);
            this.panel3.Controls.Add(this.lblImage);
            this.panel3.Controls.Add(this.lblTextFont);
            this.panel3.Controls.Add(this.comBoxLineCap);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.lblMarginColor);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.fillChecked);
            this.panel3.Controls.Add(this.lblFillColor);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(109, 4);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(673, 47);
            this.panel3.TabIndex = 6;
            // 
            // txtLineWidth
            // 
            this.txtLineWidth.DecimalPlaces = 1;
            this.txtLineWidth.Location = new System.Drawing.Point(195, 8);
            this.txtLineWidth.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLineWidth.Name = "txtLineWidth";
            this.txtLineWidth.Size = new System.Drawing.Size(60, 28);
            this.txtLineWidth.TabIndex = 8;
            this.txtLineWidth.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.BackColor = System.Drawing.Color.Transparent;
            this.lblImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblImage.Font = new System.Drawing.Font("Consolas", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImage.Image = global::Painter.Properties.Resources.image;
            this.lblImage.Location = new System.Drawing.Point(628, 8);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(22, 24);
            this.lblImage.TabIndex = 7;
            this.lblImage.Text = "M";
            this.lblImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblImage.Click += new System.EventHandler(this.lblImage_Click);
            // 
            // lblTextFont
            // 
            this.lblTextFont.AutoSize = true;
            this.lblTextFont.BackColor = System.Drawing.Color.Transparent;
            this.lblTextFont.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTextFont.Font = new System.Drawing.Font("Consolas", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextFont.Location = new System.Drawing.Point(585, 8);
            this.lblTextFont.Name = "lblTextFont";
            this.lblTextFont.Size = new System.Drawing.Size(22, 24);
            this.lblTextFont.TabIndex = 7;
            this.lblTextFont.Text = "A";
            this.lblTextFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTextFont.Click += new System.EventHandler(this.label4_Click);
            this.lblTextFont.DoubleClick += new System.EventHandler(this.label4_DoubleClick);
            // 
            // comBoxLineCap
            // 
            this.comBoxLineCap.FormattingEnabled = true;
            this.comBoxLineCap.Location = new System.Drawing.Point(58, 10);
            this.comBoxLineCap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comBoxLineCap.Name = "comBoxLineCap";
            this.comBoxLineCap.Size = new System.Drawing.Size(85, 26);
            this.comBoxLineCap.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(340, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1, 20);
            this.label3.TabIndex = 6;
            // 
            // lblMarginColor
            // 
            this.lblMarginColor.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblMarginColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMarginColor.Location = new System.Drawing.Point(305, 10);
            this.lblMarginColor.Name = "lblMarginColor";
            this.lblMarginColor.Size = new System.Drawing.Size(22, 24);
            this.lblMarginColor.TabIndex = 4;
            this.lblMarginColor.Tag = "Margin";
            this.lblMarginColor.Text = "label2";
            this.lblMarginColor.Click += new System.EventHandler(this.lblMarginColor_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "末端";
            // 
            // fillChecked
            // 
            this.fillChecked.Location = new System.Drawing.Point(454, 8);
            this.fillChecked.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fillChecked.Name = "fillChecked";
            this.fillChecked.Size = new System.Drawing.Size(111, 26);
            this.fillChecked.TabIndex = 5;
            this.fillChecked.Text = "IsFilled";
            this.fillChecked.UseVisualStyleBackColor = true;
            // 
            // lblFillColor
            // 
            this.lblFillColor.BackColor = System.Drawing.Color.Coral;
            this.lblFillColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFillColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFillColor.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblFillColor.Location = new System.Drawing.Point(399, 10);
            this.lblFillColor.Name = "lblFillColor";
            this.lblFillColor.Size = new System.Drawing.Size(22, 24);
            this.lblFillColor.TabIndex = 4;
            this.lblFillColor.Tag = "Fill";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(351, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 18);
            this.label5.TabIndex = 3;
            this.label5.Text = "填充";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "线宽";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblInfo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 708);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1290, 32);
            this.panel2.TabIndex = 5;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(3, 4);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(62, 18);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "label1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存图片ToolStripMenuItem,
            this.截取区域ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 64);
            // 
            // 保存图片ToolStripMenuItem
            // 
            this.保存图片ToolStripMenuItem.Name = "保存图片ToolStripMenuItem";
            this.保存图片ToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.保存图片ToolStripMenuItem.Text = "保存图片";
            this.保存图片ToolStripMenuItem.Click += new System.EventHandler(this.保存图片ToolStripMenuItem_Click);
            // 
            // 截取区域ToolStripMenuItem
            // 
            this.截取区域ToolStripMenuItem.Name = "截取区域ToolStripMenuItem";
            this.截取区域ToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.截取区域ToolStripMenuItem.Text = "截取区域";
            this.截取区域ToolStripMenuItem.Click += new System.EventHandler(this.截取区域ToolStripMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(590, 414);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 28);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // WinFormUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "WinFormUC";
            this.Size = new System.Drawing.Size(1290, 740);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainFormPaint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnRedo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUndo)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineWidth)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.ComboBox comBoxSelectShape;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblMarginColor;
        private System.Windows.Forms.Label lblFillColor;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox fillChecked;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTextFont;
        private System.Windows.Forms.Button btnSave;
 
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 保存图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 截取区域ToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown txtLineWidth;
        private System.Windows.Forms.ComboBox comBoxLineCap;
        private System.Windows.Forms.PictureBox btnRedo;
        private System.Windows.Forms.PictureBox btnUndo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
    }
}
