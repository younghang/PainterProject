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
using System.Windows.Shapes;
using Utils.MVVM;

namespace ActivityLog.WindowPage
{
    /// <summary>
    /// AddRecord.xaml 的交互逻辑
    /// </summary>
    public partial class AddRecord : Window
    {
        private bool isEditMode = false;
        public bool IsEdit { get { return isEditMode; } set { isEditMode = value; } }
        private RecordActivity activity = new RecordActivity();
        public RecordActivity CurRecord { get { return activity; } set { activity = value;this.DataContext = value;  } }
        public AddRecord()
        {
            InitializeComponent();
            this.DataContext = CurRecord;
            this.ACTitleCombox.ItemsSource = VMActivity.Instance.Activities;
            this.Loaded += AddRecord_Loaded;
        }
        private void UpdateTags()
        {
            List<UIElement> removeEles = new List<UIElement>();
            foreach (var item in this.tagPanel.Children)
            {
                if (item is TextBox)
                {
                    removeEles.Add(item as UIElement);
                }
            }
            foreach (var item in removeEles)
            {
                this.tagPanel.Children.Remove(item);
            }
            foreach (var item in CurRecord.Tags)
            {
                string str = item;
                TextBox textBox = new TextBox();
                textBox.Text = str;
                //textBox.ContextMenu = (ContextMenu)FindResource("txtContextMenu");
                textBox.Style = (Style)FindResource("tagText");
                textBox.Background = new SolidColorBrush(TagColors[(tagPanel.Children.Count) % (TagColors.Length)]);
                int index = tagPanel.Children.IndexOf(this.addNewTag);
                this.tagPanel.Children.Insert(index, textBox);
            }
        }
        private void AddRecord_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTags();
        }

        private void CloseWindow(object sender, MouseButtonEventArgs e)
        {
            Storyboard storyboard = (Storyboard)Resources["closeDW2"];
            if (!closeStoryBoardCompleted)
            {
                storyboard.Begin();
            } 
        }
        private bool closeStoryBoardCompleted = false;
        private void closeStoryBoard_Completed(object sender, EventArgs e)
        {
            closeStoryBoardCompleted = true;
            this.Close();
        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            //int a =AcState.SelectedIndex;
            //int b =(int) AcState.Tag;
            this.DialogResult = true;
        }

        private void ACTitleCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Activity activity= (Activity)this.ACTitleCombox.SelectedItem;
            if (!VMActivity.Instance.RecordTagsDic.ContainsKey(activity))
            {
                return;
            }
            var tags = VMActivity.Instance.RecordTagsDic[activity];
            if (tags != null&&tags.Count>0)
            {
                CurRecord.Tags = Utils.CommonUtils.Clone<string>(tags);
                UpdateTags();
            } 
        }
        private Color[] TagColors ={ 
            (Color)ColorConverter.ConvertFromString("Cyan"),
            (Color)ColorConverter.ConvertFromString("#FFFF7D00"),
            (Color)ColorConverter.ConvertFromString("#FFCE3B0B"),
            (Color)ColorConverter.ConvertFromString("#FF00A1FF"),
            (Color)ColorConverter.ConvertFromString("#FF24A346") 
        };

        private void addNewTag_Click(object sender, RoutedEventArgs e)
        {
            string str=InputWindow.PleaseInput("添加Tag");
            if (this.CurRecord.Tags.Contains(str)||string.IsNullOrEmpty(str))
            {
                return;
            }
            TextBox textBox = new TextBox();
            textBox.Text = str;
            textBox.Style = (Style)FindResource("tagText");
            textBox.Background = new SolidColorBrush(TagColors[(tagPanel.Children.Count)%(TagColors.Length)]);
            int index=tagPanel.Children.IndexOf(this.addNewTag);
            this.tagPanel.Children.Insert(index,textBox);
            this.CurRecord.Tags.Add(str);
        }
         
        #region Commands 
        #region Add/Edit Tag Commands 
        private MyCommandT<TextBox> _deleteTagCommand;
        public MyCommandT<TextBox> DeleteTagCommand
        {
            get
            {
                if (_deleteTagCommand == null)
                    _deleteTagCommand = new MyCommandT<TextBox>(
                        new Action<TextBox>(textBox =>
                        {
                            if (textBox == null)
                            {
                                return;
                            }
                            this.tagPanel.Children.Remove(textBox);
                            this.CurRecord.Tags.Remove(textBox.Text);
                        }),
                        new Func<object, bool>(o => true));
                return _deleteTagCommand;
            }
        }
        private MyCommandT<TextBox> _editTagCommand;
        public MyCommandT<TextBox> EditTagCommand
        {
            get
            {
                if (_editTagCommand == null)
                    _editTagCommand = new MyCommandT<TextBox>(
                        new Action<TextBox>(textBox =>
                        {
                            if (textBox == null)
                            {
                                return;
                            }
                            string txtTag = textBox.Text;
                            string str = InputWindow.PleaseInput("修改Tag", txtTag);
                            textBox.Text = str;
                            int index = this.CurRecord.Tags.IndexOf(txtTag);
                            this.CurRecord.Tags[index] = str;
                        }),
                        new Func<object, bool>(o => true));
                return _editTagCommand;
            }
        }
        #endregion

        #endregion

        private void editTag(object sender, RoutedEventArgs e)
        {
            MenuItem mi=sender as MenuItem;
            ContextMenu cm=mi.Parent as ContextMenu;
            TextBox textBox = cm.PlacementTarget as TextBox;
            if (textBox == null)
            {
                return;
            }
            string txtTag = textBox.Text;
            string str = InputWindow.PleaseInput("修改Tag", txtTag);
            textBox.Text = str;
            int index = this.CurRecord.Tags.IndexOf(txtTag);
            this.CurRecord.Tags[index] = str;
        }

        private void deleteTag(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi == null)
            {
                return ;
            } 
            var cm = mi.CommandParameter as ContextMenu;
            if (cm == null)
            {
                return ;
            }
            TextBox textBox = cm.PlacementTarget as TextBox;
            if (textBox == null)
            {
                return;
            } 
            this.tagPanel.Children.Remove(textBox);
            this.CurRecord.Tags.Remove(textBox.Text);
        }
    }
}
