
namespace Painter.Forms.Optical
{
    partial class NewRay
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOriX = new System.Windows.Forms.TextBox();
            this.txtOriY = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.combRayColors = new System.Windows.Forms.ComboBox();
            this.ckColor = new System.Windows.Forms.RadioButton();
            this.ckWave = new System.Windows.Forms.RadioButton();
            this.txtWaveLength = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(39, 246);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "添加";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(441, 246);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 40);
            this.button2.TabIndex = 0;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "起点";
            // 
            // txtOriX
            // 
            this.txtOriX.Location = new System.Drawing.Point(157, 37);
            this.txtOriX.Name = "txtOriX";
            this.txtOriX.Size = new System.Drawing.Size(100, 28);
            this.txtOriX.TabIndex = 2;
            this.txtOriX.Text = "0";
            // 
            // txtOriY
            // 
            this.txtOriY.Location = new System.Drawing.Point(317, 37);
            this.txtOriY.Name = "txtOriY";
            this.txtOriY.Size = new System.Drawing.Size(100, 28);
            this.txtOriY.TabIndex = 2;
            this.txtOriY.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(294, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "Y";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.combRayColors);
            this.groupBox1.Controls.Add(this.ckColor);
            this.groupBox1.Controls.Add(this.ckWave);
            this.groupBox1.Controls.Add(this.txtWaveLength);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(61, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 125);
            this.groupBox1.TabIndex = 3;
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
            this.ckColor.Checked = true;
            this.ckColor.Location = new System.Drawing.Point(256, 27);
            this.ckColor.Name = "ckColor";
            this.ckColor.Size = new System.Drawing.Size(69, 22);
            this.ckColor.TabIndex = 3;
            this.ckColor.TabStop = true;
            this.ckColor.Text = "颜色";
            this.ckColor.UseVisualStyleBackColor = true;
            // 
            // ckWave
            // 
            this.ckWave.AutoSize = true;
            this.ckWave.Location = new System.Drawing.Point(43, 27);
            this.ckWave.Name = "ckWave";
            this.ckWave.Size = new System.Drawing.Size(69, 22);
            this.ckWave.TabIndex = 3;
            this.ckWave.Text = "波长";
            this.ckWave.UseVisualStyleBackColor = true;
            this.ckWave.CheckedChanged += new System.EventHandler(this.ckWave_CheckedChanged);
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(450, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 1;
            this.label4.Text = "角度";
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(500, 37);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(100, 28);
            this.txtAngle.TabIndex = 2;
            this.txtAngle.Text = "-3";
            // 
            // NewRay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 327);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.txtOriY);
            this.Controls.Add(this.txtOriX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "NewRay";
            this.Text = "NewRay";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOriX;
        private System.Windows.Forms.TextBox txtOriY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox combRayColors;
        private System.Windows.Forms.RadioButton ckColor;
        private System.Windows.Forms.RadioButton ckWave;
        private System.Windows.Forms.TextBox txtWaveLength;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAngle;
    }
}