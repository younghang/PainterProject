using Painter.Forms.Optical;
using Painter.Models;
using Painter.Models.GameModel.SceneModel.OpticalModel;
using Painter.Models.GameModel.StageModel;
using Painter.Models.PainterModel;
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
using System.Windows.Forms.DataVisualization.Charting;
using Utils.DisplayManger.Axis;

namespace Painter
{
    public partial class OpticalForm : Form
    {
        public OpticalForm()
        {
            MaterialDataBase.Instance.LoadData();
            InitializeComponent();
            
            this.Load += OpticalForm_Load;
            this.FormClosing += OpticalForm_FormClosing;
            this.listMaterials.KeyDown += OpticalForm_KeyDown;
        }

        private void OpticalForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Delete)
            {
                if (selectedMaterial!=null)
                {
                    MaterialDataBase.Instance.RemoveMaterial(selectedMaterial.Name);
                    selectedMaterial = null;
                    LoadMaterialsList();
                } 
            }
        }

        private void OpticalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (opticalScene != null)
            {
                //opticalScene.SaveScene();
            }
            MaterialDataBase.Instance.SaveData();
            Utils.FileSettings.SaveItem("./data/set.txt", "FirstRun", "False");
        }

        private void OpticalForm_Load(object sender, EventArgs e)
        {
            opticalScene = new OpticalScene();
            this.canvasView1.SetCurStage(opticalScene);
            geoLabel = new GeoLabel(150, 20, new RectangleGeo());
            canvasView1.AddControl(geoLabel);

            geoRayImageLengthLabel = new GeoLabel(150, 20, new RectangleGeo());
            geoRayImageLengthLabel.Move(new PointGeo(0, 20));
            canvasView1.AddControl(geoRayImageLengthLabel);
            canvasView1.MouseMove += CanvasView_MouseMove;

            string firstRun=Utils.FileSettings.GetItem("./data/set.txt", "FirstRun", "True");
            if (firstRun=="True")
            {
                GeoPanel geoPanel = new GeoPanel(560, 300, new RectangleGeo());

                GeoButton geoButton = new GeoButton(100, 50, new RectangleGeo());
                geoButton.Text = "关闭";
                geoButton.ClickEvent += () =>
                {
                    geoPanel.IsVisible = false;

                };
                GeoLabel geoLabel2 = new GeoLabel(400, 70, new RectangleGeo());
                geoLabel2.Alignment = StringAlignment.Near;
                geoLabel2.Text = "鼠标中键平移视图\n鼠标滚轮缩放视图";
                geoLabel2.Background = Color.Transparent;
                geoPanel.AddControl(geoButton);
                geoPanel.AddControl(geoLabel2);
                geoPanel.Move(new PointGeo(500, 300));
                canvasView1.AddControl(geoPanel);
            }
            
            Init();
        }
        private void Init()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < MaterialDataBase.Instance.Materials.Count; i++)
            {
                names.Add(MaterialDataBase.Instance.Materials[i].Name);
            }
            this.cbMaterialNames.DataSource = names;

            LoadRayList();
            LoadLensList();
            LoadMaterialsList();
        }
        GeoLabel geoLabel;
        GeoLabel geoRayImageLengthLabel;
        private void CanvasView_MouseMove(object sender, MouseEventArgs e)
        {
            geoLabel.Text = canvasView1.GetCanvasModel().CurObjectPoint.ToString();
        }

        OpticalScene opticalScene = null;
        private void canvasView1_Load(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void addRay_Click(object sender, EventArgs e)
        {
            NewRay rayForm = new NewRay();
            rayForm.ShowDialog();
            if (rayForm.DialogResult == DialogResult.OK)
            {
                if (rayForm.rayLight != null)
                {
                    opticalScene.GetScene().InsertObject(rayForm.rayLight);

                    LoadRayList();

                }
            }
        }
        List<RayLight> rays = new List<RayLight>();
        private void LoadRayList()
        {
            listRays.Items.Clear();
            rays.Clear();
            if (opticalScene != null)
            {
                int index = 0;
                var objs = opticalScene.GetScene().GetSceneObject();
                for (int i = 0; i < objs.Count; i++)
                {
                    if (objs[i].IsDisposed == false && objs[i] is RayLight)
                    {
                        RayLight ray = objs[i] as RayLight;
                        rays.Add(ray);
                        listRays.Items.Add("Ray_" + (++index));
                    }
                }
            }
        }
        private void RemoveRay_Click(object sender, EventArgs e)
        {
            var indices = listRays.SelectedItems;
            for (int i = 0; i < indices.Count; i++)
            {
                int index = indices[i].Index;
                RayLight rayLight = rays[index];
                rayLight.IsDisposed = true;
            }
            LoadRayList();
        }

        private void addLen_Click(object sender, EventArgs e)
        {
            NewLens lenForm = new NewLens();
            lenForm.ShowDialog();
            if (lenForm.DialogResult == DialogResult.OK)
            {
                if (lenForm.lensObject != null)
                {
                    opticalScene.GetScene().AddObject(lenForm.lensObject);
                    LoadLensList();
                }
            }
        }
        List<LensObject> lens = new List<LensObject>();
        private void LoadMaterialsList()
        {
            listMaterials.Items.Clear();
            if (opticalScene != null)
            {
                var mats = MaterialDataBase.Instance.Materials;

                foreach (var item in mats)
                {
                    listMaterials.Items.Add(item.Name);
                }
            }
            List<string> names = new List<string>();
            for (int i = 0; i < MaterialDataBase.Instance.Materials.Count; i++)
            {
                names.Add(MaterialDataBase.Instance.Materials[i].Name);
            }
            this.cbMaterialNames.DataSource = names;
        }
        private void LoadLensList()
        {
            listLens.Items.Clear();
            lens.Clear();
            if (opticalScene != null)
            {
                int index = 0;
                var objs = opticalScene.GetScene().GetSceneObject();
                for (int i = 0; i < objs.Count; i++)
                {

                    if (objs[i].IsDisposed == false && objs[i] is LensObject)
                    {
                        LensObject ray = objs[i] as LensObject;
                        lens.Add(ray);
                        listLens.Items.Add("Len_" + (++index));
                    }
                }
            }
            opticalScene.ResetRays();
        }
        private void RemoveLens_Click(object sender, EventArgs e)
        {
            var indices = listLens.SelectedItems;
            for (int i = 0; i < indices.Count; i++)
            {
                int index = indices[i].Index;
                LensObject len = lens[index];
                len.IsDisposed = true;
            }
            LoadLensList();
        }

        private void groupRayProperties_Enter(object sender, EventArgs e)
        {

        }
        private void listRays_SelectedIndexChanged(object sender, EventArgs e)
        {
            var indices = listRays.SelectedItems;
            if (selectedLine != null)
            {
                selectedLine.Selected = false;
                selectedLine.FinishTrack -= SelectedLine_FinishTrack;
            }

            if (indices.Count == 0)
            {
                return;
            }
            int index = indices[0].Index;

            LoadRayProperties(rays[index]);
            selectedLine = rays[index];
            selectedLine.Selected = true;
            selectedLine.FinishTrack += SelectedLine_FinishTrack;
        }

        private void SelectedLine_FinishTrack(double obj)
        {  
            geoRayImageLengthLabel.Text = obj.ToString("f4");     
            double beta = ((selectedLine.curDir.y / selectedLine.curDir.x) / Math.Tan(8.0 / 180 * Math.PI));
            geoLabel.Text = beta.ToString("f2");
            this.Invoke((MethodInvoker)delegate {
              
      
                //this.lbl_RayImagePoint.Text = obj.ToString();
            });
        }

        private void LoadLensProperties(LensObject lens)
        {
            if (lens == null)
            {
                return;
            }
            int index = MaterialDataBase.Instance.GetMaterialIndex(lens.Name);
            if (index < 0)
            {
                return;
            }
            cbMaterialNames.SelectedIndex = index;

            float ori_x = 0;
            float ori_y = 0;
            float angle = 0;
            float thickness = 0;
            float height = 0;
            float rRadius = 50;
            float lRadius = 50;

            cbMaterialNames.Enabled = true;



            if (lens is PositiveLens)
            {
                rdPositive.Checked = true;
                PositiveLens positiveLens = lens as PositiveLens;
                ori_x = positiveLens.LeftVertex.X;
                ori_y = positiveLens.LeftVertex.Y;
                thickness = positiveLens.Thick;
                rRadius = positiveLens.RRight;
                lRadius = positiveLens.RLeft;
                height = positiveLens.Height;
                angle = positiveLens.Angle;
            }
            else if (lens is NegtiveLens)
            {
                rdNegetive.Checked = true;
                NegtiveLens positiveLens = lens as NegtiveLens;
                ori_x = positiveLens.LeftVertex.X;
                ori_y = positiveLens.LeftVertex.Y;
                thickness = positiveLens.Thick;
                rRadius = positiveLens.RRight;
                lRadius = positiveLens.RLeft;
                height = positiveLens.Height;
                angle = positiveLens.Angle;
            }
            else if (lens is ArcLens)
            {
                rdArc.Checked = true;
                ArcLens positiveLens = lens as ArcLens;
                ori_x = positiveLens.LeftVertex.X;
                ori_y = positiveLens.LeftVertex.Y;
                thickness = positiveLens.Thick;
                rRadius = positiveLens.RRight;
                lRadius = positiveLens.RLeft;
                height = positiveLens.Height;
                angle = positiveLens.Angle;
            }
            else if (lens is MirrorObject)
            {
                cbMaterialNames.Enabled = false;
                rdMirror.Checked = true;
                MirrorObject positiveLens = lens as MirrorObject;
                PointGeo startp = positiveLens.StartPos.Clone();
                PointGeo endp = positiveLens.EndPos.Clone();
                height = (endp - startp).GetLength();
                ori_x = startp.x;
                ori_y = startp.y;
                angle = (float)(Math.Atan2((endp - startp).y, (endp - startp).x) / Math.PI * 180);
            }
            this.txtLenOriX.Text = ori_x.ToString();
            this.txtLenOriY.Text = ori_y.ToString();
            this.txtLenAngle.Text = angle.ToString();
            this.txtThickness.Text = thickness.ToString();
            this.txtRadius1.Text = rRadius.ToString();
            this.txtRadius2.Text = lRadius.ToString();
            this.txtHeight.Text = height.ToString();
        }
        private void LoadRayProperties(RayLight ray)
        {
            float ori_x = ray.Ori.x;
            float ori_y = ray.Ori.y;
            float angle = (float)(Math.Atan2(ray.Dir.y, ray.Dir.x) / Math.PI * 180);
            double wave_len = ray.WaveLength;
            this.txtOriX.Text = ori_x.ToString();
            this.txtOriY.Text = ori_y.ToString();
            this.txtAngle.Text = angle.ToString("f2");
            this.txtWaveLength.Text = wave_len.ToString("f2");
        }
        private void btnUpdateRay_Click(object sender, EventArgs e)
        {
            float ori_x = 0;
            float ori_y = 0;
            float angle = 0;

            float.TryParse(this.txtOriX.Text, out ori_x);
            float.TryParse(this.txtOriY.Text, out ori_y);
            float.TryParse(this.txtAngle.Text, out angle);
            var indices = listRays.SelectedItems;
            for (int i = 0; i < indices.Count; i++)
            {
                int index = indices[i].Index;
                RayLight rayLight = rays[index];
                rayLight.Ori.X = ori_x;
                rayLight.Ori.Y = ori_y;
                rayLight.Dir.X = (float)Math.Cos(angle / 180 * Math.PI);
                rayLight.Dir.Y = (float)Math.Sin(angle / 180 * Math.PI);
                rayLight.Reset();
            }
        }
        private LensObject selectedLen = null;
        private RayLight selectedLine = null;
        private LensMaterial selectedMaterial = null;
        private void listLens_SelectedIndexChanged(object sender, EventArgs e)
        {
            var indices = listLens.SelectedItems;
            if (selectedLen != null)
            {
                selectedLen.Selected = false;
            }
            if (indices.Count == 0)
            {
                return;
            }
            int index = indices[0].Index;

            LoadLensProperties(lens[index]);
            selectedLen = lens[index];
            selectedLen.Selected = true;
        }

        private void btnUpdateLens_Click(object sender, EventArgs e)
        {
            float ori_x = 0;
            float ori_y = 0;
            float angle = 0;
            float thickness = 0;
            float height = 0;
            float rRadius = 50;
            float lRadius = 50;
            string name = cbMaterialNames.SelectedItem.ToString();
            float.TryParse(this.txtLenOriX.Text, out ori_x);
            float.TryParse(this.txtLenOriY.Text, out ori_y);
            float.TryParse(this.txtLenAngle.Text, out angle);
            float.TryParse(this.txtThickness.Text, out thickness);
            float.TryParse(this.txtRadius1.Text, out rRadius);
            float.TryParse(this.txtRadius2.Text, out lRadius);
            float.TryParse(this.txtHeight.Text, out height);
            var indices = listLens.SelectedItems;
            List<int> preSelected = new List<int>();
            for (int i = 0; i < indices.Count; i++)
            {
                int index = indices[i].Index;
                preSelected.Add(index);
                LensObject lenObject = lens[index];
                switch (lenObject.LenType)
                {
                    case LEN_TYPE.POSITIVE:
                        PositiveLens positiveLens = lenObject as PositiveLens;
                        positiveLens.LeftVertex.X = ori_x;
                        positiveLens.LeftVertex.Y = ori_y;
                        positiveLens.Name = name;
                        positiveLens.Thick = thickness;
                        positiveLens.RRight = Math.Abs(rRadius);
                        positiveLens.RLeft = Math.Abs(lRadius);
                        positiveLens.Height = height;
                        positiveLens.Angle = angle;
                        positiveLens.InitElement();
                        break;
                    case LEN_TYPE.NEGETIVE:
                        NegtiveLens negtive = lenObject as NegtiveLens;
                        negtive.LeftVertex.X = ori_x;
                        negtive.LeftVertex.Y = ori_y;
                        negtive.Name = name;
                        negtive.Thick = thickness;
                        negtive.RRight = Math.Abs(rRadius);
                        negtive.RLeft = Math.Abs(lRadius);
                        negtive.Height = height;
                        negtive.Angle = angle;
                        negtive.InitElement();
                        break;
                    case LEN_TYPE.ARC:
                        ArcLens arcLens = lenObject as ArcLens;
                        arcLens.LeftVertex.X = ori_x;
                        arcLens.LeftVertex.Y = ori_y;
                        arcLens.Name = name;
                        arcLens.Thick = thickness;
                        arcLens.RRight = (rRadius);
                        arcLens.RLeft = (lRadius);
                        arcLens.Height = height;
                        arcLens.Angle = angle;
                        arcLens.InitElement();
                        break;
                    case LEN_TYPE.MIRROR:
                        MirrorObject mirrorObject = lenObject as MirrorObject;
                        PointGeo startp = new PointGeo(ori_x, ori_y);
                        PointGeo endp = startp + height * new PointGeo((float)Math.Cos(angle / 180 * Math.PI), (float)Math.Sin(angle / 180 * Math.PI));
                        mirrorObject.StartPos = startp;
                        mirrorObject.EndPos = endp;
                        mirrorObject.Name = name;
                        mirrorObject.InitElement();
                        break;
                    default:
                        break;
                }
                opticalScene.GetScene().AddObject(lenObject); 
            }
            LoadLensList();
            for (int i = 0; i < preSelected.Count; i++)
            {
                this.listLens.Items[preSelected[i]].Selected = true;
                break;
            }

        }

        private void listMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            var indices = listMaterials.SelectedItems;
            if (indices.Count == 0)
            {
                return;
            }
            int index = indices[0].Index;
            if (selectedMaterial != null)
            {
                //selectedMaterial.Selected = false;
            }
            if (index > MaterialDataBase.Instance.Materials.Count - 1)
            {
                return;
            }
            selectedMaterial = MaterialDataBase.Instance.Materials[index];
            LoadMaterialProperties(selectedMaterial);
            //selectedMaterial.Selected = true;
        }

        private void LoadMaterialProperties(LensMaterial selectedMaterial)
        {
            this.txtMatName.Text = selectedMaterial.Name;
            this.txtMatEquation.Text = selectedMaterial.Equation;
        }

        private void btnUpdateMaterial_Click(object sender, EventArgs e)
        {
            string name = this.txtMatName.Text;
            name = name.Trim();
            string equation = this.txtMatEquation.Text;
            var mats = MaterialDataBase.Instance.Materials;
            MaterialDataBase.Instance.AddMaterail(name, equation);
            LoadMaterialsList(); 
        }

        private void btnDeleteMat_Click(object sender, EventArgs e)
        {
            CalculatorDll.Calculator.CalculatorForm calculatorForm = new CalculatorDll.Calculator.CalculatorForm();
            calculatorForm.Show();
            calculatorForm.TopMost = true;
            //string name = this.txtMatName.Text;
            //MaterialDataBase.Instance.RemoveMaterial(name);
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void btnTransLen_Click(object sender, EventArgs e)
        {
            float x = 1;
            float.TryParse(numericUpDown1.Value.ToString(), out x);
            if (selectedLen != null)
            {
                selectedLen.StatusUpdateEvent += SelectedLen_StatusUpdateEvent;
                selectedLen.AnimateTo(new PointGeo(x, 0), 150);

                //selectedLen.Move(new PointGeo(x,0)); 
                //opticalScene.ResetRays();
                //LoadLensProperties(selectedLen);
            }

        }

        private void SelectedLen_StatusUpdateEvent()
        {
            if (selectedLen != null && selectedLen.IsInAnimate == false)
            {
                selectedLen.StatusUpdateEvent -= SelectedLen_StatusUpdateEvent;
                opticalScene.ResetRays();
                this.Invoke((MethodInvoker)delegate { LoadLensProperties(selectedLen); });
                //this.Invoke(new Action(()=>{ LoadLensProperties(selectedLen); }));  

            }
        }
        private Object lockObj = new Object();
        bool AutoAdjustLensGap = false;
        
        private void btnFocusAlign_Click(object sender, EventArgs e)
        {
            
            if (AutoAdjustLensGap)
            {
                return;
            }
            
            
            if (selectedLine == null)
            {
                MessageBox.Show("请先在光线中选中一个光线作为参考");
                return;
            }
            if (selectedLine.DistanceToOri < 0)
            {
                return;
            }
            if (selectedLen == null)
            {
                MessageBox.Show("请先在透镜中选中一个透镜作为F3");
                return;
            }
            LensObject secondLen = null;
            for (int i = 0; i < lens.Count; i++)
            {
                if (lens[i] == selectedLen)
                {
                    continue;
                }
                secondLen = lens[i];
            }
            if (secondLen == null)
            {
                return;
            }
            if (!ckShowAutoAdjust.Checked)
            {
                this.canvasView1.GetStageController().StopRender();
            }
            this.btnFocusAlign.Enabled = false;

            List<double> xValues = new List<double>();
            List<double> yValues = new List<double>();
            List<double> betaValues = new List<double>();
            double L = selectedLine.DistanceToOri;
            selectedLine.FinishTrack -= SelectedLine_FinishTrack;
            int Count = 200;
            int.TryParse(txtAutoCount.Text, out Count);
            float delta_F1 = (float)0.175;
            float.TryParse(txt_AutoDelta.Text, out delta_F1);
            int cur = 0;
            float left = secondLen.LeftVertex.x;
            float right = (float)L;
            float half = (right - left) / 2;
            bool move_forward = true;
            double distance = -1;
            bool first = true;
            int cal_count = 0;
            float startMargin = secondLen.LeftVertex.x- selectedLen.LeftVertex.x;
            Stopwatch timeWatcher = new Stopwatch();
            AutoAdjustLensGap = true;
            bool isRunning = false;
            selectedLine.FinishTrack += (double v) => {
                if (isRunning==true)
                {
                    return;
                }
                isRunning = true;
                lock (lockObj)
                {
                    timeWatcher.Reset();
                    timeWatcher.Start();

                    //timeWatcher.Stop();
                    //Debug.Print(timeWatcher.ElapsedTicks + "");
                    //Debug.Print(timeWatcher.ElapsedMilliseconds + "");
                    cal_count++;
                    if (Math.Abs(v - L) < 0.0001)
                    {
                        left = secondLen.LeftVertex.x;
                        right = (float)L;
                        half = (right - left) / 2;
                        distance = -1;
                        move_forward = true;
                        first = true;
                        xValues.Add(delta_F1 * cur);
                        yValues.Add(left);
                        double beta = ((selectedLine.curDir.y / selectedLine.curDir.x) / Math.Tan(8.0 / 180 * Math.PI));
                        betaValues.Add(beta);
                        cur++;
                        if (cur >= Count|| AutoAdjustLensGap == false)
                        {
                            this.Invoke(new Action(()=> {
                                this.btnFocusAlign.Enabled = true;
                                lbl_RayImagePoint.Text = L.ToString("f4");

                                // Create a new instance of the Form class
                                Form chartForm = new Form();

                                // Set the title and size of the form
                                chartForm.Text = "Chart Form";
                                chartForm.Size = new Size(800, 600);

                                // Create a new instance of the Chart control
                                Chart chart = new Chart();

                                // Set the size and position of the chart control within the form
                                chart.Size = new Size(380, 280);
                                chart.Location = new Point(10, 10);
                                ChartArea chartArea = new ChartArea("ChartArea1");
                               
                                chartArea.BorderDashStyle = ChartDashStyle.Solid;
                                chartArea.BackColor = Color.WhiteSmoke;// Color.FromArgb(0, 0, 0, 0);       
                                chartArea.ShadowColor = Color.FromArgb(0, 0, 0, 0);
                                chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;//设置网格为虚线
                                chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
                                chart.ChartAreas.Add(chartArea);
                                // Clear any existing series in the chart
                                chart.Series.Clear();

                                // Create the first series
                                var series1 = new Series
                                {
                                    Name = "X",
                                    Color = Color.Blue,
                                    IsVisibleInLegend = true,
                                    ChartType = SeriesChartType.Line
                                };

                                // Add data points to the first series
                                for (int i = 0; i < xValues.Count; i++)
                                {
                                    series1.Points.AddXY(i, xValues[i]-xValues[0]);
                                }
                            
                                // ... add more data points as needed

                                // Create the second series
                                var series2 = new Series
                                {
                                    Name = "Y",
                                    Color = Color.Red,
                                    IsVisibleInLegend = true,
                                    ChartType = SeriesChartType.Line
                                };

                                // Add data points to the second series
                                for (int i = 0; i < yValues.Count; i++)
                                {
                                    series2.Points.AddXY(i, yValues[i]- startMargin);
                                }
                                // ... add more data points as needed

                                // Add the series to the chart
                                chart.Series.Add(series1);
                                chart.Series.Add(series2);
                                chart.Legends.Add("X");
                                chart.Legends.Add("Y");

                                // Add the chart control to the form
                                chartForm.Controls.Add(chart);
                                chart.Dock = DockStyle.Fill;
                                // Show the form
                                chartForm.ShowDialog();
                                //AxisModel axis = new AxisModel();
                                //axis.Title = "mm";
                                //axis.XLabel = "Q";
                                //axis.MaxX = -1;
                                //axis.MinX = 1E10;
                                //axis.MaxY = -1;
                                //axis.MinY = 1E10;
                                //ChartForm formChart = new ChartForm();
                                //formChart.SetAxisModel(axis); 
                                //formChart.Show();
                                //formChart.Shown += (o,f) => {
                                //    for (int i = 0; i < xValues.Count; i++)
                                //    {
                                //        //axis.AddPoint("X", new PointGeo(i, (float)xValues[i]));
                                //        axis.AddPoint("Y", new PointGeo(i, (float)yValues[i]));
                                //    }
                                //};


                            }));
                            string data = "";
                            for (int i = 0; i < xValues.Count; i++)
                            {
                                data += xValues[i].ToString("f3") + "    " + yValues[i].ToString("f3") + "    " + betaValues[i].ToString("f2") + "\n";
                            }
                            File.WriteAllText("./data/xy.txt", data);
                            geoRayImageLengthLabel.Text = "Done, β:" + beta.ToString("f2");
                            this.canvasView1.GetStageController().Resume();
                            AutoAdjustLensGap = false;
                            cur = 0;
                            return;
                        }
                        geoRayImageLengthLabel.Text = v.ToString("f4") + " [" + cur + "\\" + Count + "] β:" + beta.ToString("f2");
                        selectedLen.Move(new PointGeo(delta_F1, 0));
                        opticalScene.ResetRays();
                    }

                    if (half < 1E-7)
                    {
                        AutoAdjustLensGap = false;
                        cur = 0;
                        this.Invoke(new Action(() => {
                            this.btnFocusAlign.Enabled = true;
                            lbl_RayImagePoint.Text = L.ToString("f4");
                        }));
                        MessageBox.Show("Fail to couple image and object point!");
                        return;
                    }
                    if (v < L)
                    { 
                        secondLen.Move(new PointGeo(half, 0));
                        opticalScene.ResetRays();
                    }
                    else
                    {
                        if ((secondLen.LeftVertex.x - half) < left)
                        {
                            secondLen.Move(new PointGeo(half/2, 0));
                        }else
                        {
                            secondLen.Move(new PointGeo(-half, 0));
                        }
                        opticalScene.ResetRays();
                    }
                    geoLabel.Text = v.ToString("f4");
                    //Console.WriteLine((v - L).ToString() + "   half: " + half + "    leftVertex:" + secondLen.LeftVertex.x);
                    if (secondLen.LeftVertex.x < 5)
                    {
                        //return;
                    }
                    if (first == false && Math.Abs(distance) < Math.Abs(v - L))
                    {
                        half = half * 2;
                    }
                    else
                    {
                        //half = half / 2;
                    }
                    distance = (v - L);
                    half = half / 2;
                    first = false;
                    //timeWatcher.Reset();
                    //timeWatcher.Start();

                    timeWatcher.Stop();
                    //Debug.Print(timeWatcher.ElapsedTicks + "");
                    //Debug.Print(timeWatcher.ElapsedMilliseconds + "");
                }
                isRunning = false;
            };

            selectedLen.Move(new PointGeo(delta_F1, 0));
            opticalScene.ResetRays();
            timeWatcher.Reset();
            timeWatcher.Start();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (opticalScene != null)
            {
                opticalScene.SaveScene();
            }
        }

        private void btnStopAutoGap_Click(object sender, EventArgs e)
        {
            this.AutoAdjustLensGap = false;
        }

        private void 另存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = "";
            Utils.FileUtils.SaveFileDialog(ref fileName);
            if (this.opticalScene!=null)
            {
                this.opticalScene.SaveScene(fileName);
            }
            
        }

        private void 加载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = "";
            Utils.FileUtils.SeletFile(ref fileName);
            this.opticalScene.Clear();
            try
            {
                this.opticalScene.LoadScene(fileName);
                Init();
            }
            catch (Exception ef)
            { 
                MessageBox.Show(ef.Message);
            }
        }
    }
}
