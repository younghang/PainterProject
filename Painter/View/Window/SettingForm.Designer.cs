/*
 * Created by SharpDevelop.
 * User: DELL
 * Date: 2021/9/7
 * Time: 21:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Painter
{
	partial class SettingForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TextBox txtBeamOnTime;
		private System.Windows.Forms.TextBox txtASpeed;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtBeamOffTime;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtCycleTimes;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtFlashSpan;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtPlayRate;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnConfirm;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtLoopGap;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtMoveRange;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblColor;
		
		/// <summary>
		/// Disposes resources used by the form.
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
			this.txtBeamOnTime = new System.Windows.Forms.TextBox();
			this.txtASpeed = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtBeamOffTime = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtCycleTimes = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtFlashSpan = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtPlayRate = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.txtLoopGap = new System.Windows.Forms.TextBox();
			this.btnConfirm = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.txtMoveRange = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ckLaserCircle = new System.Windows.Forms.CheckBox();
			this.lblLogFont = new System.Windows.Forms.Label();
			this.lblGCodeFont = new System.Windows.Forms.Label();
			this.lblColor = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnSub = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.txtPID_D = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.txtPID_I = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtPID_P = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.txtDefaultMoveSpeed = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.txtDefaultCutSpeed = new System.Windows.Forms.TextBox();
			this.lblCutRatioName = new System.Windows.Forms.Label();
			this.txtCutSpeedRatio = new System.Windows.Forms.TextBox();
			this.tabControl2 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tabUI = new System.Windows.Forms.TabPage();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabControl2.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabUI.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtBeamOnTime
			// 
			this.txtBeamOnTime.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtBeamOnTime.Location = new System.Drawing.Point(162, 103);
			this.txtBeamOnTime.Margin = new System.Windows.Forms.Padding(5);
			this.txtBeamOnTime.Name = "txtBeamOnTime";
			this.txtBeamOnTime.Size = new System.Drawing.Size(216, 30);
			this.txtBeamOnTime.TabIndex = 14;
			this.txtBeamOnTime.Text = "0.5";
			// 
			// txtASpeed
			// 
			this.txtASpeed.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtASpeed.Location = new System.Drawing.Point(162, 37);
			this.txtASpeed.Margin = new System.Windows.Forms.Padding(5);
			this.txtASpeed.Name = "txtASpeed";
			this.txtASpeed.Size = new System.Drawing.Size(216, 30);
			this.txtASpeed.TabIndex = 15;
			this.txtASpeed.Text = "6666.66";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label3.Location = new System.Drawing.Point(30, 105);
			this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 26);
			this.label3.TabIndex = 12;
			this.label3.Text = "开光时间";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label2.Location = new System.Drawing.Point(30, 39);
			this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 26);
			this.label2.TabIndex = 13;
			this.label2.Text = "A轴速度";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(30, 171);
			this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 26);
			this.label1.TabIndex = 12;
			this.label1.Text = "关光时间";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtBeamOffTime
			// 
			this.txtBeamOffTime.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtBeamOffTime.Location = new System.Drawing.Point(163, 169);
			this.txtBeamOffTime.Margin = new System.Windows.Forms.Padding(5);
			this.txtBeamOffTime.Name = "txtBeamOffTime";
			this.txtBeamOffTime.Size = new System.Drawing.Size(216, 30);
			this.txtBeamOffTime.TabIndex = 14;
			this.txtBeamOffTime.Text = "0.5";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label4.Location = new System.Drawing.Point(481, 39);
			this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 26);
			this.label4.TabIndex = 12;
			this.label4.Text = "循环次数";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtCycleTimes
			// 
			this.txtCycleTimes.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtCycleTimes.Location = new System.Drawing.Point(627, 37);
			this.txtCycleTimes.Margin = new System.Windows.Forms.Padding(5);
			this.txtCycleTimes.Name = "txtCycleTimes";
			this.txtCycleTimes.Size = new System.Drawing.Size(216, 30);
			this.txtCycleTimes.TabIndex = 14;
			this.txtCycleTimes.Text = "999";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label5.Location = new System.Drawing.Point(481, 105);
			this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 26);
			this.label5.TabIndex = 12;
			this.label5.Text = "刷新间隔";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtFlashSpan
			// 
			this.txtFlashSpan.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtFlashSpan.Location = new System.Drawing.Point(627, 103);
			this.txtFlashSpan.Margin = new System.Windows.Forms.Padding(5);
			this.txtFlashSpan.Name = "txtFlashSpan";
			this.txtFlashSpan.Size = new System.Drawing.Size(216, 30);
			this.txtFlashSpan.TabIndex = 14;
			this.txtFlashSpan.Text = "20";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label6.Location = new System.Drawing.Point(481, 171);
			this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(120, 26);
			this.label6.TabIndex = 12;
			this.label6.Text = "播放速率";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtPlayRate
			// 
			this.txtPlayRate.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtPlayRate.Location = new System.Drawing.Point(627, 169);
			this.txtPlayRate.Margin = new System.Windows.Forms.Padding(5);
			this.txtPlayRate.Name = "txtPlayRate";
			this.txtPlayRate.Size = new System.Drawing.Size(216, 30);
			this.txtPlayRate.TabIndex = 14;
			this.txtPlayRate.Text = "1.0";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(199, 451);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(205, 62);
			this.btnOK.TabIndex = 16;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.BtnOKClick);
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label7.Location = new System.Drawing.Point(30, 237);
			this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(120, 26);
			this.label7.TabIndex = 12;
			this.label7.Text = "循环周期";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtLoopGap
			// 
			this.txtLoopGap.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtLoopGap.Location = new System.Drawing.Point(163, 235);
			this.txtLoopGap.Margin = new System.Windows.Forms.Padding(5);
			this.txtLoopGap.Name = "txtLoopGap";
			this.txtLoopGap.Size = new System.Drawing.Size(216, 30);
			this.txtLoopGap.TabIndex = 14;
			this.txtLoopGap.Text = "1200";
			// 
			// btnConfirm
			// 
			this.btnConfirm.Location = new System.Drawing.Point(503, 451);
			this.btnConfirm.Name = "btnConfirm";
			this.btnConfirm.Size = new System.Drawing.Size(205, 62);
			this.btnConfirm.TabIndex = 16;
			this.btnConfirm.Text = "应用";
			this.btnConfirm.UseVisualStyleBackColor = true;
			this.btnConfirm.Click += new System.EventHandler(this.BtnConfirmClick);
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label8.Location = new System.Drawing.Point(481, 237);
			this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(120, 26);
			this.label8.TabIndex = 12;
			this.label8.Text = "行程宽度";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtMoveRange
			// 
			this.txtMoveRange.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtMoveRange.Location = new System.Drawing.Point(627, 235);
			this.txtMoveRange.Margin = new System.Windows.Forms.Padding(5);
			this.txtMoveRange.Name = "txtMoveRange";
			this.txtMoveRange.Size = new System.Drawing.Size(216, 30);
			this.txtMoveRange.TabIndex = 14;
			this.txtMoveRange.Text = "1.0";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.ckLaserCircle);
			this.groupBox1.Controls.Add(this.lblLogFont);
			this.groupBox1.Controls.Add(this.lblGCodeFont);
			this.groupBox1.Controls.Add(this.lblColor);
			this.groupBox1.Location = new System.Drawing.Point(6, 10);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(859, 176);
			this.groupBox1.TabIndex = 17;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "外观";
			// 
			// ckLaserCircle
			// 
			this.ckLaserCircle.AutoSize = true;
			this.ckLaserCircle.Checked = true;
			this.ckLaserCircle.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckLaserCircle.Location = new System.Drawing.Point(257, 30);
			this.ckLaserCircle.Name = "ckLaserCircle";
			this.ckLaserCircle.Size = new System.Drawing.Size(128, 24);
			this.ckLaserCircle.TabIndex = 2;
			this.ckLaserCircle.Text = "显示激光头";
			this.ckLaserCircle.UseVisualStyleBackColor = true;
			// 
			// lblLogFont
			// 
			this.lblLogFont.AutoSize = true;
			this.lblLogFont.Location = new System.Drawing.Point(33, 142);
			this.lblLogFont.Name = "lblLogFont";
			this.lblLogFont.Size = new System.Drawing.Size(119, 20);
			this.lblLogFont.TabIndex = 1;
			this.lblLogFont.Text = "Log字体样式";
			this.lblLogFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblLogFont.Click += new System.EventHandler(this.lblLogFont_Click);
			// 
			// lblGCodeFont
			// 
			this.lblGCodeFont.AutoSize = true;
			this.lblGCodeFont.Location = new System.Drawing.Point(33, 89);
			this.lblGCodeFont.Name = "lblGCodeFont";
			this.lblGCodeFont.Size = new System.Drawing.Size(139, 20);
			this.lblGCodeFont.TabIndex = 1;
			this.lblGCodeFont.Text = "GCode字体样式";
			this.lblGCodeFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblGCodeFont.Click += new System.EventHandler(this.lblGCodeFont_Click);
			// 
			// lblColor
			// 
			this.lblColor.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.lblColor.Location = new System.Drawing.Point(33, 36);
			this.lblColor.Name = "lblColor";
			this.lblColor.Size = new System.Drawing.Size(117, 23);
			this.lblColor.TabIndex = 0;
			this.lblColor.Text = "界面颜色";
			this.lblColor.Click += new System.EventHandler(this.LblColorClick);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(387, 37);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(30, 30);
			this.btnAdd.TabIndex = 19;
			this.btnAdd.Text = "+";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnSub
			// 
			this.btnSub.Location = new System.Drawing.Point(423, 37);
			this.btnSub.Name = "btnSub";
			this.btnSub.Size = new System.Drawing.Size(30, 30);
			this.btnSub.TabIndex = 19;
			this.btnSub.Text = "-";
			this.btnSub.UseVisualStyleBackColor = true;
			this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.btnSub);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.btnAdd);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.txtMoveRange);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.txtPlayRate);
			this.groupBox2.Controls.Add(this.txtASpeed);
			this.groupBox2.Controls.Add(this.txtFlashSpan);
			this.groupBox2.Controls.Add(this.txtBeamOnTime);
			this.groupBox2.Controls.Add(this.txtCycleTimes);
			this.groupBox2.Controls.Add(this.txtBeamOffTime);
			this.groupBox2.Controls.Add(this.txtLoopGap);
			this.groupBox2.Location = new System.Drawing.Point(21, 15);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(864, 320);
			this.groupBox2.TabIndex = 20;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "运行参数";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.groupBox4);
			this.groupBox3.Controls.Add(this.label13);
			this.groupBox3.Controls.Add(this.txtDefaultMoveSpeed);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Controls.Add(this.txtDefaultCutSpeed);
			this.groupBox3.Controls.Add(this.lblCutRatioName);
			this.groupBox3.Controls.Add(this.txtCutSpeedRatio);
			this.groupBox3.Location = new System.Drawing.Point(23, 21);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(845, 343);
			this.groupBox3.TabIndex = 17;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "测试";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.Controls.Add(this.txtPID_D);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Controls.Add(this.txtPID_I);
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Controls.Add(this.txtPID_P);
			this.groupBox4.Location = new System.Drawing.Point(24, 75);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(393, 206);
			this.groupBox4.TabIndex = 16;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "PID";
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label11.Location = new System.Drawing.Point(29, 156);
			this.label11.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(134, 26);
			this.label11.TabIndex = 13;
			this.label11.Text = "Differential";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label11.Click += new System.EventHandler(this.LblCutRatioNameClick);
			// 
			// txtPID_D
			// 
			this.txtPID_D.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtPID_D.Location = new System.Drawing.Point(173, 154);
			this.txtPID_D.Margin = new System.Windows.Forms.Padding(5);
			this.txtPID_D.Name = "txtPID_D";
			this.txtPID_D.Size = new System.Drawing.Size(67, 30);
			this.txtPID_D.TabIndex = 15;
			this.txtPID_D.Text = "1.0";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label10.Location = new System.Drawing.Point(29, 99);
			this.label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(134, 26);
			this.label10.TabIndex = 13;
			this.label10.Text = "Integral ";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label10.Click += new System.EventHandler(this.LblCutRatioNameClick);
			// 
			// txtPID_I
			// 
			this.txtPID_I.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtPID_I.Location = new System.Drawing.Point(173, 97);
			this.txtPID_I.Margin = new System.Windows.Forms.Padding(5);
			this.txtPID_I.Name = "txtPID_I";
			this.txtPID_I.Size = new System.Drawing.Size(67, 30);
			this.txtPID_I.TabIndex = 15;
			this.txtPID_I.Text = "1.0";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label9.Location = new System.Drawing.Point(29, 42);
			this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(134, 26);
			this.label9.TabIndex = 13;
			this.label9.Text = "Proportion ";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label9.Click += new System.EventHandler(this.LblCutRatioNameClick);
			// 
			// txtPID_P
			// 
			this.txtPID_P.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtPID_P.Location = new System.Drawing.Point(173, 40);
			this.txtPID_P.Margin = new System.Windows.Forms.Padding(5);
			this.txtPID_P.Name = "txtPID_P";
			this.txtPID_P.Size = new System.Drawing.Size(67, 30);
			this.txtPID_P.TabIndex = 15;
			this.txtPID_P.Text = "1.0";
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label13.Location = new System.Drawing.Point(575, 33);
			this.label13.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(134, 26);
			this.label13.TabIndex = 13;
			this.label13.Text = "默认移动速度";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label13.Click += new System.EventHandler(this.LblCutRatioNameClick);
			// 
			// txtDefaultMoveSpeed
			// 
			this.txtDefaultMoveSpeed.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtDefaultMoveSpeed.Location = new System.Drawing.Point(719, 31);
			this.txtDefaultMoveSpeed.Margin = new System.Windows.Forms.Padding(5);
			this.txtDefaultMoveSpeed.Name = "txtDefaultMoveSpeed";
			this.txtDefaultMoveSpeed.Size = new System.Drawing.Size(108, 30);
			this.txtDefaultMoveSpeed.TabIndex = 15;
			this.txtDefaultMoveSpeed.Text = "10000";
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label12.Location = new System.Drawing.Point(293, 33);
			this.label12.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(134, 26);
			this.label12.TabIndex = 13;
			this.label12.Text = "默认切割速度";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label12.Click += new System.EventHandler(this.LblCutRatioNameClick);
			// 
			// txtDefaultCutSpeed
			// 
			this.txtDefaultCutSpeed.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtDefaultCutSpeed.Location = new System.Drawing.Point(437, 31);
			this.txtDefaultCutSpeed.Margin = new System.Windows.Forms.Padding(5);
			this.txtDefaultCutSpeed.Name = "txtDefaultCutSpeed";
			this.txtDefaultCutSpeed.Size = new System.Drawing.Size(114, 30);
			this.txtDefaultCutSpeed.TabIndex = 15;
			this.txtDefaultCutSpeed.Text = "5000";
			// 
			// lblCutRatioName
			// 
			this.lblCutRatioName.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.lblCutRatioName.Location = new System.Drawing.Point(25, 33);
			this.lblCutRatioName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.lblCutRatioName.Name = "lblCutRatioName";
			this.lblCutRatioName.Size = new System.Drawing.Size(134, 26);
			this.lblCutRatioName.TabIndex = 13;
			this.lblCutRatioName.Text = "切割速度系数";
			this.lblCutRatioName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblCutRatioName.Click += new System.EventHandler(this.LblCutRatioNameClick);
			// 
			// txtCutSpeedRatio
			// 
			this.txtCutSpeedRatio.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtCutSpeedRatio.Location = new System.Drawing.Point(169, 31);
			this.txtCutSpeedRatio.Margin = new System.Windows.Forms.Padding(5);
			this.txtCutSpeedRatio.Name = "txtCutSpeedRatio";
			this.txtCutSpeedRatio.Size = new System.Drawing.Size(67, 30);
			this.txtCutSpeedRatio.TabIndex = 15;
			this.txtCutSpeedRatio.Text = "1.0";
			// 
			// tabControl2
			// 
			this.tabControl2.Controls.Add(this.tabPage1);
			this.tabControl2.Controls.Add(this.tabPage3);
			this.tabControl2.Controls.Add(this.tabUI);
			this.tabControl2.Location = new System.Drawing.Point(12, 12);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size(879, 418);
			this.tabControl2.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Location = new System.Drawing.Point(4, 30);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(871, 384);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "控制";
			// 
			// tabPage3
			// 
			this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage3.Controls.Add(this.groupBox3);
			this.tabPage3.Location = new System.Drawing.Point(4, 30);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(871, 384);
			this.tabPage3.TabIndex = 1;
			this.tabPage3.Text = "测试";
			// 
			// tabUI
			// 
			this.tabUI.BackColor = System.Drawing.SystemColors.Control;
			this.tabUI.Controls.Add(this.groupBox1);
			this.tabUI.Location = new System.Drawing.Point(4, 30);
			this.tabUI.Name = "tabUI";
			this.tabUI.Padding = new System.Windows.Forms.Padding(3);
			this.tabUI.Size = new System.Drawing.Size(871, 384);
			this.tabUI.TabIndex = 2;
			this.tabUI.Text = "界面";
			// 
			// SettingForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(911, 540);
			this.Controls.Add(this.tabControl2);
			this.Controls.Add(this.btnConfirm);
			this.Controls.Add(this.btnOK);
			this.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.Margin = new System.Windows.Forms.Padding(5);
			this.Name = "SettingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "操控面板";
			this.Load += new System.EventHandler(this.SettingFormLoad);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.tabControl2.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabUI.ResumeLayout(false);
			this.ResumeLayout(false);

		}
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSub;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblCutRatioName;
        private System.Windows.Forms.TextBox txtCutSpeedRatio;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtPID_D;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPID_I;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPID_P;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabUI;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtDefaultMoveSpeed;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtDefaultCutSpeed;
        private System.Windows.Forms.Label lblGCodeFont;
        private System.Windows.Forms.Label lblLogFont;
        private System.Windows.Forms.CheckBox ckLaserCircle;
    }
}
