using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Utils.MVVM;

namespace ActivityLog.Model
{
    public class ChinaDateTimeConverter : DateTimeConverterBase
    {
        private static IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dtConverter.WriteJson(writer, value, serializer);
        }
    }
    public enum ACTIVITY_STATE { TBD, ON_GOING, PAUSE, ABORT, FINISH }

    public class Activity : INotifyPropertyChanged
    {
        public Activity()
        {
            //if (ViewModel.VMActivity.Instance.Activities.Contains(this))
            //{
            //    return;
            //}
            while (true)
            {
                bool isContains = false;
                var items = ViewModel.VMActivity.Instance.Activities;
                foreach (var item in items)
                {
                    if (item == this)
                    {
                        continue;
                    }
                    if (item.id == id)
                    {
                        id++;
                        isContains = true;
                        break;
                    }
                }
                if (isContains == false)
                {
                    break;
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        private ACTIVITY_STATE state = ACTIVITY_STATE.TBD;
        public ACTIVITY_STATE ACState {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged();
            }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        private int id =new Random(System.DateTime.Now.Millisecond).GetHashCode();
        public int ID { get { 
                return id;
            } set {
                id = value;
                OnPropertyChanged();
            } }
        private DateTime startDate = DateTime.Now;
        public DateTime StartDate
        {
            get
            { 
                return this.startDate;
            }
            set
            {
                startDate = value;
                OnPropertyChanged();
            }
        }
        private DateTime dueDate=DateTime.Now;
        [JsonConverter(typeof(ChinaDateTimeConverter))]
        public DateTime DueDate
        {
            get
            { 
                return this.dueDate;
            }
            set
            {
                dueDate = value;
                OnPropertyChanged(); 
            }
        }
        private int _totalProgress;
        public int TotalProgress
        {
            get { return _totalProgress; }
            set
            {
                _totalProgress = value;
                OnPropertyChanged();
            }
        }
        private int _estimateHour;
        public int EstimateHour
        {
            get { return _estimateHour; }
            set
            {
                _estimateHour = value;
                OnPropertyChanged();
            }
        }
        private int _priority;
        public int Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                OnPropertyChanged();
            }
        } 
        
        public double GetEstimateDay()
        {
            return this.EstimateHour / 12;
        }
        private string _remark = "";
        public string Remark
        {
            get { return _remark; }
            set
            {
                _remark = value;
                OnPropertyChanged();
            }
        }


        public static string ToJsonStr(Activity activity)
        {
            string jsonStr = "";
            jsonStr = JsonConvert.SerializeObject(activity);
            return jsonStr;
        }
        public static Activity LoadFromJson(string strJson)
        {
            Activity activity = JsonConvert.DeserializeObject<Activity>(strJson);
            return activity;
        }
    }

    public class Record:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string _remark = "";
        public string Remark
        {
            get { return _remark; }
            set
            {
                _remark = value;
                OnPropertyChanged();
            }
        }
        private DateTime fromDate=DateTime.Now;
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged();
            }
        }
        public Record() { }
        public string ToJsonStr()
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string recordStr = JsonConvert.SerializeObject(this, jsonSerializerSettings); 
            return recordStr;
        }
        public static Record LoadFromJson(string strJson)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            }; 
            Record record = JsonConvert.DeserializeObject<Record>(strJson, jsonSerializerSettings);
            return record;
        }
    }
    public class RAConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Activity);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Activity activity = new Activity();
            try
            {
                activity.ID= int.Parse(reader.Value.ToString());
            }
            catch (Exception)
            {
                activity.ID = 0; 
            }
            return activity;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((Activity)(value)).ID);
        }

        //public DateTime ConvertIntDateTime(long aSeconds)
        //{
        //    return new DateTime(1970, 1, 1).AddMilliseconds(aSeconds);
        //}

        //public long ConvertDateTimeInt(DateTime aDT)
        //{
        //    return (long)(aDT - new DateTime(1970, 1, 1)).TotalMilliseconds;
        //}
    }
    public class RecordActivity : Record
    {
        private Activity _activity;
        [JsonConverter(typeof(RAConverter))]
        public Activity Activity{
            get { return _activity; }
            set {
                _activity = value;
                OnPropertyChanged();
            }
        }
        private DateTime toDate = DateTime.Now;
        [JsonConverter(typeof(ChinaDateTimeConverter))]
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged();
            }
        }
        private int _currentProgress;
        public int CurrentProgress {
            get { return _currentProgress; }
            set
            {
                _currentProgress = value;
                OnPropertyChanged();
            }
        }
        public List<string> Tags = new List<string>();
        private int _hourSpend;
        public int Hours
        {
            get { return _hourSpend; }
            set
            {
                _hourSpend = value;
                OnPropertyChanged();
            }
        }
        private RecordActivity preRecord=null;

        public int GetProgress()
        {
            return this.CurrentProgress - this.preRecord.CurrentProgress;
        }
 
 
    }
}
