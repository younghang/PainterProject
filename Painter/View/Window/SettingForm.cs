/*
 * Created by SharpDevelop.
 * User: DELL
 * Date: 2021/9/7
 * Time: 21:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Painter.Utils;

namespace Painter
{
	/// <summary>
	/// Description of SettingForm.
	/// </summary>
	public partial class SettingForm : Form
	{
		private static SettingForm instance = new SettingForm();
        public event Action UpdateStatus;
		public static SettingForm Instance
		{
			get
			{
				if (instance != null && instance.IsDisposed == false)
				{
					return instance;
				}
				else
				{
					instance = new SettingForm();
					return instance;
				}
			}
		}
		private Font gCodeFont = new Font("Consolas", 16);
		private Font lgFont = new Font("宋体", 16);

		public Font GCodeTextFont { get { return gCodeFont; }  set { gCodeFont = value; } }
		public Font LogTextFont { get { return lgFont; } set { lgFont = value; } }

		private SettingForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void SettingFormLoad(object sender, EventArgs e)
		{
			LoadSetting();
			this.txtASpeed.Text = Settings.A_SHAFT_SPEED.ToString("f3");
			this.txtBeamOffTime.Text = Settings.BEAMOFF.ToString("F1");
			this.txtBeamOnTime.Text = Settings.BEAMON.ToString("F1");
			this.txtCycleTimes.Text = Settings.CYCLE_TIMES.ToString();
			this.txtFlashSpan.Text = Settings.TIME_SPAN.ToString();
			this.txtPlayRate.Text = Settings.SPEED_RATE.ToString("F1");
			this.txtLoopGap.Text=Settings.CYCLE_LENGTH.ToString("F3");
			this.txtMoveRange.Text=Settings.MOVE_RANGE.ToString("F3");
			lblColor.BackColor=Settings.BG_COLOR;
			this.txtDefaultCutSpeed.Text = Settings.DEFAULT_CUT_SPEED.ToString("F3");
			this.txtDefaultMoveSpeed.Text = Settings.DEFAULT_MOVE_SPEED.ToString("F3");
			this.lblLogFont.Font = Settings.LOG_TEXT_FONT;
			this.lblGCodeFont.Font = Settings.G_CODE_TEXT_FONT;
            this.ckLaserCircle.Checked = Settings.SHOW_LASER_CIRCLE;
		}
		private void LoadSetting()
		{
			Settings.LoadSettingFile();
		}
		private void SetSetting()
		{
			Settings.A_SHAFT_SPEED = double.Parse(this.txtASpeed.Text);
			Settings.BEAMOFF = double.Parse(this.txtBeamOffTime.Text);
			Settings.BEAMON = double.Parse(this.txtBeamOnTime.Text);
			Settings.CYCLE_TIMES = int.Parse(this.txtCycleTimes.Text);
			Settings.TIME_SPAN = int.Parse(this.txtFlashSpan.Text);
			Settings.SPEED_RATE = double.Parse(this.txtPlayRate.Text);
			Settings.CYCLE_LENGTH=double.Parse(this.txtLoopGap.Text);
			Settings.MOVE_RANGE=double.Parse(this.txtMoveRange.Text);
			Settings.BG_COLOR=lblColor.BackColor;
			Settings.CUT_SPEED_RATIO=double.Parse(this.txtCutSpeedRatio.Text);
			Settings.DEFAULT_CUT_SPEED=double.Parse(this.txtDefaultCutSpeed.Text);
			Settings.DEFAULT_MOVE_SPEED=double.Parse(this.txtDefaultMoveSpeed.Text);
			Settings.LOG_TEXT_FONT = this.LogTextFont;
			Settings.G_CODE_TEXT_FONT = this.GCodeTextFont;
            Settings.SHOW_LASER_CIRCLE = this.ckLaserCircle.Checked;
			Settings.SavetSettingFile();
		}
		void BtnOKClick(object sender, EventArgs e)
		{
			SetSetting();
            if (this.UpdateStatus!=null)
            {
                UpdateStatus();
            }
			this.Close();
		}
		void BtnConfirmClick(object sender, EventArgs e)
		{
			SetSetting();
		}
		void LblColorClick(object sender, EventArgs e)
		{
			Label l = sender as Label;
            ColorDialog colorDialog = new ColorDialog();
            DialogResult dialogResult = colorDialog.ShowDialog(); 
            Color color = new Color();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                colorDialog.Dispose();
                color = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B); 
                l.BackColor = color;
            }
		}
		private void lblGCodeFont_Click(object sender, EventArgs e)
		{
			FontDialog fd = new FontDialog();
			fd.Font = Settings.G_CODE_TEXT_FONT; 
			if (fd.ShowDialog() == DialogResult.OK)
			{
				GCodeTextFont = fd.Font;
				lblGCodeFont.Font = fd.Font;
			}
		}
		private void lblLogFont_Click(object sender, EventArgs e)
		{
			FontDialog fd = new FontDialog();
			fd.Font = Settings.LOG_TEXT_FONT;
			if (fd.ShowDialog() == DialogResult.OK)
			{
				LogTextFont = fd.Font;
				lblLogFont.Font = fd.Font;
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
        {
			Settings.A_SHAFT_SPEED *=1.1;
			this.txtASpeed.Text = Settings.A_SHAFT_SPEED.ToString("f3");
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
			Settings.A_SHAFT_SPEED *= 0.9;
			this.txtASpeed.Text = Settings.A_SHAFT_SPEED.ToString("f3");
		}
		void LblCutRatioNameClick(object sender, EventArgs e)
		{
			this.txtCutSpeedRatio.Text="1.0";
		}

        
    }
    public static class Settings
	{
		public static Color ANTI_BG{
			get{return Color.FromArgb(BG_COLOR.A,255-BG_COLOR.R,255-BG_COLOR.G,255-BG_COLOR.B);}
		}

		public static Color BG_COLOR=Color.DarkGray;
		public static double BEAMON = 0.5;
		//#开光耗时
		public static double BEAMOFF = 0.5;
		//#关光耗时
		public static int CYCLE_TIMES = 1;
		//#循环次数
		public static int TIME_SPAN = 5;
		//#每次刷新的时间间隔ms，影响显示精度和性能
		public static double SPEED_RATE = 1;
		//#播放的速率，影响显示精度
		public static double A_SHAFT_SPEED = 0;
		public static double CYCLE_LENGTH = 2000;
		//#循环间距
		public static double MOVE_RANGE = 1200;//行程宽度
		//切割速度系数
		public static double CUT_SPEED_RATIO=1.0;
		//默认切割速度
		public static double DEFAULT_CUT_SPEED = 5000;
		//默认移动速度
		public static double DEFAULT_MOVE_SPEED = 10000;
		public static Font G_CODE_TEXT_FONT = new Font("Consolas", 16);
		public static Font LOG_TEXT_FONT= new Font("宋体", 12);
        public static bool SHOW_LASER_CIRCLE = true;
		public static void LoadSettingFile()
		{
			
//			foreach (var item in typeof(Settings).GetProperties(BindingFlags.Public | BindingFlags.Static)) {
//				string tmp = FileSettings.GetItem("set.txt", item.Name);
//				if (item.PropertyType == typeof(int)) {
//					item.SetValue(0, int.Parse(tmp.Trim()));
//				}else
//				if (item.PropertyType == typeof(double)) {
//					item.SetValue(0, int.Parse(tmp.Trim()));
//				}
//			}
			string tmp = FileSettings.GetItem("set.txt", "BEAMON");
			if (!string.IsNullOrEmpty(tmp)) {
				BEAMON = double.Parse(tmp.Trim());
			}
			
			tmp = FileSettings.GetItem("set.txt", "BEAMOFF");
			if (!string.IsNullOrEmpty(tmp)) {
				BEAMOFF = double.Parse(tmp.Trim());
			}
			tmp = FileSettings.GetItem("set.txt", "CYCLE_TIMES");
			if (!string.IsNullOrEmpty(tmp)) {
				CYCLE_TIMES = int.Parse(tmp.Trim());
			}
			tmp = FileSettings.GetItem("set.txt", "TIME_SPAN");
			if (!string.IsNullOrEmpty(tmp)) {
				TIME_SPAN = int.Parse(tmp.Trim());
			}
			tmp = FileSettings.GetItem("set.txt", "SPEED_RATE");
			if (!string.IsNullOrEmpty(tmp)) {
				SPEED_RATE = double.Parse(tmp.Trim());
			}
			tmp = FileSettings.GetItem("set.txt", "AShaftSpeed");
			if (!string.IsNullOrEmpty(tmp))
				A_SHAFT_SPEED = double.Parse(tmp.Trim());
			tmp = FileSettings.GetItem("set.txt", "CYCLE_LENGTH");
			if (!string.IsNullOrEmpty(tmp))
				CYCLE_LENGTH = double.Parse(tmp.Trim());
			tmp = FileSettings.GetItem("set.txt", "MOVE_RANGE");
			if (!string.IsNullOrEmpty(tmp))
				MOVE_RANGE = double.Parse(tmp.Trim());
			tmp = FileSettings.GetItem("set.txt", "BG_COLOR");
			if (!string.IsNullOrEmpty(tmp))
			{
				string [] colors=tmp.Trim().Split(';');
				BG_COLOR = Color.FromArgb(int.Parse(colors[0]),int.Parse(colors[1]),int.Parse(colors[2]),int.Parse(colors[3]));
			}
			tmp = FileSettings.GetItem("set.txt", "DEFAULT_CUT_SPEED");
			if (!string.IsNullOrEmpty(tmp))
				DEFAULT_CUT_SPEED = double.Parse(tmp.Trim());
			tmp = FileSettings.GetItem("set.txt", "DEFAULT_MOVE_SPEED");
			if (!string.IsNullOrEmpty(tmp))
				DEFAULT_MOVE_SPEED = double.Parse(tmp.Trim());

			tmp = FileSettings.GetItem("set.txt", "G_CODE_TEXT_FONT"); 
			if (!string.IsNullOrEmpty(tmp))
			{
				string[] font = tmp.Trim().Split(',');
				G_CODE_TEXT_FONT = new Font(font[0], float.Parse(font[1]));
			}
			tmp = FileSettings.GetItem("set.txt", "LOG_TEXT_FONT");
			if (!string.IsNullOrEmpty(tmp))
			{
				string[] font = tmp.Trim().Split(',');
				LOG_TEXT_FONT = new Font(font[0], float.Parse(font[1]));
			}
            tmp = FileSettings.GetItem("set.txt", "SHOW_LASER_CIRCLE");
            if (!string.IsNullOrEmpty(tmp))
            {
                string[] font = tmp.Trim().Split(',');
                SHOW_LASER_CIRCLE = bool.Parse(tmp.Trim());
            }
        }
		
		public static void SavetSettingFile()
		{
//		foreach (var item in typeof(Settings).GetProperties(BindingFlags.Public | BindingFlags.Static)) {
//			FileSettings.SaveItem("set.txt",item.Name,item.GetValue(null)+"");
//		}
			FileSettings.SaveItem("set.txt", "BEAMON", BEAMON.ToString());
			FileSettings.SaveItem("set.txt", "BEAMOFF", BEAMOFF.ToString());
			FileSettings.SaveItem("set.txt", "CYCLE_TIMES", CYCLE_TIMES.ToString());
			FileSettings.SaveItem("set.txt", "TIME_SPAN", TIME_SPAN.ToString());
			FileSettings.SaveItem("set.txt", "SPEED_RATE", SPEED_RATE.ToString());
			FileSettings.SaveItem("set.txt", "AShaftSpeed", A_SHAFT_SPEED.ToString());
			FileSettings.SaveItem("set.txt", "CYCLE_LENGTH", CYCLE_LENGTH.ToString());
			FileSettings.SaveItem("set.txt", "MOVE_RANGE", MOVE_RANGE.ToString());
			FileSettings.SaveItem("set.txt", "BG_COLOR",BG_COLOR.A+";"+BG_COLOR.R+";"+BG_COLOR.G+";"+BG_COLOR.B);
			FileSettings.SaveItem("set.txt", "DEFAULT_CUT_SPEED", DEFAULT_CUT_SPEED.ToString());
			FileSettings.SaveItem("set.txt", "DEFAULT_MOVE_SPEED", DEFAULT_MOVE_SPEED.ToString());
			FileSettings.SaveItem("set.txt", "G_CODE_TEXT_FONT", G_CODE_TEXT_FONT.Name+","+ G_CODE_TEXT_FONT.Size);
			FileSettings.SaveItem("set.txt", "LOG_TEXT_FONT", LOG_TEXT_FONT.Name+","+ LOG_TEXT_FONT.Size);
			FileSettings.SaveItem("set.txt", "SHOW_LASER_CIRCLE", SHOW_LASER_CIRCLE.ToString());
        }
	}
}
