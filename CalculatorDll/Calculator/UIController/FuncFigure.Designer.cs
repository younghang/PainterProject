﻿namespace CalculatorDll.Calculator.UIController
{
    partial class FuncFigure
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
            this.SuspendLayout();
            // 
            // FuncFigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 606);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FuncFigure";
            this.Text = "FuncFigure";
            this.Load += new System.EventHandler(this.FuncFigure_Load_1);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FuncFormPaint);
            this.ResumeLayout(false);

        }

        #endregion
    }
}