using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Painter.Controller;
using Painter.DisplayManger;
 
using Painter.Models;
using Painter.Painters;
using Painter.Utils;
using Painter.View.Window;
using Utils;
using Utils.DisplayManger.Axis;

namespace Painter
{
    public partial class GCodeLaserHeadForm : Form
    {
        public GCodeLaserHeadForm()
        {
            InitializeComponent();
            this.Paint += FormTestcs_Paint;
            this.FormClosed += (o, ef) => ProgramController.Instance.Stop();

            this.FormClosing += OnClosing;
            this.Load += (o, e) =>
            {
                SetLaserText(false, null);
                this.KeyPreview = true;
            };
            this.notifyIcon1.Click += notifyIcon1_Click;
            this.notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            InitPainterUC();
            InitProgram();

        }
        private void InitPainterUC()
        {
            PainterUCManager.Instance.AddPainterUC(this.winFormCanvas);
        }
        private void InitProgram()
        {
           
            ProgramController.Instance.OnCutting += (iscutting, laserHead) =>
            {
                this.Invoke(new Action(() => { SetLaserText(iscutting, laserHead); }));
            };
            ProgramController.Instance.OnMsg += (msg, laserHead) =>
            {
                if (msg.Contains("ERROR"))
                {
                    MessageBox.Show(laserHead.FileFullName+"\n" + msg);
                    return; 
                }
                string totalTime = MyUtils.GetTimeSpan();
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        RichTextBox richMsg = this.tabMsg.TabPages[laserHead.Name].Controls[RichMsgName] as RichTextBox;
                        richMsg.AppendText("[" + totalTime + "]" + msg);
                        this.statusMsg.Text = msg.Trim();
                        if (msg.ToUpper().StartsWith("FINISH"))
                        {
                            this.btnStart.Enabled = true;
                        }
                        if (msg.Contains("2次循环"))
                        {
                            laserHead.IsUpdateMovingShape = false;
                            toolStripStatusASpeed.Text = "A轴速度：" + Settings.A_SHAFT_SPEED.ToString("f2") + " mm/min"+ laserHead.CalASpeed();
                        }
                    }));

                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }
            };
            ProgramController.Instance.OnHeadPosition += (x, y, laserHead) =>
              {
                  EndTime = DateTime.Now;
                  string totalTime = MyUtils.GetTimeSpan();
                  this.Invoke(new Action(() =>
                  {
                      //laserHead.PainterUC.SetText();
                      this.lblTime.Text = totalTime;
                      this.label1.Text = MyUtils.GetTimeSpan(EndTime, StartTime);
                      this.lblPosition.Text = "(" + x.ToString("f2") + "," + y.ToString("f2") + ")";
                  }));
              };
            
        }
        private void InitSingleWorker()
        {
            this.workers.Clear();
            LaserHeadWorker worker1 = new LaserHeadWorker(this.winFormCanvas, "Head1");
            worker1.IsUniersalGCode = IsUniversalGCode;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                string fileFullName = openFileDialog.FileName;
                if (!File.Exists(fileFullName))
                {
                    return;
                }
                worker1.FileFullName = fileFullName;
            }
            this.workers.Add(worker1);
            this.tabGCode.TabPages.Clear();
            this.tabMsg.TabPages.Clear();
            InitPageControlByWorker(worker1);
        }
        DateTime StartTime;
        DateTime EndTime;
        private void FormTestcs_Paint(object sender, PaintEventArgs e)
        {
            //            Graphics graphics = e.Graphics;
            //            graphics.DrawArc(new Pen(Color.Red, 2), new Rectangle(00, 100, 200, 200), -270, 60);
        }

        void BtnStartClick(object sender, EventArgs e)
        {
            isStop = false;
            this.btnStop.Text = "停止";
            //ProgramController.WORKER＿FINISH_COUNT=0;
            //ProgramController.WORKER＿READY_COUNT=0;
            //ProgramController.AllReadyMonitor.Reset(); 
            ProgramController.Instance.IsNoWait = this.ckWait.Checked;
            if (IsUniversalGCode)
            {
                ProgramController.Instance.IsNoWait = true;
            }
            if (ProgramController.Instance.IsNoWait)
            {
                Settings.A_SHAFT_SPEED = 0;
            } 
            ProgramController.Instance.Start();
            this.btnStart.Enabled = false;
            lblTime.Text = "00:00.000";
            StartTime = DateTime.Now;
            this.toolStripStatusASpeed.Text = "A轴速度：" + Settings.A_SHAFT_SPEED.ToString("f2") + " mm/min";
        }
        private bool isStop = false;
        void BtnStopClick(object sender, EventArgs e)
        {
            isStop = !isStop;
            if (isStop)
            {
                this.btnStop.Text = "继续";
                this.btnStart.Enabled = true;
                ProgramController.Instance.Stop();
            }
            else
            {
                this.btnStop.Text = "停止";
                this.btnStart.Enabled = false;
                ProgramController.Instance.Resume();
            }


            //ProgramController.AllReadyMonitor.Reset();
            //ProgramController.FinishedMonitor.Reset();

        }
        void BtnClearClick(object sender, EventArgs e)
        {
            ProgramController.Instance.Clear();
            foreach (TabPage item in this.tabGCode.TabPages)
            {
                ListBox listBox = item.Controls[ListCodeName] as ListBox;
                listBox.Items.Clear();
            }
            this.workers.Clear();
            PainterUCManager.Instance.Clear();
            Invalidate();

        }
        void LblPositionClick(object sender, EventArgs e)
        {
            CalculatorDll.Calculator.CalculatorForm cl = new CalculatorDll.Calculator.CalculatorForm();
            cl.Show();
        }
      
        private bool IsYUpDirection = false;
        private bool IsNoWait = false;
        private bool LoadNCFile(LaserHeadWorker laser)
        {
            ListBox listCode = this.tabGCode.TabPages[laser.Name].Controls[ListCodeName] as ListBox;
            RichTextBox richMsg = this.tabMsg.TabPages[laser.Name].Controls[RichMsgName] as RichTextBox;
            listCode.Items.Clear();
            richMsg.Text = "";
            IsNoWait = ckWait.Checked;
            IsYUpDirection = ckXAxisUp.Checked;
            PainterUCManager.IsYUpDirection = IsYUpDirection;
            if (!laser.ReadNCFile())
            {
                return false;
            }
            if (!IsUniversalGCode)
            {
                richMsg.AppendText(laser.GenerateLaserHeadDescription());
            } 
            this.statusMsg.Text = "加载完成";
            listCode.Items.AddRange(laser.GetNCCodes().ToArray());
            if (IsUniversalGCode)
            { 
                listCode.SelectedIndexChanged += ListCode_SelectedIndexChanged;
                listCode.DrawMode = DrawMode.OwnerDrawFixed; 
                listCode.DrawItem += ListCode_DrawItem; 
                listCode.MeasureItem += ListCode_MeasureItem;
                listCode.DoubleClick += ListCode_DoubleClick;
            } 
            return true;
        }

        private void ListCode_DoubleClick(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            EditCodeFrm.Instance.StrCode = listBox.Items[listBox.SelectedIndex].ToString();
            if (UniversalGCodeParser.Instance.LinesIndexedProcessor.ContainsKey(listBox.SelectedIndex))
            {
                EditCodeFrm.Instance.CurShape = UniversalGCodeParser.Instance.LinesIndexedProcessor[listBox.SelectedIndex].CutGeo.GetShapes()[0];
            }else
            {
                EditCodeFrm.Instance.CurShape = null;
            }
            if (EditCodeFrm.Instance.ShowDialog()==DialogResult.OK)
            {
                listBox.Items[listBox.SelectedIndex] = EditCodeFrm.Instance.StrCode;
            }
            //EditCodeFrm.Instance.CurShape = null;
        }

        private void ListCode_MeasureItem(object sender, MeasureItemEventArgs e)
        { 
            e.ItemHeight = (sender as ListBox).Font.Height+12;
        }

        private void ListCode_DrawItem(object sender, DrawItemEventArgs e)
        {
            (sender as ListBox).ItemHeight = e.Font.Height;
            int index = e.Index;
            if (index==-1)
            {
                return;
            }
            bool IsValidLine = false;
            if (UniversalGCodeParser.Instance.LinesIndexedProcessor.ContainsKey(index))
            {
                if (IsUniversalGCode)
                {
                    IsValidLine=true;
                }
            } 
            Color color = e.BackColor;
            if (IsValidLine)
            {
                color = Color.LightSteelBlue;
            }
            e.Graphics.FillRectangle(new SolidBrush(color), e.Bounds);  //new Rectangle(e.Bounds.X, e.Font.Height*index, e.Bounds.Width, e.Font.Height)
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font,
                new SolidBrush(e.ForeColor), e.Bounds);
            e.DrawFocusRectangle();
        }
         
        Color DefaultColor=Color.Red;
        int previewIndex = 0;
        private void ListCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            int index = listBox.SelectedIndex;
            
            if (!UniversalGCodeParser.Instance.LinesIndexedProcessor.ContainsKey(index))
            {
                SetSelectColor(previewIndex, DefaultColor, true);
                previewIndex = 0;
                this.winFormCanvas.Invalidate(true); 
                return;
            }
            if (previewIndex==0)
            {
                previewIndex = index;
            }
            SetSelectColor(previewIndex, DefaultColor,true);
            SetSelectColor(index,Color.Lime,false);
            previewIndex = index;
            this.winFormCanvas.Invalidate(true);
        }
        ShapeMeta oldMeta; 
        private void SetSelectColor(int index,Color color,bool isRestore)
        {
            if (index < 0)
            {
                return;
            }
            if (!IsUniversalGCode)
            {
                return;
            }
            if (!UniversalGCodeParser.Instance.LinesIndexedProcessor.ContainsKey(index))
            {
                return;
            }
            GeoProcessorBase processor = UniversalGCodeParser.Instance.LinesIndexedProcessor[index];
            if (isRestore)
            {
                if (oldMeta!=null)
                {
                    processor.CutGeo.GetShapes()[0].SetDrawMeta(oldMeta);
                }
                
            }else
            {
                oldMeta = (ShapeMeta)processor.CutGeo.GetShapes()[0].GetDrawMeta();
                if (oldMeta==null)
                {
                    return;
                }
                ShapeMeta shapeMata = MyUtils.CloneObject<ShapeMeta>(processor.CutGeo.GetShapes()[0].GetDrawMeta());
                shapeMata.ForeColor = color;
                shapeMata.LineWidth = 4;
                processor.CutGeo.GetShapes()[0].SetDrawMeta(shapeMata); 
            } 
        }

        private static string ListCodeName = "listGCode";
        private static string RichMsgName = "richMsg";
        private void InitPageControlByWorker(LaserHeadWorker worker)
        {
            Action<Control> zoom = (control) => {
                control.MouseWheel += (o, e) =>
                  {
                      if (Control.ModifierKeys==Keys.Control)
                      {
                          if (e.Delta > 0)
                          {
                              float fontsize = control.Font.Size * 1.2f;
                              control.Font = new Font(control.Font.Name, control.Font.Size * 1.1f);
                          }
                          else
                          {
                              float fontsize = control.Font.Size * 0.8f;
                              control.Font = new Font(control.Font.Name, fontsize > 6 ? fontsize : 6);
                          }
                          control.Refresh();
                      } 
                  };
            };
            ListBox listGCodeBox = new ListBox();
            listGCodeBox.BackColor = Settings.BG_COLOR;
            listGCodeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            listGCodeBox.ForeColor = Settings.ANTI_BG;
            listGCodeBox.FormattingEnabled = true;
            listGCodeBox.ItemHeight = 18;
            listGCodeBox.Location = new System.Drawing.Point(3, 4);
            listGCodeBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            listGCodeBox.Name = ListCodeName;
            listGCodeBox.Size = new System.Drawing.Size(376, 1306);
            listGCodeBox.Font = Settings.G_CODE_TEXT_FONT;
            zoom(listGCodeBox);
            TabPage tabPageCode = new TabPage();
            tabPageCode.Text = worker.Name;
            tabPageCode.Name = worker.Name;
            tabPageCode.Controls.Add(listGCodeBox);
            tabGCode.Controls.Add(tabPageCode);

            RichTextBox rich = new RichTextBox();
            rich.BackColor = Settings.BG_COLOR;// System.Drawing.SystemColors.GrayText;
            rich.Dock = System.Windows.Forms.DockStyle.Fill;
            rich.ForeColor = Settings.ANTI_BG;
            rich.Location = new System.Drawing.Point(3, 4);
            rich.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            rich.Size = new System.Drawing.Size(1370, 408);
            rich.Name = RichMsgName;
            rich.Text = "";
            rich.Font = Settings.LOG_TEXT_FONT;
            zoom(rich); 
            rich.TextChanged += (o, ee) =>
            {
                rich.SelectionStart = rich.Text.Length;
                rich.ScrollToCaret();
            };
            TabPage tabPageMsg = new TabPage();
            tabPageMsg.Text = worker.Name;
            tabPageMsg.Name = worker.Name;
            tabPageMsg.Controls.Add(rich);
            tabMsg.Controls.Add(tabPageMsg);
        }
        int SelectIndex = 0;
        private void AddMultiHeadCmd_Click(object sender, EventArgs e)
        {
            IsUniversalGCode = false;
            this.workers.Clear();
            if (SelectFileFrm.Instance.ShowDialog() == DialogResult.OK)
            {
                List<NCFileInfo> nCFileInfos = SelectFileFrm.Instance.GetNCFileInfos();
                if (nCFileInfos.Count == 0)
                {
                    return;
                }
                List<IPainterUC> painterUCs = new List<IPainterUC>() { this.winFormCanvas };
                int index = 0;
                tabGCode.Controls.Clear();
                tabMsg.Controls.Clear();
                foreach (var item in nCFileInfos)
                {

                    LaserHeadWorker worker = new LaserHeadWorker(painterUCs[index], item.Name);
                    worker.FileFullName = item.FilePath;
                    worker.LineColor = item.FillColor;
                    this.workers.Add(worker);
                    index++;
                    if (index >= painterUCs.Count)
                    {
                        index = 0;
                    }
                    InitPageControlByWorker(worker);
                }
                Init();
            }
        }
        List<LaserHeadWorker> workers = new List<LaserHeadWorker>();
        void Init()
        {
            this.winFormCanvas.Clear();
            ProgramController.Instance.Init();
            foreach (var item in this.workers)
            {
                try
                {
                    if (LoadNCFile(item))
                    {
                        ProgramController.Instance.AddLaserHead(item);
                        //ProgramController.Instance.RemoveLaserHead(item); 
                    }
                }
                catch (LaserException e)
                { 
                    MessageBox.Show(e.ToString());
                    return;
                } 
            }
            //SetCanvasScale();
            PainterUCManager.Instance.SetCanvasScale(this.winFormCanvas.Width,this.winFormCanvas.Height); 
            ProgramController.Instance.OnAShaftUpdate += (x) => {
                if (!IsUniversalGCode)
                {
                    PainterUCManager.Instance.HorizontalOffSetContent(x);
                }
                
            };
        }
       
        void SettingFrmCmdClick(object sender, EventArgs e)
        {
            SettingForm frm = SettingForm.Instance;
            frm.UpdateStatus += () => {
                toolStripStatusASpeed.Text = "A轴速度：" + Settings.A_SHAFT_SPEED.ToString("f2") + " mm/min";

            };
            frm.Show();
        }
        private void SetLaserText(bool isLaserOn, LaserHeadWorker worker)
        {
            if (Settings.SHOW_LASER_CIRCLE)
            {
                if (isLaserOn)
                {
                    lblLaserStatus.Text = "激光开";
                    lblLaserStatus.ForeColor = Color.White;
                    lblLaserStatus.BackColor = Color.FromArgb(255, 128, 0);
                }
                else
                {
                    lblLaserStatus.Text = "激光关";
                    lblLaserStatus.ForeColor = Color.Black;
                    lblLaserStatus.BackColor = Color.White;
                }
            } 
        } 

        #region 托盘
        bool IsClosed = false;
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            //			if (!IsClosed) {
            //				e.Cancel = true;
            //				this.Hide();
            //				return;
            //			}
        }
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            IsClosed = true;
            this.Close();
        }

        #endregion

        private void tabMsg_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = tabMsg.SelectedIndex;
            SelectIndex = index;
            if (SelectIndex<0)
            {
                SelectIndex = 0;
            }
            tabGCode.SelectedIndex = index;
        }

        private void btnShowChart_Click(object sender, EventArgs e)
        {
            if (this.workers.Count>1)
            {
                return;
            }
            AxisModel axis= CreateAxisModel();
            ChartForm formChart = new ChartForm();
            formChart.SetAxisModel(axis);
            formChart.Show();
        }

        private AxisModel CreateAxisModel()
        {
            //string name = "X_POS_CHART";
            AxisModel axis = new AxisModel();
            axis.Title = "激光头横向位置坐标变化mm";
            axis.XLabel = "时间Tick";
            axis.MaxX = 1;
            axis.MinX = 0;
            axis.MaxY = ProgramController.Instance.MaxX;
            axis.MinY = ProgramController.Instance.MinX;
            ProgramController.Instance.OnHeadPosition += (x, y, laserHead) => {
                //if (laserHead.Name== ProgramController.Instance.workers[SelectIndex].Name)
                //{
                //}
                axis.AddPoint(laserHead.Name, new PointGeo((float)(ProgramController.Instance.TICK_COUNT * 1.0f * Settings.TIME_SPAN / 1000 * Settings.SPEED_RATE), (float)x));
            };
            ProgramController.Instance.OnAShaftUpdate += (offset) => {
                axis.OnInvalidate();
            };
            return axis;
        }
        public bool IsUniversalGCode = false;
        private void WatchCmd_Click(object sender, EventArgs e)
        {
            IsUniversalGCode = true;
            ProgramController.Instance.IsUniversalGCode = IsUniversalGCode;
            InitSingleWorker();
            Init();
        }
        private void OpenCmdClick(object sender, EventArgs e)
        {
            IsUniversalGCode = false;
            ProgramController.Instance.IsUniversalGCode = IsUniversalGCode;  
            InitSingleWorker();
            Init();
        }

        private void SaveCmd_Click(object sender, EventArgs e)
        {
            if (!IsUniversalGCode)
            {
                return;
            }
            ListBox listCode = this.tabGCode.TabPages[workers[0].Name].Controls[ListCodeName] as ListBox;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < listCode.Items.Count; i++)
            {
                stringBuilder.AppendLine(listCode.Items[i].ToString());
            }
            string str = stringBuilder.ToString();
            FileUtils.SaveFile(workers[0].FileFullName , str);
            MessageBox.Show("文件已保存");
        }
    }
}
