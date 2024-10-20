using ShowDanmu.FullScreen;
using ShowDanmu.RightSide;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace DailyClickCount
{
    public partial class Form1 : Form
    {
        private Dictionary<DateTime, int> seconds = new Dictionary<DateTime, int>();
        private Dictionary<DateTime, int> clickCounts = new Dictionary<DateTime, int>();
        private Dictionary<DateTime, int> keyCounts = new Dictionary<DateTime, int>();
        private Queue<DateTime> workingTime = new Queue<DateTime>(1000);
        private DataGridView dataGridView;
        private RichTextBox textBox;
        public static string START_UP_PATH = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        System.Timers.Timer idlTimer = new System.Timers.Timer();
        System.Timers.Timer countTimer = new System.Timers.Timer();
        int workingSecs = 0;
        private static string dataFilePath = "click_counts.txt";
        public Form1()
        {
            idlTimer.Elapsed += IdlTimer_Elapsed;
            countTimer.Elapsed += CountTimer_Elapsed;
            idlTimer.Interval = 1000*60;//一分钟无动作 停止计时
            countTimer.Interval = 1000;
            idlTimer.Start();
            idlTimer.AutoReset = false;
            InitializeComponent();
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.Columns.Add("Date", "Date");
            dataGridView.Columns.Add("Clicks", "Clicks");
            dataGridView.Columns.Add("Keys", "Keys");
            dataGridView.Columns.Add("Seconds", "Seconds");
            textBox = new RichTextBox();
            textBox.Width = 600;
            textBox.Height = 200;
            textBox.Dock = DockStyle.Top;
            textBox.Visible = false;
            // 添加DataGridView到窗口
            this.Controls.Add(dataGridView);
            this.Controls.Add(textBox);

            // 从文件加载之前保存的点击次数
            LoadClickCounts();

            this.Load += Form1_Load;
        }

        private void CountTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            workingSecs++;
            DateTime today = DateTime.Today;
            if (seconds.ContainsKey(today))
            {
                seconds[today]=(workingSecs);
            }
            else
            {
                seconds[today] = 0;
            }
        }

        private void IdlTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            countTimer.Stop();
        }

        KeyboardHook key = new KeyboardHook();
        MouseHook mouse = new MouseHook();
        private void Form1_Load(object sender, EventArgs e)
        {
            // 钩子回调函数 
            mouse.OnMouseActivity += mouse_OnMouseActivity;
            key.OnKeyDownEvent += key_OnKeyDownEvent;
            mouse.Start();
            key.Start();
            key.OnCopyText += (s) => {
                this.textBox.Text = s;
            };
            this.FormClosing += Form1_FormClosing;

            // 创建一个新的 NotifyIcon 对象
            notifyIcon = new NotifyIcon();

            // 设置图标和提示文本
            notifyIcon.Icon = Properties.Resources.Icon1;
            notifyIcon.Text = "ClickCount";
            notifyIcon.Click += NotifyIcon_Click;
            // 添加菜单项
            ContextMenu menu = new ContextMenu();
            menu.MenuItems.Add("打开", OnOpen);
            menu.MenuItems.Add("退出", OnExit);
            notifyIcon.ContextMenu = menu;

            // 将窗体最小化到托盘

            this.FormClosing += OnFormClosing;
            this.Resize += OnFormResize;
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;

            // 显示托盘图标
            notifyIcon.Visible = true;
            saveTimer.Interval = 1000 * 60*5;
            saveTimer.Tick += SaveTimer_Tick;
            saveTimer.Start();
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            // 显示窗体并还原到正常大小 
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.BringToFront();
            this.TopLevel = true;
        }

        private void SaveTimer_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToString() + ": saved.";
            SaveClickCounts();
            if (workingTime.Count == 1000)
            {
                DateTime t1 = workingTime.First();
                DateTime t2 = workingTime.Last();
                if ((t2 - t1).TotalMinutes > 60)
                {
                    this.BringToFront();
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Text = ("按键强度过高，请注意休息");
                }
            }
            {

                // 获取当前exe所在目录
                string exePath = AppDomain.CurrentDomain.BaseDirectory;

                // 构建备份文件夹路径
                string backupFolderPath = Path.Combine(exePath, "BackUp");

                // 检查备份文件夹是否存在，不存在则创建
                if (!Directory.Exists(backupFolderPath))
                {
                    Directory.CreateDirectory(backupFolderPath);
                }

                // 构建待备份文件路径
                string sourceFilePath = Path.Combine(exePath, "click_counts.txt");

                // 获取备份文件夹中的文件列表
                DirectoryInfo backupFolder = new DirectoryInfo(backupFolderPath);
                FileInfo[] backupFiles = backupFolder.GetFiles();

                // 按创建时间降序排序文件列表
                backupFiles = backupFiles.OrderByDescending(f => f.CreationTime).ToArray();

                // 检查上一次备份文件的日期
                if (backupFiles.Length > 0)
                {
                    FileInfo lastBackupFile = backupFiles[0]; // 获取最近的备份文件

                    DateTime lastBackupDate = lastBackupFile.CreationTime;
                    TimeSpan timeSinceLastBackup = DateTime.Now - lastBackupDate;

                    // 如果距离上次备份超过3天，则进行备份
                    if (timeSinceLastBackup.Days >= 3)
                    {
                        // 构建备份文件名
                        string backupFileName = DateTime.Now.ToString("yyyyMMdd") + Path.GetExtension(sourceFilePath);
                        string backupFilePath = Path.Combine(backupFolderPath, backupFileName);

                        // 复制文件到备份文件夹
                        File.Copy(sourceFilePath, backupFilePath);

                        Console.WriteLine("备份成功！");
                    }
                    else
                    {
                        Console.WriteLine("上次备份距今不足3天，无需备份。");
                    }
                }
                else
                {
                    // 如果还没有进行过备份，则进行备份
                    string backupFileName = DateTime.Now.ToString("yyyyMMdd") + Path.GetExtension(sourceFilePath);
                    string backupFilePath = Path.Combine(backupFolderPath, backupFileName);

                    File.Copy(sourceFilePath, backupFilePath);

                    Console.WriteLine("备份成功！");
                }
                
                // 删除超过90天的备份文件
                foreach (FileInfo backupFile in backupFiles)
                {
                    TimeSpan timeSinceBackup = DateTime.Now - backupFile.CreationTime;

                    if (timeSinceBackup.Days > 90)
                    {
                        FileInfo ff=new FileInfo(sourceFilePath);
                        if (backupFile.Length<ff.Length)
                        {
                            File.Delete(backupFile.FullName);
                            Console.WriteLine($"删除过期备份文件：{backupFile.Name}");
                        }
                    }
                }
            }
        }

        Timer saveTimer = new Timer();
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 保存点击次数到文件
            SaveClickCounts();
        }
        void key_OnKeyDownEvent(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (countTimer.Enabled == false)
            {
                countTimer.Start();
                idlTimer.Stop();
                idlTimer.Start();
            }
            //this.Text = e.KeyData.ToString();
            // 等待用户按下 Ctrl+C
            if (e.Control && e.KeyCode == Keys.C)
            {
                //ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                //// 用户按下 Ctrl+C
                //if (keyInfo.Key == ConsoleKey.C)
                //{
                // 获取剪贴板中的文本
                string clipboardText = Clipboard.GetText();
                this.Text = clipboardText;
                //}
            }
            DateTime today = DateTime.Today;
            if (keyCounts.ContainsKey(today))
            {
                keyCounts[today]++;
            }
            else
            {
                keyCounts[today] = 1;
            }
            this.workingTime.Enqueue(DateTime.Now);
            RefreshDataGridView();
        }
        void mouse_OnMouseActivity(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (countTimer.Enabled == false)
            {
                countTimer.Start();
                idlTimer.Stop();
                idlTimer.Start(); 
            }
            if (e.Button == MouseButtons.Left && e.Delta == 1)
            {
                DateTime today = DateTime.Today;
                if (clickCounts.ContainsKey(today))
                {
                    clickCounts[today]++;
                }
                else
                {
                    clickCounts[today] = 1;
                }
                if (keyCounts.ContainsKey(today))
                {
                    
                }
                else
                {
                    keyCounts[today] = 1;
                }
                MainOverlay.Instance.AddText("Click", clickCounts[today].ToString()+"   ["+keyCounts[today].ToString()+"]");
                if (clickCounts[today]%500==0|| keyCounts[today] % 1000 == 0)
                {
                    MainOverlay.Instance.ToastMessage("超标！！！", MainOverlay.TOAST_TYPE.ERROR);
                    DanmakuCurtain.Instance.Shoot("超标！！！");
                }
                if (MainOverlay.Instance.GetTextCount()>18)
                {
                    MainOverlay.Instance.ToastMessage("静心！！！", MainOverlay.TOAST_TYPE.ERROR);
                    DanmakuCurtain.Instance.Shoot("静心！！！");
                }
                this.workingTime.Enqueue(DateTime.Now);
                RefreshDataGridView();
            }
            //this.Text = "X=" + e.X + ",Y=" + e.Y;
        }


        // 刷新DataGridView
        private void RefreshDataGridView()
        {
            if (clickCounts.Count % 500 == 0)
            {
                this.BringToFront();
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Text = ("按键强度过高，请注意休息");
                MessageBox.Show("按键强度过高，请注意休息");
            }
            dataGridView.Rows.Clear();
            for (int i = clickCounts.Count - 1; i >= 0; i--)
            {
                int keyCount = 0;
                int mouseCount = 0;
                int ms = 0;
                if (clickCounts.Count > i)
                {
                    mouseCount = clickCounts.ElementAt(i).Value;
                }
                if (keyCounts.Count > i)
                {
                    keyCount = keyCounts.ElementAt(i).Value;
                }
                var key = clickCounts.ElementAt(i).Key;
                if (seconds.ContainsKey(key))
                {
                   ms = seconds[key];
                } 
                dataGridView.Rows.Add(clickCounts.ElementAt(i).Key.ToShortDateString(), mouseCount, keyCount, ConvertSecondsToHMS(ms));
            }

        }
        public static string ConvertSecondsToHMS(int totalSeconds)
        {
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
        // 从文件加载点击次数
        private void LoadClickCounts()
        {
            if (File.Exists(START_UP_PATH + dataFilePath))
            {
                string filePath = START_UP_PATH + dataFilePath;
                FileInfo fileInfo = new FileInfo(START_UP_PATH + dataFilePath);


                // 获取当前exe所在目录
                string exePath = AppDomain.CurrentDomain.BaseDirectory;

                // 构建备份文件夹路径
                string backupFolderPath = Path.Combine(exePath, "BackUp");

                // 检查备份文件夹是否存在，不存在则创建
                if (!Directory.Exists(backupFolderPath))
                {
                    Directory.CreateDirectory(backupFolderPath);
                }
                 
                // 获取备份文件夹中的文件列表
                DirectoryInfo backupFolder = new DirectoryInfo(backupFolderPath);
                FileInfo[] backupFiles = backupFolder.GetFiles();

                // 按创建时间降序排序文件列表
                backupFiles = backupFiles.OrderByDescending(f => f.CreationTime).ToArray();

                // 检查上一次备份文件的日期
                if (backupFiles.Length > 0)
                {
                    FileInfo lastBackupFile = backupFiles[0]; // 获取最近的备份文件
                    if(fileInfo.Length<lastBackupFile.Length)
                    {
                        filePath = lastBackupFile.FullName;
                    }
                }
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 2)
                        {
                            DateTime date = DateTime.Parse(parts[0]);
                            int count = int.Parse(parts[1]);
                            clickCounts[date] = count;
                        }

                        else if (parts.Length == 3)
                        {
                            DateTime date = DateTime.Parse(parts[0]);
                            int count = int.Parse(parts[1]);
                            clickCounts[date] = count;
                            count = int.Parse(parts[2]);
                            keyCounts[date] = count;
                        }
                        else if (parts.Length == 4)
                        {
                            DateTime date = DateTime.Parse(parts[0]);
                            int count = int.Parse(parts[1]);
                            clickCounts[date] = count;
                            count = int.Parse(parts[2]);
                            keyCounts[date] = count;
                            count = 0;
                            int.TryParse(parts[3],out count);
                            seconds[date] = count;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to load click counts: " + ex.Message);
                }
            }
        }
        // 保存点击次数到文件
        private void SaveClickCounts()
        {
            Dictionary<DateTime, int> seconds_temp = new Dictionary<DateTime, int>();
            Dictionary<DateTime, int> clickCounts_temp = new Dictionary<DateTime, int>();
            Dictionary<DateTime, int> keyCounts_temp = new Dictionary<DateTime, int>();
            if (File.Exists(START_UP_PATH + dataFilePath))
            {
                string filePath = START_UP_PATH + dataFilePath;
                FileInfo fileInfo = new FileInfo(START_UP_PATH + dataFilePath);


                // 获取当前exe所在目录
                string exePath = AppDomain.CurrentDomain.BaseDirectory;

                // 构建备份文件夹路径
                string backupFolderPath = Path.Combine(exePath, "BackUp");

                // 检查备份文件夹是否存在，不存在则创建
                if (!Directory.Exists(backupFolderPath))
                {
                    Directory.CreateDirectory(backupFolderPath);
                }

                // 获取备份文件夹中的文件列表
                DirectoryInfo backupFolder = new DirectoryInfo(backupFolderPath);
                FileInfo[] backupFiles = backupFolder.GetFiles();

                // 按创建时间降序排序文件列表
                backupFiles = backupFiles.OrderByDescending(f => f.CreationTime).ToArray();

                // 检查上一次备份文件的日期
                if (backupFiles.Length > 0)
                {
                    FileInfo lastBackupFile = backupFiles[0]; // 获取最近的备份文件
                    if (fileInfo.Length < lastBackupFile.Length)
                    {
                        filePath = lastBackupFile.FullName;
                    }
                }
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 2)
                        {
                            DateTime date = DateTime.Parse(parts[0]);
                            int count = int.Parse(parts[1]);
                            clickCounts_temp[date] = count;
                        }

                        else if (parts.Length == 3)
                        {
                            DateTime date = DateTime.Parse(parts[0]);
                            int count = int.Parse(parts[1]);
                            clickCounts_temp[date] = count;
                            count = int.Parse(parts[2]);
                            keyCounts_temp[date] = count;
                        }
                        else if (parts.Length == 4)
                        {
                            DateTime date = DateTime.Parse(parts[0]);
                            int count = int.Parse(parts[1]);
                            clickCounts_temp[date] = count;
                            count = int.Parse(parts[2]);
                            keyCounts_temp[date] = count;
                            count = 0;
                            int.TryParse(parts[3], out count);
                            seconds_temp[date] = count;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to load click counts: " + ex.Message);
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(START_UP_PATH + dataFilePath))
                {
                    for (int i = 0; i < clickCounts.Count; i++)
                    {
                        int keyCount = 0;
                        int mouseCount = 0;
                        int ms = 0;
                        if (clickCounts.Count > i)
                        {
                            mouseCount = clickCounts.ElementAt(i).Value;
                        }
                        if (keyCounts.Count > i)
                        {
                            keyCount = keyCounts.ElementAt(i).Value;
                        }
                        var key = clickCounts.ElementAt(i).Key;
                        if (seconds.ContainsKey(key))
                        {
                            ms = seconds[key];
                        }
                        if (keyCount<keyCounts_temp[key])
                        {
                            keyCount = keyCounts_temp[key];
                        }
                        if (mouseCount < clickCounts_temp[key])
                        {
                            mouseCount = clickCounts_temp[key];
                        }
                        if (ms < seconds_temp[key])
                        {
                            ms = seconds_temp[key];
                        }
                        writer.WriteLine($"{clickCounts.ElementAt(i).Key},{mouseCount},{keyCount},{ms}");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save click counts: " + ex.Message);
            }
        }
        #region 托盘
        NotifyIcon notifyIcon;
        void OnOpen(object sender, EventArgs e)
        {
            // 显示窗体并还原到正常大小 
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.BringToFront();
        }

        void OnExit(object sender, EventArgs e)
        {
            key.Stop();
            mouse.Stop();
            // 退出应用程序
            notifyIcon.Visible = false;
            Application.Exit();
        }

        void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // 取消窗体的关闭事件，最小化到托盘
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Form form = (Form)sender;
                form.Hide();
            }
        }

        void OnFormResize(object sender, EventArgs e)
        {
            // 当窗体最小化时，隐藏窗体并显示托盘图标
            Form form = (Form)sender;
            if (form.WindowState == FormWindowState.Minimized)
            {
                form.Hide();
            }
        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            MainOverlay.Instance.ToastMessage("nihao", MainOverlay.TOAST_TYPE.ALERT);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainOverlay.Instance.AddText("ME", "nihao");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DanmakuCurtain.Instance.Shoot("hahahahah");
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
  
}

