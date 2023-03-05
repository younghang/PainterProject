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
using System.Windows.Media.Animation;
using ActivityLog.Model.ViewModel;
using ActivityLog.Model;

namespace ActivityLog.WindowPage.UserControls
{
    public interface IUCSwitch
    {
        UserControl GetUC();
        void Detach();
        void AfterDetach();
        event Action AttachNextEvent;
    }
    /// <summary>
    /// UCOverview.xaml 的交互逻辑
    /// </summary>
    public partial class UCOverview : UserControl, IUCSwitch
    {
        public event Action AttachNextEvent;

        public UserControl GetUC() { return this; }
        public void Detach()
        {
            Storyboard sb = (Storyboard)App.Current.FindResource("closeDW1");
            if (sb.IsFrozen)
            {
                sb = sb.Clone();
            }
            sb.Completed += Sb_Completed;
            sb.Begin(MainBorder);
        }

        private void Sb_Completed(object sender, EventArgs e)
        {
            AfterDetach();
        }

        public void AfterDetach()
        {
            if (AttachNextEvent != null)
            {
                AttachNextEvent();
            }
        }
        public UCOverview()
        {
            InitializeComponent();
            this.Loaded += UCOverview_Loaded;
            brushes.Add((SolidColorBrush)FindResource("level0"));
            brushes.Add((SolidColorBrush)FindResource("level1"));
            brushes.Add((SolidColorBrush)FindResource("level2"));
            brushes.Add((SolidColorBrush)FindResource("level3"));
            brushes.Add((SolidColorBrush)FindResource("level4"));
            InitCalendarGraph();
        }

        private void UCOverview_Loaded(object sender, RoutedEventArgs e)
        {
            int activitiesCount = VMActivity.Instance.Activities.Count;
            DateTime lastActivityDate = DateTime.Now;
            if (activitiesCount > 0)
            {
                lastActivityDate = VMActivity.Instance.Activities[activitiesCount - 1].StartDate;
            }

            int ongoingCount = 0;
            foreach (var item in VMActivity.Instance.Activities)
            {
                if (item.ACState == ACTIVITY_STATE.ON_GOING)
                {
                    ongoingCount++;
                }
            }
            ActivitiesCount.Text = activitiesCount + "";
            ActivitiesOngoing.Text = ongoingCount + "";
            LastActivityDate.Text = lastActivityDate.ToString("r");

            int recordsCount = VMActivity.Instance.Records.Count;
            DateTime lastRecordDate = DateTime.Now;
            if (recordsCount > 0)
            {
                lastRecordDate = VMActivity.Instance.Records[recordsCount - 1].FromDate;
            }
            double recordHours = 0;
            foreach (var item in VMActivity.Instance.Records)
            {
                if (item is RecordActivity)
                {
                    recordHours += (item as RecordActivity).Hours;
                }
            }
            RecordsCount.Text = recordsCount + "";
            RecordsHours.Text = recordHours + "";
            LastRecordDate.Text = lastRecordDate.ToString("r");



            int recordsCountRecent = VMActivity.Instance.Records.Count;
            DateTime lastRecordDateRecent = DateTime.Now;
            if (recordsCount > 0)
            {
                lastRecordDateRecent = VMActivity.Instance.Records[recordsCount - 1].FromDate;

            }
            double recordHoursRecent = 0;
            recordsCountRecent = 0;
            foreach (var item in VMActivity.Instance.Records)
            {
                if ((DateTime.Now - item.FromDate).TotalDays < 30)
                {
                    recordsCountRecent++;
                    if (item is RecordActivity)
                    {
                        recordHoursRecent += (item as RecordActivity).Hours;
                    }
                }
            }
            RecordsCountRecent.Text = recordsCountRecent + "";
            RecordsHoursRecent.Text = recordHoursRecent + "";
            LastRecordDateRecent.Text = lastRecordDateRecent.ToString("r");

        }


        List<SolidColorBrush> brushes = new List<SolidColorBrush>();

        private async void InitCalendarGraph()
        {
            calendarGraph.Children.Clear();

            Task<Dictionary<int, int>> task = Task<Dictionary<int, int>>.Run(() =>
            { 
                var dataQuery = from rc in VMActivity.Instance.Records
                           group rc by rc.FromDate.DayOfYear;
                Dictionary<int, int> values = new Dictionary<int, int>();
                foreach (var data in dataQuery)
                {
                    int rank = 0;
                    if (data.Count()>=4)
                    {
                        rank = 4;
                    }
                    else if (data.Count()>=3)
                    {
                        rank = 3;
                    }
                    else if (data.Count() >= 2)
                    {
                        rank = 2;
                    }
                    else if (data.Count() >= 1)
                    {
                        rank = 1;
                    }
                    else if (data.Count() >= 0)
                    {
                        rank = 0;
                    }
                    values[data.Key] = rank;
                }
                return values;
                 
            });
            var dic = await task;
            DateTime dateTime = DateTime.Now;
            int year = dateTime.Year;
            int days = 365;
            if (year % 400 == 0 || year % 4 == 0 && year % 100 != 0)
            {
                days = 366;
            }
            else
            {
                days = 365;
            }
            List<Rectangle> rectangles = new List<Rectangle>();
            for (int i = 0; i < days; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Tag = 0;
                rectangles.Add(rect);
            }
            foreach (var item in dic)
            {
                rectangles[item.Key].Tag = item.Value;
            } 
            foreach (var rect in rectangles)
            {
                int index = (int)rect.Tag;
                rect.Fill = brushes[index];
                calendarGraph.Children.Add(rect);
            }
        }
        //随机产生一个符合正态分布的数 u均数，d为方差
        public static double Rand(double u, double d, int seed)
        {
            double u1, u2, z, x;
            //Random ram = new Random();
            if (d <= 0)
            {

                return u;
            }
            u1 = (new Random(seed)).NextDouble();
            u2 = (new Random(seed)).NextDouble();

            z = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);

            x = u + d * z;
            return x;

        }
    }

}
