using Painter.Models;
using Painter.View.Window.UC;
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
    public partial class SelectFileFrm : Form
    {
        public static SelectFileFrm Instance = new SelectFileFrm();
        private SelectFileFrm()
        {
            InitializeComponent();
        }
        private int index = 0;
        private List<NCFileInfo> nCFileInfos = new List<NCFileInfo>();
        public List<NCFileInfo> GetNCFileInfos()
        {
            return this.nCFileInfos;
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            nCFileInfos.Clear();
            foreach (var item in this.flowLayoutPanel1.Controls)
            {
                if (item is LaserHeadUC)
                {
                    LaserHeadUC laserHeadUC=item as LaserHeadUC ;
                    NCFileInfo info = laserHeadUC.GetNCFileInfo();
                    info.No = index++;
                    if (info.IsShow)
                    {
                        this.nCFileInfos.Add(info);
                    }
                    
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close(); 
        }
       
        private void addNewCmd_Click(object sender, EventArgs e)
        {
            LaserHeadUC laserHeadUC = new LaserHeadUC();
            laserHeadUC.OnDelete += () => {
                if (laserHeadUC != null&&this.flowLayoutPanel1.Controls.Contains(laserHeadUC))
                {
                    this.flowLayoutPanel1.Controls.Remove(laserHeadUC);
                }
            };
            this.flowLayoutPanel1.Controls.Add(laserHeadUC);
            this.flowLayoutPanel1.AutoScroll = true;
        }

        private void DeleteAllCmd_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Controls.Clear();
            this.nCFileInfos.Clear();
            this.index = 0;
        }
         
        private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    this.contextMenuStrip1.Show(new System.Drawing.Point(this.Location.X+this.flowLayoutPanel1.Left+e.Location.X  ,this.Location.Y+ e.Location.Y+this.flowLayoutPanel1.Top +30 ));
            //}
        }
    }
}
