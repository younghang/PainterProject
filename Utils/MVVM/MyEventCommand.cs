using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Utils.MVVM
{
    public class MyEventCommand : TriggerAction<DependencyObject>
    {
        /// <summary>
        /// 事件要绑定的命令
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// 注册Command依赖属性。这将启用动画、样式设置、绑定等
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(MyEventCommand), new PropertyMetadata(null));

        /// <summary>
        /// 绑定命令的参数，保持为空就是事件的参数
        /// </summary>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// 注册CommandParateter依赖属性。这将启用动画、样式设置、绑定等
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(MyEventCommand), new PropertyMetadata(null));

        //执行事件
        protected override void Invoke(object parameter)
        {
            //if (CommandParameter != null)
            //    parameter = CommandParameter;

            var cmd = Command;
            if (cmd != null)
            {
                if (CommandParameter == null)
                    cmd.Execute(parameter);
                else
                    cmd.Execute(new object[] { parameter, CommandParameter });
            }
        }
    }
}
