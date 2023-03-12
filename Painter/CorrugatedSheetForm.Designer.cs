
namespace Painter
{
    partial class CorrugatedSheetForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtStartNum = new System.Windows.Forms.NumericUpDown();
            this.txtLineMargin = new System.Windows.Forms.NumericUpDown();
            this.txtLineLength = new System.Windows.Forms.NumericUpDown();
            this.txtSlotWidth = new System.Windows.Forms.NumericUpDown();
            this.txtSlotHeight = new System.Windows.Forms.NumericUpDown();
            this.txtLineCount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.OriX = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.OriY = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.canvasView = new Painter.View.CanvasView.CanvasView();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlotWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlotHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OriX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OriY)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.txtStartNum);
            this.groupBox1.Controls.Add(this.txtLineMargin);
            this.groupBox1.Controls.Add(this.txtLineLength);
            this.groupBox1.Controls.Add(this.txtSlotWidth);
            this.groupBox1.Controls.Add(this.txtSlotHeight);
            this.groupBox1.Controls.Add(this.txtLineCount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 721);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1197, 253);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数配置";
            // 
            // txtStartNum
            // 
            this.txtStartNum.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStartNum.Location = new System.Drawing.Point(205, 199);
            this.txtStartNum.Name = "txtStartNum";
            this.txtStartNum.Size = new System.Drawing.Size(200, 39);
            this.txtStartNum.TabIndex = 3;
            this.txtStartNum.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // txtLineMargin
            // 
            this.txtLineMargin.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLineMargin.Location = new System.Drawing.Point(207, 148);
            this.txtLineMargin.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtLineMargin.Name = "txtLineMargin";
            this.txtLineMargin.Size = new System.Drawing.Size(200, 39);
            this.txtLineMargin.TabIndex = 3;
            this.txtLineMargin.Value = new decimal(new int[] {
            86,
            0,
            0,
            0});
            // 
            // txtLineLength
            // 
            this.txtLineLength.DecimalPlaces = 2;
            this.txtLineLength.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLineLength.Location = new System.Drawing.Point(207, 95);
            this.txtLineLength.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.txtLineLength.Name = "txtLineLength";
            this.txtLineLength.Size = new System.Drawing.Size(200, 39);
            this.txtLineLength.TabIndex = 3;
            this.txtLineLength.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // txtSlotWidth
            // 
            this.txtSlotWidth.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSlotWidth.Location = new System.Drawing.Point(657, 86);
            this.txtSlotWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtSlotWidth.Name = "txtSlotWidth";
            this.txtSlotWidth.Size = new System.Drawing.Size(200, 39);
            this.txtSlotWidth.TabIndex = 3;
            this.txtSlotWidth.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // txtSlotHeight
            // 
            this.txtSlotHeight.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSlotHeight.Location = new System.Drawing.Point(657, 29);
            this.txtSlotHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtSlotHeight.Name = "txtSlotHeight";
            this.txtSlotHeight.Size = new System.Drawing.Size(200, 39);
            this.txtSlotHeight.TabIndex = 3;
            this.txtSlotHeight.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // txtLineCount
            // 
            this.txtLineCount.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLineCount.Location = new System.Drawing.Point(207, 38);
            this.txtLineCount.Name = "txtLineCount";
            this.txtLineCount.Size = new System.Drawing.Size(200, 39);
            this.txtLineCount.TabIndex = 3;
            this.txtLineCount.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(41, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 33);
            this.label2.TabIndex = 1;
            this.label2.Text = "起始序号";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(41, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 33);
            this.label4.TabIndex = 1;
            this.label4.Text = "焊条间距";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(41, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 33);
            this.label3.TabIndex = 1;
            this.label3.Text = "焊条长度";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(476, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 33);
            this.label6.TabIndex = 1;
            this.label6.Text = "气槽长度";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(41, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "焊条数量";
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.Location = new System.Drawing.Point(918, 169);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(223, 58);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGenerate.Location = new System.Drawing.Point(918, 91);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(223, 58);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "生成";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.OriY);
            this.groupBox2.Controls.Add(this.OriX);
            this.groupBox2.Location = new System.Drawing.Point(471, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(386, 116);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "起点";
            // 
            // OriX
            // 
            this.OriX.DecimalPlaces = 2;
            this.OriX.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OriX.Location = new System.Drawing.Point(40, 44);
            this.OriX.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.OriX.Name = "OriX";
            this.OriX.Size = new System.Drawing.Size(129, 39);
            this.OriX.TabIndex = 4;
            this.OriX.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(476, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 33);
            this.label5.TabIndex = 1;
            this.label5.Text = "气槽宽度";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(3, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 33);
            this.label7.TabIndex = 5;
            this.label7.Text = "X";
            // 
            // OriY
            // 
            this.OriY.DecimalPlaces = 2;
            this.OriY.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OriY.Location = new System.Drawing.Point(238, 44);
            this.OriY.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.OriY.Name = "OriY";
            this.OriY.Size = new System.Drawing.Size(129, 39);
            this.OriY.TabIndex = 4;
            this.OriY.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(201, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 33);
            this.label8.TabIndex = 5;
            this.label8.Text = "Y";
            // 
            // canvasView
            // 
            this.canvasView.BackColor = System.Drawing.Color.White;
            this.canvasView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasView.Location = new System.Drawing.Point(0, 0);
            this.canvasView.Name = "canvasView";
            this.canvasView.Size = new System.Drawing.Size(1197, 721);
            this.canvasView.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(918, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(223, 58);
            this.button1.TabIndex = 0;
            this.button1.Text = "测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CorrugatedSheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1197, 974);
            this.Controls.Add(this.canvasView);
            this.Controls.Add(this.groupBox1);
            this.Name = "CorrugatedSheetForm";
            this.Text = "CorrugatedSheet";
            this.Load += new System.EventHandler(this.CorrugatedSheetForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlotWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlotHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OriX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OriY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private View.CanvasView.CanvasView canvasView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown txtStartNum;
        private System.Windows.Forms.NumericUpDown txtLineCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.NumericUpDown txtLineMargin;
        private System.Windows.Forms.NumericUpDown txtLineLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown txtSlotHeight;
        private System.Windows.Forms.NumericUpDown txtSlotWidth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown OriY;
        private System.Windows.Forms.NumericUpDown OriX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
    }
}