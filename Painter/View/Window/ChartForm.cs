 
using Painter.Models;
using Painter.MyUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;
using Utils.DisplayManger.Axis;

namespace Painter
{
    public partial class ChartForm : Form
    {
        public ChartForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;//防止频闪
            this.Load += OnLoad;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.SizeChanged += OnSizeChanged;
            this.Paint += OnPaint;
            this.FormClosing += (o, e) =>
              {
                  if (this.chartModel != null)
                  {
                      this.chartModel.Clear();
                  }
              };
            
        }
        AxisModel chartModel;
        public void SetAxisModel(AxisModel axis)
        {
            this.chartModel = axis;
            chartModel.Invalidate += this.Invalidate;
        }
        void OnLoad(object sender, EventArgs e)
        { 
            BackColor = Color.White;
            if (chartModel==null)
            {
                return; 
            }
            chartModel.OnLoad(sender, e);
        }
        public void AddPoint(string name,PointGeo point)
        {
            if (chartModel == null)
            {
                return;
            }
            chartModel.AddPoint(name,point);
        }
        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (chartModel == null)
            {
                return;
            }
            chartModel.OnSizeChanged(sender, e);
            this.Invalidate();
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (chartModel == null)
            {
                return;
            }
            chartModel.OnPaint(sender, e);
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (chartModel == null)
            {
                return;
            }
            chartModel.OnMouseMove(sender, e);
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (chartModel == null)
            {
                return;
            }
            chartModel.OnMouseUp(sender, e);
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (chartModel == null)
            {
                return;
            }
            chartModel.OnKeyUp(sender, e); 
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (chartModel == null)
            {
                return;
            }
            chartModel.OnKeyDown(sender, e); 
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (chartModel == null)
            {
                return;
            }
            if (e.Button==MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(new Point(e.X+this.Left,e.Y+this.Top+20));
            }
            chartModel.OnMouseDown(sender, e); 
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chartModel.GetData()!=null)
            {
                FileUtils.SavePoints(chartModel.GetData());
            } 
        }
    }
}
