using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Activities;

namespace Utils.Database
{
    public class SQLiteManager 
    {
        private SQLiteConnection conn=null;
        public string OpenDB(string fileName)
        { 
            try
            {
                string[] paramArr = new string[] { "Data Source=", fileName, ";Version=3;" };
                string connStr = string.Concat(paramArr);
                if (conn == null)
                {
                    conn = new SQLiteConnection(connStr);
                    conn.Open();
                }else
                {
                    if (conn.State!=ConnectionState.Open)
                    {
                        conn.Open();
                    }
                }
                return string.Empty;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        public void Close()
        {
            if (conn!=null)
            {
                conn.Close();
            }
            //conn = null;
        }
        //private void CreateTable(string tableName)
        //{
        //    string sql = "create table if not exists activity (id integer PRIMARY KEY autoincrement, name text , content text,create_date TEXT )";
        //    SQLiteCommand command = new SQLiteCommand(sql, conn);
        //    command.ExecuteNonQuery();
        //}
        public void ExecuteSQL(string sql)
        { 
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
           
        }
        public SQLiteDataReader QueryData(string sql)
        {           
            //string sql = "select * from activity order by id";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            return command.ExecuteReader();
            
            //while (reader.Read())
            //{
            //    string jsonStr = (string)reader["text"];
            //}
            //return activities;
        }
    }
}
