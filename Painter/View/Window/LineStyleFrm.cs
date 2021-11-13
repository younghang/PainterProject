using Painter.TempTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.View.Window
{
    public partial class LineStyleFrm : Form
    {
        private Array lineCapStyle = Enum.GetValues(typeof(LineCap));
        public float LineWidth = 2;
        public float[] lineDash=null;
        public LineCap lineCap = LineCap.Flat; 
        public LineStyleFrm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.Apply();
        }
        public void Apply()
        {
            switch (this.comboLineStyle.SelectedIndex)
            {
                case 0:
                    this.lineDash = null;
                    break;
                case 1:
                    this.lineDash = new float[] { 1,1,1};
                    break;
                case 2:
                    List<float> t = new List<float>();
                    var ss=txtLineDash.Text.Split(new char[] { ';',',' });
                    foreach (var item in ss)
                    {
                        t.Add(float.Parse(item));
                    }
                    this.lineDash =t.ToArray();
                    break;
                default:
                    break;
            }
            float.TryParse(txtLineWidth.Text,out this.LineWidth);
            this.lineCap=(LineCap)lineCapStyle.GetValue(comboLineCap.SelectedIndex);
            PainterTest.CurShapeMeta.LineWidth = this.LineWidth;
            PainterTest.CurShapeMeta.CapStyle = this.lineCap;
            PainterTest.CurShapeMeta.DashLineStyle = this.lineDash;  
        }

        private void LineStyleFrm_Load(object sender, EventArgs e)
        {
            txtLineWidth.Text = PainterTest.CurShapeMeta.LineWidth.ToString("f3");
            comboLineCap.DataSource = Enum.GetNames(typeof(LineCap));
        }
    }
}
