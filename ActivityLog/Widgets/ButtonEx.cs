﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ActivityLog.Widgets
{
    // <local:ButtonEx Content = "信息" Width="75" Height="25" Margin="10" ButtonType="Normal"/>
    //<local:ButtonEx Icon = "/Images/button1.png" Margin="10" ButtonType="Icon"/>
    //<local:ButtonEx Content = "文字按钮" Margin="10" ButtonType="Text"/>
    //<local:ButtonEx Content = "图文按钮" Icon="/Images/adshut.png" Margin="10" ButtonType="IconText"/>
    public class ButtonEx : Button
    {
        static ButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonEx), new FrameworkPropertyMetadata(typeof(ButtonEx)));
        }


        public ButtonType ButtonType
        {
            get { return (ButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        public static readonly DependencyProperty ButtonTypeProperty =
         DependencyProperty.Register("ButtonType", typeof(ButtonType), typeof(ButtonEx), new PropertyMetadata(ButtonType.Normal));


        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
         DependencyProperty.Register("Icon", typeof(ImageSource), typeof(ButtonEx), new PropertyMetadata(null));

        public MahApps.Metro.IconPacks.PackIconMaterialKind PathIcon
        {
            get { return (MahApps.Metro.IconPacks.PackIconMaterialKind)GetValue(PathIconProperty); }
            set { SetValue(PathIconProperty, value); }
        }

        public static readonly DependencyProperty PathIconProperty =
         DependencyProperty.Register("PathIcon", typeof(MahApps.Metro.IconPacks.PackIconMaterialKind), typeof(ButtonEx), new PropertyMetadata(null));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
         DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ButtonEx), new PropertyMetadata(new CornerRadius(0)));


        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverForegroundProperty =
         DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());


        public Brush MousePressedForeground
        {
            get { return (Brush)GetValue(MousePressedForegroundProperty); }
            set { SetValue(MousePressedForegroundProperty, value); }
        }

        public static readonly DependencyProperty MousePressedForegroundProperty =
         DependencyProperty.Register("MousePressedForeground", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());


        public Brush MouseOverBorderbrush
        {
            get { return (Brush)GetValue(MouseOverBorderbrushProperty); }
            set { SetValue(MouseOverBorderbrushProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBorderbrushProperty =
         DependencyProperty.Register("MouseOverBorderbrush", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());


        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBackgroundProperty =
         DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());


        public Brush MousePressedBackground
        {
            get { return (Brush)GetValue(MousePressedBackgroundProperty); }
            set { SetValue(MousePressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MousePressedBackgroundProperty =
         DependencyProperty.Register("MousePressedBackground", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());

        public Brush CheckedBackground
        {
            get { return (Brush)GetValue(CheckedBackgroundProperty); }
            set { SetValue(CheckedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty CheckedBackgroundProperty =
         DependencyProperty.Register("CheckedBackground", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());


        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
         DependencyProperty.Register("IsChecked", typeof(bool), typeof(ButtonEx), new PropertyMetadata());

    }

    public enum ButtonType
    {
        Normal,
        Icon,
        Text,
        IconText
    }
}
