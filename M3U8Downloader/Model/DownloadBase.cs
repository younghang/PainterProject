
/*
 * Created by SharpDevelop.
 * User: young
 * Date: 2016/3/9 星期三
 * Time: 19:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
namespace M3U8Downloader.Model
{
    public interface IFinishShow
    {
        void FinishATsI(string str);
    }
    public class Downloader
    {
        public bool cancle = false;
        public System.Timers.Timer trytimer = new System.Timers.Timer();
        public event CallToStop NotOnline;
        public event CallToUpdateUI DownloadUpdate;
        public event UnKownWrongHappen UnKownHappen;
        public event FinishADownload FinishATs;

        protected DownLoadUrl dlr;
        static int Safe = 0;
        public static bool merger = true;
        public static bool first = true;
        public static string filedir;
        public static bool downloading;
        public static int safecount = 0;
        protected virtual void FinishATShow(string str)
        {
            FinishATs(str);
        }
        protected virtual void UnKownHappenShow(string str)
        {
            UnKownHappen(str);
        }
        protected virtual void DownloadUpdateShow(int percent, int i, int pos)
        {
            DownloadUpdate(percent, i,pos);
        }
        protected virtual void NotOnlineShow()
        {
            this.NotOnline();
        }

        protected string videoList = "";
        virtual public void Clear()
        {
            dlr.Empty(); 
        }
        virtual public void Stop() { }
        virtual public void Start() { }
        #region 文件合并操作
        protected void FileTsDeal()
        {
            if (downloading)
            {
                return;
            }

            Safe += 1;
            if (!Directory.Exists(filedir))
            {
                return;
            }

            if (Directory.Exists(filedir) && Directory.GetFiles(filedir).Length == 0)
            {
                Directory.Delete(filedir);
            }
            if (Safe != 1)
            {
                return;
            }
            if (!merger)
            {
                FinishATShow("下载完成，不合并");
                return;
            }
            FinishATShow("下载完成，开始合并");

            var files = Directory.GetFiles(filedir);
            if (files.Length == 0)
            {
                FinishATShow("没有视频,可以关闭");
                return;
            }
            var tsfiles = from file in files
                          let tsfile = new FileInfo(file)
                          where SelectTs(tsfile)
                          orderby int.Parse(tsfile.Name.Split('.')[0])
                          select tsfile;

            var filelist = tsfiles.ToList();

            FileStream sumfile;
            try
            {
                string SumFilePath = filedir + "/" + "视频文件" + ".ts";
                if (File.Exists(SumFilePath))
                {
                    FinishATShow("合并文件已经存在，没有执行，请手动处理");
                    return;
                }
                sumfile = File.Create(SumFilePath);
            }
            catch
            {
                FinishATShow("文件处理异常");
                return;
            }
            sumfile.Close();


            for (int i = 0; i < filelist.Count; i++)
            {
                sumfile = File.Open(filedir + "/" + "视频文件" + ".ts", FileMode.Append);
                FileStream file = null;
                try
                {
                    file = File.Open(filelist[i].FullName, FileMode.Open);
                }
                catch
                {
                    Thread.Sleep(2000);
                    //					if (filelist[i].FullName==sumfile.Name) {
                    //
                    //					}else
                    try
                    {
                        file = File.Open(filelist[i].FullName, FileMode.Open);
                    }
                    catch
                    {
                        if (file != null)
                        {
                            file.Close();
                        }

                        sumfile.Close();
                        FinishATShow("出错了，文件没有合并完成。");
                        Safe -= 1;
                    }

                }
                byte[] buffer = new byte[2048];
                int len = 0;
                while ((len = file.Read(buffer, 0, buffer.Length)) != 0)
                {
                    sumfile.Write(buffer, 0, len);
                }
                file.Close();
                File.Delete(filelist[i].FullName);
                sumfile.Flush();
                sumfile.Close();

            }
            sumfile.Close();
            FinishATShow("合并完成。可以关闭");
            Safe -= 1;

        }
        bool SelectTs(FileInfo di)
        { 
            if (di.Extension == ".ts")
            {
                return true;
            }
            return false;
        }
        #endregion
        public static string filePath;
        protected string fileName;
        public Downloader(string filepath = "./Video")
        {
            filePath = filepath;

        }
        protected void InitialFileDir()
        {
            filedir = filePath + "/" + filedir;
            if (!Directory.Exists(filedir))
            {
                Directory.CreateDirectory(filedir);
            }
        }
        //		protected  void DownloadVideo()
        //		{
        //			if (!dlr.Hasurl()) {
        //				return;
        //			}
        //			string str = dlr.GetOneLink();
        //			downloading = true;
        //			dlr.RemoveUrl( dlr.downloadpos - 3);
        //			int split = str.IndexOf('$');
        //			fileName = str.Substring(0, split);
        //			string url = str.Substring(split + 1, str.Length - split - 1);
        //
        //			WebClient webClient = new WebClient();
        //			webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
        //			webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
        //			string fileLocal = filedir + "/" + fileName + ".ts";
        //			webClient.DownloadFileAsync(new Uri(url), fileLocal);
        //		}
        //		void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //		{
        //			first = true;
        //			downloading = true;
        //			DownloadUpdate(e.ProgressPercentage, dlr.i, dlr.downloadpos);
        //		}
        //
        //		public event CallToUpdateUI DownloadUpdate;
        //		void Completed(object sender, AsyncCompletedEventArgs e)
        //		{
        //			FinishATShow("文件" + fileName + "已经下载好了");
        //			downloading = false;
        //			if (dlr.Hasurl()) {
        //				DownloadVideo();
        //
        //			} else {
        //				FileTsDeal();
        //			}
        //
        //
        //
        //		}

    }
    public class DownLoadUrl
    {
        Dictionary<int, string> dic = new Dictionary<int, string>();
        public DownLoadUrl()
        {
        }
        //		 videoId=75835_YKlQ8   videoId=102152_R1tO7  52320_RuIHA
        public int i = 0;
        public int downloadpos = 1;
        public string timelength;
        public static int safeCount = 0;
         
        protected string local;
        public void Setlocal(string str)
        {
            local = str;
        }

        public string duration;
        public virtual void AnalysisUrl(string str)
        {

        }



        public void AddUrl(string value)
        {

            if (dic.ContainsValue(value))
            {
                return;
            }
            i++;
            dic.Add(i, value);

        }
        public void RemoveUrl(int pos)
        {
            if (dic.ContainsKey(pos))
            {
                dic.Remove(pos);
            }
        }



        public bool Hasurl()
        {
            if (i != 0 && downloadpos <= i)
            {
                return true;
            }
            return false;
        }

        public void Empty()
        {
            dic.Clear();
            i = 0;
            downloadpos = 1;

        }
        public string GetOneLink()
        {
            safeCount++;
            if (safeCount != 1)
            {
                return null;
            }
            if (i == 0 || downloadpos > i)
            {
                return null;
            }
            string link = dic[downloadpos];
            string re = downloadpos.ToString() + "$" + link;
            downloadpos++;
            safeCount--;
            return re;
        }
    }
}