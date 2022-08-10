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
    /// UCRecord.xaml 的交互逻辑
    /// </summary>
    public partial class UCRecord : UserControl,IUCSwitch
    {
        public UCRecord()
        {
            InitializeComponent();
            this.DataContext = VMActivity.Instance;
            VMActivity.Instance.OnRecordEditEvent += Instance_OnRecordEditEvent;
            this.Loaded += UCRecord_Loaded;
        }

        private void Instance_OnRecordEditEvent(Record obj)
        {
            AddRecord addnew = new AddRecord();
            addnew.CurRecord = (RecordActivity)obj;
            addnew.IsEdit = true;
            int index=VMActivity.Instance.Activities.IndexOf( addnew.CurRecord.Activity);
            addnew.ACTitleCombox.SelectedIndex = index;
            addnew.ShowDialog();
            RecordActivity record = addnew.CurRecord;
            record.Activity = (Activity)addnew.ACTitleCombox.SelectedItem;
        }

        private void UCRecord_Loaded(object sender, RoutedEventArgs e)
        {
 
        }

        public event Action AttachNextEvent;

        public UserControl GetUC() { return this; }
        public void Detach()
        {
            Storyboard sb = (Storyboard)App.Current.FindResource("closeDW0");
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
 
        private void AddNewRecord(object sender, RoutedEventArgs e)
        {
            AddRecord addnew = new AddRecord();
            addnew.ShowDialog();
            if (addnew.DialogResult == true)
            {
                RecordActivity record = addnew.CurRecord;
                record.Activity = (Activity)addnew.ACTitleCombox.SelectedItem;

                VMActivity.Instance.AddRecord(record); 
                App.GetMainWindow().ToastMessage("add a new record",(MainWindow.TOAST_TYPE.MESSAGE));
            }
        }
        ListCollectionView listCollectionView = null;
        private void SearchTextBox_TextChanged(string msg)
        {
            listCollectionView = CollectionViewSource.GetDefaultView(dataGridAc.ItemsSource) as ListCollectionView;
            listCollectionView.Filter = (item) => {
                Record record = item as Record;
                if (record == null)
                {
                    return false;
                }
                if (record.Remark.ToLower().Contains(msg.ToLower()))
                {
                    return true;
                }
                RecordActivity activity = item as RecordActivity;
                if (activity == null)
                {
                    return false;
                }
                if (activity.Activity.Title.ToLower().Contains(msg.ToLower()))
                {
                    return true;
                } 
                return false;
            };
        }
        private Color[] TagColors ={
            (Color)ColorConverter.ConvertFromString("Cyan"),
            (Color)ColorConverter.ConvertFromString("#FFFF7D00"),
            (Color)ColorConverter.ConvertFromString("#FFCE3B0B"),
            (Color)ColorConverter.ConvertFromString("#FF00A1FF"),
            (Color)ColorConverter.ConvertFromString("#FF24A346")
        };
        private void dataGridAc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.tagPanel.Children.Clear();
            foreach (var item in (dataGridAc.SelectedValue as RecordActivity).Tags)
            {
                string str = item;
                TextBox textBox = new TextBox();
                textBox.Text = str;
                //textBox.ContextMenu = (ContextMenu)FindResource("txtContextMenu");
                textBox.Style = (Style)FindResource("tagText");
                textBox.Background = new SolidColorBrush(TagColors[(tagPanel.Children.Count) % (TagColors.Length)]);
                this.tagPanel.Children.Insert(0, textBox);
            }
        }
    }
}
