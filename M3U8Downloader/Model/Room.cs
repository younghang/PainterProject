using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3U8Downloader.Model
{
    enum ROOM_STATE
    {
        OFF_LINE,LVING
    }
    public class Room
    {
        public string url { get; set; }
        public string title { get; set; }

    }
}
