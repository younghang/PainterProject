 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls; 
namespace TwoArm.Model
{

    public class PointModel : INotifyPropertyChanged
    {
        public PointModel(string pointName, int _x, int _y, double _speed)
        {
            this.X = _x;
            Y = _y;
            Speed = _speed;
            PointName = pointName;
        }
        public PointModel()
        {
        }
        private double _length;
        public double Length
        {
            get { return _length; }
        }
        private int x;
        private int y;
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                if (value < 0) x = 0;
                x = value;
                OnPropertyChanged();
                CalDistance();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Length"));
            }
        }
        public int Y { get { return y; } set { if (value < 0) y = 0; y = value; OnPropertyChanged(); CalDistance(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Length")); } }
        private double speed;
        private string _pointName;
        public string PointName
        {
            get { return _pointName; }
            set
            {
                if (value == "") _pointName = "Point"; _pointName = value; OnPropertyChanged();
            }
        }
        public double Speed
        {
            get { return speed; }
            set
            {
                if (value < 0) speed = 1;
                speed = value;
                OnPropertyChanged();
            }
        }
        public void CalDistance()
        {
            _length = Math.Sqrt(X * X * 1.0 + Y * Y * 1.0);
        }
        public double GetDegree()
        {
            return Math.Atan2(y, x);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}