using System; 
using System.ComponentModel; 
using System.Runtime.CompilerServices; 
using Newtonsoft.Json;
using Newtonsoft.Json.Converters; 

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


}
