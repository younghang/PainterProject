namespace Painter.View.Window
{
    partial class SelectFileFrm
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
            this.btnConfirm = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addNewCmd = new System.Windows.Forms.ToolStripButton();
            this.DeleteAllCmd = new System.Windows.Forms.ToolStripButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(218, 646);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(310, 74);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewCmd,
            this.DeleteAllCmd});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(766, 38);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // addNewCmd
            // 
            this.addNewCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addNewCmd.Image = global::Painter.Properties.Resources.addNew;
            this.addNewCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addNewCmd.Name = "addNewCmd";
            this.addNewCmd.Size = new System.Drawing.Size(34, 33);
            this.addNewCmd.Text = "添加激光头";
            this.addNewCmd.Click += new System.EventHandler(this.addNewCmd_Click);
            // 
            // DeleteAllCmd
            // 
            this.DeleteAllCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteAllCmd.Image = global::Painter.Properties.Resources.delete;
            this.DeleteAllCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteAllCmd.Name = "DeleteAllCmd";
            this.DeleteAllCmd.Size = new System.Drawing.Size(34, 33);
            this.DeleteAllCmd.Text = "清空所有";
            this.DeleteAllCmd.Click += new System.EventHandler(this.DeleteAllCmd_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 38);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 36, 3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(766, 592);
            this.flowLayoutPanel1.TabIndex = 11;
            this.flowLayoutPanel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.flowLayoutPanel1_MouseClick);
            // 
            // SelectFileFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(766, 736);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnConfirm);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SelectFileFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SelectFileForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addNewCmd;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStripButton DeleteAllCmd;
    }
}