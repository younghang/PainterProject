using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Text;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Painter.Models;
using Painter.DisplayManger;
using Painter.Painters;

namespace Painter.WinFormDisplay
{
	public partial class WinFormUC : UserControl,IPainterUC
	{
	public void OnMessage(string msg)
		{
		return;
		}

	public void OnUpdatePos(double offset)
	{
		return;
	}

	public double GetWidth()
	{
		return this.Width;
	}

	public double GetHeight()
	{
		return this.Height;
	}

 

	public GraphicsLayerManager GetMoveableLayerManager()
	{
		return this.moveableLayerManager;
	}

	public GraphicsLayerManager GetHoldLayerManager()
	{
		return this.holdLayerManager;
	}

	public GraphicsLayerManager GetFreshLayerManager()
	{
		return this.freshLayerManager;
	}

		public WinFormUC()
		{
			InitializeComponent();
			this.DoubleBuffered = true;//设置本窗体
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
			SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque, true);
			
			SetLayerManagerGraphic(ref moveableLayerManager);
			SetLayerManagerGraphic(ref holdLayerManager);
			SetLayerManagerGraphic(ref freshLayerManager);
		
		}
		public event Action<Bitmap> GetImage;
		DRAW_TYPE currentType;
		private Array lineCapStyle = Enum.GetValues(typeof(LineCap));
		GraphicsLayerManager moveableLayerManager=null;//平移的
		GraphicsLayerManager holdLayerManager=null;//禁止不动，不刷新的
		GraphicsLayerManager freshLayerManager=null;//会刷新，但是固定不动
		static Color ShapeBackColor = Color.LimeGreen;
		static Color ShapeForeColor = Color.OrangeRed;
		static Font DrawTextFont = new Font("黑体", 12, FontStyle.Bold);
		
