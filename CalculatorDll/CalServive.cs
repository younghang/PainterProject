using CalculatorDll.Calculator.UIController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculatorDll
{
    public class CalService
    {
        static UIController Controller = new UIController();
        public static string Input(string inputStr)
        {
            Result = "";
            Controller.Run(inputStr);
            return Result;
        }
        private static readonly CalService Instance = new CalService();
        private CalService()
        {
            UIController.ShowOutput += ShowMessage;
        }
        private static string Result = "";
        private static void ShowMessage(string info, string type = "")
        {
            switch (type)
            {
                case ">>":
                    break;
                case "result":
                    //"Express:27.7095299375  Value:27.7095299375\n"
                    string[] ss = info.Trim().Split(':');
                    Result = ss[ss.Length - 1];
                    break;
                case "instruction":
                    break;
                case "error":
                    break;
                default:
                    break;
            }

        }
    }
}
