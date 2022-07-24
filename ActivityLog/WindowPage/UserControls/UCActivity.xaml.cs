using ActivityLog.Model;
using ActivityLog.Model.ViewModel;
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
    }
}
