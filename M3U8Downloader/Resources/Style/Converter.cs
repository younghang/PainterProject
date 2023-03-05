using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace M3U8Downloader.Resources.Style
{
    public class HeightToCornerConverter: IValueConverter
    {
        /// <summary>
        /// 后台转换往前端传值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return System.Convert.ToDouble(value);
            }
            return null;
        }
        /// <summary>
        /// 前端往后端传值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToWindowTitle : IValueConverter
    {
        /// <summary>
        /// 后台转换往前端传值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool isEdit = System.Convert.ToBoolean(value);
                if(isEdit)
                {
                    return "Edit "+ parameter.ToString();
                }else
                {
                    return "Add New "+ parameter.ToString();
                }
            }
            return null;
        }
        /// <summary>
        /// 前端往后端传值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ActivityStateToColor : IValueConverter
    {
 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                M3U8Downloader.Model.ROOM_STATE acState = (Model.ROOM_STATE)System.Convert.ToInt32(value);
                switch (acState)
                {
                    case Model.ROOM_STATE.OFF_LINE:
                        return new SolidColorBrush( Colors.Aqua);
                    case Model.ROOM_STATE.LVING:
                        return new SolidColorBrush(Colors.Lime); 
                }
            }
            return null;
        }
 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ActivityStateToString : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                M3U8Downloader.Model.ROOM_STATE acState = (Model.ROOM_STATE)System.Convert.ToInt32(value);
                return acState.ToString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ComboBoxItem item = (ComboBoxItem)value; 
            return (M3U8Downloader.Model.ROOM_STATE)Enum.Parse(typeof(M3U8Downloader.Model.ROOM_STATE), item.Name.ToString().Trim());
        }
    }
    public class DateTimeToDaysAgo : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
               DateTime date = (DateTime)(value);
                int days = (DateTime.Now - date).Days;
                if (days<90)
                {
                    return (DateTime.Now-date).Days.ToString()+" Days Ago";
                }else if(days<365)
                {
                    int month = days / 30;
                    return month+" Months Ago";
                }else
                {
                    int year = days / 365;
                    return year + " Years Ago";
                } 
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