		Dictionary<string, DRAW_TYPE> dicDrawTypes = new Dictionary<string, DRAW_TYPE>() { {
				"圆",
				DRAW_TYPE.CIRCLE
			}
			, { "矩形", DRAW_TYPE.RECTANGLE }, { "直线", DRAW_TYPE.LINE }, {
				"椭圆",
				DRAW_TYPE.ELLIPSE
			},
			{ "铅笔", DRAW_TYPE.RANDOMLINES }
		};
		Point oldP;
		Point newP;
		bool IsMouseDown = false;
		IScreenPrintable ips = null;
		bool IsStraightLine = false;
		bool IsContinusLine = false;
		List<PointGeo> points = new List<PointGeo>();
		private static TextBox tx = null;
		bool IsTextAdded = false;
		bool IsSaveFile = false;
		bool IsDrawByOutCall = false;
		public void SetIPS(IScreenPrintable screenPrintable)
		{
			ips = screenPrintable;
			currentType = DRAW_TYPE.RANDOMLINES;
			IsDrawByOutCall = true;
		}
		public GraphicsLayerManager GetTrackLayerManager()
		{
			return this.holdLayerManager;
		}
		public GraphicsLayerManager GetShapeLayerManager()
		{
			return this.moveableLayerManager;
		}
		public GraphicsLayerManager GetFixedLayerManager()
		{
			return this.freshLayerManager;
		}
		private Geo AxisGeo=new Geo();
		public void DrawAxis()
		{
			AxisGeo.ClearShape();
			float length=Math.Max(this.Width,this.Height);
			length=length/16;
			//X轴
			ShapeMeta XMeta=new ShapeMeta();
			XMeta.LineWidth=3;
			XMeta.ForeColor=Color.Green;
			AxisGeo.AddShape(new LineGeo(new PointGeo(length*3,0),new PointGeo(length*2,length/6)).SetDrawMeta(XMeta));
			AxisGeo.AddShape(new LineGeo(new PointGeo(length*2,length/6),new PointGeo(length*2,-length/6)).SetDrawMeta(XMeta));
			AxisGeo.AddShape(new LineGeo(new PointGeo(length*2,-length/6),new PointGeo(length*3,0)).SetDrawMeta(XMeta));
			AxisGeo.AddShape(new LineGeo(new PointGeo(0,0),new PointGeo(length*2,0)).SetDrawMeta(XMeta));
			
			
			//Y轴
			ShapeMeta YMeta=new ShapeMeta();
			YMeta.LineWidth=3;
			YMeta.ForeColor=Color.Red;
			AxisGeo.AddShape(new LineGeo(new PointGeo(0,length*3),new PointGeo(length/6,length*2)).SetDrawMeta(YMeta));
			AxisGeo.AddShape(new LineGeo(new PointGeo(length/6,length*2),new PointGeo(-length/6,length*2)).SetDrawMeta(YMeta));
			AxisGeo.AddShape(new LineGeo(new PointGeo(-length/6,length*2),new PointGeo(0,length*3)).SetDrawMeta(YMeta));
			AxisGeo.AddShape(new LineGeo(new PointGeo(0,0),new PointGeo(0,length*2)).SetDrawMeta(YMeta));
			
//			foreach (var element in AxisGeo.GetShapes()) {
//				element.Translate(WinFormPainter.OffsetPoint);
//			}
			AxisGeo.Draw(this.freshLayerManager.GetPainter());
			DrawableText xlabel=new DrawableText();
			TextMeta xtextMeta=new TextMeta("X");
			xtextMeta.ForeColor=Color.Green;
			xtextMeta.TEXTFONT=new Font(DrawTextFont,DrawTextFont.Style);
			xlabel.SetDrawMeta(xtextMeta);
			xlabel.pos=new PointGeo(4*length,0);
			xlabel.Draw(this.freshLayerManager.GetPainter());
			
			DrawableText ylabel=new DrawableText();
			TextMeta ytextMeta=new TextMeta("Y");
			ytextMeta.ForeColor=Color.Red;
			ytextMeta.TEXTFONT=new Font(DrawTextFont,DrawTextFont.Style);
			ylabel.SetDrawMeta(ytextMeta);
			ylabel.pos=new PointGeo(0,4*length);
			ylabel.Draw(this.freshLayerManager.GetPainter());
			
		}
		private void SetLayerManagerGraphic(ref GraphicsLayerManager layerGraphic)
		{
			if (layerGraphic != null) {
				layerGraphic.Dispose();
				Bitmap canvasBitmap = new Bitmap(this.Width, this.Height);
				Graphics canGraphics = Graphics.FromImage(canvasBitmap);
				canGraphics.Clip = new Region(new Rectangle(0, 0, this.Width, this.Height));
				canGraphics.Clear(Color.Transparent);
				canGraphics.SmoothingMode = SmoothingMode.AntiAlias;
				if (layerGraphic.GetPainter() == null) {
					layerGraphic.SetPainter(new WinFormPainter(canGraphics, canvasBitmap));
				} else {
					(layerGraphic.GetPainter() as WinFormPainter).SetGraphics(canGraphics);
					(layerGraphic.GetPainter() as WinFormPainter).SetCanvas(canvasBitmap);
				}
			} else {
				layerGraphic = new GraphicsLayerManager();
				layerGraphic.Invalidate += Invalidate;
				SetLayerManagerGraphic(ref layerGraphic);
			}
		}
		void MainFormLoad(object sender, EventArgs e)
		{
			comBoxSelectShape.DataSource = new List<string>(dicDrawTypes.Keys);
			comBoxSelectShape.SelectedIndex = 2;
			BackColor = Color.White;
			lblInfo.Text = "";
			lblMarginColor.Text = "";
			lblFillColor.Text = "";
			lblMarginColor.BackColor = ShapeForeColor;
			lblFillColor.BackColor = ShapeBackColor;
			lblFillColor.Click += this.myColorSelectHandler;
			lblMarginColor.Click += this.myColorSelectHandler;
			tx = new TextBox();
			comBoxLineCap.DataSource = Enum.GetNames(typeof(LineCap));
		}
		void Button1Click(object sender, EventArgs e)
		{
			Clear();
		}
		public void Clear()
		{
			this.moveableLayerManager.Clear();
			this.holdLayerManager.Clear();
			this.freshLayerManager.Clear();
			this.points.Clear();
			if (ips != null) {
				if (ips is RandomLines) {
					(ips as RandomLines).Clear();
					
				}
				ips = null;
			}
			this.Invalidate();
		}
		//protected override void OnClosing(CancelEventArgs e)
		//{
		//    if (GetImage != null) this.GetImage(canvasBitmap);
		//    base.OnClosing(e);
		//}
		protected override CreateParams CreateParams {
			get {
				var cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;
				return cp;
			}
		}
		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			RefreshCanvas();
		}
		void RefreshCanvas()
		{
			if (freshLayerManager != null) {
				SetLayerManagerGraphic(ref moveableLayerManager);
				SetLayerManagerGraphic(ref holdLayerManager);//这里要记得更新，不然会截掉一部风
				SetLayerManagerGraphic(ref freshLayerManager);
			}
			
		}
		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				if (this.freshLayerManager.GetCurPos() > 5) {
					this.btnSave.Text = "Save";
					IsSaveFile = true;
				} else {
					this.btnSave.Text = "Load";
					IsSaveFile = false;
				}
				currentType = dicDrawTypes[(string)comBoxSelectShape.SelectedItem];
				if (currentType == DRAW_TYPE.RANDOMLINES) {
					IsMouseDown = true;
					ips = ScreenElementCreator.CreateElement(currentType);
					oldP = new Point(e.X, e.Y);
					points.Clear();
					points.Add(new PointGeo(oldP.X, oldP.Y));
					this.lblInfo.Text = "铅笔绘制模式";
				}
				if (IsContinusLine && currentType == DRAW_TYPE.LINE && ips != null && (ips as Shape).HasDrawMeta()) {
					freshLayerManager.Add(ips);
					ips = null;
					IsMouseDown = false;
					Invalidate();
					//ScreenPaint();
				} else {
					IsMouseDown = true;
					oldP = new Point(e.X, e.Y);
					if (IsDrawText) {
						currentType = DRAW_TYPE.TEXT;
						if (IsTextAdded) {
							ips = ScreenElementCreator.CreateElement(currentType);
							DrawableText dt = ips as DrawableText;
							dt.pos = new PointGeo(tx.Location.X, tx.Location.Y);
							TextMeta dm = new TextMeta(tx.Text);
							dm.TEXTFONT = new Font(DrawTextFont, DrawTextFont.Style);
							dm.ForeColor = ShapeForeColor;
							dt.SetDrawMeta(dm);
							freshLayerManager.Add(ips);
							this.Controls.Remove(tx);
							ips = null;
							IsTextAdded = false;
							return;
						} else {
							CreateTextView();
							this.Invalidate();
						}
					}
					ips = ScreenElementCreator.CreateElement(currentType);
				}
			} else {
				this.contextMenuStrip1.Show(new System.Drawing.Point(e.Location.X + this.Left, e.Location.Y + this.Top));
			}
		}
		private void CreateTextView()
		{
			if (tx != null) {
				this.Controls.Remove(tx);
			}
			tx.Location = new System.Drawing.Point(oldP.X, oldP.Y);
			tx.Font = DrawTextFont;
			int fontSize = (int)(DrawTextFont.Size);
			tx.Size = new System.Drawing.Size(10 * fontSize, fontSize);
			tx.TabIndex = 2;
			tx.Text = "在此输入文本";
			this.Controls.Add(tx);
			tx.Focus();
			tx.SelectAll();
			IsTextAdded = true;
		}
		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			newP = new Point(e.X, e.Y);
			if (IsMouseDown) {
				if (currentType == DRAW_TYPE.RANDOMLINES)
					points.Add(new PointGeo(newP.X, newP.Y));
				Invalidate(new System.Drawing.Rectangle(0, 0, this.Width, this.Height));
			}
		}
		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			if (IsMouseDown && ips != null) {
				if (ips is Shape && (ips as Shape).HasDrawMeta()) {
					freshLayerManager.Add(ips);
					ips = null;
				} else if (ips is RandomLines) {
					RandomLines rl = (ips as RandomLines);
					ShapeMeta pm = new ShapeMeta();
					pm.ForeColor = Color.FromArgb(ShapeForeColor.A, ShapeForeColor.R, ShapeForeColor.G, ShapeForeColor.B);
					pm.LineWidth = float.Parse(txtLineWidth.Text);
					rl.SetDrawMeta(pm);
					rl.AddPoints(points);
					freshLayerManager.Add(rl);
					ips = null;
					points.Clear();
				}
			}
			if (IsContinusLine && currentType == DRAW_TYPE.LINE) {
				IsMouseDown = true;
				oldP = new Point(e.X, e.Y);
				currentType = dicDrawTypes[(string)comBoxSelectShape.SelectedItem];
				ips = ScreenElementCreator.CreateElement(currentType);
			} else {
				IsMouseDown = false;
				//ScreenPaint();
				Invalidate();
			}
		}

		private Bitmap BGBitmap = null;
		public void SetImagePath(string image_path)
		{
			this.ImageFilePath = image_path;
		}
		public void SetImage(Bitmap b)
		{
			BGBitmap = b.Clone() as Bitmap;
		}
		void ScreenPaint()
		{
			if (IsDrawByOutCall) {
				(freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().Clear(Color.White);
				freshLayerManager.Draw();
			} else {
				
//				if (ips != null) {
//					//Debug.Print("IPS is NULL return");
//					return;
//				} else {
//					Console.WriteLine("IPS is not NULL. Paint All");
//				}
				(freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().Clear(Color.White);
				if (ImageFilePath != "") {
					try {
						if (BGBitmap == null) {
							System.Drawing.Size s = new Size(this.Width, this.Height);
							using (Image image = Bitmap.FromFile(ImageFilePath)) {
								Bitmap btm = new Bitmap(image, s);
								(freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
								(freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().DrawImage(btm, new PointF(0, 0));
								btm.Dispose();
								image.Dispose();
							}
						} else {
							(freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
							(freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().DrawImage(BGBitmap, new PointF(0, 0));
						}

					} catch (Exception) {
						this.lblInfo.Text = ("图片加载失败");
					}
				}
				(freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
				DrawAxis();
				freshLayerManager.Draw();
			}
		}
		private void MainFormPaint(object sender, PaintEventArgs e)
		{
			//if (IsLabelPaint) return; //Paint--> Draw--> OnMessageUpdate--> Paint --> Draw;
			
			#region fixedLayer.Draw
			ScreenPaint();
			
			if (currentType == DRAW_TYPE.RANDOMLINES) {
				if (IsDrawByOutCall) {
					if (ips != null) {
						ips.Draw(freshLayerManager.GetPainter());
					}
				} else {
					DrawMeta dm = new DrawMeta();
					dm.ForeColor = Color.FromArgb(ShapeForeColor.A, ShapeForeColor.R, ShapeForeColor.G, ShapeForeColor.B);
					dm.LineWidth = float.Parse(txtLineWidth.Text);
					dm.CapStyle = (LineCap)lineCapStyle.GetValue(comBoxLineCap.SelectedIndex);
					Pen p = new Pen(dm.ForeColor, dm.LineWidth);
					for (int i = 0; i < points.Count - 1; i++) {
						e.Graphics.DrawLine(p, points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y);
					}
				}
			} else if (currentType == DRAW_TYPE.TEXT) {
			} else {
				if (IsMouseDown && ips != null) {
					DrawRealTimeShape(e);
				}
			}
			
			#endregion
			e.Graphics.DrawImage((freshLayerManager.GetPainter() as WinFormPainter).GetCanvas(), new PointF(0, 0));
			moveableLayerManager.GetPainter().Clear();
			moveableLayerManager.Draw();
			e.Graphics.DrawImage((moveableLayerManager.GetPainter() as WinFormPainter).GetCanvas(), new PointF(0, 0));
			holdLayerManager.Draw();
			e.Graphics.DrawImage((holdLayerManager.GetPainter() as WinFormPainter).GetCanvas(), new PointF(0, 0));
		}
		private void DrawRealTimeShape(PaintEventArgs e)
		{
			//(shapeLayerManager.GetPainter() as WinFormPainter).GetGraphics().Clear(Color.White);
			Shape s = ips as Shape;
			if (IsStraightLine && currentType == DRAW_TYPE.LINE) {
				if (Math.Abs(newP.X - oldP.X) < Math.Abs(newP.Y - oldP.Y)) {
					newP.X = oldP.X;
				} else {
					newP.Y = oldP.Y;
				}
			}
			Func<float,float> transXBack = (value) => (float)(value - this.GetFixedLayerManager().GetPainter().OffsetPoint.X) / this.GetFixedLayerManager().GetPainter().Scale.X;
			Func<float,float> transYBack = (value) => (float)(value - this.GetFixedLayerManager().GetPainter().OffsetPoint.Y) / this.GetFixedLayerManager().GetPainter().Scale.Y;
			
			s.SecondPoint = new PointGeo(transXBack(newP.X), transYBack(newP.Y));
			if (!s.HasDrawMeta()) {
				s.FirstPoint = new PointGeo(transXBack(oldP.X), transYBack(oldP.Y));
				ShapeMeta sm = new ShapeMeta();
				sm.IsFill = fillChecked.Checked;
				sm.BackColor = Color.FromArgb(ShapeBackColor.A, ShapeBackColor.R, ShapeBackColor.G, ShapeBackColor.B);
				sm.ForeColor = Color.FromArgb(ShapeForeColor.A, ShapeForeColor.R, ShapeForeColor.G, ShapeForeColor.B);
				sm.LineWidth = float.Parse(txtLineWidth.Text);
				sm.CapStyle = (LineCap)lineCapStyle.GetValue(comBoxLineCap.SelectedIndex);
				s.SetDrawMeta(sm);
			}
			if (s.UpdateMessage == null)
				s.UpdateMessage += MsgHandler;
			
			ips.Draw(freshLayerManager.GetPainter());
			this.lblInfo.Text = s.MSG;
		}
		private void MsgHandler(string msg)
		{
			this.lblInfo.Text = msg;
		}
		private void btnUndo_Click(object sender, EventArgs e)
		{
			this.Invalidate();
			this.freshLayerManager.Undo();
		}
		private void btnRedo_Click(object sender, EventArgs e)
		{
			this.Invalidate();
			this.freshLayerManager.Redo();
		}
		private void MainForm_KeyUp(object sender, KeyEventArgs e)
		{
			IsStraightLine = false;
			IsContinusLine = false;
		}
		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Modifiers == Keys.Shift) {
				IsStraightLine = true;
			} else {
				IsStraightLine = false;
			}
			if (e.Modifiers == Keys.Control) {
				IsContinusLine = true;
			} else {
				IsContinusLine = false;
			}
		}
		private EventHandler myColorSelectHandler = (o, e) => {
			Label l = o as Label;
			ColorDialog colorDialog = new ColorDialog();
			DialogResult dialogResult = colorDialog.ShowDialog();
			Console.WriteLine(dialogResult.ToString());
			Color color = new Color();
			if (dialogResult == System.Windows.Forms.DialogResult.OK) {
				colorDialog.Dispose();
				color = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
				if (l.Tag.ToString() == "Fill") {
					ShapeBackColor = color;
				}
				if (l.Tag.ToString() == "Margin") {
					ShapeForeColor = color;
				}
				l.BackColor = color;
			}
		};
		bool IsDrawText = false;
		private void label4_Click(object sender, EventArgs e)
		{
			IsDrawText = !IsDrawText;
			if (IsDrawText) {
				this.lblInfo.Text = "插入文本, 双击[A]修改字体";
				this.lblTextFont.BackColor = Color.Wheat;
			} else {
				this.lblInfo.Text = "绘图，点击色块修改颜色";
				this.lblTextFont.BackColor = Color.Transparent;
			}
		}
		private void label4_DoubleClick(object sender, EventArgs e)
		{
			IsDrawText = !IsDrawText;
			if (IsDrawText) {
				this.lblInfo.Text = "插入文本, 双击[A]修改字体";
				this.lblTextFont.BackColor = Color.Wheat;
			} else {
				this.lblInfo.Text = "绘图，点击色块修改颜色";
				this.lblTextFont.BackColor = Color.Transparent;
			}
			FontDialog fd = new FontDialog();
			if (fd.ShowDialog() == DialogResult.OK) {
				DrawTextFont = fd.Font;
			}
		}
		private void btnSave_Click(object sender, EventArgs e)
		{
			this.freshLayerManager.SaveOrLoadFile(IsSaveFile);
			this.ips = null;
			this.Invalidate();
		}
		public void LoadShapes(List<Shape> shapes)
		{
			if (shapes != null && shapes.Count > 0) {
				this.moveableLayerManager.LoadShape(shapes);
			}
			this.ips = null;
			this.Invalidate();
		}

		public void AddTrack(RandomLines rl)
		{
			if (rl != null) {
				this.holdLayerManager.Add(rl);
			}
		}
		string ImageFilePath = "";
		private void lblImage_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp" }) {
				if (ofd.ShowDialog() == DialogResult.OK) {
					if (!String.IsNullOrEmpty(ofd.FileName)) {
						this.ImageFilePath = ofd.FileName;
						//ips = ScreenElementCreator.CreateElement(DRAW_TYPE.IMAGE);
						//DrawableImage di=ips as DrawableImage;
						//di.SetDrawMeta(new ImageMeta(this.ImageFilePath));
						//screenManager.Add(ips);
						//ips = null;
						//Invalidate();
					}
				}
			}
		}
		private void SaveImage()
		{
			using (SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.DefaultExt = "bmp";
				sfd.AddExtension = true;
				sfd.Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
				if (sfd.ShowDialog() == DialogResult.OK && (moveableLayerManager.GetPainter() as WinFormPainter).GetCanvas() != null) {
					(moveableLayerManager.GetPainter() as WinFormPainter).GetCanvas().Save(sfd.FileName);
				}
			}
		}
		private void 保存图片ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveImage();
		}
		bool IsScreenCut = false;
		private void 截取区域ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IsScreenCut = true;
		}
		private void button2_Click(object sender, EventArgs e)
		{
			Brush b = (new SolidBrush(Color.FromArgb(50, 255, 255, 255)));
			Invalidate();
			this.CreateGraphics().FillRectangle(b, new Rectangle(0, 0, this.Width, this.Height));
		}
	}
}
