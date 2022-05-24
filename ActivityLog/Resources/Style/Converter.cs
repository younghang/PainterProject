using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    return "Edit Activity";
                }else
                {
                    return "Add New Activity";
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
}
