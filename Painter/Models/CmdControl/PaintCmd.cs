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
                CurCmd.Excute();
                CurCmd.MoveNext();
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
            CurCmd.Excute( );
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
                    Undo();
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
        public virtual void Excute( )
        { 
        }
        public virtual void Update( )
        {
        }
        public virtual void MoveNext()
        {

        }
        public virtual void MovePrev()
        {
            
        }
        public virtual void Cancel()
        {

        }
        public virtual void Resume()
        {

        }
        public CMD_STATUS Status = CMD_STATUS.SLEEP;
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
        public override void MoveNext()
        {
            this.process = this.process + 1;
        }
        private LineGeo line = new LineGeo(); 
        public override void Excute( )
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
        public override void MoveNext()
        {
            this.process = this.process + 1;
        }
        private CircleGeo circle = new CircleGeo();
        public override void Resume()
        {
            this.circle.IsDisposed = false;
            this.canvas.GetFreshLayerManager().Add(circle);
        }
        public override void Excute( )
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
        public override void MoveNext()
        {
            this.process = this.process + 1;
        }
        private ArcGeo arc = new ArcGeo();
        public override void Resume()
        {
            this.arc.IsDisposed = false;
            this.canvas.GetFreshLayerManager().Add(arc);
        }
        public override void Excute()
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
        public override void MoveNext()
        {
            this.process = this.process + 1;
        }
        private RectangeGeo rectangle = new RectangeGeo();
        public override void Excute( )
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
        public override void MoveNext()
        {
            this.process = this.process + 1;
        }
        private EllipseGeo ellipse = new EllipseGeo();
        public override void Excute( )
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
}
