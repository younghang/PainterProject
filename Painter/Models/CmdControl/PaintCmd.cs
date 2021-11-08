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
                if (CurCmd.Status==CMD_STATUS.DONE)
                {
                    CurCmd = null;
                    return;
                } 
                CurCmd.Excute_MouseUpdate();
                CurCmd.MoveNext_MouseDown();
            } 
        }
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
            {
                if (CurCmd!=null)
                {
                    CurCmd.End(); 
                }
            }
        } 
        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (CurCmd==null)
            {
                return;
            }
            if (CurCmd.Status == CMD_STATUS.DONE)
            {
                CurCmd = null;
                return;
            }
            CurCmd.Excute_MouseUpdate();
        }

        private Command CurCmd;
        private int curIndex = 0;
        public void AddCmd(Command cmd)
        {
            if (this.cmds.Contains(cmd)==false)
            {
                for (int i = this.cmds.Count-1; i >=0; i--)
                {
                    if (i>curIndex)
                    {
                        this.cmds.RemoveAt(i);
                    }
                }
                if (CurCmd!=null&&CurCmd.Status!=CMD_STATUS.DONE)
                {
                    if (CurCmd.Status==CMD_STATUS.RUNNING)
                    {
                        Undo();
                    }else
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
            if (curIndex>0)
            {
                curIndex--; 
                CurCmd = this.cmds[curIndex];
                if (CurCmd!=null)
                {
                    CurCmd.Cancel();
                } 
            }
        }
        public void Redo()
        {
            if (CurCmd != null && CurCmd.Status != CMD_STATUS.DONE)
            {
                CurCmd.Cancel();
            }
            if (curIndex < this.cmds.Count)
            { 
                 CurCmd = this.cmds[curIndex];
                if (CurCmd.Status==CMD_STATUS.DONE)
                {
                    CurCmd.Resume();
                }
                curIndex++;
               
            }
        }
    }
    public enum CMD_STATUS { SLEEP, RUNNING, DONE }
    public class Command
    {
        public Command(CanvasModel model)
        {
            this.canvas = model;
        }
        protected CanvasModel canvas = null;
        public virtual void Excute_MouseUpdate( )
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
    public class DeleteCmd: Command
    {

        public DeleteCmd(CanvasModel model,List<DrawableObject> ls) : base(model)
        {
            this.curSelectObjs = ls;
        }
        List<DrawableObject> curSelectObjs=new List<DrawableObject>();
        static List<DrawableObject> deletedObjs=new List<DrawableObject>();
        public override void Cancel()
        {
            foreach (var item in deletedObjs)
            {
                item.IsDisposed = false;
                canvas.GetFreshLayerManager().Add(item);
            }
            deletedObjs.Clear();
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
        public SaveOrLoadCmd(CanvasModel model,bool isSave=true) : base(model)
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
    }

    public class LineCmd: GeoCmd
    {
        public override void Cancel()
        {
            this.line.IsDisposed = true;
        }
        public override void Resume()
        {
            this.line.IsDisposed = false;
            this.canvas.GetFreshLayerManager().Add(line);
        }
        public LineCmd(CanvasModel model,ShapeMeta meta):base(model)
        {
            this.line.SetDrawMeta(meta);
        }
        
        public enum LINE_PROCESS { START,FIRST_POINT,SECOND_POINT,END}
        public LINE_PROCESS process=LINE_PROCESS.START;
        public override void MoveNext_MouseDown()
        {
            this.process = this.process + 1;
        }
        private LineGeo line = new LineGeo(); 
        public override void Excute_MouseUpdate( )
        { 
            PointGeo point = canvas.CurObjectPoint;
            switch (this.process)
            {
                case LINE_PROCESS.START: 
                    break;
                case LINE_PROCESS.FIRST_POINT:
                    line.FirstPoint = point.Clone(); 
                    //MoveNext();
                    break;
                case LINE_PROCESS.SECOND_POINT:
                    line.SecondPoint = point.Clone();
                    canvas.GetFreshLayerManager().Add(line);
                    break;
                case LINE_PROCESS.END:
                    this.Status = CMD_STATUS.DONE; 
                    break;
                default:
                    break;
            }
        }
    }
    public class CircleCmd : GeoCmd
    {
        public override void Cancel()
        {
            this.circle.IsDisposed = true;
        }
        public CircleCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.circle.SetDrawMeta(meta);
        }

        public enum LINE_PROCESS { START, FIRST_POINT, SECOND_POINT, END }
        public LINE_PROCESS process = LINE_PROCESS.START;
        public override void MoveNext_MouseDown()
        {
            this.process = this.process + 1;
        }
        private CircleGeo circle = new CircleGeo();
        public override void Resume()
        {
            this.circle.IsDisposed = false;
            this.canvas.GetFreshLayerManager().Add(circle);
        }
        public override void Excute_MouseUpdate( )
        {
            PointGeo point = canvas.CurObjectPoint;
            switch (this.process)
            {
                case LINE_PROCESS.START:

                    break;
                case LINE_PROCESS.FIRST_POINT:
                    circle.FirstPoint = point.Clone();
                    //MoveNext();
                    break;
                case LINE_PROCESS.SECOND_POINT:
                    circle.SecondPoint = point.Clone();
                    canvas.GetFreshLayerManager().Add(circle);
                    break;
                case LINE_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
        }
    }
    public class ArcCmd : GeoCmd
    {
        public override void Cancel()
        {
            this.arc.IsDisposed = true;
        }
        public ArcCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.arc.SetDrawMeta(meta);
        }

        public enum LINE_PROCESS { START, FIRST_POINT, SECOND_POINT,THIRD_POINT, END }
        public LINE_PROCESS process = LINE_PROCESS.START;
        public override void MoveNext_MouseDown()
        {
            this.process = this.process + 1;
        }
        private ArcGeo arc = new ArcGeo();
        public override void Resume()
        {
            this.arc.IsDisposed = false;
            this.canvas.GetFreshLayerManager().Add(arc);
        }
        public override void Excute_MouseUpdate()
        {
            PointGeo point = canvas.CurObjectPoint;
            switch (this.process)
            {
                case LINE_PROCESS.START:

                    break;
                case LINE_PROCESS.FIRST_POINT:
                    arc.FirstPoint = point.Clone();
                    //MoveNext();
                    break;
                case LINE_PROCESS.SECOND_POINT:
                    arc.SecondPoint = point.Clone(); 
                    canvas.GetFreshLayerManager().Add(arc);
                    break;
                case LINE_PROCESS.THIRD_POINT:
                    arc.ThirdPoint = point.Clone();
                    
                    break;
                case LINE_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
        }
    }
    public class RectangleCmd : GeoCmd
    {
        public override void Cancel()
        {
            this.rectangle.IsDisposed = true;
        }
        public override void Resume()
        {
            this.rectangle.IsDisposed = false;
            this.canvas.GetFreshLayerManager().Add(rectangle);
        }
        public RectangleCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.rectangle.SetDrawMeta(meta);
        }

        public enum LINE_PROCESS { START, FIRST_POINT, SECOND_POINT, END }
        public LINE_PROCESS process = LINE_PROCESS.START;
        public override void MoveNext_MouseDown()
        {
            this.process = this.process + 1;
        }
        private RectangleGeo rectangle = new RectangleGeo();
        public override void Excute_MouseUpdate( )
        {
            PointGeo point = canvas.CurObjectPoint;
            switch (this.process)
            {
                case LINE_PROCESS.START: 
                    break;
                case LINE_PROCESS.FIRST_POINT:
                    rectangle.FirstPoint = point.Clone(); 
                    break;
                case LINE_PROCESS.SECOND_POINT:
                    rectangle.SecondPoint = point.Clone();
                    canvas.GetFreshLayerManager().Add(rectangle);
                    break;
                case LINE_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
        }
    }
    public class EllipseCmd : GeoCmd
    {
        public override void Resume()
        {
            this.ellipse.IsDisposed = false;
            this.canvas.GetFreshLayerManager().Add(ellipse);
        }
        public override void Cancel()
        {
            this.ellipse.IsDisposed = true;
        }
        public EllipseCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.ellipse.SetDrawMeta(meta);
        }

        public enum LINE_PROCESS { START, FIRST_POINT, SECOND_POINT, END }
        public LINE_PROCESS process = LINE_PROCESS.START;
        public override void MoveNext_MouseDown()
        {
            this.process = this.process + 1;
        }
        private EllipseGeo ellipse = new EllipseGeo();
        public override void Excute_MouseUpdate( )
        {
            PointGeo point = canvas.CurObjectPoint;
            switch (this.process)
            {
                case LINE_PROCESS.START:
                    break;
                case LINE_PROCESS.FIRST_POINT:
                    ellipse.FirstPoint = point.Clone();
                    break;
                case LINE_PROCESS.SECOND_POINT:
                    ellipse.SecondPoint = point.Clone();
                    canvas.GetFreshLayerManager().Add(ellipse);
                    break;
                case LINE_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
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
            this.poly.IsDisposed = true;
        }
        public override void Resume()
        {
            this.poly.IsDisposed = false;
            this.canvas.GetFreshLayerManager().Add(poly);
        }
        public PolygonCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.poly.SetDrawMeta(meta);
        }

        public enum POLYGON_PROCESS { START,POINT, END }
        public POLYGON_PROCESS process = POLYGON_PROCESS.START;
        public override void MoveNext_MouseDown()
        {
            if (this.process==POLYGON_PROCESS.START)
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
        private PolygonGeo poly = new PolygonGeo();
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
