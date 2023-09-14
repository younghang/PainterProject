using Painter.Models;
using Painter.Models.GameModel.SceneModel.OpticalModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.Forms.Optical
{

    public partial class NewLens : Form
    {
      
        public NewLens()
        {
            InitializeComponent();
            this.Load += NewLens_Load;
        }

        private void NewLens_Load(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < MaterialDataBase.Instance.Materials.Count; i++)
            {
                names.Add(MaterialDataBase.Instance.Materials[i].Name);
            }
            this.comboBox1.DataSource = names;
        }

        public LEN_TYPE LenType= LEN_TYPE.POSITIVE;
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
        public LensObject lensObject=null;
        private void btnAdd_Click(object sender, EventArgs e)
        { 
            float ori_x = 0;
            float ori_y = 0;
            float angle = 0;
            float thickness = 0;
            float height = 0;
            float rRadius = 50;
            float lRadius = 50;
            string name = comboBox1.SelectedItem.ToString();

            if (rdArc.Checked)
            {
                LenType = LEN_TYPE.ARC;
            }
            else if (rdMirror.Checked)
            {
                LenType = LEN_TYPE.MIRROR;
            }else if(rdPositive.Checked)
            {
                LenType = LEN_TYPE.POSITIVE;
            }
            else if (rdNegetive.Checked)
            {
                LenType = LEN_TYPE.NEGETIVE;
            }

            float.TryParse(this.txtOriX.Text, out ori_x);
            float.TryParse(this.txtOriY.Text, out ori_y);
            float.TryParse(this.txtAngle.Text, out angle);
            float.TryParse(this.txtThickness.Text, out thickness);
            float.TryParse(this.txtRadius1.Text, out rRadius);
            float.TryParse(this.txtRadius2.Text, out lRadius);
            float.TryParse(this.txtHeight.Text, out height);


            switch (LenType)
            {
                case LEN_TYPE.POSITIVE:
                    rRadius = Math.Abs(rRadius);
                    lRadius = Math.Abs(lRadius);
                    lensObject = new PositiveLens(thickness,rRadius,lRadius,height,new Models.PointGeo(ori_x,ori_y),angle, name); 
                    break;
                case LEN_TYPE.NEGETIVE:
                    rRadius = Math.Abs(rRadius);
                    lRadius = Math.Abs(lRadius);
                    lensObject = new NegtiveLens(thickness, rRadius, lRadius, height, new Models.PointGeo(ori_x, ori_y), angle, name);
                    break;
                case LEN_TYPE.ARC:
                    lensObject = new ArcLens(thickness, rRadius, lRadius, height, new Models.PointGeo(ori_x, ori_y), angle, name);
                    break;
                case LEN_TYPE.MIRROR:
                    PointGeo startp = new PointGeo(ori_x, ori_y);
                    PointGeo endp= startp+ height  *new PointGeo((float)Math.Cos(angle/180*Math.PI), (float)Math.Sin(angle / 180 * Math.PI));
                    lensObject = new MirrorObject(startp,endp, "Mirror");
                    break;
                default:
                    break;
            }
            lensObject.BackColor = LenShapeColor;
            this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; 
        }
        Color LenShapeColor;
        private void NewLens_Load_1(object sender, EventArgs e)
        {

        }

        private void lblColor_Click(object sender, EventArgs e)
        {
            Label l = sender as Label;
            ColorDialog colorDialog = new ColorDialog();
            DialogResult dialogResult = colorDialog.ShowDialog();
            Console.WriteLine(dialogResult.ToString());
            Color color = new Color();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                colorDialog.Dispose();
                color = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                 
                LenShapeColor = color;
               
                l.BackColor = color;
            }
        }
    }
}
