﻿
/*
 * Created by SharpDevelop.
 * User: young
 * Date: 02/15/2016
 * Time: 18:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Timers;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace M3U8Downloader.Model
{
    /// <summary>
    /// 负责管理下载视频的地址
    /// </summary>
    public class DownLoadLiveVideoUrl : DownLoadUrl
    {
        public DownLoadLiveVideoUrl(string url)
        {
            string[] sp = { ".m3u8" };
            string temp = url.Split(sp, StringSplitOptions.RemoveEmptyEntries)[0];
            int pos1 = temp.LastIndexOf('\\');
            int pos2 = temp.LastIndexOf('/');
            int pos = Math.Max(pos1, pos2);
            local = temp.Substring(0, pos + 1);
        }
        //		 videoId=75835_YKlQ8   videoId=102152_R1tO7  52320_RuIHA

        public override void AnalysisUrl(string str)
        {
            if (!str.Contains("SEQUENCE"))
            {
                return;
            }
            string[] strlist = str.Split('\n');
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains("TARGETDURATION"))
                {
                    int ins = strlist[i].IndexOf(":") + 1;
                    int end = strlist[i].Length;
                    duration = strlist[i].Substring(ins, end - ins);

                }
                if (strlist[i].Contains(".ts"))
                {
                    string key = strlist[i];
                    AddUrl(local + key);
                }
            }
            StartDownload();
        }
        public event CallToDownload StartDownload;
    }

    public delegate void CallToDownload();
    public delegate void UnKownWrongHappen(string str);
    public delegate void CallToStop();
    public delegate bool CallToStopRecord();
    public delegate void CallToUpdateUI(int percent, int i, int pos);
    public delegate void FinishADownload(string str);
    /// <summary>
    /// 负责下载视频
    /// </summary>
    public class LiveVideoDownloader : Downloader
    {

        Timer timer = new System.Timers.Timer();

        override public void Stop()
        {
            if (timer != null)
            {
                timer.Stop();
            }

            //慎重点
            if (Directory.Exists(filedir) && Directory.GetFiles(filedir).Length == 0)
            {
                Directory.Delete(filedir);
            }

            if (WrongConnect)
            {
                if (cancle == true)
                {
                    cancle = false;
                    count_try = 0;
                    FinishATShow("已停止，完成");

                }
                else
                {
                    FinishATShow("不在线 或者 连接异常.尝试 重新连接一分钟");
                    Retry();
                    return;
                }

            }

            if (notOnline)
            {
                FinishATShow("不在线。尝试合并文件，可能失败，单击0.0可以打开简单合并程序");

            }
            if (!dlr.Hasurl() && !downloading)
            {
                if (safecount == 0)
                {
                    FileTsDeal();
                }
            }
        }


        public int count_try = 0;
        void Retry()
        {

            if (count_try > 6 || trytimer.Enabled)
            {
                FinishATShow("重试次数" + count_try + "。   你可以点停止了0.0");
                return;
            }
            if (!trytimer.Enabled)
            {
                trytimer.Interval = 3 * 1000;
                trytimer.Elapsed += TickTry;
                trytimer.Start();
            }

        }

        void TickTry(object sender, EventArgs e)
        {

            if (count_try >= 6 || !WrongConnect)
            {
                trytimer.Stop();

                if (count_try >= 6)
                {
                    downloading = false;
                    FinishATShow("合并文件");
                    if (safecount == 0 && downloading == false)
                    {
                        FileTsDeal();
                    }
                    System.Threading.Thread.Sleep(1000);
                    FinishATShow("Finish");
                    UnKownHappenShow("连接异常,重试没结果。不在线,可以关闭");
                    return;
                }
                else
                {
                    count_try = 0;
                    DownloadVideoList();
                    return;
                }

            }
            count_try++;
            DownloadVideoList();

        }

        override public void Start()
        {
            notOnline = false;
            WrongConnect = false;
            DateTime dt = DateTime.Now;
            filedir = dt.Year + "-" + dt.Month + "-" + dt.Day + "-" + dt.Hour.ToString() + "@" + dt.Minute.ToString() + "@" + dt.Second;

            filedir = filePath + "/" + filedir;
            if (!Directory.Exists(filedir))
            {
                Directory.CreateDirectory(filedir);
            }

            if (timer == null)
            {
                timer = new Timer();
            }
            timer.Interval = 2 * 1000;
            timer.Start();
        }



        public LiveVideoDownloader(string m3u8_url, string filepath = "./Video")
        {
            videoList = m3u8_url;
            dlr = new DownLoadLiveVideoUrl(m3u8_url);
            filePath = filepath;
            timer.Elapsed += TimerDownloadVideoListTick;
            ((DownLoadLiveVideoUrl)dlr).StartDownload += DownLoadVedio;
        }
        static bool notOnline = false;
        static bool WrongConnect;


        void TimerDownloadVideoListTick(object sender, ElapsedEventArgs e)
        {
            DownloadVideoList();
        }

        void DownloadVideoList()
        {
            Console.WriteLine("准备下载");
            if (WrongConnect)
            {
                FinishATShow("连接重试");
            }
            string str;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(videoList);
            request.Method = "GET";
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (Exception)
            {
                WrongConnect = true;
                Stop();
                Console.WriteLine("biubiu");
                return;
            }

            using (Stream s = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                str = reader.ReadToEnd();
                if (WrongConnect)
                {
                    FinishATShow("恢复连接成功");
                    WrongConnect = false;
                    trytimer.Stop();
                    count_try = 0;
                    timer.Interval = 2 * 1000;
                    timer.Start();
                }

                #region 检查在不在？这个好像没有用
                if (str.Contains("#EXT"))
                    FinishATShow("开始下载");
                else
                {
                    FinishATShow("Error");
                    notOnline = true;
                    Stop();
                    NotOnlineShow();
                    return;
                }
                if (!str.Contains("SEQUENCE"))
                {
                    Stop();
                    UnKownHappenShow("Parse M3U8 data Error!");
                    return;
                }
                #endregion
                notOnline = false;

            }
            PushToDownloadUrl(str);
        }

        void PushToDownloadUrl(string str)
        {
            dlr.AnalysisUrl(str);
            if (string.IsNullOrEmpty(dlr.duration))
            {
                return;
            }
            timer.Interval = 1400 * Double.Parse(dlr.duration);

        }

        void DownLoadVedio()
        {
            if (first)
            {
                try
                {
                    int time = Int32.Parse(dlr.timelength) * Int32.Parse(dlr.duration) / 60;
                    int hour = time / 60;
                    int minute = time - hour * 60;
                    FinishATShow("已经直播时间：" + hour + "小时" + minute + "分钟");
                    // disable once EmptyGeneralCatchClause
                }
                catch (Exception)
                {


                }

            }
            if (!dlr.Hasurl())
            {
                return;
            }
            string str = dlr.GetOneLink();
            safecount++;
            if (str == null)
            {
                return;
            }
            downloading = true;
            dlr.RemoveUrl(dlr.downloadpos - 30);
            int split = str.IndexOf('$');
            fileName = str.Substring(0, split);
            string url = str.Substring(split + 1, str.Length - split - 1);
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                string fileLocal = filedir + "/" + fileName + ".ts";
                webClient.DownloadFileAsync(new Uri(url), fileLocal);

            }
            first = false;
        }


        void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            first = true;
            downloading = true;
            DownloadUpdateShow(e.ProgressPercentage, dlr.i, dlr.downloadpos);
        }
        void Completed(object sender, AsyncCompletedEventArgs e)
        {
            safecount--;
            if (e.Error != null)
            {
                return;
            }
            if (WrongConnect)
                return;
            FinishATShow("文件" + fileName + "已经下载好了");
            downloading = false;
            if (dlr.Hasurl())
            {
                DownLoadVedio();
            }
            else if (!timer.Enabled)
            {
                if (safecount == 0)
                    FileTsDeal();
                else
                    FinishATShow("等待");
            }
        }
    }

    public class SimpleFlvVideoDownloader : Downloader
    {

        public SimpleFlvVideoDownloader(string m3u8_url, string filepath = "./Video",string preFix="")
        {
            videoList = m3u8_url;
            filePath = filepath;
            strFFmpegDir = Utils.FileSettings.GetItem("setting.txt", "FFMPEG_DIR", @"./FFmpeg/4.4/ffmpeg.exe");
            if (!string.IsNullOrEmpty(strFFmpegDir))
            {
                if (File.Exists(strFFmpegDir))
                {
                    IsUsingffmpeg = true;
                }
            }
            PreFix = preFix;
        }
        string PreFix = "";
        bool IsUsingffmpeg = false;
        WebClient webClient = null;
        string strFFmpegDir="";
        string fileLocal = "";
        string fileFullName = "";
        bool IsRunning = false;
        override public void Start()
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            DateTime dt = DateTime.Now;
            string fileName =PreFix + dt.Year + "-" + dt.Month + "-" + dt.Day + "-" + dt.Hour.ToString() + "@" + dt.Minute.ToString() + "@" + dt.Second;
            fileLocal = filePath + "/" + fileName;
            fileFullName = fileLocal;
            if (IsUsingffmpeg)
            {
                Application.Current.Dispatcher.Invoke(() => {
                    App.GetMainWindow().ToastMessage("使用FFMPEG保存", MainWindow.TOAST_TYPE.MESSAGE);
                });
                fileLocal +=  ".flv";
                //ffmpeg  -i rtmp://server/live/streamName -c copy dump.flv 
                ExecuteCommand(strFFmpegDir, " -i " + videoList + " -timeout 30 -c copy " + fileLocal);

            }
            else
            {
                if (webClient != null)
                {
                    webClient.Dispose();
                }
                webClient = new WebClient();
                fileLocal += ".flv";
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.Disposed += WebClient_Disposed;

                webClient.DownloadFileAsync(new Uri(videoList), fileLocal);
            }
            IsRunning = true;
        }

        private void WebClient_Disposed(object sender, EventArgs e)
        {
            Console.WriteLine("123");
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (IsRunning == false)
            {
                return;
            }
            string fileSize = Utils.CommonUtils.GetFileSize(e.BytesReceived);
            FinishATShow(fileSize);
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            FinishATShow("Finish");
        }
        private Process process = null;

        private void ExecuteCommand(string exePath,string command)
        {

            bool showInCmd = false;
            string str = Utils.FileSettings.GetItem("setting.txt", "SHOW_FFMPEG_CMD", "0");
            if (string.IsNullOrEmpty(str))
            {
                showInCmd = false;
            }
            else
            {
                int result = 0;
                int.TryParse(str, out result);
                if (result > 0)
                {
                    showInCmd = true;
                }
                else
                {
                    showInCmd = false;
                }
            }
            var processInfo = new ProcessStartInfo()
            {
                FileName = exePath,
                Arguments = command,
                CreateNoWindow = !showInCmd,
                UseShellExecute = false,
                RedirectStandardError = !showInCmd,
                RedirectStandardOutput = false
            }; 
            process = new Process(); 
            process.StartInfo = processInfo;
            if (!showInCmd)
            {
                process.ErrorDataReceived += Process_ErrorDataReceived; 
            }
            process.Start();
            if (!showInCmd)
            {
                process.BeginErrorReadLine();

            }

        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
            if (IsRunning==false)
            {
                return;
            }
            //frame = 184 fps = 55 q = -1.0 size = 3584kB time = 00:00:09.17 bitrate = 3201.8kbits / s speed = 2.72x
            string msg=e.Data;
            string[] lines = msg.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.Contains("size"))
                {
                    string s = line.Substring(line.IndexOf("size"));
                    s=s.Substring(0, s.IndexOf("kB"));
                    s = s.Replace("size", "");
                    s = s.Replace("=", "");
                    int filesize = 0;
                    int.TryParse(s, out filesize);
                    string fileSize = Utils.CommonUtils.GetFileSize(filesize*1024);
                    FinishATShow(fileSize);
                }
                else if (line.Contains("Stream ends")||line.Contains("I/O error"))
                {
                    FinishATShow("Finish");
                    Stop();
                }else
                {
                    FinishATShow(line);
                }
            }
        }

        override public void Stop()
        {
            IsRunning = false;
            if (IsUsingffmpeg)
            {
                if (process != null)
                {
                    if (!process.HasExited)
                    { 
                        process.Kill();
                        process = null; 
                    }
                    //ffmpeg - i "concat:1.ts|2.ts|3.ts|4.ts" - c copy output.mp4
                    bool converToMp4 = Utils.FileSettings.GetBool("setting.txt", "结束后自动转码成MP4", true);
                    if (converToMp4)
                    {
                        FinishATShow("开始转码，请稍后...");
                        string cmd = " -i \"" + fileLocal + "\" -c copy \"" + fileFullName + "\".mp4";
                        new Task(() => {
                            MainWindow.ExecuteCommandModel(strFFmpegDir, cmd);
                            bool deleteFile = Utils.FileSettings.GetBool("setting.txt", "转码后删除flv文件", false);
                            if (deleteFile)
                            {
                                File.Delete(fileLocal);// 删除文件
                            }
                            FinishATShow("Finish");
                        }).Start(); 
                    }  
                } 
            }
            else
            {
                if (webClient != null)
                {
                    webClient.CancelAsync();
                }
                webClient.Dispose();
                webClient = null;
            }

        }
        override public void Clear()
        {

        }
    }
}
/// 
/// 为WebClient增加超时时间
/// 从WebClient派生一个新的类，重载GetWebRequest方法
/// 

public class NewWebClient : WebClient
{

    private int _timeout;
    /// 
    /// 超时时间(毫秒)
    /// 

    public int Timeout
    {
        get
        {
            return _timeout;
        }
        set
        {
            _timeout = value;
        }
    }
    public NewWebClient()
    {
        this._timeout = 10000;
    }
    public NewWebClient(int timeout)
    {
        this._timeout = timeout;
    }
    protected override WebRequest GetWebRequest(Uri address)
    {
        var result = base.GetWebRequest(address);
        result.Timeout = this._timeout;
        return result;
    }
}