
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Controller
{ 
    class LaserException:Exception
    {
        public LaserException()
        {
            
        }
        public LaserException(string type,string msg):base(msg)
        {
            this.MSG_TYPE = type;
        }
        public override string ToString()
        {
            return "["+MSG_TYPE+"]:"+Message;
        }
        public string MSG_TYPE { get; set; }
    }
}
