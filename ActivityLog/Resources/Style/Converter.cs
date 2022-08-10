using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ActivityLog.Resources.Style
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
                ActivityLog.Model.ACTIVITY_STATE acState = (Model.ACTIVITY_STATE)System.Convert.ToInt32(value);
                switch (acState)
                {
                    case Model.ACTIVITY_STATE.TBD:
                        return new SolidColorBrush( Colors.Aqua);
                    case Model.ACTIVITY_STATE.ON_GOING:
                        return new SolidColorBrush(Colors.Lime);
                    case Model.ACTIVITY_STATE.FINISH:
                        return new SolidColorBrush(Colors.Purple);
                    case Model.ACTIVITY_STATE.ABORT:
                        return new SolidColorBrush(Colors.Red);
                    case Model.ACTIVITY_STATE.PAUSE:
                        return new SolidColorBrush(Colors.Orange);
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
                ActivityLog.Model.ACTIVITY_STATE acState = (Model.ACTIVITY_STATE)System.Convert.ToInt32(value);
                return acState.ToString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ComboBoxItem item = (ComboBoxItem)value; 
            return (ActivityLog.Model.ACTIVITY_STATE)Enum.Parse(typeof(ActivityLog.Model.ACTIVITY_STATE), item.Name.ToString().Trim());
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
