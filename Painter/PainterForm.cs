using Painter.Models;
using Painter.Painters;
using Painter.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter
{
	public partial class PainterForm : Form
	{
		public static int TICK_COUNT = 0;
		public PainterForm()
		{
			InitializeComponent();
			this.FormClosing += (o, ef) => {
				if (gCodeInterpreter1 != null) {
					gCodeInterpreter1.Stop();
				}
				
			};
			this.richTextBox1.TextChanged += (o, e) => {
				richTextBox1.SelectionStart = richTextBox1.Text.Length;
				richTextBox1.ScrollToCaret();
			};
		}
		string fileFullName = "";
		// "nc_1.txt";
		GGodeInterperter gCodeInterpreter1;
		private RandomLines trackLines = new RandomLines();
		private CircleGeo laserHeadCircle = new CircleGeo();
		private LineGeo leftSafeLine = new LineGeo();
		private DrawableText leftBorderText=new DrawableText();
		private LineGeo rightSafeLine = new LineGeo();
		private DrawableText rightBoderText=new DrawableText();
		private bool IsUpdateShape = true;
		private bool IsShowLaserHeadCircle = true;

		string CalASpeed()
		{
			double milliSecs = (1f * PainterForm.TICK_COUNT * Settings.TIME_SPAN)+this.gCodeInterpreter1.GetProcessors()[0].CutGeo.GetPerimeter()/this.gCodeInterpreter1.GetProcessors()[0].cutSpeed*60;
			double seconds = milliSecs / 1000;
			double maxASpeed = Settings.CYCLE_LENGTH / seconds * 60/Settings.SPEED_RATE ;
			return string.Format("[MAX：{0:f3}]", maxASpeed);
		}

		private void OpenCmd_Click(object sender, EventArgs e)
		{
			listGCode.Items.Clear();
			richTextBox1.Text = "";
			
			using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
				if (openFileDialog.ShowDialog() != DialogResult.OK) {
					return;
				}
				fileFullName = openFileDialog.FileName;
				if (!File.Exists(fileFullName)) {
					return;
				}
			}
			if (this.gCodeInterpreter1!=null) {
				this.gCodeInterpreter1.GetTrackPoints().Clear();
				this.winFormUC1.Clear(); 
			}
			
			
			List<string> strCodes = new List<string>();
			using (StreamReader sr = File.OpenText(fileFullName)) {
				string str = "";
				while (true) {
					str = sr.ReadLine();
					if (str != null) {
						listGCode.Items.Add(str);
						if (GCodeParser.Instance.IsValidLine(str)) {
							strCodes.Add(str);
						}
					} else {
						break;
					}
				}
			}
			gCodeInterpreter1 = new GGodeInterperter(); 
			gCodeInterpreter1.OnMsg += (msg) => {
				EndTime = DateTime.Now;
				string totalTime = MyUtils.GetTimeSpan();
				try {
					this.Invoke(new Action(() => {
						this.richTextBox1.AppendText("[" + totalTime + "]" + msg);
						this.statusMsg.Text = msg.Trim();
						if (msg.ToUpper().StartsWith("FINISH")) {
							this.btnStart.Enabled = true;
						}
						if (msg.Contains("次循环")) {
							//this.winFormUC1.GetShapeLayerManager().OffsetShapes((float)-Settings.CYCLE_LENGTH, 0);//这样就是循环
							IsUpdateShape = false; //这样就是隐藏
					                       		
							if (msg.Contains("2次循环")) {
								toolStripStatusASpeed.Text += this.CalASpeed();
							}
						}
						if (msg.Contains("BeamOn")) {
							SetLaserText(true);
							(laserHeadCircle.GetDrawMeta() as ShapeMeta).IsFill = true;
							trackLines.GetDrawMeta().LineWidth = 4;
							trackLines.GetDrawMeta().ForeColor = Color.Blue;
						}
						if (msg.Contains("BeamOff")) {
							SetLaserText(false);
							(laserHeadCircle.GetDrawMeta() as ShapeMeta).IsFill = false;
							trackLines.GetDrawMeta().LineWidth = 3;
							trackLines.GetDrawMeta().ForeColor = Color.Black;
//							trackLines.GetDrawMeta().DashLineStyle=new float[]{2f,4f};
						}
						IsShowLaserHeadCircle = true;
					}));
					
				} catch (Exception) {

				}
			};
//			gCodeInterpreter1.OnAShaftUpdate += (moveDistance) => {
//				laserHeadCircle.CenterX = (float)gCodeInterpreter1.CurrentX;
//				laserHeadCircle.CenterY = (float)gCodeInterpreter1.CurrentY;
//				EndTime = DateTime.Now;
//				string totalTime = CommonUtils.GetTimeSpan();
//				try {
//					this.BeginInvoke(new Action(() => {
//						if (IsUpdateShape) {
//							this.winFormUC1.GetShapeLayerManager().OffsetShapes((float)moveDistance, 0);
//						} else {
//							this.winFormUC1.GetShapeLayerManager().Clear();
//						}
//						if (IsShowLaserHeadCircle) {
//							this.winFormUC1.GetShapeLayerManager().Add(laserHeadCircle);
//							laserHeadCircle.IsShow = true;
//						}
//						//this.winFormUC1.SetIPS(gCodeInterpreter1.GetTrack());
//						//this.winFormUC1.GetTrackLayerManager().Add(gCodeInterpreter1.GetLatestLine());
//						this.winFormUC1.Invalidate();
//					                            	
//						this.lblTime.Text = totalTime;
//						this.label1.Text = CommonUtils.GetTimeSpan(EndTime, StartTime);
//						this.lblPosition.Text = "(" + gCodeInterpreter1.CurrentX.ToString("f2") + "," + gCodeInterpreter1.CurrentY.ToString("f2") + ")";
//					                            	
//					}));
//					
//				} catch (Exception) {
//					
//				}
//			};
			GCodeParser.Instance.SeperateGCodes(strCodes).SetGCodeInterperter(gCodeInterpreter1).InitializeGCodeInterperter();
			//this.richTextBox1.AppendText(this.gCodeInterpreter1.GenerateDescription());
			List<Shape> shapes = gCodeInterpreter1.GetShapes();
			//if (ckXAxisUp.Checked) {
			//	WinFormPainter.OffsetPoint = new PointGeo(100, this.winFormUC1.Height*0.9f);//(this.winFormUC1.GetShapeLayerManager().GetPainter() as WinFormPainter).OffsetPoint = new PointGeo(100, 0);
			//} else {
			//	WinFormPainter.OffsetPoint = new PointGeo(100, winFormUC1.Height*0.1f);//(this.winFormUC1.GetShapeLayerManager().GetPainter() as WinFormPainter).OffsetPoint = new PointGeo(100, 0);
			//}
			//float ratioX = (float)((this.winFormUC1.Width - 100) * 1.0f / (gCodeInterpreter1.MaxX+Settings.MOVE_RANGE+200));
			//float ratioY = (float)((this.winFormUC1.Height * 0.8f) / (gCodeInterpreter1.MaxY - gCodeInterpreter1.MinY));
			//float ratio = Math.Min(ratioY, ratioX);
			//if (ckXAxisUp.Checked) {
			//	WinFormPainter.Scale = new PointGeo(ratio, -ratio);//(this.winFormUC1.GetShapeLayerManager().GetPainter() as WinFormPainter).Scale = new PointGeo(ratio, ratio);
			//} else {
			//	WinFormPainter.Scale = new PointGeo(ratio, ratio);//(this.winFormUC1.GetShapeLayerManager().GetPainter() as WinFormPainter).Scale = new PointGeo(ratio, ratio);
				
			//}
			trackLines.SetPoints(gCodeInterpreter1.GetTrackPoints());
			
			laserHeadCircle.Radius = 30;
			laserHeadCircle.IsShow = false;
			laserHeadCircle.IsNonTrans = true;
			ShapeMeta meta = new ShapeMeta();
			meta.IsFill = false;
			meta.BackColor = Color.FromArgb(255, 128, 0);
			meta.ForeColor = Color.Black;
			meta.LineWidth = 2;
			laserHeadCircle.SetDrawMeta(meta);
			
			leftSafeLine.FirstPoint = new PointGeo(gCodeInterpreter1.StartPoint.X, (float)gCodeInterpreter1.MinY);
			leftSafeLine.SecondPoint = new PointGeo(gCodeInterpreter1.StartPoint.X, (float)gCodeInterpreter1.MaxY);
			rightSafeLine.FirstPoint = new PointGeo(gCodeInterpreter1.StartPoint.X +(float) Settings.MOVE_RANGE, (float)gCodeInterpreter1.MinY);
			rightSafeLine.SecondPoint = new PointGeo(gCodeInterpreter1.StartPoint.X + (float)Settings.MOVE_RANGE, (float)gCodeInterpreter1.MaxY);
			
			ShapeMeta safeLineStyle = new ShapeMeta();
			safeLineStyle.IsFill = false;
			safeLineStyle.BackColor = Color.White;
			safeLineStyle.ForeColor = Color.Lime;
			safeLineStyle.LineWidth = 3;
			leftSafeLine.SetDrawMeta(safeLineStyle);
			this.winFormUC1.GetFixedLayerManager().Add(leftSafeLine);
			rightSafeLine.SetDrawMeta(safeLineStyle);
			this.winFormUC1.GetFixedLayerManager().Add(rightSafeLine);
			
			TextMeta leftTextMeta=new TextMeta("左边界");
			leftTextMeta.ForeColor=Color.Black;
			leftTextMeta.TEXTFONT=new Font("宋体" ,18, FontStyle.Bold);
			leftBorderText.SetDrawMeta(leftTextMeta);
			leftBorderText.pos=new PointGeo(leftSafeLine.FirstPoint.X,leftSafeLine.FirstPoint.Y);
			this.winFormUC1.GetFixedLayerManager().Add(leftBorderText);
		 
			TextMeta rightTextMeta=new TextMeta("右边界");
			rightTextMeta.ForeColor=Color.Black;
			rightTextMeta.TEXTFONT=new Font("宋体",18, FontStyle.Bold);
			rightBoderText.SetDrawMeta(rightTextMeta);
			rightBoderText.pos=new PointGeo(rightSafeLine.FirstPoint.X,rightSafeLine.FirstPoint.Y);
			this.winFormUC1.GetFixedLayerManager().Add(rightBoderText);
			
			this.winFormUC1.LoadShapes(shapes);
			this.winFormUC1.AddTrack(trackLines);
			statusMsg.Text = "加载完成";
			

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			btnStart.Text = "测试";
			btnStop.Text = "停止";

		}
		
		DateTime StartTime;
		DateTime EndTime;
		private void button2_Click(object sender, EventArgs e)
		{
			if (gCodeInterpreter1 != null) {
				if (ckWait.Checked) {//屏蔽等待
					this.gCodeInterpreter1.StartPoint.X = 0;
				}
				this.btnStart.Enabled = false;
				TICK_COUNT = 0;
				//gCodeInterpreter1.Run();
				lblTime.Text = "00:00.000";
				StartTime = DateTime.Now;
				IsUpdateShape = true;
				this.toolStripStatusASpeed.Text = "A轴速度：" + Settings.A_SHAFT_SPEED.ToString("f2")+" mm/min";
				this.toolStripStatusSingleCutLength.Text = "单次切割长度：" + (Settings.TIME_SPAN * gCodeInterpreter1.GetProcessors()[0].cutSpeed / 60 / 1000 * Settings.SPEED_RATE).ToString("f3")+" mm";
			}
			
		}
		
		private void SetLaserText(bool isLaserOn)
		{
			if (isLaserOn) {
				lblLaserStatus.Text = "激光开";
				lblLaserStatus.ForeColor = Color.White;
				lblLaserStatus.BackColor = Color.FromArgb(255, 128, 0);
			} else {
				lblLaserStatus.Text = "激光关";
				lblLaserStatus.ForeColor = Color.Black;
				lblLaserStatus.BackColor = Color.White;
			}
			
		}
		bool IsClosed = false;
		private void notifyIcon1_Click(object sender, EventArgs e)
		{
			this.Show();
			this.WindowState = FormWindowState.Normal;
			this.BringToFront();
		}
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!IsClosed) {
				e.Cancel = true;
				this.Hide();
				return;
			}
		}
		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			IsClosed = true;
			this.Close();
		}

		private void Form1_Load_1(object sender, EventArgs e)
		{
			SetLaserText(false);
			
			this.KeyPreview = true;
			this.winFormUC1.Select();
			
			//this.FormClosing += MainForm_FormClosing;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			gCodeInterpreter1.Stop();
			this.btnStart.Enabled = true;
		}
		void BtnClearClick(object sender, EventArgs e)
		{
			this.gCodeInterpreter1.GetTrackPoints().Clear();
			this.winFormUC1.Clear();
			this.listGCode.Items.Clear();
		}
		void SettingFrmCmdClick(object sender, EventArgs e)
		{
			SettingForm frm = SettingForm.Instance;
			frm.Show();
		}
		void LblPositionClick(object sender, EventArgs e)
		{
			CalculatorDll.Calculator.CalculatorForm cl = new CalculatorDll.Calculator.CalculatorForm();
			cl.Show();
		}

        private void winFormUC1_Load(object sender, EventArgs e)
        {

        }
    }

}
