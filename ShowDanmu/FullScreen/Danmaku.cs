﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ShowDanmu.FullScreen
{
    /// <summary>
    /// Interactive logic of Danmaku Canvas
    /// </summary>
    public class OutlinedDanmaku : OutlinedTextBlock
    {
        public OutlinedDanmaku(string text)
        {
            FontSize = 28;
            FontWeight = FontWeights.Bold;
            Text = text;
            Fill = Brushes.White;
            Stroke = Brushes.Green;

            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Arrange(new Rect(0, 0, DesiredSize.Width, DesiredSize.Height));
        }
    }

    public class ShadowDanmaku : TextBlock
    {
        //TODO: Optimizing the performance of danmaku animation with shadow.
        public ShadowDanmaku(string text)
        {
            FontSize = 20;
            Text = text;

            Foreground = Brushes.White;

            Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                BlurRadius = 2,
                ShadowDepth = 1,
                Opacity = 0.8,
                RenderingBias = RenderingBias.Performance
            };

            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Arrange(new Rect(0, 0, DesiredSize.Width, DesiredSize.Height));
        }
    }


    public class DanmakuManager
    {
        private Grid container;

        private int lineHeight = 40;
        private int paddingTop = 8;

        private Boolean[] isOccupy;
        private Boolean enableShadowEffect;
        private int lines;

        public int usableLine()
        {
            for (int line = 0; line < lines; line += 1)
            {
                if (!isOccupy[line])
                {
                    isOccupy[line] = true;
                    return line;
                }
            }
            return -1;
        }

        public void clearLine()
        {
            for (int line = 0; line < lines; line += 1)
            {
                isOccupy[line] = false;
            }
        }

        public int lineLocationY(int line)
        {
            return (line * lineHeight) + paddingTop;
        }

        public DanmakuManager(Grid grid, bool enableShadow)
        {
            container = grid;

            lines = (int)(container.RenderSize.Height / lineHeight) - 1;
            if (lines == -1)
            {
                lines = 0;
            }
            isOccupy = new Boolean[lines];

            enableShadowEffect = enableShadow;
        }

        public void Shoot(string text)
        {
            var line = usableLine();

            if (line == -1)
            {
                clearLine();
                line = usableLine();
            }

            FrameworkElement danmaku = new OutlinedDanmaku(text);

            // Danmaku initilization and display
            if (enableShadowEffect)
            {
                danmaku = new ShadowDanmaku(text);
            }

            danmaku.Margin = new Thickness(0, lineLocationY(line), 0, 0);


            container.Children.Add(danmaku);

            // Initilizing animation
            var anim = new DoubleAnimation();
            anim.From = this.container.RenderSize.Width;
            anim.To = -danmaku.DesiredSize.Width - 1600;
            anim.SpeedRatio = danmaku.DesiredSize.Width > 80 ?
                (.04 * (danmaku.DesiredSize.Width / 1500 + 1)) :
                (.08 * ((100 - danmaku.DesiredSize.Width) / 100 + 1));
            TranslateTransform trans = new TranslateTransform();
            danmaku.RenderTransform = trans;

            // Handling the end of danmaku
            anim.Completed += new EventHandler(delegate (Object o, EventArgs a) {
                container.Children.Remove(danmaku);
            });

            // Managing the danmaku lines
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(300);
            timer.Tick += new EventHandler(delegate (Object o, EventArgs a) {
                Point relativePoint = danmaku.TransformToAncestor(container)
                          .Transform(new Point(0, 0));
                if (relativePoint.X < container.ActualWidth - danmaku.DesiredSize.Width - 50)
                {
                    timer.Stop();
                    isOccupy[line] = false;
                }
            });
            timer.Start();

            // Play animation
            trans.BeginAnimation(TranslateTransform.XProperty, anim);
        }
    }
}
