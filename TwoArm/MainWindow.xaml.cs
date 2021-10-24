 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwoArm.Model;

namespace TwoArm
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            pointController = new PointController(this);
            this.PointsCombox.ItemsSource = pointController.models;
            this.PointsCombox.DisplayMemberPath = "PointName";
        }
        public PointController pointController;
        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Math.Acos(0.5)/Math.PI*180 + "");
            //for convenience only, prefer to be like this pController.MoveTo(txtX.Text,textY.Text,textSpeed.Text);
            //string.IsNullOrEmpty PointsCombox.SelectionBoxItem
            if (PointsCombox.SelectionBoxItem == null || PointsCombox.SelectionBoxItem.ToString() == "")
            {
                this.lblInfo.Text = "pls select a point"; return;
            }
            PointModel pm = (PointModel)PointsCombox.SelectionBoxItem;
            lblInfo.Text = pointController.MoveTo(pm.X, pm.Y, pm.Speed);
        }
        private DrawingBrush _gridBrush;
        private void mainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (_gridBrush == null)
            {
                _gridBrush = new DrawingBrush(new GeometryDrawing(
                    new SolidColorBrush(Colors.White),
                    new Pen(new SolidColorBrush(Colors.LightGray), 1.0),
                    new RectangleGeometry(new Rect(0, 0, 20, 20))));
                _gridBrush.Stretch = Stretch.None;
                _gridBrush.TileMode = TileMode.Tile;
                _gridBrush.Viewport = new Rect(0.0, 0.0, 20, 20);
                _gridBrush.ViewportUnits = BrushMappingMode.Absolute;
                mainCanvas.Background = _gridBrush;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Title = this.Width + "," + this.Height;
        }
        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            //pointController.SetPoint();
        }
    }
}
