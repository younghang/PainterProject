using System;
using System.Collections.Generic;
using CalculatorDll.Calculator.UIController; 
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace CalculatorDll.Calculator
{
    public partial class CalculatorForm : Form
    {
        public CalculatorForm()
        {
            InitializeComponent();
            this.Load += Form2_Load;
            this.richTextBox1.TextChanged += richTextBox1_TextChanged;
            this.richTextBox1.KeyDown += richTextBox1_KeyDown;
            this.richTextBox1.KeyUp += richTextBox1_KeyUp;
            this.FormClosing += CalculatorForm_FormClosing;

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //richTextBox1.SelectionStart = richTextBox1.Text.Length;
            //richTextBox1.ScrollToCaret(); //不要用这种东西
            richTextBox1.HideSelection = false;
        }
        UIController.UIController con;
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = "[Tab] for result";
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            UIController.UIController.ShowOutput += AppendText;
            con = new UIController.UIController();
            con.GetInput += GetLastLineOfRichTextBox;
            AppendText("Hello World! enter: help() for illustration" + System.Environment.NewLine, ">>");
            con.GetDataToInput();
        }
        private void AppendText(string info, string type = "")
        {
            int len = richTextBox1.Text.Length;
            this.richTextBox1.AppendText(info);
            richTextBox1.Select(len, info.Length);
            switch (type)
            {
                case ">>":
                    richTextBox1.SelectionFont = new Font("Consolas", 12, FontStyle.Bold);
                    this.richTextBox1.SelectionColor = Color.FromArgb(0, 0, 0);
                    break;
                case "result":
                    richTextBox1.SelectionFont = new Font("Consolas", 12, FontStyle.Bold);
                    this.richTextBox1.SelectionColor = Color.FromArgb(0, 176, 240);
                    break;
                case "instruction":
                    richTextBox1.SelectionFont = new Font("Consolas", 9, FontStyle.Italic);
                    this.richTextBox1.SelectionColor = Color.FromArgb(0, 176, 80);
                    break;
                case "error":
                    richTextBox1.SelectionFont = new Font("Consolas", 9, FontStyle.Bold);
                    this.richTextBox1.SelectionColor = Color.FromArgb(255, 0, 0);
                    break;
                default:
                    richTextBox1.SelectionFont = new Font("Consolas", 9);
                    this.richTextBox1.SelectionColor = Color.FromArgb(0, 0, 0);
                    break;
            }
            richTextBox1.HideSelection = false;
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
        }
        public bool IsRichBoxDisposed()
        {
            //这个是个问题，new form的时候，不会重新创建
            return this.richTextBox1.Disposing;
        }
        private string LastLine = "";
        private string GetLastLineOfRichTextBox()
        {
            return LastLine;
        }
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string[] lines = this.richTextBox1.Text.Split('\n');
                LastLine = lines[lines.Length - 1];
                LastLine = LastLine.Substring(2);
                e.Handled = true;
                this.richTextBox1.AppendText("\n");
                con.Run();
                con.GetDataToInput();
            }
        }
        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                AppendText(LastLine, ">>");
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Tab)
            {
                string[] lines = this.richTextBox1.Text.Split('\n');
                if (lines.Length < 2)
                    return;
                string result = lines[lines.Length - 2];
                try
                {
                    double.Parse(result);
                    AppendText(result, ">>");
                    e.Handled = true;
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
        private void CalculatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }
    }
}