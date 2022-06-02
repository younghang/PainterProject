using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ActivityLog.Model
{
    public enum ACTIVITY_STATE { TBD, ON_GOING, PAUSE, ABORT, FINISH }

    public class Activity : INotifyPropertyChanged
    {
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
        private int id;
        public int ID { get { return id; } set {
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
        public Activity() { }
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

    class Record:INotifyPropertyChanged
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
        public DateTime FromDate=DateTime.Now;
        public Record() { }
    }
    class RecordActivity : Record
    {
        private Activity _activity;
        public Activity Activity {
            get { return _activity; }
            set {
                _activity = value;
                OnPropertyChanged();
            }
        }
        public DateTime ToDate = DateTime.Now;
        private int _currentProgress;
        public int CurrentProgress {
            get { return _currentProgress; }
            set
            {
                _currentProgress = value;
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
