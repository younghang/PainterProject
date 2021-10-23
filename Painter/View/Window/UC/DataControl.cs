/*
 * Created by SharpDevelop.
 * User: hang
 * Date: 2021/9/16
 * Time: 21:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Painter.View.Window.UC
{
	/// <summary>
	/// Description of DataControl.
	/// </summary>
	public partial class DataControl : UserControl
	{
        private string txtIn = "B11";
        public string TxtIn { get { return txtIn; } 
            set {
                txtIn = value; this.label1.Text = txtIn;
                this.Invalidate();
            } }
        public DataControl()
		{
       
        //
        // The InitializeComponent() call is required for Windows Forms designer support.
        //
        InitializeComponent();
			InitializeComponent();
            this.Load += (o,e) => {
                this.btnAdd.Visible = this.IsShowEdit;
                this.btnSub.Visible = this.IsShowEdit;
               // this.lblName.Text=DataName;
            };
//            this.SizeChanged+=(o,e)=>{
//            	this.Size=new Size(340, 35);
//            };
			 
            IsShowEdit = true;
            
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		public bool IsShowEdit { get; set; }
        private double dataValue=3;
        public new string Text
        {
            get { return GetValue().ToString("f3"); }
            set { SetValue(double.Parse(value)); }
        }
        [Category("自定义"), Browsable(true), Description("名称")]
        public string DataName { get { return this.lblName.Text.Trim(); } set { this.lblName.Text = value; } }
        public double GetValue()
        {
            return this.dataValue;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.dataValue *= 1.12;
            this.txtValue.Text = this.dataValue.ToString("f3");
        }
        private void SetValue(double value)
        {
            this.dataValue = value;
            this.txtValue.Text = this.dataValue.ToString("f3");
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            this.dataValue *= 0.88;
            this.txtValue.Text = this.dataValue.ToString("f3");
        }
    }
}
