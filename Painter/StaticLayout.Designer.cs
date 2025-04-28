
namespace Painter
{
    partial class StaticLayout
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
            this.canvasView1 = new Painter.View.CanvasView.CanvasView();
            this.SuspendLayout();
            // 
            // canvasView1
            // 
            this.canvasView1.BackColor = System.Drawing.Color.White;
            this.canvasView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasView1.Location = new System.Drawing.Point(0, 0);
            this.canvasView1.Name = "canvasView1";
            this.canvasView1.Size = new System.Drawing.Size(1359, 923);
            this.canvasView1.TabIndex = 0;
            // 
            // StaticLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1359, 923);
            this.Controls.Add(this.canvasView1);
            this.Name = "StaticLayout";
            this.Text = "StaticLayout";
            this.ResumeLayout(false);

        }

        #endregion

        private View.CanvasView.CanvasView canvasView1;
    }
}