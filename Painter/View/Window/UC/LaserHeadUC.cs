using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Painter.Models;
using System.IO;

namespace Painter.View.Window.UC
{
    public partial class LaserHeadUC : UserControl
    {
        public LaserHeadUC()
        {
            InitializeComponent();
            this.NCInfo.IsShow=true;
            lblColor.BackColor = new Color[] {Color.AliceBlue, Color.Purple,   Color.White,Color.SkyBlue, Color.SteelBlue }[new Random(System.DateTime.Now.Millisecond).Next(0, 5)];
        }

        private NCFileInfo NCInfo = new NCFileInfo();
        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            string fileFullName = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                fileFullName = openFileDialog.FileName;
                if (!File.Exists(fileFullName))
                {
                    return;
                }
                this.NCInfo.FilePath = fileFullName;
                this.NCInfo.FillColor = lblColor.BackColor;
                this.lblPath.Text = fileFullName;
                this.txtName.Text = new FileInfo(fileFullName).Name;
            }
        }
        public NCFileInfo GetNCFileInfo()
        {
            return this.NCInfo;
        }
        private void lblColor_Click(object sender, EventArgs e)
        {
            Label l = sender as Label;
            ColorDialog colorDialog = new ColorDialog();
            DialogResult dialogResult = colorDialog.ShowDialog(); 
            Color color = new Color();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                colorDialog.Dispose();
                color = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                this.NCInfo.FillColor = color;
                l.BackColor = color;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            this.NCInfo.Name = this.txtName.Text.Trim();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.NCInfo.IsShow = this.checkBox1.Checked;
        }
        public event Action OnDelete;
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OnDelete!=null)
            {
                OnDelete();
            }
        }

        private void LaserHeadUC_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(new System.Drawing.Point(this.Parent.Parent.Location.X+this.Location.X +  e.Location.X, this.Parent.Parent.Location.Y+this.Location.Y + e.Location.Y +  30));
            }
        }
    }
}
