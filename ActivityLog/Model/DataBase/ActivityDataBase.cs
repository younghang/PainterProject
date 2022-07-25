using ActivityLog.WindowPage;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Database;

namespace ActivityLog.Model.DataBase
{
    public class ALDataBase
    {
        protected ALDataBase()
        {
            manager = new SQLiteManager();
        }
        static readonly string DATA_BASE_FILEPATH = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "database\\data.db";
        protected SQLiteManager manager = null;
        protected void Open()
        {
            string error = manager.OpenDB(DATA_BASE_FILEPATH);
            if (error != "")
            {
                MessageWin.MSG(error);
            }
        }
        protected void Close()
        {
            if (manager != null)
            {
                manager.Close();
            }
        }
        public virtual void CreateTable()
        {
            throw new NotImplementedException();
        }
    }
    public class ActivityDataBase : ALDataBase
    {
        static ActivityDataBase _instance = null;
        public static ActivityDataBase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActivityDataBase();
                }
                return (ActivityDataBase)_instance;
            }
        }
        public void DeleteActivity(Activity activity)
        {
            //DELETE FROM TableName WHERE 时间 = '时间' AND 数据 = '数据' AND 状态 = '状态';
            Open();
            string deleteSQL = "delete from activity where activity_id = " + activity.ID;
            manager.ExecuteSQL(deleteSQL);
            Close();
        }
        public override void CreateTable()
        {
            Open();
            string sql = "create table if not exists activity (id integer PRIMARY KEY autoincrement,activity_id integer, name text , content text,create_date TEXT )";
            manager.ExecuteSQL(sql);
            Close();
        }
        public Task InsertActivity(Activity activity)
        {
            return Task.Run(() =>
            {
                //INSERT INTO TableName(时间,数据,状态) VALUES('时间','数据','状态');
                Open();
                string json = Activity.ToJsonStr(activity);
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into activity (activity_id, name, content, create_date) values (");
                sb.Append("'" + activity.ID + "',");
                sb.Append("'" + activity.Title + "',");
                sb.Append("'" + json + "',");
                sb.Append("'" + activity.StartDate.ToString() + "'");
                sb.Append(")");
                try
                {
                    manager.ExecuteSQL(sb.ToString());
                }
                catch (Exception ex)
                {
                    MessageWin.MSG(ex.Message);
                }
                Close();
            });
        }
        public List<Activity> LoadActivitiesFromDataBase()
        {
            List<Activity> activities = new List<Activity>();
            Open();
            string querySQL = "select * from activity";
            SQLiteDataReader reader = manager.QueryData(querySQL);
            while (reader.Read())
            {
                string jsonStr = (string)reader["content"];
                activities.Add(Activity.LoadFromJson(jsonStr));
            }
            Close();
            var alist = from a in activities orderby a.StartDate descending select a;
            return alist.ToList();
        }
        public async Task UpdateActivity(Activity activity)
        {
            Open();
            string querySQL = "select * from activity where activity_id = " + activity.ID;
            SQLiteDataReader reader = manager.QueryData(querySQL);
            bool hasValue = await reader.ReadAsync();
            if (hasValue)
            {
                //UPDATE TableName SET 时间 = '时间', 数据 = '数据', 状态 = '状态' WHERE 时间 = '时间' AND 数据 = '数据' AND 状态 = '状态';

                string json = Activity.ToJsonStr(activity);
                StringBuilder sb = new StringBuilder();
                sb.Append("update activity set name =");
                sb.Append("'" + activity.Title + "',");
                sb.Append("content = '" + json + "' ");
                sb.Append("where activity_id = " + activity.ID);
                try
                {
                    manager.ExecuteSQL(sb.ToString());
                }
                catch (Exception ex)
                {
                    MessageWin.MSG(ex.Message);
                }
            }
            else
            {
                _ = InsertActivity(activity);
            }
            Close();

        }
    }
    public class RecordDataBase : ALDataBase
    {
        static RecordDataBase _instance = null;
        public static RecordDataBase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RecordDataBase();
                }
                return (RecordDataBase)_instance;
            }
        }
        public override void CreateTable()
        {
            Open();
            string sql = "create table if not exists record (id integer PRIMARY KEY autoincrement, remark text , content text,create_date TEXT )";
            manager.ExecuteSQL(sql);
            Close();
        }
        public void DeleteRecord(Record record)
        {
            //DELETE FROM TableName WHERE 时间 = '时间' AND 数据 = '数据' AND 状态 = '状态';
            Open();
            string deleteSQL = "delete from record where create_date = '" + record.FromDate.ToString() + "'";
            manager.ExecuteSQL(deleteSQL);
            Close();
        }
        public Task InsertRecord(Record record)
        {
            return Task.Run(() =>
            {
                //INSERT INTO TableName(时间,数据,状态) VALUES('时间','数据','状态');
                Open();
                string json = record.ToJsonStr();
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into record ( remark, content, create_date) values (");
                sb.Append("'" + record.Remark + "',");
                sb.Append("'" + json + "',");
                sb.Append("'" + record.FromDate.ToString() + "'");
                sb.Append(")");
                try
                {
                    manager.ExecuteSQL(sb.ToString());
                }
                catch (Exception ex)
                {
                    MessageWin.MSG(ex.Message);
                }
                Close();
            });
        }
        public List<Record> LoadRecordsFromDataBase()
        {
            List<Record> records = new List<Record>();
            Open();
            string querySQL = "select * from record";
            SQLiteDataReader reader = manager.QueryData(querySQL);
            while (reader.Read())
            {
                string jsonStr = (string)reader["content"];
                try
                {
                    Record record = (Record.LoadFromJson(jsonStr));
                    if (record is RecordActivity)
                    {
                        
                        RecordActivity recordActivity = record as RecordActivity;
                        if (recordActivity.Activity == null)
                        {
                            continue;
                        }
                        foreach (var activity in ViewModel.VMActivity.Instance.Activities)
                        {
                            
                            if (recordActivity.Activity.ID == activity.ID)
                            {
                                recordActivity.Activity = activity;
                                break;
                            }
                        }
                    }
                    records.Add(record);
                }
                catch (Exception)
                {
                    continue;
                }
               
            }
            Close();
            var rs = from r in records orderby r.FromDate descending select r;
            return rs.ToList();
        }
        public async Task UpdateRecord(Record record)
        {
            Open();
            string querySQL = "select * from record where create_date = '" + record.FromDate.ToString()+"'";
            try
            {
                SQLiteDataReader reader = manager.QueryData(querySQL);
                bool hasValue = await reader.ReadAsync();
                if (hasValue)
                {
                    //UPDATE TableName SET 时间 = '时间', 数据 = '数据', 状态 = '状态' WHERE 时间 = '时间' AND 数据 = '数据' AND 状态 = '状态';

                    string json = record.ToJsonStr();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("update record set remark =");
                    sb.Append("'" + record.Remark + "',");
                    sb.Append("content = '" + json + "' ");
                    sb.Append("where create_date = '" + record.FromDate.ToString()+"'");
                    try
                    {
                        manager.ExecuteSQL(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageWin.MSG(ex.Message);
                        App.GetMainWindow().ToastMessage("修改记录失败", MainWindow.TOAST_TYPE.ERROR);
                    }
                }
                else
                {
                    _ = InsertRecord(record);
                }
                
            }
            catch (Exception e)
            { 
                MessageWin.MSG(e.Message) ;
            }
            Close();
        }

    }
}
