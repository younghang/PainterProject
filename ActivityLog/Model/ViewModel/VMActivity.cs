using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using ActivityLog.Model.DataBase;
using ActivityLog.WindowPage;
using Newtonsoft.Json;
using Utils.Database;
using Utils.MVVM;

namespace ActivityLog.Model.ViewModel
{
     
    class VMActivity : INotifyPropertyChanged
    {
        
        private VMActivity()
        {
            Activities = new ObservableCollection<Activity>();
            Records = new ObservableCollection<Record>();
            loadData = new DataBaseLoader();
        }
        IDataHandle loadData;
        public void LoadData()
        { 
            loadData.LoadData();
            if (ARecorsPair==null)
            {
                ARecorsPair = new Dictionary<Activity, List<RecordActivity>>();
            }
            else
            {
                ARecorsPair.Clear();
            }
            foreach (var item in Records)
            {
                if (item is RecordActivity)
                {
                    RecordActivity recordAc = item as RecordActivity;
                    if (!ARecorsPair.ContainsKey(recordAc.Activity))
                    {
                        ARecorsPair.Add(recordAc.Activity, new List<RecordActivity>());
                    }
                    ARecorsPair[recordAc.Activity].Add(recordAc);
                }
            }  
        }
        private void SaveData()
        { 
            loadData.SaveData();
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
        public void AddActivity(Activity activity)
        {
            if (! Instance.Activities.Contains(activity))
            {
                Instance.Activities.Insert(0,activity);
                ActivityDataBase.Instance.InsertActivity(activity);
            } else
            {
                App.GetMainWindow().ToastMessage("Already exist", (MainWindow.TOAST_TYPE.ERROR));
            }
        }
        public void AddRecord(Record record)
        {
            if (!Instance.Records.Contains(record))
            {
                Instance.Records.Insert(0,record);
                RecordDataBase.Instance.InsertRecord(record);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Activity> Activities { get; set; }
        public ObservableCollection<Record> Records { get; set; }
        public Dictionary<Activity, List<RecordActivity>> ARecorsPair { get; set; }
        #region Commands
        #region Add/Edit Activity Commands 
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
                                   ActivityDataBase.Instance.DeleteActivity((Activity)list[0]);
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
                                _ = ActivityDataBase.Instance.UpdateActivity((Activity)list[0]);
                            }
                        }),
                        new Func<object, bool>(o => true));
                return _editACCommand;
            }
        }
        public event Action<Activity> OnActivityEditEvent;
        #endregion
        #region Add/Edit Record Commands 
        private MyCommandT<IList> _deleteRecordCommand;
        public MyCommandT<IList> DeleteRecordCommand
        {
            get
            {
                if (_deleteRecordCommand == null)
                    _deleteRecordCommand = new MyCommandT<IList>(
                        new Action<IList>(list =>
                        {
                            if (list.Count > 0)
                            {
                                if (MessageWin.Confirm("确定删除？"))
                                {
                                    RecordDataBase.Instance.DeleteRecord((Record)list[0]);
                                    this.Records.Remove((Record)list[0]);
                                    MessageWin.MSG("已经删除");
                                    App.GetMainWindow().ToastMessage("delete a record", (MainWindow.TOAST_TYPE.ALERT));
                                }
                            }
                        }),
                        new Func<object, bool>(o => true));
                return _deleteRecordCommand;
            }
        }
        private MyCommandT<IList> _editRecordCommand;
        public MyCommandT<IList> EditRecordCommand
        {
            get
            {
                if (_editRecordCommand == null)
                    _editRecordCommand = new MyCommandT<IList>(
                        new Action<IList>(list =>
                        {
                            if (list.Count > 0)
                            {
                                OnRecordEditEvent?.Invoke((Record)list[0]);
                                _ = RecordDataBase.Instance.UpdateRecord((Record)list[0]);
                            }
                        }),
                        new Func<object, bool>(o => true));
                return _editRecordCommand;
            }
        }
        public event Action<Record> OnRecordEditEvent;
        #endregion

        #endregion
    }
}
