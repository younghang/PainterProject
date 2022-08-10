using ActivityLog.Model.ViewModel;
using ActivityLog.WindowPage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActivityLog.Model.DataBase
{
    interface IDataHandle
    {
        void LoadData();
        void SaveData();
    }
    class DataBaseLoader : IDataHandle
    {
        public void LoadData()
        {
            string msg = "";
            var result = MessageWin.LoadingAction(() => {
                MessageWin.SetLoadingMsg("Loading DataBase...");
                ActivityDataBase.Instance.CreateTable();
                var activities = ActivityDataBase.Instance.LoadActivitiesFromDataBase();
                foreach (var item in activities)
                {
                    VMActivity.Instance.Activities.Add(item);
                }
                Thread.Sleep(200);
                try
                {
                    MessageWin.SetLoadingMsg("Loading Records...");
                    RecordDataBase.Instance.CreateTable();
                    var records = RecordDataBase.Instance.LoadRecordsFromDataBase();
                    foreach (var item in records)
                    {
                        VMActivity.Instance.Records.Add(item);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    msg = e.Message;
                    return false;
                }

            });
            if (result == false)
            {
                MessageWin.MSG(msg);
            }
        }

        public void SaveData()
        {
             
        }
    }
    [Obsolete]
    //For data.txt.No Use anymore
    class JsonFileLoader : IDataHandle
    {
        public static string FILE_NAME = "data.json";
        public void LoadData()
        {
            if (!File.Exists(FILE_NAME))
            {
                return;
            }
            try
            {
                VMActivity.Instance.Activities.Clear();
                VMActivity.Instance.Records.Clear();
                string text = File.ReadAllText(FILE_NAME);
                Dictionary<string, string> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                string activityStr = keyValuePairs["activity"];
                string recordStr = keyValuePairs["record"];
                ObservableCollection<Activity> activities = JsonConvert.DeserializeObject<ObservableCollection<Activity>>(activityStr);
                foreach (var item in activities)
                {
                    VMActivity.Instance.Activities.Add(item);
                }
                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                ObservableCollection<Record> records = JsonConvert.DeserializeObject<ObservableCollection<Record>>(recordStr, jsonSerializerSettings);
                foreach (var item in records)
                {
                    if (item is RecordActivity)
                    {
                        RecordActivity recordActivity = item as RecordActivity;
                        foreach (var activity in VMActivity.Instance.Activities)
                        {
                            if (recordActivity.Activity.ID == activity.ID)
                            {
                                recordActivity.Activity = activity;
                                break;
                            }
                        }
                    }
                    VMActivity.Instance.Records.Add(item);
                }
 
            }
            catch (Exception)
            {
                MessageWin.MSG("Fail to load data.");
            }
        }

        public void SaveData()
        {
            string activityStr = JsonConvert.SerializeObject(VMActivity.Instance.Activities);
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string recordStr = JsonConvert.SerializeObject(VMActivity.Instance.Records, jsonSerializerSettings);

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs["activity"] = activityStr;
            keyValuePairs["record"] = recordStr;
            string text = JsonConvert.SerializeObject(keyValuePairs);
            File.WriteAllText(FILE_NAME, text);
        }
    }
}
