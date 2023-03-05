
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
            this.canvasView = new Painter.View.CanvasView.CanvasView();
            this.SuspendLayout();
            // 
            // canvasView
            // 
            this.canvasView.BackColor = System.Drawing.Color.White;
            this.canvasView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasView.Location = new System.Drawing.Point(0, 0);
            this.canvasView.Name = "canvasView";
            this.canvasView.Size = new System.Drawing.Size(1197, 974);
            this.canvasView.TabIndex = 0;
            // 
            // CorrugatedSheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1197, 974);
            this.Controls.Add(this.canvasView);
            this.Name = "CorrugatedSheetForm";
            this.Text = "CorrugatedSheet";
            this.Load += new System.EventHandler(this.CorrugatedSheetForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private View.CanvasView.CanvasView canvasView;
    }
}