using System;
using System.Collections.Generic;
using System.Linq;
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
    }

}
