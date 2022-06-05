using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ActivityLog.WindowPage;
using Newtonsoft.Json;
using Utils.MVVM;

namespace ActivityLog.Model.ViewModel
{
    class VMActivity : INotifyPropertyChanged
    {
        private VMActivity()
        {
            Activities = new ObservableCollection<Activity>();
        }
        private static VMActivity _instance = null;
        public static VMActivity Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VMActivity();
                }
                return _instance;
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Activity> Activities { get; set; }
        public static string FILE_NAME = "data.json";
        public void LoadDataFromJson()
        {
            if (!File.Exists(FILE_NAME))
            {
                return;
            }
            try
            {
                Activities.Clear();
                string text = File.ReadAllText(FILE_NAME);
                ObservableCollection<Activity> activities = JsonConvert.DeserializeObject<ObservableCollection<Activity>>(text);
                foreach (var item in activities)
                {
                    Activities.Add(item);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Fail to load data.");
            }
        }
        public void SaveData()
        {
            string text = JsonConvert.SerializeObject(Activities);
            File.WriteAllText(FILE_NAME, text);
        }

        private MyCommandT<IList> _mouseDownCommand;
        public MyCommandT<IList> DeleteACCommand
        {
            get
            {
                if (_mouseDownCommand == null)
                    _mouseDownCommand = new MyCommandT<IList>(
                        new Action<IList>(list =>
                       {
                           if (list.Count > 0)
                           {
                               if (MessageWin.Confirm("确定删除？"))
                               {
                                   this.Activities.Remove((Activity)list[0]);
                                   MessageWin.MSG("已经删除");
                               } 
                           }
                       }),
                        new Func<object, bool>(o => true));
                return _mouseDownCommand;
            }
        }
        private MyCommandT<IList> _editACCommand;
        public MyCommandT<IList> EditACCommand
        {
            get
            {
                if (_editACCommand == null)
                    _editACCommand = new MyCommandT<IList>(
                        new Action<IList>(list =>
                        {
                            if (list.Count > 0)
                            {
                                OnActivityEditEvent?.Invoke((Activity)list[0]);
                            }
                        }),
                        new Func<object, bool>(o => true));
                return _editACCommand;
            }
        }
        public event Action<Activity> OnActivityEditEvent;

    }
}
