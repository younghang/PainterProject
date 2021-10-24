using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utils.MVVM;
using Point = System.Windows.Point;
namespace FireWorks.Model
{
    class ControllerModel : INotifyPropertyChanged
    {
        public static DateTime TIME_TICK;
        public event Action Start;
        public event PropertyChangedEventHandler PropertyChanged;
        private double _gravity = 0.98;
        public double Gravity//1-10
        {
            get { return _gravity; }
            set { if (value < 0 || value > 10) _gravity = 0.98; _gravity = value; OnPropertyChanged(); }
        }
        private int _particlesCount = 100;
        public int ParticleCount//100-500
        {
            get { return _particlesCount; }
            set { if (value < 0 || value > 1000) _particlesCount = 100; _particlesCount = value; OnPropertyChanged(); }
        }
        private double _accelerate = 1.05;
        public double Accelerator//1-2 
        {
            get { return _accelerate; }
            set
            {
                _accelerate = value;
                OnPropertyChanged();
            }
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ControllerModel()
        {
            fireworks = new ObservableCollection<Fireworks>();
            TIME_TICK = DateTime.Now;
            //Fireworks f = new Fireworks(new Point(10,10),new Point(100,100));
            //fireworks.Add(f);
#if DEBUG
            LoadFromDataBase();
#endif
        }
        List<Particles> listParticles = new List<Particles>();
        public ObservableCollection<Fireworks> fireworks { get; set; }
        public void AddFireWork(Point p1, Point p2)
        {
            Fireworks f = new Fireworks(p1, p2);
            f.Accelerator = Accelerator;
            f.ParticleCount = ParticleCount;
            f.TimeTick = (int)(DateTime.Now - TIME_TICK).TotalMilliseconds;
            f.CreateParticles += CreateParticles;
            fireworks.Add(f);
            AddToDataBase(f);


        }
        public void LoadFromDataBase()
        {
            return;
            System.Data.DataSet ds = new System.Data.DataSet();
            string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=firework;Integrated Security=True;Pooling=False";
            string sqlStr = "select * from Fireworks;";
            try
            {

                using (var connection = new System.Data.SqlClient.SqlConnection(connectionStr))
                {
                    connection.Open();
                    using (var dataAdapter = new System.Data.SqlClient.SqlDataAdapter(sqlStr, connection))
                    {
                        dataAdapter.Fill(ds);
                    }
                    foreach (DataRow theRow in ds.Tables[0].Rows)
                    {

                        string[] ss = theRow["Position"].ToString().Split(',');
                        Fireworks f = new Fireworks(new Point(120, 300), new Point(double.Parse(ss[0]), double.Parse(ss[1])));
                        f.Accelerator = Accelerator;
                        f.ParticleCount = ParticleCount;
                        f.TimeTick = int.Parse(theRow["TimeTick"].ToString());
                        f.ParticleCount = int.Parse(theRow["Count"].ToString());
                        f.ID = int.Parse(theRow["num"].ToString());
                        Fireworks.CurrentID = f.ID;
                        f.CreateParticles += CreateParticles;
                        fireworks.Add(f);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("DataBase Not found"); ;
            }
        }
        private void AddToDataBase(Fireworks fire)
        {
            return;
            string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=firework;Integrated Security=True;Pooling=False";
            string sqlStr = @"insert into Fireworks(num,Position,Count,TimeTick) values(@Index,@Position,@Count,@TimeTick);";
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionStr))
            {
                connection.Open();
                using (var command = new System.Data.SqlClient.SqlCommand(sqlStr, connection))
                {
                    command.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
                        new System.Data.SqlClient.SqlParameter("@Index",fire.ID),
                        new System.Data.SqlClient.SqlParameter("@Position",fire.toPoint.ToString()),
                        new System.Data.SqlClient.SqlParameter("@Count",fire.ParticleCount),
                        new System.Data.SqlClient.SqlParameter("@TimeTick",fire.TimeTick),
                    });
                    int r = command.ExecuteNonQuery();
                    if (r < 1)
                    {
                        MessageBox.Show("Error");
                    }
                }
            }
        }
        private void CreateParticles(Point fromPos, int hue)
        {
            for (int i = 0; i < ParticleCount; i++)
            {
                Particles p = new Particles(fromPos, hue);
                p.gravity = Gravity;
                listParticles.Add(p);
            }
        }
        private MyCommandT<MouseButtonEventArgs> _mouseDownCommand;
        public MyCommandT<MouseButtonEventArgs> MouseDownCommand
        {
            get
            {
                if (_mouseDownCommand == null)
                    _mouseDownCommand = new MyCommandT<MouseButtonEventArgs>(
                        new Action<MouseButtonEventArgs>(e =>
                        {
                            var point = e.GetPosition(e.Device.Target);
                            AddFireWork(new System.Windows.Point((e.Source as System.Windows.Controls.Image).Width / 2, (e.Source as System.Windows.Controls.Image).Height), new System.Windows.Point(point.X, point.Y));
                            Start();
                        }),
                        new Func<object, bool>(o => true));
                return _mouseDownCommand;
            }
        }
        public bool CheckFireAndParticles()
        {
            bool particles = true;
            for (int i = 0; i < this.listParticles.Count; i++)
            {
                if (!this.listParticles[i].IsDisposed) { particles = false; break; }
                else { this.listParticles.RemoveAt(i); }
            }
            bool fire = true;
            for (int i = 0; i < this.fireworks.Count; i++)
            {
                if (!this.fireworks[i].IsDisposed) { fire = false; break; }
            }
            return fire && particles;
        }
        public void Update()
        {
            foreach (var i in fireworks)
            {
                i.Update();
            }
            foreach (var i in listParticles)
            {
                i.Update();
            }
        }
        public void Draw(Graphics g)
        {
            foreach (var i in fireworks)
            {
                i.Draw(g);
            }
            foreach (var i in listParticles)
            {
                i.Draw(g);
            }
        }
    }
}