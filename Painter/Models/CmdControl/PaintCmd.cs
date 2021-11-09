using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.Models.CmdControl
{
    public class CommandMgr
    {
        public CommandMgr(CanvasModel model)
        {
            this.canvas = model;
        }
        List<Command> cmds = new List<Command>();
        public void Excute()
        {
            foreach (var item in cmds)
            {
                //item.Excute();
            }
        }
        private CanvasModel canvas;
        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (CurCmd == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                if (CurCmd.Status == CMD_STATUS.DONE)
                {
                    //CurCmd = null;
                    return;
                }
                CurCmd.Excute_MouseUpdate();
                CurCmd.MoveNext_MouseDown();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (CurCmd != null)
                {
                    CurCmd.End();
                }
            }
        }
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (CurCmd != null)
                {
                    CurCmd.End();
                }
            }
        }
        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (CurCmd == null)
            {
                return;
            }
            if (CurCmd.Status == CMD_STATUS.DONE)
            {
                //CurCmd = null;
                //return;
            }
            CurCmd.Excute_MouseUpdate();
        }

        private Command CurCmd;
        private int curIndex = 0;
        public void AddCmd(Command cmd)
        {
            if (this.cmds.Contains(cmd) == false)
            {
                for (int i = this.cmds.Count - 1; i >= 0; i--)
                {
                    if (i > curIndex)
                    {
                        this.cmds.RemoveAt(i);
                    }
                }
                if (CurCmd != null && CurCmd.Status != CMD_STATUS.DONE)
                {
                    if (CurCmd.Status == CMD_STATUS.RUNNING)
                    {
                        Undo();
                    }
                    else
                    {
                        CurCmd.Status = CMD_STATUS.DONE;
                    }
                }
                this.cmds.Add(cmd);
                this.curIndex++;
                this.CurCmd = cmd;
            }
        }
        public void Undo()
        {
            if (curIndex > 0)
            { 
                if (CurCmd != null)
                {
                    CurCmd.Cancel();
                    if (CurCmd.Status==CMD_STATUS.DONE)
                    { 
                        curIndex--;
                        if (curIndex>0)
                        {
                            CurCmd = this.cmds[curIndex];
                        }else
                        {
                            CurCmd = null;
                        }
                       
                    }
                } 
               
            }
        }
        public void Redo()
        {
            if (CurCmd != null && CurCmd.Status != CMD_STATUS.DONE)
            {
                CurCmd.Cancel();
            }
            if (curIndex <  this.cmds.Count)
            {
                CurCmd = this.cmds[curIndex];
                if (CurCmd.Status == CMD_STATUS.DONE)
                {
                    CurCmd.Resume();
                }
                curIndex++;

            }
        }
    }
    public enum CMD_STATUS { SLEEP, RUNNING, DONE,ABORT }
    public class Command
    {
        public Command(CanvasModel model)
        {
            this.canvas = model;
        }
        protected CanvasModel canvas = null;
        public virtual void Excute_MouseUpdate()
        {
        }
        public virtual void Update()
        {
        }
        public virtual void MoveNext_MouseDown()
        {
            this.Status = CMD_STATUS.RUNNING;
        }
        public virtual void MovePrev()
        {

        }
        public virtual void Cancel()
        {

        }
        public virtual void End()
        {

        }
        public virtual void Resume()
        {

        }
        public CMD_STATUS Status = CMD_STATUS.SLEEP;
    }
    public class DeleteCmd : Command
    {

        public DeleteCmd(CanvasModel model, List<DrawableObject> ls) : base(model)
        {
            this.curSelectObjs = ls;
        }
        List<DrawableObject> curSelectObjs = new List<DrawableObject>();
        static List<DrawableObject> deletedObjs = new List<DrawableObject>();
        public override void Cancel()
        {
            foreach (var item in deletedObjs)
            {
                item.IsDisposed = false;
                canvas.GetFreshLayerManager().Add(item);
            }
            deletedObjs.Clear();
            this.Status = CMD_STATUS.DONE;
        }
        public override void Resume()
        {

        }
        public override void End()
        {
            this.Status = CMD_STATUS.DONE;
        }
        public override void MoveNext_MouseDown()
        {
            foreach (var item in this.curSelectObjs)
            {
                item.IsDisposed = true;
                deletedObjs.Add(item);
            }
        }

        public override void Excute_MouseUpdate()
        {

        }
    }
    public class ClearCmd : Command
    {
        private bool isInProcessing = false;

        public ClearCmd(CanvasModel model) : base(model)
        {
        }
        public override void Excute_MouseUpdate()
        {
            if (isInProcessing)
            {
                return;
            }
            isInProcessing = true;
            Status = CMD_STATUS.DONE;
            canvas.GetFreshLayerManager().Clear();
        }
    }
    public class SaveOrLoadCmd : Command
    {
        private bool IsSave = true;
        public SaveOrLoadCmd(CanvasModel model, bool isSave = true) : base(model)
        {
            IsSave = isSave;
        }
        private bool isInProcessing = false;
        public override void Excute_MouseUpdate()
        {
            if (isInProcessing)
            {
                return;
            }
            isInProcessing = true;
            Status = CMD_STATUS.DONE;
            canvas.GetFreshLayerManager().SaveOrLoadFile(IsSave);
        }
    }
    public class GeoCmd : Command
    {
        public GeoCmd(CanvasModel model) : base(model)
        {
        }
        protected DrawableObject shape;
        public override void Cancel()
        {
            this.shape.IsDisposed = true;
            this.Status = CMD_STATUS.DONE;
        }
    }
    public class GeoTwoPointCmd : GeoCmd
    {
        public GeoTwoPointCmd(CanvasModel model) : base(model)
        {
            process = LINE_PROCESS.FIRST_POINT;
        }

        public GeoTwoPointCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {

        }

        public enum LINE_PROCESS { START, FIRST_POINT, SECOND_POINT, END }
        public LINE_PROCESS process = LINE_PROCESS.START;
        public override void MoveNext_MouseDown()
        {
            this.process = this.process + 1;
        }

        public override void Resume()
        {
            this.shape.IsDisposed = false;
            this.canvas.GetFreshLayerManager().Add(shape, true);
        }
        public override void Excute_MouseUpdate()
        {
            PointGeo point = canvas.CurObjectPoint;
            switch (this.process)
            {
                case LINE_PROCESS.START: 
                    break;
                case LINE_PROCESS.FIRST_POINT:
                    (shape as Shape).FirstPoint = point.Clone();
                    //MoveNext();
                    break;
                case LINE_PROCESS.SECOND_POINT:
                    (shape as Shape).SecondPoint = point.Clone();
                    canvas.GetFreshLayerManager().Add(shape, true);
                    break;
                case LINE_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
        }
    }

    public class LineCmd : GeoTwoPointCmd
    {

        public LineCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.shape = new LineGeo();
            this.shape.SetDrawMeta(meta);
        }

    }
    public class CircleCmd : GeoTwoPointCmd
    {
        public CircleCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.shape = new CircleGeo();
            this.shape.SetDrawMeta(meta);
        }

    }

    public class ArcCmd : Command
    {
        public override void Cancel()
        {
            if (this.Status!=CMD_STATUS.DONE)
            {
                this.process = this.process - 1;
                if (this.process == ARC_PROCESS.START)
                {
                    this.arc.IsDisposed = true;
                    this.Status = CMD_STATUS.DONE;
                }
            }else
            {
                this.arc.IsDisposed = true;
                this.Status = CMD_STATUS.DONE;
            } 

        }
        public ArcCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.arc.SetDrawMeta(meta);
            process = ARC_PROCESS.FIRST_POINT;
        }

        public enum ARC_PROCESS { START, FIRST_POINT, SECOND_POINT, THIRD_POINT, END }
        public ARC_PROCESS process = ARC_PROCESS.START;
        public override void MoveNext_MouseDown()
        {
            this.process = this.process + 1;
        }
        private ArcGeo arc = new ArcGeo();
        public override void Resume()
        {
            //this.arc.IsDisposed = false;
            //this.canvas.GetFreshLayerManager().Add(arc);
        }
        public override void Excute_MouseUpdate()
        {
            PointGeo point = canvas.CurObjectPoint;
            switch (this.process)
            {
                case ARC_PROCESS.START:

                    break;
                case ARC_PROCESS.FIRST_POINT:
                    arc.FirstPoint = point.Clone();
                    //MoveNext();
                    break;
                case ARC_PROCESS.SECOND_POINT:
                    arc.SecondPoint = point.Clone();
                    canvas.GetFreshLayerManager().Add(arc, true);
                    break;
                case ARC_PROCESS.THIRD_POINT:
                    arc.ThirdPoint = point.Clone();

                    break;
                case ARC_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
        }
    }
    public class RectangleCmd : GeoTwoPointCmd
    {
        public RectangleCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.shape = new RectangleGeo();
            this.shape.SetDrawMeta(meta);
        }
    }
    public class EllipseCmd : GeoTwoPointCmd
    {
        public EllipseCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.shape = new EllipseGeo();
            this.shape.SetDrawMeta(meta);
        }
    }
    public class CurveCmd : PolygonCmd
    {
        public override void Cancel()
        {
            if (this.Status != CMD_STATUS.DONE)
            {
                this.poly.RemovePoint();
                if (this.poly.points.Count == 2)
                {
                    this.poly.IsDisposed = true;
                    this.Status = CMD_STATUS.DONE;
                }
            }else
            {
                this.poly.IsDisposed = true;
                this.Status = CMD_STATUS.DONE;
            }
            
        }
        public CurveCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            poly = new CurveGeo();
            this.poly.SetDrawMeta(meta);
        }
        public override void MoveNext_MouseDown()
        {
            if (this.process == POLYGON_PROCESS.START)
            {
                this.process = this.process + 1;
                return;
            }
            PointGeo point = canvas.CurObjectPoint;
            poly.AddPoint(point.Clone());
            if (this.poly.points.Count < 2)
            {
                return;
            }
            canvas.GetFreshLayerManager().Add(poly);
        }
    }
    public class PolygonCmd : GeoCmd
    {
        public override void End()
        {
            this.process = POLYGON_PROCESS.END;
        }
        public override void Cancel()
        {
            if (this.Status != CMD_STATUS.DONE)
            {
                this.poly.RemovePoint();
                if (this.poly.points.Count == 3)
                {
                    this.poly.IsDisposed = true;
                    this.Status = CMD_STATUS.DONE;
                }
            }
            else
            {
                this.poly.IsDisposed = true;
                this.Status = CMD_STATUS.DONE;
            }
           
        }
        public override void Resume()
        {
            this.poly.IsDisposed = false;
            this.canvas.GetFreshLayerManager().Add(poly);
        }
        public PolygonCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            poly = new PolygonGeo();
            this.poly.SetDrawMeta(meta);
        }
        protected PolygonCmd(CanvasModel model) : base(model)
        {
            process = POLYGON_PROCESS.POINT;
        }

        public enum POLYGON_PROCESS { START, POINT, END }
        public POLYGON_PROCESS process = POLYGON_PROCESS.START;
        public override void MoveNext_MouseDown()
        {
            if (this.process == POLYGON_PROCESS.START)
            {
                this.process = this.process + 1;
                return;
            }
            PointGeo point = canvas.CurObjectPoint;
            poly.AddPoint(point.Clone());
            if (this.poly.points.Count < 3)
            {
                return;
            }
            canvas.GetFreshLayerManager().Add(poly);
        }
        protected PolygonGeo poly;
        public override void Excute_MouseUpdate()
        {

            switch (this.process)
            {
                case POLYGON_PROCESS.START:
                    break;
                case POLYGON_PROCESS.POINT:
                    break;
                case POLYGON_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
        }
    }
}
