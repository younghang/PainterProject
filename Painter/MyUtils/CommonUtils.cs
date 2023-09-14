using Painter.Controller;
using Painter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Utils
{
    class CommonUtils
    {

        /// <summary>
        /// Clones the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List">The list.</param>
        /// <returns>List{``0}.</returns>
        public static List<T> Clone<T>(object List)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }
        public static T CloneObject<T>(object obj)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, obj);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
        /// <summary>
        /// 返回类似00:00.000s的时间差
        /// </summary>
        /// <param name="EndTime"></param>
        /// <param name="StartTime"></param>
        /// <returns></returns>
        public static string GetTimeSpan(DateTime EndTime,DateTime StartTime)
        {
            double seconds = (EndTime - StartTime).TotalMilliseconds / 1000*Settings.SPEED_RATE;
            int minue = (int)(seconds / 60);
            string strMin = "" + minue;

            if (minue < 10)
            {
                strMin = "0" + minue;
            }
            int sec = (int)(seconds - minue * 60);
            string strSecond = "" + sec;
            if (sec < 10)
            {
                strSecond = "0" + sec;
            }
            double milliseconds = (seconds - minue * 60 - sec) * 1000;
            string strMilli = ((int)milliseconds).ToString("000");//ToString("D3")
            string totalTime = strMin + ":" + strSecond + "." + strMilli;
            return totalTime;
        }
          public static string GetTimeSpan()
        {
          	int milliSecs=ProgramController.Instance.TICK_COUNT * Settings.TIME_SPAN;
        	milliSecs=(int)(1f*milliSecs*Settings.SPEED_RATE);
            int seconds = (int)Math.Floor(1f*milliSecs/1000 );//直接整除也行
            int minue = (int)Math.Floor(1.0f*seconds / 60);
            string strMin = "" + minue;

            if (minue < 10)
            {
                strMin = "0" + minue;
            }
            int sec = (int)(seconds - minue * 60);
            string strSecond = "" + sec;
            if (sec < 10)
            {
                strSecond = "0" + sec;
            }
            int milliseconds = milliSecs-sec*1000 - minue * 60*1000 ;
            string strMilli = ((int)milliseconds).ToString("D3");//ToString("000")
            string totalTime = strMin + ":" + strSecond + "." + strMilli;
            return totalTime;
        }
    }
}
