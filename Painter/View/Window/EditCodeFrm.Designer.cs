
namespace Painter.View.Window
{
    partial class EditCodeFrm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPerimeter = new System.Windows.Forms.Label();
            this.lblEndPos = new System.Windows.Forms.Label();
            this.lblStartPos = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.shapePreview1 = new Painter.View.Window.UC.ShapePreview();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(29, 431);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(1022, 67);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtCode
            // 
            this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCode.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(0, 0);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(639, 213);
            this.txtCode.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 22);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.lblPerimeter);
            this.splitContainer1.Panel2.Controls.Add(this.lblEndPos);
            this.splitContainer1.Panel2.Controls.Add(this.lblStartPos);
            this.splitContainer1.Size = new System.Drawing.Size(1069, 379);
            this.splitContainer1.SplitterDistance = 639;
            this.splitContainer1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(3, 128);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(423, 248);
            this.panel1.TabIndex = 1;
            // 
            // lblPerimeter
            // 
            this.lblPerimeter.AutoSize = true;
            this.lblPerimeter.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPerimeter.Location = new System.Drawing.Point(30, 105);
            this.lblPerimeter.Name = "lblPerimeter";
            this.lblPerimeter.Size = new System.Drawing.Size(49, 20);
            this.lblPerimeter.TabIndex = 0;
            this.lblPerimeter.Text = "长度";
            // 
            // lblEndPos
            // 
            this.lblEndPos.AutoSize = true;
            this.lblEndPos.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblEndPos.Location = new System.Drawing.Point(30, 67);
            this.lblEndPos.Name = "lblEndPos";
            this.lblEndPos.Size = new System.Drawing.Size(49, 20);
            this.lblEndPos.TabIndex = 0;
            this.lblEndPos.Text = "终点";
            // 
            // lblStartPos
            // 
            this.lblStartPos.AutoSize = true;
            this.lblStartPos.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStartPos.Location = new System.Drawing.Point(30, 29);
            this.lblStartPos.Name = "lblStartPos";
            this.lblStartPos.Size = new System.Drawing.Size(49, 20);
            this.lblStartPos.TabIndex = 0;
            this.lblStartPos.Text = "起点";
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
            this.splitContainer2.Panel1.Controls.Add(this.txtCode);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.shapePreview1);
            this.splitContainer2.Size = new System.Drawing.Size(639, 379);
            this.splitContainer2.SplitterDistance = 213;
            this.splitContainer2.TabIndex = 0;
            // 
            // shapePreview1
            // 
            this.shapePreview1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shapePreview1.Location = new System.Drawing.Point(0, 0);
            this.shapePreview1.Name = "shapePreview1";
            this.shapePreview1.Size = new System.Drawing.Size(639, 162);
            this.shapePreview1.TabIndex = 0;
            // 
            // EditCodeFrm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 510);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnOK);
            this.Name = "EditCodeFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditCodeFrm";
            this.Load += new System.EventHandler(this.EditCodeFrm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblEndPos;
        private System.Windows.Forms.Label lblStartPos;
        private System.Windows.Forms.Label lblPerimeter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private UC.ShapePreview shapePreview1;
    }
}