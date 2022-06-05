﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ActivityLog.WindowPage
{
    /// <summary>
    /// AddRecord.xaml 的交互逻辑
    /// </summary>
    public partial class AddRecord : Window
    {
        public AddRecord()
        {
            InitializeComponent();
        }
        private void CloseWindow(object sender, MouseButtonEventArgs e)
        {
            Storyboard storyboard = (Storyboard)Resources["closeDW2"];
            if (!closeStoryBoardCompleted)
            {
                storyboard.Begin();
            }
        }
        private bool closeStoryBoardCompleted = false;
        private void closeStoryBoard_Completed(object sender, EventArgs e)
        {
            closeStoryBoardCompleted = true;
            this.Close();
        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            //int a =AcState.SelectedIndex;
            //int b =(int) AcState.Tag;
            this.DialogResult = true;
        }
    }
}