using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models
{
    public class NCFileInfo
    {
        private string _name = "";

		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}
		private int no = 0;
		public int No { get { return no; } set { no = value; } }
        private string filePath = "";

		public string FilePath {
			get {
				return filePath;
			}
			set {
				filePath = value;
			}
		}

        private Color color;

		public Color FillColor {
			get {
				return color;
			}
			set {
				color = value;
			}
		}

        private bool isShow = true;

		public bool IsShow {
			get {
				return isShow;
			}
			set {
				isShow = value;
			}
		}
        public NCFileInfo()
        {
            this._name = "Head1";
            this.filePath = "./nc_1.txt";
            this.color = Color.AliceBlue;
            this.isShow = false;
        }
        public NCFileInfo(string name, string filePath, Color color, bool isShow)
        {
            this._name = name;
            this.filePath = filePath;
            this.color = color;
            this.isShow = isShow;
        }

    }
}
