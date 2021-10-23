using Painter.Controller;
using Painter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Painter.Painters;
using Painter.View.CanvasView;

namespace Painter.DisplayManger
{
    class PainterUCManager
    { 
    	public static bool IsYUpDirection=false;
        public static PainterUCManager Instance = new PainterUCManager();
        private PainterUCManager()
        {

        } 
        private List<IPainterUC> painterUCs = new List<IPainterUC>();
        public void AddPainterUC(IPainterUC painterUC)
        {
            if (!this.painterUCs.Contains(painterUC))
            {
                this.painterUCs.Add(painterUC);
            }
        }
        public List<IPainterUC> GetPainterUCs()
        {
            return this.painterUCs;
        }
        public void Clear()
        {
            foreach (var item in this.painterUCs)
            {
                item.GetFreshLayerManager().Clear();
                item.GetHoldLayerManager().Clear();
                item.GetMoveableLayerManager().Clear();
            }
        }
        public void HorizontalOffSetContent(double x)
        {
            foreach (var item in this.painterUCs)
            {
                item.GetMoveableLayerManager().OffsetShapes((float)x, 0);
            }
        }
        public void SetCanvasScale(double width,double height)
        {
            //先缩放后平移
            PointGeo offsetPoint;
            PointGeo scaleP;
            float ratioX = (float)((width *0.9) * 1.0f / (ProgramController.Instance.MaxX + Settings.MOVE_RANGE - ProgramController.Instance.MinX));
            float ratioY = (float)((height * 0.8f) / (ProgramController.Instance.MaxY - ProgramController.Instance.MinY));
            float ratio = Math.Min(ratioY, ratioX);
            if (IsYUpDirection)
            {
                scaleP = new PointGeo(ratio, -ratio);//(this.winFormUC1.GetShapeLayerManager().GetPainter() as WinFormPainter).Scale = new PointGeo(ratio, ratio);
            }
            else
            {
                scaleP = new PointGeo(ratio, ratio);//(this.winFormUC1.GetShapeLayerManager().GetPainter() as WinFormPainter).Scale = new PointGeo(ratio, ratio);
            }
            //if (IsYUpDirection)
            //{
            //    offsetPoint = new PointGeo(100, (float)height * 0.9f);//(this.winFormUC1.GetShapeLayerManager().GetPainter() as WinFormPainter).OffsetPoint = new PointGeo(100, 0);
            //}
            //else
            //{
            //    offsetPoint = new PointGeo(100, (float)height * 0.1f);//(this.winFormUC1.GetShapeLayerManager().GetPainter() as WinFormPainter).OffsetPoint = new PointGeo(100, 0);
            //}
            if (IsYUpDirection)
            {
                offsetPoint = new PointGeo((float)(-ProgramController.Instance.MinX * scaleP.X +50),  (float)(height -50 - ProgramController.Instance.MinY * scaleP.Y));
            }
            else
            {
                offsetPoint = new PointGeo((float)(-ProgramController.Instance.MinX * scaleP.X + 50), -(float)(ProgramController.Instance.MinY * scaleP.Y - 50));
            }

            foreach (var item in this.painterUCs)
            {
                (item as WinFormCanvas).SetCanvasTranlate(offsetPoint, scaleP);
            }  
        } 
    }
}
