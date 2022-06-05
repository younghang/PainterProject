using Painter.Controller;
using Painter.Painters;
using Painter.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Painter.Models
{
    /// <summary>
    /// 负责零件某个切割头的整个零件切割过程（多个GeoProcess）的控制，启动（对G代码的解释）
    /// 外面还需要在再加一层ProgramController整体控制，多头协调
    /// </summary>
    public class GGodeInterperter
    {
        public GGodeInterperter()
        {

        }

        public event Action<string> OnMsg;

        public double AxisSpeed { get; set; }
        private List<GeoProcessorBase> geoProcessors = new List<GeoProcessorBase>();
        public List<GeoProcessorBase> LoopProcessor = new List<GeoProcessorBase>();

        private double _curX;
        private bool IsStopRun = false;
        private double _minX = 1E6;
        private double _minY = 1E6;
        private double _maxX = -100;
        private double _maxY = -100;
        public double MinX { get { return _minX; } set { _minX = value; } }
        public double MinY { get { return _minY; } set { _minY = value; } }
        public double MaxX { get { return _maxX; } set { _maxX = value; } }
        public double MaxY { get { return _maxY; } set { _maxY = value; } }
        private GeoProcessorBase curGeoProcessor;
        public int CurProcessorIndex = 0;
        public int CurCycleTimes = 0;
        public int CutIndex = 0;//20220603好像没有什么用，不知道去年写的时候是干嘛的
        private List<PointGeo> HeadTrack = new List<PointGeo>();
        public static int MAX_TRACK_COUNT = 3000;
        public double CurrentY = 0;
        public PointGeo StartPoint;
        public PointGeo EndPoint;
        public double CurrentX
        {//激光头的位置
            get
            {
                return _curX;
            }
            set
            {
                _curX = value;
            }
        }
        public void Stop()
        {

        }
        //给计时器用
        public bool IsStartCount = false;
        public void Init()
        {
            CurProcessorIndex = 0;
            CurCycleTimes = 0;
            CutIndex = 0;
            HeadTrack.Clear();
            if (ProgramController.Instance.IsUniversalGCode)
            {
                CurrentX = this.StartPoint.X;
                CurrentY = this.StartPoint.Y;
            }
            IsStartCount = false;
            //			this.geoProcessors[0].StartRapidGeo.GetShapes()[0].GetDrawMeta().LineWidth = 2;
            //			this.geoProcessors[0].StartRapidGeo.GetShapes()[0].GetDrawMeta().CapStyle = System.Drawing.Drawing2D.LineCap.Round;
            //			this.geoProcessors[0].StartRapidGeo.GetShapes()[0].GetDrawMeta().DashLineStyle = new float[] { 2f, 2f };


        }
        public void OnTimerTick()
        {
            if (this.geoProcessors.Count == 0)
            {
                return;
            }
            CurrentX += ProgramController.Instance.AShaftMoveDistance;
            curGeoProcessor = geoProcessors[CurProcessorIndex];
            if (!ProgramController.Instance.IsUniversalGCode)
            {
                CutIndex = CurProcessorIndex;
            }
            else
            {
                if (curGeoProcessor is CutGeoProcessor)
                {
                    if (CurProcessorIndex >= CutIndex)
                    {
                        CutIndex++;
                    }
                }
            }
            //new Thread(() => curGeoProcessor.Execute(CurrentX)).Start(); 
            curGeoProcessor.Execute(CurrentX);
            if (CurProcessorIndex == this.geoProcessors.Count)
            {
                if (CurCycleTimes == 0)
                {
                    if (!ProgramController.Instance.IsUniversalGCode)
                    {
                        this.geoProcessors.RemoveAt(0);
                        for (int i = this.LoopProcessor.Count - 1; i >= 0; i--)
                        {
                            this.AddGeoProcessor(this.LoopProcessor[i], 0);
                        }
                        //this.geoProcessors[0].CutGeo = loopRapidGeo;// 下一次循环的起点变更
                        //this.geoProcessors[0].cutDistance = loopRapidGeo.GetShapes()[0].GetPerimeter();
                        //this.geoProcessors[0].cutDistanceInXDirection = loopRapidGeo.GetShapes()[0].StartPoint.X - loopRapidGeo.GetShapes()[0].EndPoint.X;
                    }
                }
                CurProcessorIndex = 0;
                CurCycleTimes++;
                if (CurCycleTimes == Settings.CYCLE_TIMES + 1)
                {
                    ProgramController.Instance.Stop();
                    return;
                }
                OnMsg.Invoke("***********开始第" + (CurCycleTimes + 1) + "次循环**********.\n");
            }
            HeadTrack.Add(new PointGeo((float)CurrentX, (float)CurrentY));
            if (HeadTrack.Count > MAX_TRACK_COUNT)
            {
                HeadTrack.RemoveAt(0);
            }
        }
        public void SetLoopGeo()
        {
            //设定循环G00
            // Geo loopRapidGeo = new Geo();
            // LineGeo loopG00Line = new LineGeo();
            // loopG00Line.FirstPoint = this.EndPoint;
            // if (ProgramController.Instance.IsNoWait)
            // {
            //     loopG00Line.SecondPoint = new PointGeo((float)(this.StartPoint.X + Settings.CYCLE_LENGTH), this.StartPoint.Y);
            // }
            // else
            // {
            //     loopG00Line.SecondPoint = new PointGeo((float)(this.StartPoint.X - Settings.CYCLE_LENGTH), this.StartPoint.Y);
            // } 
            // loopRapidGeo.AddShape(loopG00Line);
            // this.loopRapidGeo = loopRapidGeo;
        }


        /// <summary>
        /// 实时计算的切割头位置坐标累计的数组
        /// </summary>
        /// <returns></returns>
        internal IScreenPrintable GetTrack()
        {
            RandomLines randomLines = new RandomLines();
            randomLines.AddPoints(this.HeadTrack);
            ShapeMeta pointsMeta = new ShapeMeta();
            pointsMeta.ForeColor = Color.Blue;
            pointsMeta.LineWidth = 4;
            pointsMeta.CapStyle = System.Drawing.Drawing2D.LineCap.Round;
            //pointsMeta.DashLineStyle = new float[] { 2f, 2f };
            randomLines.SetDrawMeta(pointsMeta);
            return randomLines;
        }
        public List<PointGeo> GetTrackPoints()
        {
            return this.HeadTrack;
        }
        public void Clear()
        {
            this.shapes.Clear();
            this.HeadTrack.Clear();
            this.ClearGeoProcessor();
        }
        public LineGeo GetLatestLine()
        {
            LineGeo line;
            PointGeo p1 = new PointGeo(0, 0);
            PointGeo p2 = new PointGeo(0, 0);

            if (HeadTrack.Count > 2)
            {
                p1 = HeadTrack[HeadTrack.Count - 2];
                p2 = HeadTrack[HeadTrack.Count - 1];

            }
            line = new LineGeo(p1, p2);
            ShapeMeta pointsMeta = new ShapeMeta();
            pointsMeta.ForeColor = Color.Blue;
            pointsMeta.LineWidth = 4;
            pointsMeta.CapStyle = System.Drawing.Drawing2D.LineCap.Round;
            line.SetDrawMeta(pointsMeta);
            return line;
        }
        private void UpdateCurPos(double deltaX, double curY)
        {
            _curX += deltaX;
            CurrentY = curY;
        }
        public void AddGeoProcessor(GeoProcessorBase geoProcessor, int index = -1)
        {
            if (!geoProcessors.Contains(geoProcessor))
            {
                geoProcessor.UpdatePositionEvent += this.UpdateCurPos;
                geoProcessor.MesssageEvent += this.OnMsg;
                geoProcessor.SetInterpreter(this);
                if (index != -1)
                {
                    geoProcessors.Insert(index, geoProcessor);
                }
                else
                {
                    geoProcessors.Add(geoProcessor);
                }
                if (geoProcessor.MinX < this.MinX)
                {
                    this.MinX = geoProcessor.MinX;
                }
                if (geoProcessor.MaxX > this.MaxX)
                {
                    this.MaxX = geoProcessor.MaxX;
                }
                if (geoProcessor.MinY < this.MinY)
                {
                    this.MinY = geoProcessor.MinY;
                }
                if (geoProcessor.MaxY > this.MaxY)
                {
                    this.MaxY = geoProcessor.MaxY;
                }
            }
        }
        public List<GeoProcessorBase> GetProcessors()
        {
            return this.geoProcessors;
        }
        public int GetCutProcessorsCount()
        {
            int count = 0;
            foreach (var item in this.geoProcessors)
            {
                if (item is CutGeoProcessor)
                {
                    count++;
                }
            }
            return count;
        }
        public void ClearGeoProcessor()
        {
            this.geoProcessors.Clear();
        }

        private List<Shape> shapes = new List<Shape>();
        public void SetCutMetaStyle(ShapeMeta meta)
        {
            cutMeta = meta;
        }
        private ShapeMeta cutMeta;
        private void InitShapesWithMeta()
        {
            shapes.Clear();
            for (int i = 0; i < this.geoProcessors.Count; i++)
            {
                GeoProcessorBase geoProcessor = geoProcessors[i];
                if (geoProcessor is MoveGeoProcessor)
                {
                    List<Shape> g00Shape = geoProcessor.CutGeo.GetShapes();
                    foreach (var item in g00Shape)
                    {
                        ShapeMeta sm = new ShapeMeta();
                        sm.IsFill = false;
                        sm.BackColor = Color.Green;
                        sm.ForeColor = Color.Blue;
                        sm.LineWidth = 2;
                        sm.CapStyle = System.Drawing.Drawing2D.LineCap.Round;
                        sm.DashLineStyle = new float[] { 2f, 2f };
                        item.SetDrawMeta(sm);
                        shapes.Add(item);
                    }
                }
                else if (geoProcessor is CutGeoProcessor)
                {
                    List<Shape> cutShape = geoProcessor.CutGeo.GetShapes();
                    foreach (var item in cutShape)
                    {
                        if (cutMeta == null)
                        {
                            cutMeta = new ShapeMeta();
                            cutMeta.IsFill = false;
                            cutMeta.BackColor = Color.Black;
                            cutMeta.ForeColor = Color.Red;
                            cutMeta.LineWidth = 2;
                            cutMeta.CapStyle = System.Drawing.Drawing2D.LineCap.Round;
                        }
                        item.SetDrawMeta(cutMeta);
                        shapes.Add(item);
                    }
                }
            }
        }
        public List<Shape> GetShapes()
        {
            InitShapesWithMeta();
            return shapes;
        }
    }
    public enum PROCESS_STATUS
    {
        SLEEP,
        MOVEWAIT,
        MOVE,
        CUTWAIT,
        BEAMON,
        PROCESSING,
        BEAMOFF,
        DONE
    }

    /// <summary>
    /// 负责单次切割，包括空移到起点，等待，开始加工过程
    /// </summary>
    public class GeoProcessorBase
    {
        protected GGodeInterperter interpreter;
        public GeoProcessorBase()
        {
        }
        public void SetInterpreter(GGodeInterperter gCodeInterpreter)
        {
            interpreter = gCodeInterpreter;
        }
        public PointGeo StartPoint;
        public PointGeo EndPoint;
        protected Geo cutGeo;
        public double cutDistanceInXDirection;
        public double cutDistance;
        public double cutSpeed;

        public event Action<string> MesssageEvent;
        public event Action<double, double> UpdatePositionEvent;
        protected object lockObj = new object();
        private double _minX = 1E6;
        private double _minY = 1E6;
        private double _maxX;
        private double _maxY;
        public double MinX { get { return _minX; } set { _minX = value; } }
        public double MinY { get { return _minY; } set { _minY = value; } }
        public double MaxX { get { return _maxX; } set { _maxX = value; } }
        public double MaxY { get { return _maxY; } set { _maxY = value; } }
        public Geo CutGeo
        {
            get { return cutGeo; }
            set
            {
                cutGeo = value;
                InitialMinMax(cutGeo);
            }
        }
        private void InitialMinMax(Geo geo)
        {
            if (geo.MinX < this.MinX)
            {
                this.MinX = geo.MinX;
            }
            if (geo.MaxX > this.MaxX)
            {
                this.MaxX = geo.MaxX;
            }
            if (geo.MinY < this.MinY)
            {
                this.MinY = geo.MinY;
            }
            if (geo.MaxY > this.MaxY)
            {
                this.MaxY = geo.MaxY;
            }
        }
        protected int _curShapeIndex = 0;
        public virtual void OnMessage(string msg)
        {
            if (this.MesssageEvent != null)
            {
                this.MesssageEvent(msg);
            }
        }
        public virtual void OnUpdatePosition(double x, double y)
        {
            //方便子类调用
            if (this.UpdatePositionEvent != null)
            {
                this.UpdatePositionEvent(x, y);
            }
        }
        public virtual bool CutGeoProcess(Geo geo, double speed)
        {
            Shape shape = geo.GetShapes()[_curShapeIndex];
            double tickProcessLength = Settings.TIME_SPAN * speed / 60 / 1000 * Settings.SPEED_RATE;
            if (shape.GetPerimeter() < 1E-3)
            {//存在部分线段长度为0的情况
                _curShapeIndex++;
                _cutLength = 0;
                if (_curShapeIndex == geo.GetShapes().Count)
                {
                    _curShapeIndex = 0;
                    return true;
                }
                return false;
            }
            PointGeo curPos = shape.GetXDirectionAtLength((float)(_cutLength));
            PointGeo nextPos = shape.GetXDirectionAtLength((float)(_cutLength + tickProcessLength));
            double deltaX = nextPos.X - curPos.X;
            if (Math.Abs(_cutLength - shape.GetPerimeter()) < tickProcessLength)
            {
                UpdatePositionEvent.Invoke(shape.SecondPoint.X - curPos.X, shape.SecondPoint.Y);
                _curShapeIndex++;
                _cutLength = 0;

                if (_curShapeIndex == geo.GetShapes().Count)
                {
                    _curShapeIndex = 0;
                    return true;
                }
            }
            else
            {
                _cutLength += tickProcessLength;
                UpdatePositionEvent.Invoke(deltaX, curPos.Y);//这个顺序不能乱改，不然有偏差
            }
            return false;
        }
        protected bool isProcess = false;
        protected bool isReady = false;

        protected double _cutLength = 0;
        public virtual void Execute(double pos)
        {
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
            if (interpreter == null)
            {
                return;
            }

        }
    }

    public enum CUT_PROCESS_STATUS
    {
        SLEEP,
        CUTWAIT,
        BEAMON,
        PROCESSING,
        BEAMOFF,
        DONE
    }
    public class CutGeoProcessor : GeoProcessorBase
    {
        public CUT_PROCESS_STATUS curState;
        private int _beamOnCount = 0;
        private int _beamOffCount = 0;

        public override void Execute(double pos)
        {

            base.Execute(pos);
            if (!isProcess)
            {
                isProcess = true;
                switch (curState)
                {
                    case CUT_PROCESS_STATUS.SLEEP:
                        OnMessage("Cutting 等待板材出料中...\n");
                        curState = CUT_PROCESS_STATUS.CUTWAIT;
                        break;

                    case CUT_PROCESS_STATUS.CUTWAIT:
                        if (ProgramController.Instance.IsNoWait || (pos - interpreter.StartPoint.X + (cutDistance / cutSpeed * 60 + Settings.BEAMON) * Settings.A_SHAFT_SPEED / 60) > cutDistanceInXDirection)
                        {
                            if (!isReady)
                            {
                                if (interpreter.CurCycleTimes == 0 && interpreter.CutIndex == 1)
                                {
                                    ProgramController.Instance.WORKER＿READY_COUNT++;
                                    OnMessage("等待其他Head同步中...\n");
                                }
                                isReady = true;
                            }
                            if (ProgramController.Instance.WORKER＿READY_COUNT == ProgramController.Instance.TOTAL_WORKER_COUNT)
                            {
                                ProgramController.Instance.IsStartCount = true;
                                curState = CUT_PROCESS_STATUS.BEAMON;
                                _beamOnCount = 0;
                                OnMessage("BeamOn [" + interpreter.CurProcessorIndex + "]...\n");
                            }
                        }
                        break;
                    case CUT_PROCESS_STATUS.BEAMON:
                        _beamOnCount++;
                        if (_beamOnCount * Settings.TIME_SPAN >= Settings.BEAMON * 1000)
                        {
                            curState = CUT_PROCESS_STATUS.PROCESSING;
                            OnMessage("开始切割轮廓[" + interpreter.CurProcessorIndex + "]...\n");
                            _beamOnCount = 0;
                        }
                        break;
                    case CUT_PROCESS_STATUS.PROCESSING:
                        //这里存在一个问题要弄清楚：就是我在加工一个元素的时候，
                        //需要知道他在哪里吗？他的X,Y 坐标重要吗？
                        //在这里，是不需要的， 我只需要知道开始加工了，
                        //这里所谓的加工，是以某一个元素为单位的
                        //那么每个tick阶段，他的X会变化多少才是我关心的。
                        if (CutGeoProcess(CutGeo, cutSpeed * Settings.CUT_SPEED_RATIO))
                        {
                            OnMessage("结束切割轮廓[" + interpreter.CurProcessorIndex + "]...\n");
                            curState = CUT_PROCESS_STATUS.BEAMOFF;
                        }
                        break;
                    case CUT_PROCESS_STATUS.BEAMOFF:
                        _beamOffCount++;
                        if (_beamOffCount * Settings.TIME_SPAN >= Settings.BEAMOFF * 1000)
                        {
                            OnMessage("BeamOff [" + interpreter.CurProcessorIndex + "]...\n");
                            curState = CUT_PROCESS_STATUS.DONE;
                            _beamOffCount = 0;
                        }
                        break;
                    case CUT_PROCESS_STATUS.DONE:
                        this.interpreter.CurProcessorIndex++;
                        curState = CUT_PROCESS_STATUS.SLEEP;
                        break;
                    default:
                        break;
                }
                isProcess = false;
            }

        }
    }
    public enum MOVE_PROCESS_STATUS
    {
        SLEEP,
        MOVEWAIT,
        MOVE,
        DONE
    }
    public class MoveGeoProcessor : GeoProcessorBase
    {
        public MOVE_PROCESS_STATUS curState;
        public override void Execute(double pos)
        {
            base.Execute(pos);
            if (!isProcess)
            {
                isProcess = true;
                switch (curState)
                {
                    case MOVE_PROCESS_STATUS.SLEEP:
                        OnMessage("Moving 等待板材出料中...\n");
                        curState = MOVE_PROCESS_STATUS.MOVEWAIT;
                        break;
                    case MOVE_PROCESS_STATUS.MOVEWAIT:
                        if (ProgramController.Instance.IsNoWait || (pos - interpreter.StartPoint.X + (cutDistance / cutSpeed * 60) * Settings.A_SHAFT_SPEED / 60) > this.cutDistanceInXDirection)
                        {
                            curState = MOVE_PROCESS_STATUS.MOVE;
                            //OnMesssage?.BeginInvoke("开始切割轮廓[" + interpreter.CutIndex + "]...\n", null, null);
                            OnMessage("GOO快速移动[" + interpreter.CurProcessorIndex + "]...\n");
                        }
                        break;
                    case MOVE_PROCESS_STATUS.MOVE:
                        if (CutGeoProcess(cutGeo, cutSpeed))
                        {
                            OnMessage("G00完成...\n");
                            curState = MOVE_PROCESS_STATUS.DONE;
                        }
                        break;
                    case MOVE_PROCESS_STATUS.DONE:
                        this.interpreter.CurProcessorIndex++;
                        curState = MOVE_PROCESS_STATUS.SLEEP;
                        break;
                    default:
                        break;
                }
                isProcess = false;
            }
        }
    }
    /// <summary>
    /// 负责单次切割，包括空移到起点，等待，开始加工过程
    /// </summary>
    public class GeoProcessor0123
    {
        private GGodeInterperter interpreter;
        public GeoProcessor0123()
        {


        }

        public void SetInterpreter(GGodeInterperter gCodeInterpreter)
        {
            interpreter = gCodeInterpreter;
        }
        public PointGeo StartPoint;
        public PointGeo EndPoint;
        private Geo cutGeo;
        private Geo startRapidGeo;
        public double cutDistanceInXDirection;
        public double cutDistance;
        public double cutSpeed;
        //加工速度
        public double rapidSpeed;
        //空载速度
        public PROCESS_STATUS curState;



        //关光耗时，可通过set.txt设置
        public event Action<string> OnMesssage;
        public event Action<double, double> UpdatePosition;
        private object lockObj = new object();
        private double _minX;
        private double _minY;
        private double _maxX;
        private double _maxY;
        public double MinX { get { return _minX; } set { _minX = value; } }
        public double MinY { get { return _minY; } set { _minY = value; } }
        public double MaxX { get { return _maxX; } set { _maxX = value; } }
        public double MaxY { get { return _maxY; } set { _maxY = value; } }
        public Geo CutGeo
        {
            get { return cutGeo; }
            set
            {
                cutGeo = value;
                InitialMinMax(cutGeo);
            }
        }

        public Geo StartRapidGeo
        {
            get { return startRapidGeo; }
            set
            {
                startRapidGeo = value;
            }
        }
        private void InitialMinMax(Geo geo)
        {
            if (geo.MinX < this.MinX)
            {
                this.MinX = geo.MinX;
            }
            if (geo.MaxX > this.MaxX)
            {
                this.MaxX = geo.MaxX;
            }
            if (geo.MinY < this.MinY)
            {
                this.MinY = geo.MinY;
            }
            if (geo.MaxY > this.MaxY)
            {
                this.MaxY = geo.MaxY;
            }
        }
        private int _curShapeIndex = 0;
        public virtual bool CutGeoProcess(Geo geo, double speed)
        {
            Shape shape = geo.GetShapes()[_curShapeIndex];
            double tickProcessLength = Settings.TIME_SPAN * speed / 60 / 1000 * Settings.SPEED_RATE;
            if (shape.GetPerimeter() < 1E-3)
            {//存在部分线段长度为0的情况
                _curShapeIndex++;
                _cutLength = 0;
                if (_curShapeIndex == geo.GetShapes().Count)
                {
                    _curShapeIndex = 0;
                    return true;
                }
                return false;
            }
            _cutLength += tickProcessLength;
            PointGeo curPos = shape.GetXDirectionAtLength((float)(_cutLength));
            PointGeo nextPos = shape.GetXDirectionAtLength((float)(_cutLength + tickProcessLength));

            double deltaX = nextPos.X - curPos.X;
            UpdatePosition.Invoke(deltaX, curPos.Y);//这个顺序不能乱改，不然有偏差
            if (Math.Abs(_cutLength - shape.GetPerimeter()) < tickProcessLength)
            {
                _curShapeIndex++;
                _cutLength = 0;
                if (_curShapeIndex == geo.GetShapes().Count)
                {
                    _curShapeIndex = 0;
                    return true;
                }
            }
            return false;
        }
        private bool isProcess = false;
        private bool isReady = false;
        private int _beamOnCount = 0;
        private int _beamOffCount = 0;
        private double _cutLength = 0;
        public virtual void Execute(double pos)
        {
            if (interpreter == null)
            {
                return;
            }

            switch (curState)
            {
                case PROCESS_STATUS.SLEEP:
                    OnMesssage.Invoke("Moving 等待板材出料中...\n");
                    curState = PROCESS_STATUS.MOVEWAIT;
                    break;
                case PROCESS_STATUS.MOVEWAIT:
                    if ((pos - interpreter.StartPoint.X + startRapidGeo.GetShapes()[0].GetPerimeter() / rapidSpeed * Settings.A_SHAFT_SPEED) > (startRapidGeo.GetShapes()[0].FirstPoint.X - startRapidGeo.GetShapes()[0].SecondPoint.X))
                    {
                        curState = PROCESS_STATUS.MOVE;
                        //OnMesssage?.BeginInvoke("开始切割轮廓[" + interpreter.CutIndex + "]...\n", null, null);
                        OnMesssage.Invoke("GOO快速移动[" + interpreter.CurProcessorIndex + "]...\n");
                    }
                    break;
                case PROCESS_STATUS.MOVE:
                    if (CutGeoProcess(StartRapidGeo, rapidSpeed))
                    {
                        OnMesssage.Invoke("Cutting 等待板材出料中...\n");
                        curState = PROCESS_STATUS.CUTWAIT;
                    }

                    break;
                case PROCESS_STATUS.CUTWAIT:
                    if ((pos - interpreter.StartPoint.X + cutDistance / cutSpeed * Settings.A_SHAFT_SPEED) > cutDistanceInXDirection)
                    {
                        if (!isReady)
                        {
                            if (interpreter.CurCycleTimes == 0 && interpreter.CurProcessorIndex == 0)
                            {
                                ProgramController.Instance.WORKER＿READY_COUNT++;
                                OnMesssage.Invoke("等待其他Head同步中...\n");
                            }
                            isReady = true;
                        }
                        if (ProgramController.Instance.WORKER＿READY_COUNT == ProgramController.Instance.TOTAL_WORKER_COUNT)
                        {
                            ProgramController.Instance.IsStartCount = true;
                            curState = PROCESS_STATUS.BEAMON;
                            _beamOnCount = 0;
                            OnMesssage.Invoke("BeamOn [" + interpreter.CurProcessorIndex + "]...\n");
                        }
                    }
                    break;
                case PROCESS_STATUS.BEAMON:
                    _beamOnCount++;
                    if (_beamOnCount * Settings.TIME_SPAN >= Settings.BEAMON * 1000)
                    {
                        curState = PROCESS_STATUS.PROCESSING;
                        OnMesssage.Invoke("开始切割轮廓[" + interpreter.CurProcessorIndex + "]...\n");
                        _beamOnCount = 0;
                    }
                    break;
                case PROCESS_STATUS.PROCESSING:
                    //这里存在一个问题要弄清楚：就是我在加工一个元素的时候，
                    //需要知道他在哪里吗？他的X,Y 坐标重要吗？
                    //在这里，是不需要的， 我只需要知道开始加工了，
                    //这里所谓的加工，是以某一个元素为单位的
                    //那么每个tick阶段，他的X会变化多少才是我关心的。
                    if (CutGeoProcess(CutGeo, cutSpeed * Settings.CUT_SPEED_RATIO))
                    {
                        OnMesssage.Invoke("结束切割轮廓[" + interpreter.CurProcessorIndex + "]...\n");
                        curState = PROCESS_STATUS.BEAMOFF;
                    }
                    break;
                case PROCESS_STATUS.BEAMOFF:
                    _beamOffCount++;
                    if (_beamOffCount * Settings.TIME_SPAN >= Settings.BEAMOFF * 1000)
                    {
                        OnMesssage.Invoke("BeamOff [" + interpreter.CurProcessorIndex + "]...\n");
                        curState = PROCESS_STATUS.DONE;
                        _beamOffCount = 0;
                    }
                    break;
                case PROCESS_STATUS.DONE:
                    this.interpreter.CurProcessorIndex++;
                    curState = PROCESS_STATUS.SLEEP;
                    break;
                default:
                    break;
            }
        }
    }

}
