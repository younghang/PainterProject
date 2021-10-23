using Painter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.View.Window
{
    public partial class EditCodeFrm : Form
    {
        private EditCodeFrm()
        {
            this.DoubleBuffered = true;
            InitializeComponent(); 
        }
        public static EditCodeFrm Instance = new EditCodeFrm();
        public string StrCode { get; set; }
        private Shape curShape;
        public Shape CurShape { 
            get { return this.curShape; } 
            set {
                this.curShape = value;
                if (this.curShape != null)
                {
                    this.shapePreview1.SetShape(curShape);
                } 
            } }
        private void btnOK_Click(object sender, EventArgs e)
        {
            StrCode = txtCode.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void EditCodeFrm_Load(object sender, EventArgs e)
        {
            this.txtCode.Text = StrCode;
            
            if (this.CurShape!=null)
            {
                //this.shapePreview1.SetShape(curShape);
                this.lblEndPos.Text ="终点："+ this.CurShape.EndPoint.ToString();
                this.lblStartPos.Text = "起点：" + this.CurShape.StartPoint.ToString();
                this.lblPerimeter.Text = "长度：" + this.CurShape.GetPerimeter().ToString();
                this.panel1.Controls.Clear();
                int MarginSpan= (this.lblEndPos.Top - this.lblStartPos.Bottom);
                if (this.CurShape is LineGeo) 
                {
                    Label lblSlop = new Label();
                    lblSlop.Font = this.lblPerimeter.Font;
                    lblSlop.Text = "斜率：" + ((this.CurShape as LineGeo).GetK()==LineGeo.MAX_SLOP?float.PositiveInfinity.ToString():(this.CurShape as LineGeo).GetK().ToString());
                    panel1.Controls.Add(lblSlop);
                    lblSlop.Top = MarginSpan;
                    lblSlop.Left = this.lblPerimeter.Left; 
                }else if (this.CurShape is ArcGeo)
                {
                    Label lblStartAngel = new Label();
                    lblStartAngel.Font = this.lblPerimeter.Font; 
                    lblStartAngel.Text = "起始角度：" + (this.CurShape as ArcGeo).StartAngle.ToString();
                    panel1.Controls.Add(lblStartAngel); 
                    lblStartAngel.Top = MarginSpan;
                    lblStartAngel.Left = this.lblPerimeter.Left;

                    Label lblEndAngel = new Label();
                    lblEndAngel.Font = this.lblPerimeter.Font; 
                    lblEndAngel.Text = "终止角度：" + (this.CurShape as ArcGeo).EndAngle.ToString();
                    panel1.Controls.Add(lblEndAngel);
                    lblEndAngel.Top = lblStartAngel.Bottom + MarginSpan;
                    lblEndAngel.Left = lblStartAngel.Left;

                    Label lbl圆心X = new Label();
                    lbl圆心X.Font = this.lblPerimeter.Font;
                    lbl圆心X.Text = "圆心X坐标：" + (this.CurShape as ArcGeo).CenterX.ToString();
                    panel1.Controls.Add(lbl圆心X);
                    lbl圆心X.Top = lblEndAngel.Bottom + MarginSpan;
                    lbl圆心X.Left = lblStartAngel.Left;

                    Label lbl圆心Y = new Label();
                    lbl圆心Y.Font = this.lblPerimeter.Font;
                    lbl圆心Y.Text = "圆心Y坐标：" + (this.CurShape as ArcGeo).CenterY.ToString();
                    panel1.Controls.Add(lbl圆心Y);
                    lbl圆心Y.Top = lbl圆心X.Bottom + MarginSpan;
                    lbl圆心Y.Left = lblStartAngel.Left; 

                    Label lbl半径 = new Label();
                    lbl半径.Font = this.lblPerimeter.Font;
                    lbl半径.Text = "半径：" + (this.CurShape as ArcGeo).Radius.ToString();
                    panel1.Controls.Add(lbl半径);
                    lbl半径.Top = lbl圆心X.Bottom + MarginSpan;
                    lbl半径.Left = lblStartAngel.Left;
                }
                foreach (Label item in this.panel1.Controls)
                {
                    item.AutoSize = true;
                }
                this.splitContainer1.Panel2Collapsed = false; 
                this.splitContainer2.Panel2Collapsed = false; 
            }
            else
            {
                this.splitContainer1.Panel2Collapsed = true;
                this.splitContainer2.Panel2Collapsed = true;
            }
        }
    }
}
