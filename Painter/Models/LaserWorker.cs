/*
 * Created by SharpDevelop.
 * User: DELL
 * Date: 2021/9/11
 * Time: 10:16
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Painter.Controller;
using Painter.DisplayManger;
using Painter.Models;
using Painter.Painters;
using Painter.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CalculatorDll;
using System.Threading;

namespace Painter.Models
{
    //视图ViewPainter的代表（WPF、WinForm）
    public interface IPainterUC
    {
        void OnMessage(string msg);
        void OnUpdatePos(double offset);
        //清空当前视图的图素
        void Clear();
        void Invalidate();
        //移动的，类似板材上的图形，需要像流水线一样移动
        GraphicsLayerManager GetMoveableLayerManager();
        //禁止不动，不刷新的,只需要在原图上继续追加，类似轨迹
        GraphicsLayerManager GetHoldLayerManager();
        //会刷新，但是相对固定不动，像是激光头，限位标记
        GraphicsLayerManager GetFreshLayerManager();

    }
    /// <summary>
    /// Description of PainterViewModel.
    /// 负责管理HeadWorker和ViewPainterUC之间的关系
    /// </summary>
    public class LaserHeadWorker
    {
        public Color LineColor = Color.Blue;
        public IPainterUC PainterUC;
        private string fileFullName = "";
        public string Name = "Head1";
        GGodeInterperter gCodeInterpreter1;
        List<string> listGCodeStr = new List<string>();
        List<string> strValidCodes = new List<string>();
        public CircleGeo laserHeadCircle = new CircleGeo();
        public RandomLines trackLines = new RandomLines();
        public event Action<string> OnMsg;
        //切割光开就为true
        public event Action<bool> OnCutting;
        public event Action<double, double> OnHeadPosition;
        public Action<double> OnAShaftUpdate;
        public bool IsUpdateMovingShape { get; set; }
        private LineGeo leftSafeLine = new LineGeo();
        private DrawableText leftBorderText = new DrawableText();
        private LineGeo rightSafeLine = new LineGeo();
        private DrawableText rightBoderText = new DrawableText();
        public Action OnTimerTick;
        public bool IsUniersalGCode = false;
        public string FileFullName
        {
            get
            {
                return fileFullName;
            }
            set
            {
                fileFullName = value;
            }
        }
        public double MinX { get { return this.gCodeInterpreter1.MinX; } }
        public double MinY { get { return this.gCodeInterpreter1.MinY; } }
        public double MaxX { get { return this.gCodeInterpreter1.MaxX; } }
        public double MaxY { get { return this.gCodeInterpreter1.MaxY; } }

        public LaserHeadWorker(IPainterUC painterUC, string name = "Head1")
        {
            this.PainterUC = painterUC;
            this.Name = name;
            gCodeInterpreter1 = new GGodeInterperter();
            //ProgramController.WORKER＿READY_COUNT++;
            //ProgramController.workers.Add(this);
            //单线程执行多个worker的tick事件
            //OnTimerTick = this.gCodeInterpreter1.OnTimerTick;
            //每个worker单独执行
            OnTimerTick = new Action(() =>
            {
                WaitCallback waitCallback = new WaitCallback((o) => {
                    this.gCodeInterpreter1.OnTimerTick();
                });
                ThreadPool.QueueUserWorkItem(waitCallback);
                // new Thread(() => { this.gCodeInterpreter1.OnTimerTick(); }).Start();
            });
        }
        public double TotalCutLength
        {
            get;
            set;
        }

        public double TotalMoveLength
        {
            get;
            set;
        }

        public void Clear()
        {
            if (this.gCodeInterpreter1 != null)
            {
                this.gCodeInterpreter1.Clear();
            }
            //if (this.PainterUC != null) {
            //	this.PainterUC.Clear();
            //	this.PainterUC.Invalidate();
            //}
        }

        public string GenerateLaserHeadDescription()
        {
            for (int i = 0; i < gCodeInterpreter1.GetProcessors().Count; i++)
            {
                GeoProcessorBase processor = this.gCodeInterpreter1.GetProcessors()[i];
                if (processor is CutGeoProcessor)
                {
                    this.TotalCutLength += processor.CutGeo.GetPerimeter();
                }
                else
                {
                    if (i == 0)
                    {
                        foreach (var item in gCodeInterpreter1.LoopProcessor)
                        {
                            this.TotalMoveLength += item.cutDistance;
                        }
                    }
                    else
                    {
                        this.TotalMoveLength += processor.CutGeo.GetPerimeter();
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=".PadLeft(50, '='));
            sb.AppendLine(this.Name);
            sb.AppendLine(this.FileFullName);
            sb.AppendLine("循环长度:" + Settings.CYCLE_LENGTH.ToString("f2"));
            sb.AppendLine("切割段数:" + this.gCodeInterpreter1.GetProcessors().Count);
            sb.AppendLine(("总切割长度:" + this.TotalCutLength.ToString("f2") + " mm").PadLeft(23) + (", 切割速度:" + double.Parse(CalService.Input("CUTSPEED1")).ToString("f3") + " mm/min").PadLeft(20));
            sb.AppendLine(("总空行程长度:" + this.TotalMoveLength.ToString("f2") + " mm").PadLeft(23) + (", 空载速度:" + double.Parse(CalService.Input("RAPIDSPEED")).ToString("f3") + " mm/min").PadLeft(20));
            double time = (this.TotalCutLength / double.Parse(CalService.Input("CUTSPEED1")) * 60 + this.TotalMoveLength * 60 / double.Parse(CalService.Input("RAPIDSPEED"))) + (Settings.BEAMOFF + Settings.BEAMON) * this.gCodeInterpreter1.GetCutProcessorsCount();
            sb.AppendLine("无等待极限最短加工时间：" + time.ToString("f2") + " s");
            sb.AppendLine("无等待极限A轴最大速度：" + (Settings.CYCLE_LENGTH / time * 60).ToString("f3") + " mm/min");
            sb.AppendLine("=".PadLeft(50, '='));
            return sb.ToString();
        }
        private bool InitLaserWorker()
        {
            if (strValidCodes.Count == 0)
            {
                throw new LaserException("ERROR", "空文件\n无有效行");
            }
            Clear();
            IsUpdateMovingShape = true;
            this.TotalMoveLength = 0;
            this.TotalCutLength = 0;
            this.FileFullName = this.fileFullName;

            laserHeadCircle.CenterX = 0;
            laserHeadCircle.CenterY = 0;
            laserHeadCircle.Radius = 30;
            laserHeadCircle.IsShow = false;
            laserHeadCircle.IsNonTrans = true;
            ShapeMeta meta = new ShapeMeta();
            meta.IsFill = false;
            meta.BackColor = Color.FromArgb(255, 128, 0);
            meta.ForeColor = Color.Black;
            meta.LineWidth = 2;
            laserHeadCircle.SetDrawMeta(meta);
            if (Settings.SHOW_LASER_CIRCLE)
            {
                this.PainterUC.GetFreshLayerManager().Add(laserHeadCircle);
            }
            gCodeInterpreter1.OnMsg += (msg) => {
                if (msg.Contains("开始切割"))
                {
                    OnCutting(true);
                    (laserHeadCircle.GetDrawMeta() as ShapeMeta).IsFill = true;
                    trackLines.GetDrawMeta().LineWidth = 4;
                    trackLines.GetDrawMeta().ForeColor = this.LineColor;
                }
                if (msg.Contains("BeamOff"))
                {
                    OnCutting(false);
                    (laserHeadCircle.GetDrawMeta() as ShapeMeta).IsFill = false;
                    trackLines.GetDrawMeta().LineWidth = 3;
                    trackLines.GetDrawMeta().ForeColor = Color.Black;
                    //							trackLines.GetDrawMeta().DashLineStyle=new float[]{2f,4f};
                }
                PainterUC.OnMessage(msg);
                if (OnMsg != null)
                {
                    OnMsg.Invoke(msg);
                }
            };
            this.OnAShaftUpdate += (moveDistance) => {
                laserHeadCircle.CenterX = (float)gCodeInterpreter1.CurrentX;
                laserHeadCircle.CenterY = (float)gCodeInterpreter1.CurrentY;
                //if (IsUpdateMovingShape) {
                //	this.PainterUC.GetMoveableLayerManager().OffsetShapes((float)moveDistance, 0);

                //} else {
                //	this.PainterUC.GetMoveableLayerManager().Clear();
                //}
                if (OnHeadPosition != null)
                {//显示坐标label
                    OnHeadPosition(this.gCodeInterpreter1.CurrentX, this.gCodeInterpreter1.CurrentY);
                }
                PainterUC.Invalidate();
            };
            if (IsUniersalGCode)
            {
                if (!UniversalGCodeParser.Instance.SetGCodeInterperter(gCodeInterpreter1).InitializeGCodeInterperter(listGCodeStr))
                {
                    throw new LaserException("ERROR", "文件读取异常，UniversalGCodeParser初始化InitializeGCodeInterperter失败");
                }
            }
            else
            {
                if (!GCodeParser.Instance.SeperateGCodes(strValidCodes).SetGCodeInterperter(gCodeInterpreter1).InitializeGCodeInterperter())
                {
                    throw new LaserException("ERROR", "文件读取异常，GCodeParser初始化InitializeGCodeInterperter失败");
                }
            }
            this.gCodeInterpreter1.Init();
            ShapeMeta cutMeta = new ShapeMeta();
            cutMeta.IsFill = false;
            cutMeta.BackColor = Color.Black;
            cutMeta.ForeColor = Color.Red;
            cutMeta.LineWidth = 2;
            cutMeta.CapStyle = System.Drawing.Drawing2D.LineCap.Round;
            this.gCodeInterpreter1.SetCutMetaStyle(cutMeta);
            List<Shape> shapes = gCodeInterpreter1.GetShapes();

            trackLines.SetPoints(gCodeInterpreter1.GetTrackPoints());
            this.PainterUC.GetMoveableLayerManager().LoadShape(shapes);
            this.PainterUC.GetHoldLayerManager().Add(trackLines);
            if (!IsUniersalGCode)
            {
                InitSafeBorder();
            }
            this.PainterUC.Invalidate();
            return true;
        }

        private void InitSafeBorder()
        {
            leftSafeLine.FirstPoint = new PointGeo(gCodeInterpreter1.StartPoint.X, (float)gCodeInterpreter1.MinY);
            leftSafeLine.SecondPoint = new PointGeo(gCodeInterpreter1.StartPoint.X, (float)gCodeInterpreter1.MaxY);
            rightSafeLine.FirstPoint = new PointGeo(gCodeInterpreter1.StartPoint.X + (float)Settings.MOVE_RANGE, (float)gCodeInterpreter1.MinY);
            rightSafeLine.SecondPoint = new PointGeo(gCodeInterpreter1.StartPoint.X + (float)Settings.MOVE_RANGE, (float)gCodeInterpreter1.MaxY);

            ShapeMeta safeLineStyle = new ShapeMeta();
            safeLineStyle.IsFill = false;
            safeLineStyle.BackColor = Color.White;
            safeLineStyle.ForeColor = Color.Lime;
            safeLineStyle.LineWidth = 3;
            leftSafeLine.SetDrawMeta(safeLineStyle);
            this.PainterUC.GetFreshLayerManager().Add(leftSafeLine);
            rightSafeLine.SetDrawMeta(safeLineStyle);
            this.PainterUC.GetFreshLayerManager().Add(rightSafeLine);

            TextMeta leftTextMeta = new TextMeta("左边界");
            leftTextMeta.ForeColor = Color.Black;
            leftTextMeta.TEXTFONT = new Font("宋体", 18, FontStyle.Bold);
            leftBorderText.SetDrawMeta(leftTextMeta);
            leftBorderText.pos = new PointGeo(leftSafeLine.FirstPoint.X, leftSafeLine.FirstPoint.Y);
            this.PainterUC.GetFreshLayerManager().Add(leftBorderText);

            TextMeta rightTextMeta = new TextMeta("右边界");
            rightTextMeta.ForeColor = Color.Black;
            rightTextMeta.TEXTFONT = new Font("宋体", 18, FontStyle.Bold);
            rightBoderText.SetDrawMeta(rightTextMeta);
            rightBoderText.pos = new PointGeo(rightSafeLine.FirstPoint.X, rightSafeLine.FirstPoint.Y);
            this.PainterUC.GetFreshLayerManager().Add(rightBoderText);
        }
        public int No = 0;
        public string CalASpeed()
        {
            double milliSecs = (1f * ProgramController.Instance.TICK_COUNT * Settings.TIME_SPAN) + this.gCodeInterpreter1.GetProcessors()[0].CutGeo.GetPerimeter() / this.gCodeInterpreter1.GetProcessors()[0].cutSpeed * 60;
            double seconds = milliSecs / 1000;
            double maxASpeed = Settings.CYCLE_LENGTH / seconds * 60 / Settings.SPEED_RATE;
            return string.Format("[MAX：{0:f3}]", maxASpeed);
        }
        public void StartWorker()
        {
            if (gCodeInterpreter1 == null || gCodeInterpreter1.GetProcessors().Count == 0)
            {
                return;
            }
            gCodeInterpreter1.SetLoopGeo();
            laserHeadCircle.IsShow = true;
        }
        public void StopWorker()
        {
            if (gCodeInterpreter1 != null)
            {
                gCodeInterpreter1.Stop();
            }
        }
        public List<string> GetNCCodes()
        {
            return this.listGCodeStr;
        }
        public bool ReadNCFile()
        {
            this.listGCodeStr.Clear();
            this.strValidCodes.Clear();

            if (!File.Exists(fileFullName))
            {
                return false;
            }

            if (this.gCodeInterpreter1 != null)
            {
                this.gCodeInterpreter1.GetTrackPoints().Clear();
                //this.PainterUC.Clear();
            }
            using (StreamReader sr = File.OpenText(fileFullName))
            {
                try
                {
                    string str = "";
                    while (true)
                    {
                        str = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(str) && str != "")
                        {
                            listGCodeStr.Add(str);
                            if (GCodeParser.Instance.IsValidLine(str))
                            {
                                strValidCodes.Add(str);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    throw new LaserException("ERROR", "NC 文件读取失败");
                }
            }
            return InitLaserWorker();
        }

    }
}


