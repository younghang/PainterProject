/*
 * Created by SharpDevelop.
 * User: yanghang
 * Date: 2021/9/7
 * Time: 21:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic; 
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Painter.Models;

namespace Painter.Controller
{

    /// <summary>
    /// 负责管理多个PainterViewModel的协同关系
    /// </summary>
    public class ProgramController
    {
        public int TICK_COUNT = 0;
        public static ProgramController Instance = new ProgramController();
        private ProgramController()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Elapsed -= TimerTick;
            }
            else
            {
                timer = new System.Timers.Timer();
                timer.Elapsed += TimerTick;
            }
        }
        public event Action<string,LaserHeadWorker> OnMsg;
		public event Action<bool,LaserHeadWorker> OnCutting;//切割光开就为true
		public event Action<double,double,LaserHeadWorker> OnHeadPosition;
		public event Action<double> OnAShaftUpdate;
        private System.Timers.Timer timer = null;
        public List<LaserHeadWorker> workers = new List<LaserHeadWorker>();
        public int WORKER＿READY_COUNT = 0;
        public int WORKER＿FINISH_COUNT = 0;
        public int TOTAL_WORKER_COUNT = 0;
        public ManualResetEvent AllReadyMonitor = new ManualResetEvent(false);
        public ManualResetEvent FinishedMonitor = new ManualResetEvent(false);
        public Action OnTimerTick;
        private double _minX;
		private double _minY;
		private double _maxX;
		private double _maxY;
		public double MinX { get { return _minX; } set { _minX = value; } }
		public double MinY { get { return _minY; } set { _minY = value; } }
		public double MaxX { get { return _maxX; } set { _maxX = value; } }
		public double MaxY { get { return _maxY; } set { _maxY = value; } }
        public void Start()
        {
            IsStopRun = false;
        	if (this.workers.Count==0) {
        		return;
        	}
            ProgramController.Instance.TICK_COUNT = 0; 
         
            foreach (var item in this.workers)
            {
                item.StartWorker();
            }
            timer.Interval = Settings.TIME_SPAN; 
            timer.Start();
        }
        public void Init()
        {
        	this.Clear();
            this.MaxX = 0;
            this.MinX = 1E6;
            this.MaxY = 0;
            this.MinY = 1E6;
            TOTAL_WORKER_COUNT =0;
        	WORKER＿READY_COUNT=0;
        	TICK_COUNT=0;
            this.OnTimerTick = null;
            this.OnAShaftUpdate = null;
            IsStopRun = false;
        }
        public void Stop()
        {
            IsStopRun = true;
            foreach (var item in this.workers)
            {
                item.StopWorker();
            } 
        }
        public void Resume()
        {
            this.IsStopRun = false;
            this.timer.Start();
        }
        public void Clear()
        { 
        	foreach (var item in this.workers)
            { 
                this.OnTimerTick -= item.OnTimerTick;
                this.OnAShaftUpdate -=item.OnAShaftUpdate;
                item.Clear();
            }
            this.workers.Clear();
            if (this.OnTimerTick!=null)
            {
                foreach (var item in this.OnTimerTick.GetInvocationList())
                {
                    this.OnTimerTick -= (Action)item;
                }
            }
            if (this.OnAShaftUpdate!=null)
            {
                foreach (var item in this.OnAShaftUpdate.GetInvocationList())
                {
                    this.OnAShaftUpdate -= (Action<double>)item;
                } 
            }
            

        }
        private bool IsStopRun = false;
        public bool IsStartCount = false;
        public bool IsNoWait = false;
        public bool IsUniversalGCode = false;
        public void AddLaserHead(LaserHeadWorker worker)
        {
        	this.workers.Add(worker); 
        	worker.OnCutting+= (isCutting) => this.OnCutting(isCutting, worker);
        	worker.OnMsg+= (msg) => this.OnMsg(msg, worker);
        	worker.OnHeadPosition+=(x,y)=>this.OnHeadPosition(x,y,worker); 
        	this.OnTimerTick +=worker.OnTimerTick;
        	this.OnAShaftUpdate+=worker.OnAShaftUpdate; 
        	TOTAL_WORKER_COUNT++;
            UpdateMaxMin(worker);
        }
        public void UpdateMaxMin(LaserHeadWorker worker)
        {
            if (worker.MinX < this.MinX)
            {
                this.MinX = worker.MinX;
            }
            if (worker.MaxX > this.MaxX)
            {
                this.MaxX = worker.MaxX;
            }
            if (worker.MinY < this.MinY)
            {
                this.MinY = worker.MinY;
            }
            if (worker.MaxY > this.MaxY)
            {
                this.MaxY = worker.MaxY;
            }
        }
        public void RemoveLaserHead(LaserHeadWorker worker)
        {
            this.workers.Remove(worker);
            worker.OnCutting -= (isCutting) => this.OnCutting(isCutting, worker);
            worker.OnMsg -= (msg) => this.OnMsg(msg, worker);
            worker.OnHeadPosition -= (x, y) => this.OnHeadPosition(x, y, worker);
            this.OnTimerTick -= worker.OnTimerTick;
            this.OnAShaftUpdate -= worker.OnAShaftUpdate; 
            TOTAL_WORKER_COUNT--;
        }
        public double AShaftMoveDistance=0;

        private void TimerTick(object sender, System.Timers.ElapsedEventArgs e)
        { 
            if(this.workers.Count==0)
            {
                this.timer.Stop();
                return;
            }

            if (WORKER＿READY_COUNT == TOTAL_WORKER_COUNT)
            {
                AllReadyMonitor.Set();
            }
            //					if (ProgramController.WORKER＿FINISH_COUNT == ProgramController.TOTAL_WORKER_COUNT) {
            //						ProgramController.FinishedMonitor.Set();
            //						ProgramController.WORKER＿FINISH_COUNT=0;
            //					}
            if (IsStartCount)
            {
                TICK_COUNT++;//计时器 	 
            } 
            AShaftMoveDistance = Settings.A_SHAFT_SPEED / 60 / 1000 * Settings.TIME_SPAN * Settings.SPEED_RATE;
            
            OnAShaftUpdate(AShaftMoveDistance);
            
            if (IsStopRun)
            {
                timer.Stop();
                IsStopRun = false;
            }
            if (this.OnTimerTick!=null)
            {
                OnTimerTick();
            } 
            //timer.Stop();
        }
    }

}