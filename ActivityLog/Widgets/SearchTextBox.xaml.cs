using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
 
namespace ActivityLog.Widgets
{
    /// <summary>
    /// searchTextBox选择后触发的事件
    /// </summary>
    /// <param name="no"></param>
    public delegate void OnSearchItemSelected(int index, NotifyObject no);
    public delegate void OnTextChanged(string msg);
    /// <summary>
    /// searchTextBox的数据来源对象
    /// </summary>
    public interface ISearchDataSource
    {
        bool ContainsItem(string searchKey);
        NotifyObject ToSearchItem();
    }
    /// <summary>
    /// searchTextBox的储存对象
    /// </summary>
    [Serializable]
    public class NotifyObject : INotifyPropertyChanged
    {
        //private string displayName = "";
        public string DisplayName
        {
            get
            {
                //if (string.IsNullOrEmpty(displayName)) displayName= GetItemName();
                return GetItemName();
            }
            //set { displayName = value; RaisePropertyChanged("DisplayName"); }
        }
        /// <summary>
        /// 作为Search Item的下拉显示项目
        /// </summary>
        /// <returns></returns>
        public virtual string GetItemName()
        {
            return "Need to be override.!!!";
        }
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性发生改变时调用该方法发出通知
        /// </summary>
        /// <param name="propertyName">属性名称</param> 
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException("propertyNames");
            foreach (var name in propertyNames)
            {
                this.RaisePropertyChanged(name);
            }
        }
    }
    /// <summary>
    /// MySearchTextBox.xaml 的交互逻辑
    /// 需要设置数据来源对象，显示对象，以及选中触发事件处理
    /// </summary>
    public partial class SearchTextBox : UserControl
    {
        public event OnTextChanged OnSearchFinished;
        public event OnSearchItemSelected OnItemSelected;
        public Action OnHeaderDoubleClick;
        double imageHeight;
        public double ImageHeight
        {
            get { return imageHeight; }
            set
            {
                imageHeight = value;
                //searchImage
            }
        }
        public void SetText(string text)
        {
            this.txtSearchText.Text = text;
        }
        public string GetText()
        {
            return this.txtSearchText.Text;
        }
        public string HintText { get; set; }
        public SearchTextBox()
        {
            InitializeComponent();
            searchItems = new ObservableCollection<NotifyObject>();
            this.DataContext = this;
        }
        //不能直接赋值需手动转换
        public void SetDatas<T>(ObservableCollection<T> l)
        {
            if (this.models == null)
            {
                this.models = new ObservableCollection<ISearchDataSource>();
            }
            this.models.Clear();
            searchItems.Clear();
            foreach (var item in l)
            {
                this.models.Add(item as ISearchDataSource);
            }
        }
        private ObservableCollection<ISearchDataSource> models = null;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (models == null)
            {
                return;
            }
            searchItems.Clear();
            string strSearch = (sender as TextBox).Text.Trim();
            if (strSearch == "")
            {
                return;
            }
            for (int i = 0; i < models.Count; i++)
            {
                if (models[i].ContainsItem(strSearch))
                {
                    if (searchItems.Count < 30)
                    {//只搜索30条数据，不然非常卡
                        searchItems.Add(models[i].ToSearchItem());
                        if (OnSearchFinished != null)
                        {
                            OnSearchFinished("所有结果已找到,共" + searchItems.Count + "条");
                        }
                    }
                    else
                    {
                        if (OnSearchFinished != null)
                            OnSearchFinished("前30条结果已找到,其他不显示");
                    }
                    //if (!viewModel.CurModels.Contains(viewModel.Models[i]))
                    //{
                    //    viewModel.CurModels.Add(viewModel.Models[i]);
                    //}
                }
            }
        }
        //内部下拉菜单自己有的数据
        public ObservableCollection<NotifyObject> searchItems { get; set; }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid listBox = txtSearchText.Template.FindName("listSearch", txtSearchText) as DataGrid;
            Viewbox imageBox = txtSearchText.Template.FindName("searchImage", txtSearchText) as Viewbox;
            Border border = txtSearchText.Template.FindName("txtBorder", txtSearchText) as Border;
            Button clearButton = txtSearchText.Template.FindName("clearBox", txtSearchText) as Button;
            txtHintText.Text = HintText;
            if (listBox != null)
            {
                listBox.SelectionChanged += (s, ev) =>
                {
                    NotifyObject cm = listBox.SelectedItem as NotifyObject;
                    if (cm == null)
                    {
                        return;
                    }
                    this.txtSearchText.Text = cm.GetItemName();
                    OnItemSelected(listBox.SelectedIndex, cm);
                };
            }
            if (Double.IsNaN(this.Height))
            {
                this.Height = 16;
            }
            if (imageBox != null)
            {
                imageBox.Width = this.Height * 2 / 3;
                imageBox.Height = this.Height * 2 / 3;
            }
            if (border != null)
            {
                border.CornerRadius = new CornerRadius(this.Height / 2);
            }
            if (clearButton != null)
            {
                clearButton.Height = this.Height * 2 / 3;
                clearButton.Width = this.Height * 2 / 3;
                // disable once LocalVariableHidesMember
                Viewbox clearBox = clearButton.Template.FindName("clearBox", clearButton) as Viewbox;
                if (clearBox != null)
                {
                    clearBox.Height = clearButton.Height;
                    clearBox.Width = clearButton.Height;
                }
            }
            txtSearchText.FontSize = this.Height * 2 / 4;

        }

        private void clearBox_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            txtSearchText.Text = "";
        }
        private void listSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OnHeaderDoubleClick != null) OnHeaderDoubleClick();
        }

    }
}