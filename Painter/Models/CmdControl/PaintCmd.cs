using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Painter.Models.CmdControl
{
    [Serializable]
    public class CommandMgr
    {
        Command repeat;
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
                    CurCmd.MouseRightDown_EndConfirm();
                    AddCmd(repeat);
                }
            }
        }
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (CurCmd != null)
                {
                    CurCmd.MouseRightDown_EndConfirm();
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
        public void OnMouseUp(object sender, MouseEventArgs e)
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
            CurCmd.End_MouseUp();
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
                repeat = this.CurCmd.Clone();
                repeat.canvas = cmd.canvas;
            }
        }
        public void Undo()
        {
            if (curIndex > 0)
            {
                if (CurCmd != null)
                {
                    CurCmd.Cancel();
                    if (CurCmd.Status == CMD_STATUS.DONE)
                    {
                        curIndex--;
                        if (curIndex > 0)
                        {
                            CurCmd = this.cmds[curIndex];
                        }
                        else
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
            if (curIndex < this.cmds.Count)
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
    public enum CMD_STATUS { SLEEP, RUNNING, DONE, ABORT }
    [Serializable]
    public class Command
    {
        //canvas 需要手动赋值一下
        public virtual Command Clone()
        {
            Command command = CommonUtils.CloneObject<Command>(this);
            command.Status = CMD_STATUS.SLEEP;
            if (command is GeoTwoPointCmd)
            {
                (command as GeoTwoPointCmd).process=GeoTwoPointCmd.LINE_PROCESS.FIRST_POINT; 
            }
            return command;
        }
        public Command(CanvasModel model)
        {
            this.canvas = model;
        }
        [NonSerialized]
        public CanvasModel canvas = null;
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
        public virtual void MouseRightDown_EndConfirm()
        {

        }
        public virtual void Resume()
        {

        }

        public virtual void End_MouseUp()
        {

        }

        public CMD_STATUS Status = CMD_STATUS.SLEEP;
    }
    [Serializable]
    public class TextCmd : Command
    {
        public TEXT_PROCESS process = TEXT_PROCESS.START;
        public enum TEXT_PROCESS { START, TEXT_LOC, TEXT_INPUT, END }
        public TextCmd(CanvasModel model, TextMeta drawMeta) : base(model)
        {

            textMeta = drawMeta.Copy();
            textMeta.IsScaleble = true;
            process = TEXT_PROCESS.START;
            text.SetDrawMeta(textMeta);
        }
        [NonSerialized]
        public Action<string> ShowTextBox;
        [NonSerialized]
        public Func<string> GetTextBox;
        private DrawableText text = new DrawableText();
        private TextMeta textMeta;
        public override void MoveNext_MouseDown()
        {
            if (process == TEXT_PROCESS.START)
            {
                text.pos = canvas.CurObjectPoint.Clone();
                ShowTextBox?.Invoke(textMeta.Text);
            }
            process++;
            if (process == TEXT_PROCESS.TEXT_INPUT)
            {
                if (GetTextBox != null)
                {
                    textMeta.Text = GetTextBox();
                    canvas.GetFreshLayerManager().Add(text, true);
                }
                process = TEXT_PROCESS.END;
            }
        }
        public override void Excute_MouseUpdate()
        {

            switch (this.process)
            {
                case TEXT_PROCESS.START:
                    canvas.OnCmdMsg("鼠标左键单击插入文本");
                    break;
                case TEXT_PROCESS.TEXT_LOC:
                    canvas.OnCmdMsg("鼠标左键单击结束输入");
                    break;
                case TEXT_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
        }
        public override Command Clone()
        {
            TextCmd textCmd = new TextCmd(this.canvas, this.textMeta.Copy());
            textCmd.ShowTextBox = this.ShowTextBox;
            textCmd.GetTextBox = this.GetTextBox;
            return textCmd;
        }
    }
    [Serializable]
    public class CopyCmd : GeoTwoPointCmd
    {
        public override Command Clone()
        {
            CopyCmd moveCmd = new CopyCmd(this.canvas, this.curSelectObjs);
            return moveCmd;
        }
        public override void Cancel()
        {
            this.Status = CMD_STATUS.DONE;
        }
        List<DrawableObject> curSelectObjs = new List<DrawableObject>();
        public CopyCmd(CanvasModel model, List<DrawableObject> ls) : base(model)
        {
            if (ls.Count == 0)
            {
                this.Status = CMD_STATUS.DONE;
                this.process = LINE_PROCESS.END;
            }
            else
            {
                curSelectObjs.AddRange(ls);
                for (int i = 0; i < curSelectObjs.Count; i++)
                {
                    canvas.GetFreshLayerManager().Add(CommonUtils.CloneObject<DrawableObject>(curSelectObjs[i]), true);
                }
            }
        }
        PointGeo startP = new PointGeo();
        PointGeo endP = new PointGeo();
        public override void Excute_MouseUpdate()
        {
            PointGeo point = canvas.CurObjectPoint;
            switch (this.process)
            {
                case LINE_PROCESS.START:
                    break;
                case LINE_PROCESS.FIRST_POINT:
                    canvas.OnCmdMsg("请鼠标左键单击选择起点");
                    startP = point.Clone();
                    endP = point.Clone();
                    //MoveNext();
                    break;
                case LINE_PROCESS.SECOND_POINT:
                    canvas.OnCmdMsg("请鼠标左键单击选择终点");
                    foreach (var item in this.curSelectObjs)
                    {
                        item.Translate(startP - endP);
                    }
                    endP = point.Clone();
                    foreach (var item in this.curSelectObjs)
                    {
                        item.Translate(endP - startP);
                    }
                    break;
                case LINE_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    if (curSelectObjs.Count == 0)
                    {
                        canvas.OnCmdMsg("请先选择图形后，再使用复制命令");
                    }
                    else
                        canvas.OnCmdMsg("复制命令结束");
                    break;
                default:
                    break;
            }
        }
    }
    [Serializable]
    public class MoveCmd : GeoTwoPointCmd
    {
        public override void Cancel()
        {
            this.Status = CMD_STATUS.DONE;
        }
        public override Command Clone()
        {
            MoveCmd moveCmd = new MoveCmd(this.canvas, this.curSelectObjs);
            return moveCmd;
        }
        List<DrawableObject> curSelectObjs = new List<DrawableObject>();
        public MoveCmd(CanvasModel model, List<DrawableObject> ls) : base(model)
        {
            if (ls.Count == 0)
            {
                this.Status = CMD_STATUS.DONE;
                this.process = LINE_PROCESS.END;
            }
            else
            {
                curSelectObjs.AddRange(ls);
            }
        }
        PointGeo startP = new PointGeo();
        PointGeo endP = new PointGeo();
        public override void Excute_MouseUpdate()
        {
            PointGeo point = canvas.CurObjectPoint;
            switch (this.process)
            {
                case LINE_PROCESS.START:
                    break;
                case LINE_PROCESS.FIRST_POINT:
                    canvas.OnCmdMsg("请鼠标左键单击选择起点");
                    startP = point.Clone();
                    endP = point.Clone();
                    //MoveNext();
                    break;
                case LINE_PROCESS.SECOND_POINT:
                    canvas.OnCmdMsg("请鼠标左键单击选择终点");
                    foreach (var item in this.curSelectObjs)
                    {
                        item.Translate(startP - endP);
                    }
                    endP = point.Clone();
                    foreach (var item in this.curSelectObjs)
                    {
                        item.Translate(endP - startP);
                    }
                    break;
                case LINE_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    if (curSelectObjs.Count == 0)
                    {
                        canvas.OnCmdMsg("请先选择图形后，再使用移动命令");
                    }
                    else
                        canvas.OnCmdMsg("移动命令结束");
                    break;
                default:
                    break;
            }
        }
    }
    [Serializable]
    public class DeleteCmd : Command
    {

        public DeleteCmd(CanvasModel model, List<DrawableObject> ls) : base(model)
        {
            this.curSelectObjs = ls;//这里传递是的引用，可以源源不断的拿到现在选择的对象
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
        public override void MouseRightDown_EndConfirm()
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
            canvas.OnCmdMsg("鼠标左键拾取删除元素，鼠标右键结束命令");
        }

        public override void Excute_MouseUpdate()
        {

        }
    }
    [Serializable]
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
    [Serializable]
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
    [Serializable]
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
    [Serializable]
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
                    canvas.OnCmdMsg("请鼠标左键单击选择第一个点");
                    //MoveNext();
                    break;
                case LINE_PROCESS.SECOND_POINT:
                    (shape as Shape).SecondPoint = point.Clone();
                    canvas.GetFreshLayerManager().Add(shape, true);
                    canvas.OnCmdMsg((shape as Shape).MSG);
                    break;
                case LINE_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
        }
    }

    [Serializable]
    public class LineCmd : GeoTwoPointCmd
    {
        public override void Excute_MouseUpdate()
        {
            PointGeo point = canvas.CurObjectPoint;
            switch (this.process)
            {
                case LINE_PROCESS.START:
                    break;
                case LINE_PROCESS.FIRST_POINT:
                    (shape as Shape).FirstPoint = point.Clone();
                    canvas.OnCmdMsg("请鼠标左键单击选择第一个点");
                    //MoveNext();
                    break;
                case LINE_PROCESS.SECOND_POINT:
                    PointGeo p=point.Clone();
                    if (this.canvas.IsShiftDown)
                    {
                        if (Math.Abs(p.X - (shape as Shape).FirstPoint.X) < Math.Abs(p.Y - (shape as Shape).FirstPoint.Y))
                        {
                            p.X = (shape as Shape).FirstPoint.X;
                        }
                        else
                        {
                            p.Y = (shape as Shape).FirstPoint.Y;
                        }
                    } 
                    (shape as Shape).SecondPoint = p;
                    canvas.GetFreshLayerManager().Add(shape, true);
                    canvas.OnCmdMsg((shape as Shape).MSG);
                    break;
                case LINE_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    if (this.canvas.IsControlDown)
                    {
                        LineCmd lineCmd = (LineCmd)this.Clone();
                        (lineCmd.shape as Shape).FirstPoint = (this.shape as Shape).SecondPoint.Clone();
                        lineCmd.process = LINE_PROCESS.SECOND_POINT;
                        lineCmd.canvas = this.canvas;
                        lineCmd.Status = CMD_STATUS.RUNNING;
                        this.canvas.GetCmdMgr().AddCmd(lineCmd);
                    }
                    break;
                default:
                    break;
            }
        }
        public LineCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.shape = new LineGeo();
            this.shape.SetDrawMeta(meta);
        }

    }
    [Serializable]
    public class CircleCmd : GeoTwoPointCmd
    {
        public CircleCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.shape = new CircleGeo();
            this.shape.SetDrawMeta(meta);
        }

    }

    [Serializable]
    public class ArcCmd : Command
    {
        public override void Cancel()
        {
            if (this.Status != CMD_STATUS.DONE)
            {
                this.process = this.process - 1;
                if (this.process == ARC_PROCESS.START)
                {
                    this.arc.IsDisposed = true;
                    this.Status = CMD_STATUS.DONE;
                }
            }
            else
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
                    canvas.OnCmdMsg("请鼠标左键单击选择圆心");
                    arc.FirstPoint = point.Clone();
                    //MoveNext();
                    break;
                case ARC_PROCESS.SECOND_POINT:
                    arc.SecondPoint = point.Clone();
                    canvas.GetFreshLayerManager().Add(arc, true);
                    canvas.OnCmdMsg("请鼠标左键单击确定半径和圆弧起始角度");

                    break;
                case ARC_PROCESS.THIRD_POINT:
                    arc.ThirdPoint = point.Clone();
                    canvas.OnCmdMsg("请鼠标左键单击确定圆弧终止角度，“撤销”回退，单击右键确定");

                    break;
                case ARC_PROCESS.END:
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
        }
    }
    [Serializable]
    public class RectangleCmd : GeoTwoPointCmd
    {
        public RectangleCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.shape = new RectangleGeo();
            this.shape.SetDrawMeta(meta);
        }
    }
    [Serializable]
    public class EllipseCmd : GeoTwoPointCmd
    {
        public EllipseCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.shape = new EllipseGeo();
            this.shape.SetDrawMeta(meta);
        }
    }
    [Serializable]
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
            }
            else
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
            canvas.OnCmdMsg("请鼠标左键单击添加点，要求点数量>3,“撤销”回退，单击右键确定");
            PointGeo point = canvas.CurObjectPoint;
            poly.AddPoint(point.Clone());
            if (this.poly.points.Count < 2)
            {
                return;
            }
            canvas.GetFreshLayerManager().Add(poly);
        }
    }
    [Serializable]
    public class PenCmd : PolygonCmd
    {
        public override void Excute_MouseUpdate()
        {

            switch (this.process)
            {
                case POLYGON_PROCESS.START:
                    break;
                case POLYGON_PROCESS.POINT:
                    PointGeo point = canvas.CurObjectPoint;
                    poly.AddPoint(point.Clone());
                    if (this.poly.points.Count < 2)
                    {
                        return;
                    }
                    canvas.GetFreshLayerManager().Add(poly);
                    break;
                case POLYGON_PROCESS.END:
                    poly.SamplePointByStep(0.06);
                    this.Status = CMD_STATUS.DONE;
                    break;
                default:
                    break;
            }
        }
        public override void End_MouseUp()
        {
            if (process==POLYGON_PROCESS.POINT&& this.poly.points.Count > 2)
            { 
                this.process = POLYGON_PROCESS.END;
            } 
        }
        public override void Cancel()
        {
            this.poly.IsDisposed = true;
            this.Status = CMD_STATUS.DONE;
        }
        public PenCmd(CanvasModel model, ShapeMeta meta) : base(model)
        {
            this.process = POLYGON_PROCESS.START;
            poly = new CurveGeo();
            meta.IsFill = false;
            this.poly.SetDrawMeta(meta);
        }

        public override void MoveNext_MouseDown()
        {
            if (this.process == POLYGON_PROCESS.START)
            {
                this.process = this.process + 1;
                return;
            }
            canvas.OnCmdMsg("请鼠标左键单击添加点，要求点数量>3,“撤销”回退，单击右键确定");
          
        }
    }
    [Serializable]
    public class PolygonCmd : GeoCmd
    {
        public override void MouseRightDown_EndConfirm()
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
                //return;
            }
            canvas.OnCmdMsg("请鼠标左键单击添加点，要求点数量>3,“撤销”回退，单击右键确定");

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
