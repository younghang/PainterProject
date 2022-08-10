using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ActivityLog.Model
{
    public class Record : INotifyPropertyChanged
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
        private DateTime fromDate = DateTime.Now;
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
                activity.ID = int.Parse(reader.Value.ToString());
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
        public Activity Activity
        {
            get { return _activity; }
            set
            {
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
        public int CurrentProgress
        {
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
        private RecordActivity preRecord = null;

        public int GetProgress()
        {
            return this.CurrentProgress - this.preRecord.CurrentProgress;
        }


    }
}
