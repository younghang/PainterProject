using ActivityLog.Model;
using ActivityLog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ActivityLog.WindowPage.UserControls
{
    /// <summary>
    /// UCActivity.xaml 的交互逻辑
    /// </summary>
    public partial class UCActivity : UserControl, IUCSwitch
    {
        public event Action AttachNextEvent;

        public UserControl GetUC() { return this; }
        public void Detach()
        { 
            Storyboard sb = (Storyboard)App.Current.FindResource("closeDW2");
            if (sb.IsFrozen)
            {
                sb = sb.Clone();
            }
            sb.Completed += Sb_Completed;
            sb.Begin(this);
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
        public UCActivity()
        {
            InitializeComponent();
            this.DataContext = VMActivity.Instance;
            VMActivity.Instance.OnActivityEditEvent += Instance_OnActivityEditEvent;
            this.Loaded += UCActivity_Loaded;
 
        }

        private void Instance_OnActivityEditEvent(Activity obj)
        {
            AddNewActivity addnew = new AddNewActivity();
            addnew.CurActivity = obj;
            addnew.IsEdit = true;
            addnew.ShowDialog(); 
        }

        private void UCActivity_Loaded(object sender, RoutedEventArgs e)
        {
            //Activity activity = new Activity();
            //RecordActivity recordActivity = new RecordActivity();
            //recordActivity.Hours = 3;
            //recordActivity.Activity = activity;
            //recordActivity.Remark = "nihao";
            //string str = RecordActivity.ToJsonStr(recordActivity);
            //MessageWin.MSG(str);
        }

        private void addNewActivity_Click(object sender, RoutedEventArgs e)
        {
            AddNewActivity addnew = new AddNewActivity();
            addnew.ShowDialog();
            if (addnew.DialogResult == true)
            {
                Activity activity = addnew.CurActivity;
                VMActivity.Instance.AddActivity(activity);
                App.GetMainWindow().ToastMessage("add a new acitivity", (MainWindow.TOAST_TYPE.MESSAGE));

            }
        }
        ListCollectionView listCollectionView = null;
        private void SearchTextBox_TextChanged(string msg)
        {
            listCollectionView=  CollectionViewSource.GetDefaultView(dataGridAc.ItemsSource) as ListCollectionView;
            listCollectionView.Filter=(item) => {
                Activity activity = item as Activity;
                if (activity==null)
                {
                    return false;
                }
                if (activity.Title.ToLower().Contains(msg.ToLower()))
                {
                    return true;
                }
                if (activity.Remark.ToLower().Contains(msg.ToLower()))
                {
                    return true;
                }
                return false;
            }; 
        }

        private void dataGridAc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lineChart.Title = "Count";
            lineChart.Values.Clear();
            hoursChart.Values.Clear();
            Activity activity = dataGridAc.SelectedValue as Activity;
            List<int> times_data = new List<int>();
            List<double> hours_data = new List<double>();
            var records = from a in VMActivity.Instance.ARecorsPair
                      where a.Key == activity
                      select a.Value;
            if (records.Count()==0)
            {
                return;
            }
            var rs = records.ToList()[0];
            var rgs = from r in rs
                     orderby r.FromDate    descending
                     group r by (int)((DateTime.Now-r.FromDate).Days/7) into gps
                     select gps.AsParallel();
           
            foreach (var recordGroup in rgs)
            {
                times_data.Add(recordGroup.Count());
            }
            times_data.Reverse();
            foreach (var item in times_data)
            {
                if (lineChart.Values.Count>10)
                {
                    continue;
                }
                lineChart.Values.Add(item*1.0);
            }
            //Hours Statics
            foreach (var recordGroup in rgs)
            {
                double hh = 0;
                foreach (var item in recordGroup)
                {
                    hh += item.Hours;
                }
                hours_data.Add(hh);
            }
            hours_data.Reverse();
            double sum = 0;
            foreach (var item in hours_data)
            {
                if (hoursChart.Values.Count > 10)
                {
                    continue;
                }
                sum += item;
                hoursChart.Values.Add(sum);
                //hoursChart.Values.Add(item * 1.0);
            }

        }
    }
}
