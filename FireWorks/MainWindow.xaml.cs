using FireWorks.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace FireWorks
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            fireCanvas.Background = null;
            this.Background = null;
            controllerModel = new ControllerModel();
            controllerModel.Start += () => { StartFire(); };
            this.DataContext = controllerModel;
            //this.Closing += Window_Closing;
            //this.SizeChanged += Window_SizeChanged;
            //this.Loaded += this.Window_Loaded; 
        }
        private ControllerModel controllerModel;
        int width;
        int height;
        WriteableBitmap wBitmap;
        private delegate void TimerDispatcherDelegate();
        private Timer aTimer = null;
        bool IsStart = false;
        List<Particles> listParticles = new List<Particles>();

        private void StartFire()
        {
            IsStart = true;
            aTimer.Enabled = true;
            aTimer.Start();
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new TimerDispatcherDelegate(updateUI));
        }

        private void updateUI()
        {
            if (controllerModel.CheckFireAndParticles())
            {
                aTimer.Stop();
                IsStart = false;
            }
            controllerModel.Update();
            this.InvalidateVisual();
        }


        protected override void OnRender(DrawingContext drawContext)
        {
            base.OnRender(drawContext);
            if (controllerModel.CheckFireAndParticles())
            {
                return;
            }
            wBitmap = new WriteableBitmap(width, height, 72, 72, PixelFormats.Bgr24, null);
            if (wBitmap == null)
                return;
            wBitmap.Lock();
            Bitmap backBitmap = new Bitmap(width, height, wBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, wBitmap.BackBuffer);
            Graphics graphics = Graphics.FromImage(backBitmap);
            //graphics.Clear(System.Drawing.Color.White);//整张画布置为白色
            graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(50, System.Drawing.Color.Black)), new RectangleF(0, 0, width, height));
            if (IsStart)
                controllerModel.Draw(graphics);
            graphics.Flush();
            graphics.Dispose();
            graphics = null;
            backBitmap.Dispose();
            backBitmap = null;
            wBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            wBitmap.Unlock();
            DisplayImage.Source = wBitmap;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            aTimer.Stop();
            aTimer.Dispose();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            width = (int)fireCanvas.ActualWidth;
            height = (int)fireCanvas.ActualHeight;
            DisplayImage.Width = width;
            DisplayImage.Height = height;
            wBitmap = new WriteableBitmap(width, height, 72, 72, PixelFormats.Bgr24, null);
            DisplayImage.Source = wBitmap;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            width = (int)fireCanvas.ActualWidth;
            height = (int)fireCanvas.ActualHeight;
            if (width > 0 && height > 0)
            {
                DisplayImage.Width = width;
                DisplayImage.Height = height;
                wBitmap = new WriteableBitmap(width, height, 72, 72, PixelFormats.Bgr24, null);
                DisplayImage.Source = wBitmap;
            }
            aTimer = new Timer(20);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 20;
        }
    }
}
