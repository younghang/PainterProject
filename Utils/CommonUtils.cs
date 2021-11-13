using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class CommonUtils
    {
        //This method converts the values to RGB
        public static RgbColor HslToRgb(int Hue, int Saturation, int Lightness)
        {
            double num4 = 0.0;
            double num5 = 0.0;
            double num6 = 0.0;
            double num = ((double)Hue) % 360.0;
            double num2 = ((double)Saturation) / 100.0;
            double num3 = ((double)Lightness) / 100.0;
            if (num2 == 0.0)
            {
                num4 = num3;
                num5 = num3;
                num6 = num3;
            }
            else
            {
                double d = num / 60.0;
                int num11 = (int)Math.Floor(d);
                double num10 = d - num11;
                double num7 = num3 * (1.0 - num2);
                double num8 = num3 * (1.0 - (num2 * num10));
                double num9 = num3 * (1.0 - (num2 * (1.0 - num10)));
                switch (num11)
                {
                    case 0:
                        num4 = num3;
                        num5 = num9;
                        num6 = num7;
                        break;
                    case 1:
                        num4 = num8;
                        num5 = num3;
                        num6 = num7;
                        break;
                    case 2:
                        num4 = num7;
                        num5 = num3;
                        num6 = num9;
                        break;
                    case 3:
                        num4 = num7;
                        num5 = num8;
                        num6 = num3;
                        break;
                    case 4:
                        num4 = num9;
                        num5 = num7;
                        num6 = num3;
                        break;
                    case 5:
                        num4 = num3;
                        num5 = num7;
                        num6 = num8;
                        break;
                }
            }
            return new RgbColor((int)(num4 * 255.0), (int)(num5 * 255.0), (int)(num6 * 255.0));
        }
        //The structure that will hold the RGB Values
        public struct RgbColor
        {
            public RgbColor(int r, int g, int b)
            {
                red = r;
                green = g;
                blue = b;
            }
            public int red;
            public int green;
            public int blue;
        }
        public static void PointRotateAroundOrigin(float degree, ref float deltax, ref float deltay,float originx=0,float originy=0)
        {
            double rad = degree / 180 * Math.PI;
            float tempX,tempY;
            tempX = (float)((deltax) * Math.Cos(rad) - (deltay) * Math.Sin(rad));
            tempY = (float)((deltax) * Math.Sin(rad) + (deltay) * Math.Cos(rad));
            deltax = tempX+originx;
            deltay = tempY+originy;
        }
        public static Color ReverseColor(Color color)
        {
            return Color.FromArgb(255-color.R, 255 - color.G, 255 - color.B);
        }
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
        public static string GetTimeSpan(DateTime EndTime, DateTime StartTime,double rate=1)
        {
            double seconds = (EndTime - StartTime).TotalMilliseconds / 1000 * rate;
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
 
    }

}
