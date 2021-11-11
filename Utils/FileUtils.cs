using Painter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Utils
{
    public class FileUtils
    {
        public static List<DrawableObject> LoadDrawableFile(string FileName)
        {
            List<DrawableObject> list = new List<DrawableObject>();
            using (FileStream fs = new FileStream(FileName, FileMode.Open))
            {
                IFormatter formatter = new BinaryFormatter();
                list = ((List<DrawableObject>)formatter.Deserialize(fs));
            }
            return list;
        }
        public static void SavePoints(List<PointGeo> track, string fileName = "head_track.txt")
        {
            using (StreamWriter sw = File.CreateText(fileName))
            {
                for (int i = 0; i < track.Count; i++)
                {
                    sw.WriteLine(track[i].X + " " + track[i].Y);
                }
            }
        }

        public static void SaveFile(string name, string str)
        {
            File.WriteAllText(name, str);
        }
        /// <summary>
        /// 遍历文件夹下面的所有文件，并处理
        /// </summary>
        /// <param name="filePath">初始文件夹</param>
        /// <param name="validFileAction">有效文件</param>
        /// <param name="dealFileAction">处理文件</param>
        public static void FindFiles(string filePath, Func<FileInfo, bool> validFileAction, Action<FileInfo> dealFileAction)
        {
            DirectoryInfo theFolder = new DirectoryInfo(filePath);
            //遍历文件夹 
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo NextFile in thefileInfo)
            {  //遍历文件 
                if (validFileAction(NextFile))
                {
                    dealFileAction(NextFile);
                }
            }
            //遍历子文件夹 
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                FindFiles(filePath + "/" + NextFolder.ToString(), validFileAction, dealFileAction);
            }
        }
        public static bool SelectFilePath(ref string FilePath)
        {
            FilePathSelectDialog folderBrowserDialog = new FilePathSelectDialog();
            if (folderBrowserDialog.ShowDialog(null) == DialogResult.OK)
            {
                if (!Directory.Exists(folderBrowserDialog.DirectoryPath))
                {
                    return false;
                }
                FilePath = folderBrowserDialog.DirectoryPath;
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool SeletFile(ref string FileName)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }
                string fileFullName = openFileDialog.FileName;
                if (!File.Exists(fileFullName))
                {
                    return false;
                }
                FileName = fileFullName;
                return true;
            }
        }
    }
    //保存程序配置文件
    public static class FileSettings
    {
        public static readonly string ExecuteFilePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        //private static readonly Dictionary<string, InFiles> settingItems = new Dictionary<string, InFiles>();
        public static void SaveItem(string name, string key, string value)
        {
            new InFiles(name).SetSettingItem(key, value);
        }
        public static string GetItem(string name, string key)
        {
            if (File.Exists(name))
            {
                return new InFiles(name).GetSettingItem(key);
            }
            return "";
        }
    }
    class InFiles
    {
        /// <summary>
        /// []分类
        /// #说明注释
        /// </summary>
        /// <param name="fileName"></param>
        public InFiles(string fileName)
        {
            this.FileName = fileName;
            ConfigureFileName = SettingFilePath + FileName;
        }
        private string FileName = "data/set.in";
        public static string DEFAULT_FILE_NAME = "data/set.in";
        private Dictionary<string, string> settingItems = new Dictionary<string, string>();
        private static readonly string SettingFilePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        private string ConfigureFileName;
        private string CONFIGURE_TEXT = "";
        private void LoadConfigurations()
        {
            settingItems.Clear();
            CONFIGURE_TEXT = "";
            if (File.Exists(ConfigureFileName))
            {
                CONFIGURE_TEXT = File.ReadAllText(ConfigureFileName);
            }
            string[] setItems = CONFIGURE_TEXT.Split('\n');
            for (int i = 0; i < setItems.Length; i++)
            {
                if (setItems[i].StartsWith("[") || setItems[i].StartsWith("#"))
                    continue;
                if (setItems[i].Contains("="))
                {
                    string key = setItems[i].Split('=')[0];
                    string value = setItems[i].Split('=')[1].Split('#')[0];
                    if (key != "" && !settingItems.ContainsKey(key))
                    {
                        settingItems.Add(key, value);
                    }
                }
            }
        }
        private void SaveConfigurations()
        {
            CONFIGURE_TEXT = "";
            foreach (var item in settingItems)
            {
                CONFIGURE_TEXT += (item.Key + "=" + item.Value + "\n");
            }
            File.WriteAllText(ConfigureFileName, CONFIGURE_TEXT);
        }
        public string GetSettingItem(string key)
        {
            LoadConfigurations();
            if (settingItems.ContainsKey(key))
            {
                return settingItems[key];
            }
            return "";
        }
        public void SetSettingItem(string key, string value)
        {
            LoadConfigurations();
            if (key != "" && !settingItems.ContainsKey(key))
            {
                settingItems.Add(key, value);
            }
            if (settingItems.ContainsKey(key))
            {
                settingItems[key] = value;
            }
            SaveConfigurations();
        }

    }
}
