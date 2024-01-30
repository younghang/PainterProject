using HtmlAgilityPack;
using M3U8Downloader.Model;
using M3U8Downloader.Widgets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
 
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
 
using Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Windows.Resources;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace M3U8Downloader
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool userStop = false;
        private WaveOutEvent outputDevice;
        private MainOverlay mainOverlay=null;
        private string FilePath = "./Video";
        public bool TestHuya()
        {
            string url;
            string flv;
            // string filePath = "F:/bin/testdouyin.html";
            try
            {

                string roomid = "222624";
                string web_url = "https://m.huya.com/" + roomid; //"305607727597"



                //doc = new HtmlWeb().Load(web_url);

                //通过字符串加载
                string result = "";
                int hours = (DateTime.Now - CookieDate).Hours;


                HttpWebRequest req2 = (HttpWebRequest)WebRequest.Create(web_url);
                req2.Timeout = 5000;
                req2.Method = "GET";
                req2.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Mobile Safari/537.36 Edg/112.0.1722.39";

                HttpWebResponse resp2 = (HttpWebResponse)req2.GetResponse();

                Stream stream = resp2.GetResponseStream();

                try
                {
                    //获取内容
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                finally
                {
                    stream.Close();
                }
                resp2.Close();



                Regex r = new Regex(@"HNF_GLOBAL_INIT =[\s\S]*} </script>");
                var match = r.Match(result);
                string re = match.Value;
                if (string.IsNullOrEmpty(re))
                {
                    Console.WriteLine("Error");
                }

                // 解码
                re = re.Substring(0, re.Length - ("</script>").Length);
                int indexOffirstEqual = re.IndexOf("=");
                string jsonStr = re.Substring(indexOffirstEqual + 1, re.Length - indexOffirstEqual - 1);
                jsonStr = jsonStr.Trim();
                jsonStr = jsonStr.Substring(0, jsonStr.Length );

                JObject jsonObject = JObject.Parse(jsonStr);
                File.WriteAllText("./data2.txt", jsonObject.ToString());
                try
                {
                    var gameStreamInfo = jsonObject["roomInfo"]["tLiveInfo"]["tLiveStreamInfo"]["vStreamInfo"]["value"].First;
                    try
                    {
                        if (gameStreamInfo["sFlvUrl"] != null)
                        {
                            //string info = gameStreamInfo["sFlvUrl"]+"/"+gameStreamInfo["sStreamName"]+"."+ gameStreamInfo["sFlvUrlSuffix"]+"?"+ gameStreamInfo["sFlvAntiCode"] ;
                            string info = gameStreamInfo["sHlsUrl"] + "/" + gameStreamInfo["sStreamName"] + "." + gameStreamInfo["sHlsUrlSuffix"] + "?" + gameStreamInfo["sHlsAntiCode"];

                        }
                        else
                        {

                        }

                    }
                    catch (Exception)
                    {

                    }


                }
                catch (Exception)
                {


                }


                //    try
                //    {
                //        var data = doc.GetElementbyId("RENDER_DATA");
                //        string jsonUrlEncodeStr = data.InnerText;

                //        // 解码
                //        string jsonStr = System.Web.HttpUtility.UrlDecode(jsonUrlEncodeStr);
                //        JObject jsonObject = JObject.Parse(jsonStr);
                //        File.WriteAllText("./room_info.json", jsonObject.ToString());

                //        var room = jsonObject["app"]["initialState"]["roomStore"]["roomInfo"]["room"];
                //        try
                //        {
                //            if (room["stream_url"] != null && room["stream_url"].HasValues)
                //            {
                //                File.WriteAllText("./stream_url.json", room["stream_url"].ToString());
                //                string info = room["stream_url"]["hls_pull_url"].ToString();
                //                url = info;
                //                var streamObj = room["stream_url"];
                //                if (streamObj["flv_pull_url"] != null && streamObj["flv_pull_url"].HasValues)
                //                {

                //                    //var resol = streamObj["default_resolution"].ToString();
                //                    //flv = streamObj["flv_pull_url"][resol].ToString();
                //                    var dic = streamObj["flv_pull_url"].First;
                //                    flv = dic.Last.ToString();
                //                }

                //                return true;
                //            }
                //            else
                //            {
                //                url = "Not on the air.";
                //                return false;
                //            }

                //        }
                //        catch (TimeoutException e)
                //        {
                //            Console.WriteLine(e.Message);
                //            url = "Response Time out.";
                //            return false;
                //        }
                //        catch (Exception)
                //        {
                //            url = "Not on the air.";
                //            return false;
                //        }

                //    }
                //    catch (TimeoutException e)
                //    {
                //        Console.WriteLine(e.Message);
                //        url = "Response Time out.";
                //        return false;
                //    }
                //    catch (Exception)
                //    {
                //        url = "Parse Error!";
                //        return false;
                //    }
                //}
                //catch (TimeoutException e)
                //{
                //    Console.WriteLine(e.Message);
                //    url = "Response Time out.";
                //    return false;
                //}
                //catch (System.Net.WebException e)
                //{
                //    url = "Notify" + e.Message;
                //    return false;
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //    url = "Not on the air.";
                //    return false;
                //}

            }
            catch (Exception e)
            { }
            return false;
        }
    
        public MainWindow()
        {
            InitializeComponent();
            //TestHuya();
            //return;
            string fp = Utils.FileSettings.GetItem("setting.txt", "FilePath", "./Video");
            if (!string.IsNullOrEmpty(fp))
            {
                FilePathEdit.Text = fp;
                this.FilePath = fp;
            }

            string str = Utils.FileSettings.GetItem("setting.txt", "Disable_M3U8", "1");
            if (string.IsNullOrEmpty(str))
            {
                M3U8_Fail = true;
            }
            else
            {
                int result = 0;
                int.TryParse(str, out result);
                if (result > 0)
                {
                    M3U8_Fail = true;
                }
                else
                {
                    M3U8_Fail = false;
                }
            } 
            infoMsg.Text = DateTime.Now.ToString();
            MessageWin.SetLoadingMsg("Parse Live Stream...");
            string liveUrl = "";
            string flv = "";
            var state = MessageWin.LoadingAction(() => GetLiveUrl(out liveUrl, ref flv));
            if (state == false)
            {
                if (liveUrl.Contains("Error"))
                {
                    MessageWin.MSG(liveUrl);
                    ToastMessage("解析失败,程序失效了", TOAST_TYPE.ERROR);
                    Application.Current.Shutdown();
                }else if (liveUrl.Contains("Notify"))
                {
                    ToastMessage(liveUrl, TOAST_TYPE.ALERT);
                }else
                ToastMessage(liveUrl, TOAST_TYPE.MESSAGE);
                StartLiveCheckTimer();
            }
            else
            {
                ToastMessage("已获取到直播流地址:" + liveUrl, TOAST_TYPE.MESSAGE);
                infoMsg.Text = "流地址:" + liveUrl;
                StartLiveStreamSaveThread(liveUrl, flv);
            }
            
            string strShodao = Utils.FileSettings.GetItem("setting.txt", "Play_Shoudao", "1");
            if (!string.IsNullOrEmpty(strShodao))
            {
                int result = 0;
                int.TryParse(strShodao, out result);
                if (result > 0)
                {
                    PlayShoudao();
                }
            }
            int interval = 3;
            string timerInterval = Utils.FileSettings.GetItem("setting.txt", "检测间隔(s)", "3");
            if (!string.IsNullOrEmpty(timerInterval))
            {
                int result = 3;
                int.TryParse(timerInterval, out result);
                if (result > 0)
                {
                    interval = result;
                }
            }
            checkLiveTimer.Elapsed += CheckLiveTimer_Elapsed;

            checkLiveTimer.Interval = interval*1000;
            mainOverlay = new MainOverlay();
            mainOverlay.OpenOverlay();
            mainOverlay.Show();
        }
        private bool cancelCheck = false;
        private void CheckLiveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (lockObj)
            {
                if (cancelCheck==true)
                {
                    infoMsg.Dispatcher.Invoke(() =>
                    {
                        infoMsg.Text = DateTime.Now + " " + "Cancel";
                    });
                    return;
                }
                Random random = new Random();
                int span = random.Next(1, 4);
                Thread.Sleep(span * 1000 );
                string liveUrl = "";
                string flv = "";
                if (GetLiveUrl(out liveUrl, ref flv))
                {
                    cancelCheck = true;
                    ToastMessage("已获取到直播流地址:" + flv, TOAST_TYPE.MESSAGE);
                    checkLiveTimer.Stop();
                    StartLiveStreamSaveThread(liveUrl, flv);
                } 
                else
                {
                    if (liveUrl.Contains("Notify"))
                    {
                        ToastMessage(liveUrl, TOAST_TYPE.ALERT);
                    }else
                    {
                        ToastMessage(liveUrl, TOAST_TYPE.MESSAGE);
                    }
                    infoMsg.Dispatcher.Invoke(() =>
                    {
                        infoMsg.Text = DateTime.Now + " " + liveUrl;
                    });

                    this.Dispatcher.Invoke(
                            () =>
                            {
                                hotCircle.Visibility = Visibility.Hidden;
                                Storyboard storyboard = this.FindResource("loadingAnimation") as Storyboard;
                                storyboard.Stop();
                            }
                            );
                }
            }
        }

        static object lockObj = new object();
        System.Timers.Timer checkLiveTimer = new System.Timers.Timer();

        private void StartLiveCheckTimer()
        {
            cancelCheck = false;
            checkLiveTimer.Start();
        }
        private void StartLiveStreamSaveThread(string url, string flv_url)
        {
            hotCircle.Dispatcher.Invoke(() =>
            {
                hotCircle.Visibility = Visibility.Visible;
                Storyboard storyboard = this.FindResource("loadingAnimation") as Storyboard;
                storyboard.Begin();
            });
            URL = url;
            FLV = flv_url;
            if (userStop == false)
            {
                Start();
            }
            if (mainOverlay != null)
            {
                mainOverlay.ToastMessage("开播啦",TOAST_TYPE.ALERT);
                mainOverlay.ToastMessage("开播啦",TOAST_TYPE.ALERT);
                mainOverlay.ToastMessage("开播啦",TOAST_TYPE.ALERT);
            }
            PlayShoudao();
            if (true)
            {
                string roomid = Utils.FileSettings.GetItem("setting.txt", "Room_ID", "945130793525");

            }
        }
        private void CloseWindow(object sender, MouseButtonEventArgs e)
        {
            Storyboard storyboard = (Storyboard)Resources["closeDW2"];
            if (!closeStoryBoardCompleted)
            {
                storyboard.Begin();
            }
        }
        private bool closeStoryBoardCompleted = false;
        private void closeStoryBoard_Completed(object sender, EventArgs e)
        {
            closeStoryBoardCompleted = true;
            
            checkLiveTimer.Stop();
            outputDevice?.Stop();
            Stop();
            if (mainOverlay != null)
            {
                mainOverlay.Close();
            }
            this.Close();
            Application.Current.Shutdown();
        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        public enum TOAST_TYPE { MESSAGE, ERROR, ALERT } 
        public void ToastMessage(string msgStr, TOAST_TYPE msgType)
        {
            string Title = "Message";
            System.Windows.Media.Color txtColor = Colors.Gray;
            System.Windows.Media.Color iconColor = Colors.White;
            MahApps.Metro.IconPacks.PackIconMaterialKind icon = MahApps.Metro.IconPacks.PackIconMaterialKind.BellAlert;
            switch (msgType)
            {
                case TOAST_TYPE.MESSAGE:
                    Title = "Message";
                    icon = MahApps.Metro.IconPacks.PackIconMaterialKind.BellAlert; ;
                    txtColor = Colors.Gray;
                    iconColor = Colors.White;
                    break;
                case TOAST_TYPE.ALERT:
                    Title = "Alert";
                    icon = MahApps.Metro.IconPacks.PackIconMaterialKind.AlertCircleOutline; ;
                    txtColor = Colors.Orange;
                    iconColor = Colors.Wheat;
                    break;
                case TOAST_TYPE.ERROR:
                    Title = "Error";
                    iconColor = Colors.Red;
                    icon = MahApps.Metro.IconPacks.PackIconMaterialKind.Alert; ;
                    txtColor = Colors.OrangeRed;
                    break;
            }
            if (Dispatcher.CheckAccess())
            {
                LogSnackBar toast = new LogSnackBar();
                toast.LogMsg.Text = msgStr;
                toast.msgIcon.Kind = icon;
                toast.msgIcon.Foreground = new SolidColorBrush(iconColor);
                toast.LogMsg.Foreground = new SolidColorBrush(txtColor);
                toast.LogType.Text = Title;
                toast.ChangeHeight();
                ((Storyboard)toast.Resources["Storyboard1"]).Completed += new EventHandler(this.Toast_Completed);
                this.SnackBar.Children.Add(toast);
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    this.ToastMessage(msgStr, msgType);
                    //if (mainOverlay != null)
                    //{
                    //    mainOverlay.ToastMessage(msgStr, msgType);
                    //}
                }));
            } 
        }
        bool M3U8_Fail = true;
        private void Toast_Completed(object sender, EventArgs e)
        {
            ClockGroup clockGroup = sender as ClockGroup;
            if (clockGroup == null)
            {
                return;
            }
            LogSnackBar toast = Storyboard.GetTarget(clockGroup.Children[2].Timeline) as LogSnackBar;
            if (toast != null)
            {
                this.SnackBar.Children.Remove(toast);
            }
        }
        [Obsolete]
        private void TestGetUrl()
        {
            //RENDER_DATA
            string room_id = "7196701667274771260";
            string app_id = "6383";
            string web_rid = "671182533573";
            string url = "https://live.douyin.com/webcast/room/web/enter/?aid=" + app_id + "&live_id=1&device_platform=web&language=zh-CN&room_id_str=" + room_id + "&web_rid=" + web_rid;
            string other = "https://live.douyin.com/webcast/room/web/enter/?aid=6383&live_id=1&device_platform=web&language=zh-CN&room_id_str=7196701667274771260&web_rid=671182533573";
            bool s = url == other;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36 Edg/109.0.1518.78";
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();

            Stream resultStream = response.GetResponseStream();

            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1024];
            while (true)
            {
                int sz = resultStream.Read(buffer, 0, 1024);
                if (sz == 0) break;
                ms.Write(buffer, 0, sz);
            }
            string jsonStr = Encoding.UTF8.GetString(ms.ToArray());


            var blogRequest = Newtonsoft.Json.Linq.JObject.Parse(jsonStr);
            int result = (int)blogRequest["status_code"];
            if (result != 0)
            {
                App.GetMainWindow().ToastMessage("获取地址失败", MainWindow.TOAST_TYPE.ERROR);
                return;
            }
            string m3u8_url = (string)blogRequest["data"]["data"][0]["stream_url"]["hls_pull_url"];
            MessageBox.Show(m3u8_url);
        }
        Downloader downloader = null;
        string URL = "";
        string FLV = "";
        /// <summary>
        /// 复制或剪切文件到剪切板
        /// </summary>
        /// <param name="files">文件路径数组</param>
        /// <param name="cut">true:剪切；false:复制</param>
        public static void CopyToClipboard(string[] files, bool cut)
        {
            if (files == null) return;
            IDataObject data = new DataObject(DataFormats.FileDrop, files);
            MemoryStream memo = new MemoryStream(4);
            byte[] bytes = new byte[] { (byte)(cut ? 2 : 5), 0, 0, 0 };
            memo.Write(bytes, 0, bytes.Length);
            data.SetData("PreferredDropEffect", memo);
            Clipboard.SetDataObject(data, false);
        }

        void UpdateProgress(int percent, int i, int pos)
        {
            //SetInfoText(progressBar1, percent.ToString());
            //SetInfoText(txt_totalTs, (i + 1).ToString());
            //SetInfoText(txt_CurrentDownload, pos.ToString());
        }
        void PlayShoudao()
        {
            byte[] fileByte = Properties.Resources.shoudao;
            Stream stream = new MemoryStream(fileByte);
            var reader = new NAudio.Wave.Mp3FileReader(stream);
            var waveOut = new WaveOut();
            waveOut.Init(reader);
            waveOut.Play();
        }
        void UpdateInfo(string str)
        {
            if (str.Contains("Error") || str.Contains("Finish"))
            {
                Stop();
                if (str.Contains("Error"))
                {
                    this.Dispatcher.Invoke(
                    () =>
                    {
                        hotCircle.Visibility = Visibility.Hidden;
                        Storyboard storyboard = this.FindResource("loadingAnimation") as Storyboard;
                        storyboard.Stop();
                    }
                    );
                    ToastMessage(str, TOAST_TYPE.ERROR);
                }
                else
                {
                    ToastMessage(str, TOAST_TYPE.ALERT);
                }
                this.Dispatcher.Invoke(() =>
                {
                    infoMsg.Text = DateTime.Now + " 启动在线状态检测";
                });
                StartLiveCheckTimer();
            }
            else
            {
                if (M3U8_Fail == false)
                {
                    ToastMessage(str, TOAST_TYPE.MESSAGE);
                }
                infoMsg.Dispatcher.Invoke(() =>
                {
                    infoMsg.Text = DateTime.Now + " [" + Utils.CommonUtils.GetTimeSpan(DateTime.Now, StartDownloadDate, 1, false) + "] " + str;
                });
            }

            //SetInfoText(txt_Info, str);
            //if (LiveVideoDownloader.first)
            //{
            //    SetInfoText(this, str);
            //}
        }
        void NotOnline()
        {
            UnkownHappen("不在线");
        }

        void UnkownHappen(string str)
        {
            ToastMessage(str, TOAST_TYPE.ALERT);
            if (str.Contains("Error"))
            {
                Stop();
            }
            if (str.Contains("Parse M3U8 data Error")) 
            {
                if (M3U8_Fail == false)
                {
                    M3U8_Fail = true;
                    ToastMessage("更换FLV下载", TOAST_TYPE.ALERT);
                    Start();
                }
            } 
            //txt_Info.Text = str;
            //btn_StartDownload.Enabled = true;
            //btn_StopDownload.Enabled = false;
            //checkBox.Enabled = true;
            //txt_Status.BackColor = Color.Gray;
        }
        DateTime StartDownloadDate = DateTime.Now;
        DateTime CookieDate=DateTime.Now;
        CookieContainer cookieContainer=new CookieContainer();
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            userStop = false;
            Start();
        }
        private void Start()
        {
            //string url = "http://pull-flv-l6.douyincdn.com/stage/stream-112453867740069919_or4.flv?k=5cdc61c705354fe5&t=1676300670";
            //using (WebClient webClient = new WebClient())
            //{
            //    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            //    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            //    string fileLocal = this.FilePathEdit.Text + "/" + "Videos" + ".ts";
            //    webClient.DownloadFileAsync(new Uri(url), fileLocal);
            //}

            //return;

            if (string.IsNullOrEmpty(URL))
            {
                return;
            }
            if (downloader != null)
            {
                this.Dispatcher.Invoke(
                 () =>
                 {
                     infoMsg.Text = "已启动下载";
                 });
                return;
            }

            StartDownloadDate = DateTime.Now;
            this.Dispatcher.Invoke(
                () =>
                {
                    FilePath = FilePathEdit.Text;
                    this.Confirm.IsEnabled = false;
                    this.Cancel.IsEnabled = true;
                }
                );
            if (M3U8_Fail == false)
            {
                downloader = new LiveVideoDownloader(URL, this.FilePath);
            }
            else
            {
                downloader = new SimpleFlvVideoDownloader(FLV, this.FilePath);
            }
            downloader.Clear();
            downloader.DownloadUpdate += UpdateProgress;
            downloader.FinishATs += UpdateInfo;
            downloader.NotOnline += NotOnline;
            downloader.UnKownHappen += UnkownHappen;
            downloader.Start();
        }

        private HtmlNode GetStreamNode(HtmlNode parentNode)   {
            if (parentNode.InnerHtml.Contains(".flv"))
            {
                if (parentNode.ChildNodes.Count == 0)
                {
                    return parentNode;
                }
                else
                {
                    foreach (var item in parentNode.ChildNodes) 
                    {
                        var childs = GetStreamNode(item);
                        if (childs != null)
                        {
                            return childs;
                        }
                    }
                    return null;
                }

            }
            return null;
        }
        public bool GetLiveUrl(out string url, ref string flv)
        {
            // string filePath = "F:/bin/testdouyin.html";
            try
            {
                //https://live.douyin.com/
                // 双子星 208998801140
                //肥宝 945130793525
                //元子 633279863465
                string roomid = "945130793525";//683808954455 realai
                Utils.FileSettings.GetItem("setting.txt", "肥宝", "945130793525");
                //Utils.FileSettings.GetItem("setting.txt", "双子星", "208998801140");
                //Utils.FileSettings.GetItem("setting.txt", "元子", "633279863465");
                roomid =Utils.FileSettings.GetItem("setting.txt", "Room_ID", "945130793525");
                if (roomid=="0")
                {
                    roomid = "945130793525";
                } 
                //roomid = "208998801140";
                string web_url = "https://live.douyin.com/" + roomid; //"305607727597"
                
                HtmlDocument doc = new HtmlDocument();

                //doc = new HtmlWeb().Load(web_url);

                //通过字符串加载
                string result = "";
                int hours = (DateTime.Now-CookieDate).Hours;

                if (hours>2||cookieContainer.Count==0)
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(web_url);
                    req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/109.0";
                    req.Timeout = 5000;
                    req.Method = "GET";
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    var cookies = resp.Headers[HttpResponseHeader.SetCookie].ToString().Split(';',',');
                    
                    cookieContainer = new CookieContainer();
                    for (int i = 0; i < cookies.Length; i++)
                    {
                        var item = cookies[i];
                        Cookie cookie = new Cookie();
                        if (item.ToString().Contains("=")==false)
                        {
                            continue;
                        } 
                        cookie.Name = item.ToString().Split('=')[0].Trim();
                        cookie.Value = item.ToString().Split('=')[1].Trim();
                        cookie.Domain = ".douyin.com";
                        cookieContainer.Add(cookie);
                    }
                    resp.Close();
                    Thread.Sleep(1000);
                    CookieDate = DateTime.Now;
                }
                  
                HttpWebRequest req2 = (HttpWebRequest)WebRequest.Create(web_url);
                req2.Timeout = 5000;
                req2.Method = "GET";
                req2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/109.0";
                req2.CookieContainer = cookieContainer;
                HttpWebResponse resp2 = (HttpWebResponse)req2.GetResponse();

                Stream stream = resp2.GetResponseStream();

                try
                {
                    //获取内容
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                finally
                {
                    stream.Close();
                }
                resp2.Close();

                doc.LoadHtml(result);

                //var web = new HtmlWeb();
                //HttpWebRequest req2 = new HttpWebRequest(); 

                //web.PreRequest = new HtmlWeb.PreRequestHandler(req2);
                //var node = web.Load("https://www.cplemom.com/");

                //doc.Load(filePath, Encoding.UTF8, false);

                try
                {
                    //20230904 Failed
                    //var data = doc.GetElementbyId("RENDER_DATA");
 
                    var data = GetStreamNode(doc.DocumentNode);
                    if (data==null)
                    {
                        url = "Not on the air.";
                        return false;
                    }

                    string jsonUrlEncodeStr = data.InnerText;
                    if (!jsonUrlEncodeStr.Contains(".flv"))
                    //if (!jsonUrlEncodeStr.Contains("self.__pace_f.push")|| !jsonUrlEncodeStr.Contains("origin")|| !jsonUrlEncodeStr.Contains("\\\\u0026"))
                    {
                        throw new Exception("Faild to find flv");
                    }
                     
                    Func<int,string,string> get_flv = (int startIndex,string s )=>{
                        string _flv = "";
                        string ss = s.Substring(startIndex);
                        int index_of_flv = ss.IndexOf(".flv");
                        if (index_of_flv==-1)
                        {
                            return _flv;
                        }
                        string sss = ss.Substring(index_of_flv);
                        int index_of_quote = sss.IndexOf("\"");
                        string sss_pre = ss.Substring(0, index_of_flv);
                        int index_of_http = sss_pre.LastIndexOf("http");
                        if (index_of_http==-1)
                        {
                            return "";
                        }
                        _flv = ss.Substring(index_of_http, index_of_quote + index_of_flv - index_of_http);
                        return _flv;
                            };
                    int index_of_temp = 0;
                    string tempstr = "";
                    List<string> playList = new List<string>();
                    while(index_of_temp < jsonUrlEncodeStr.Length)
                    { 
                        tempstr = get_flv(index_of_temp, jsonUrlEncodeStr);
                        if (tempstr=="")
                        {
                            break;
                        }
                        if (tempstr.Contains("only"))
                        {
                            index_of_temp = jsonUrlEncodeStr.IndexOf(tempstr)+tempstr.Length;
                        }
                        else
                        { 
                            playList.Add(tempstr);
                            index_of_temp = jsonUrlEncodeStr.IndexOf(tempstr) + tempstr.Length;
                        }
                    }
                    if (playList.Count== 0)
                    {
                        url = "Not on the air.";
                        return false;
                    }
                    foreach (var item in playList)
                    {
                        if (item.Contains("hd")|| item.Contains("or4"))
                        {
                            jsonUrlEncodeStr = item;
                            break;
                        }
                    }
                    // 解码
                    string jsonStr = System.Web.HttpUtility.UrlDecode(jsonUrlEncodeStr);
                    //int startPos = jsonStr.IndexOf("\"");
                    //jsonStr =  jsonStr.Substring(startPos+1, jsonStr.Length - startPos-3);
                    //jsonStr = JsonConvert.DeserializeObject<string>(jsonStr);
                    //string flvStr = jsonStr.Substring(jsonStr.IndexOf("origin"));
                    //flvStr = flvStr.Substring(flvStr.IndexOf("http"));
                    //flvStr = flvStr.Substring(0,  flvStr.IndexOf("\""));
                    string flvStr = jsonStr;
                    flvStr = flvStr.Replace("\\u0026", "&");
                    flvStr = System.Web.HttpUtility.UrlDecode(flvStr);
                    flvStr = flvStr.Substring(0, flvStr.Length - 1);
                    url = flvStr;
                    flv = flvStr;
                    return true;

                    JObject jsonObject = JObject.Parse(jsonStr);
                    File.WriteAllText("./room_info.json", jsonObject.ToString());

                    var room = jsonObject["app"]["initialState"]["roomStore"]["roomInfo"]["room"];
                    try
                    {
                        if (room["stream_url"] != null && room["stream_url"].HasValues)
                        {
                            File.WriteAllText("./stream_url.json", room["stream_url"].ToString());
                            string info = room["stream_url"]["hls_pull_url"].ToString();
                            url = info;
                            var streamObj = room["stream_url"];
                            if (streamObj["flv_pull_url"] != null && streamObj["flv_pull_url"].HasValues)
                            {

                                //var resol = streamObj["default_resolution"].ToString();
                                //flv = streamObj["flv_pull_url"][resol].ToString();
                                var dic = streamObj["flv_pull_url"].First;
                                flv = dic.Last.ToString();
                            }

                            return true;
                        }
                        else
                        {
                            url = "Not on the air.";
                            return false;
                        }

                    }
                    catch (TimeoutException e)
                    {
                        Console.WriteLine(e.Message);
                        url = "Response Time out.";
                        return false;
                    }
                    catch (Exception)
                    {
                        url = "Not on the air.";
                        return false;
                    }

                }
                catch (TimeoutException e)
                {
                    Console.WriteLine(e.Message);
                    url = "Response Time out.";
                    return false;
                }
                catch (Exception edd)
                {
                    url = "Parse Error!";
                    return false;
                }
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message);
                url = "Response Time out.";
                return false;
            }
            catch (System.Net.WebException e)
            {
                url ="Notify" + e.Message;
                cookieContainer = new CookieContainer();
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                url = "Not on the air.";
                return false;
            }
        }
        private void SelectPath_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "./Video";
            FileUtils.SelectFilePath(ref filePath);
            ToastMessage(filePath, TOAST_TYPE.MESSAGE);
            this.FilePathEdit.Text = filePath;
            this.FilePath = filePath;
            Utils.FileSettings.SaveItem("setting.txt", "FilePath", filePath);
 
        }
        private void Stop()
        {
            this.Dispatcher.Invoke(
                () =>
                {
                    this.Confirm.IsEnabled = true;
                    this.Cancel.IsEnabled = false;
                }
                );
            if (downloader == null)
            {
                return;
            }
            if (downloader.trytimer.Enabled)
            {
                downloader.trytimer.Stop();
                downloader.trytimer.Enabled = false;
                downloader.cancle = true;
            }
            cookieContainer = new CookieContainer();
            downloader.Stop();
            downloader = null;
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            userStop = true;
            Stop();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var result = MessageWin.Confirm("肥宝贴贴！iヾ(≧▽≦*)oヾ(≧▽≦*)o", "贴贴", "贴");

            string fileName = "tietie2.mp3";
            if (result)
            {
                fileName = "tietie2.mp3";
                byte[] fileByte = Properties.Resources.tietie2;
                Stream stream = new MemoryStream(fileByte);
                var reader = new NAudio.Wave.Mp3FileReader(stream);
                var waveOut = new WaveOut();
                waveOut.Init(reader);
                waveOut.Play();
            }
            else
            {
                fileName = "tietie1.mp3";
                byte[] fileByte = Properties.Resources.tietie1;
                Stream stream = new MemoryStream(fileByte);
                var reader = new NAudio.Wave.Mp3FileReader(stream);
                var waveOut = new WaveOut();
                waveOut.Init(reader);
                waveOut.Play();
            }
            //if (outputDevice == null)
            //{
            //    outputDevice = new WaveOutEvent();
            //    outputDevice.PlaybackStopped += OnPlaybackStopped;
            //    Stream mp3file = GetResourceStream(fileName);
            //    //Uri uri = new Uri("pack://application:,,,/Resources/Audio/" + fileName, UriKind.RelativeOrAbsolute);
            //   // StreamResourceInfo info = Application.GetResourceStream(uri); 
            //    //MemoryStream sound = new MemoryStream(Properties.Resources..);   
            //    WaveStream ws = new Mp3FileReader(mp3file);

            //    outputDevice.Init(ws);
            //    ws.Close();
            //}
            //outputDevice.Play();



        }
        public Stream GetResourceStream(string filename)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string resname = asm.GetName().Name + "./Resources/Audio/" + filename;
            return asm.GetManifestResourceStream(resname);
        }
        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            outputDevice?.Dispose();
            outputDevice = null;  
        }

        private void Avatar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (FLV.Contains("http"))
            {
                try
                {
                    Clipboard.Clear();//清空剪切板 
                    Clipboard.SetDataObject(FLV);
                    Clipboard.Flush();
                    MessageWin.MSG("直播流地址已复制到粘贴板");
                }
                catch (Exception ex)
                { 
                     
                }
               
            }
            PlayShoudao();
            
        }
        public static void ExecuteCommandModel(string exePath, string command)
        {
            bool showInCmd = true;

            var processInfo = new ProcessStartInfo()
            {
                FileName = exePath,
                Arguments = command,
                CreateNoWindow = !showInCmd,
                UseShellExecute = false,
                RedirectStandardError = !showInCmd,
                RedirectStandardOutput = false
            };
            using (var mergeProcess = new Process())
            {

                mergeProcess.StartInfo = processInfo;
                mergeProcess.Start();
                mergeProcess.WaitForExit();
            }
        }
        private void Border_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    string filePath = files[0];
                    string fileName = Path.GetFileName(filePath);
                    FileInfo info = new FileInfo(filePath);
                    MessageWin.SetLoadingMsg("开始转码成MP4...");
                    bool state = MessageWin.LoadingAction(()=> {
                        string strFFmpegDir = Utils.FileSettings.GetItem("setting.txt", "FFMPEG_DIR", @"./FFmpeg/4.4/ffmpeg.exe");
                        if (!string.IsNullOrEmpty(strFFmpegDir))
                        {
                            if (File.Exists(strFFmpegDir))
                            {
                                string cmd = " -i \"" + filePath + "\" -c copy \"" + info.Directory + "/" + info.Name.Replace(info.Extension,"") + "\".mp4";
                                ExecuteCommandModel(strFFmpegDir, cmd);
                                return true;
                            }
                        } 
                        return false;
                    });
                    if (state&&MessageWin.Confirm("删除原文件？"))
                    { 
                        // Delete the file
                        File.Delete(filePath); 
                    }
                    
                   
                }
            }
        }
    }

}
