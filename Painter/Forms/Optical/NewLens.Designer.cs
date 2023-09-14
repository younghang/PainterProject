
namespace Painter.Forms.Optical
{
    partial class NewLens
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
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.txtOriY = new System.Windows.Forms.TextBox();
            this.txtOriX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRadius1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRadius2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdMirror = new System.Windows.Forms.RadioButton();
            this.rdArc = new System.Windows.Forms.RadioButton();
            this.rdNegetive = new System.Windows.Forms.RadioButton();
            this.rdPositive = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.txtThickness = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(515, 18);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(100, 28);
            this.txtAngle.TabIndex = 10;
            this.txtAngle.Text = "0";
            // 
            // txtOriY
            // 
            this.txtOriY.Location = new System.Drawing.Point(332, 18);
            this.txtOriY.Name = "txtOriY";
            this.txtOriY.Size = new System.Drawing.Size(100, 28);
            this.txtOriY.TabIndex = 11;
            this.txtOriY.Text = "0";
            // 
            // txtOriX
            // 
            this.txtOriX.Location = new System.Drawing.Point(172, 18);
            this.txtOriX.Name = "txtOriX";
            this.txtOriX.Size = new System.Drawing.Size(100, 28);
            this.txtOriX.TabIndex = 12;
            this.txtOriX.Text = "120";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(309, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "Y";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(465, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 8;
            this.label4.Text = "角度";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 9;
            this.label1.Text = "位置";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(390, 267);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(159, 40);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "取消";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(109, 267);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(159, 40);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 18);
            this.label5.TabIndex = 7;
            this.label5.Text = "半径R1";
            // 
            // txtRadius1
            // 
            this.txtRadius1.Location = new System.Drawing.Point(117, 140);
            this.txtRadius1.Name = "txtRadius1";
            this.txtRadius1.Size = new System.Drawing.Size(100, 28);
            this.txtRadius1.TabIndex = 12;
            this.txtRadius1.Text = "50";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(245, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 18);
            this.label6.TabIndex = 7;
            this.label6.Text = "半径R2";
            // 
            // txtRadius2
            // 
            this.txtRadius2.Location = new System.Drawing.Point(335, 140);
            this.txtRadius2.Name = "txtRadius2";
            this.txtRadius2.Size = new System.Drawing.Size(100, 28);
            this.txtRadius2.TabIndex = 12;
            this.txtRadius2.Text = "50";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdMirror);
            this.groupBox1.Controls.Add(this.rdArc);
            this.groupBox1.Controls.Add(this.rdNegetive);
            this.groupBox1.Controls.Add(this.rdPositive);
            this.groupBox1.Location = new System.Drawing.Point(34, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(647, 63);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "透镜类型";
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
            this.rdMirror.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
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
            this.rdArc.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 206);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 18);
            this.label7.TabIndex = 7;
            this.label7.Text = "厚度";
            // 
            // txtThickness
            // 
            this.txtThickness.Location = new System.Drawing.Point(117, 201);
            this.txtThickness.Name = "txtThickness";
            this.txtThickness.Size = new System.Drawing.Size(100, 28);
            this.txtThickness.TabIndex = 12;
            this.txtThickness.Text = "2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(481, 145);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 18);
            this.label8.TabIndex = 7;
            this.label8.Text = "材料";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "NBK7"});
            this.comboBox1.Location = new System.Drawing.Point(547, 142);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 26);
            this.comboBox1.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(246, 206);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 18);
            this.label9.TabIndex = 7;
            this.label9.Text = "高度";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(336, 201);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(100, 28);
            this.txtHeight.TabIndex = 12;
            this.txtHeight.Text = "40";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(481, 201);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 18);
            this.label10.TabIndex = 7;
            this.label10.Text = "颜色";
            // 
            // lblColor
            // 
            this.lblColor.BackColor = System.Drawing.Color.Aqua;
            this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColor.Location = new System.Drawing.Point(544, 201);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(44, 18);
            this.lblColor.TabIndex = 7;
            this.lblColor.Click += new System.EventHandler(this.lblColor_Click);
            // 
            // NewLens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 319);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.txtOriY);
            this.Controls.Add(this.txtRadius2);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.txtThickness);
            this.Controls.Add(this.txtRadius1);
            this.Controls.Add(this.txtOriX);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAdd);
            this.Name = "NewLens";
            this.Text = "NewLens";
            this.Load += new System.EventHandler(this.NewLens_Load_1);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.TextBox txtOriY;
        private System.Windows.Forms.TextBox txtOriX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRadius1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRadius2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdArc;
        private System.Windows.Forms.RadioButton rdNegetive;
        private System.Windows.Forms.RadioButton rdPositive;
        private System.Windows.Forms.RadioButton rdMirror;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtThickness;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblColor;
    }
}