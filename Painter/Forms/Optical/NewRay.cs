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
    public partial class NewRay : Form
    {
        public NewRay()
        {
            InitializeComponent();
             
            combRayColors.DataSource = Enum.GetNames(typeof(RayLight.RAY_COLOR));
            combRayColors.SelectedIndex = 0;
            ckWave.Checked = false;
        }
        public  RayLight rayLight = null;
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ckWave.Checked)
            {
                float ori_x = 0;
                float ori_y = 0;
                float angle = 0;
                double wave_len = 600;
                float.TryParse(this.txtOriX.Text, out ori_x);
                float.TryParse(this.txtOriY.Text, out ori_y);
                float.TryParse(this.txtAngle.Text, out angle);
                rayLight = new RayLight(new PointGeo(ori_x, ori_y), angle, wave_len);
            }
            else
            {
                float ori_x = 0;
                float ori_y = 0;
                float angle = 0;
                int  color_index = 0;
                float.TryParse(this.txtOriX.Text, out ori_x);
                float.TryParse(this.txtOriY.Text, out ori_y);
                float.TryParse(this.txtAngle.Text, out angle);
                color_index = this.combRayColors.SelectedIndex;
                rayLight = RayLight.CreatRayByColor(new PointGeo(ori_x, ori_y), angle, (RayLight.RAY_COLOR)color_index);
            }
            this.DialogResult = DialogResult.OK;
        
        }

        private void ckWave_CheckedChanged(object sender, EventArgs e)
        {
            if (ckWave.Checked)
            {
                this.combRayColors.Enabled = false;
                this.txtWaveLength.Enabled = true;
            }else
            {
                this.combRayColors.Enabled = true;
                this.txtWaveLength.Enabled = false;

            }
        }
    }
}
