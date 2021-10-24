 
using System;
using System.Diagnostics;
namespace TwoArm.Model
{
    public class TwoArmModel
    {
        //which is ought to bind with WPF UI elements but here has not
        //thus first arm using Binding but not this model
        //public static readonly TwoArmModel Instance=new TwoArmModel();
        public static readonly int FirstArmLength = 200 - 50 - 25;
        public static readonly int SecondArmLength = 100 - 25 - 15;
        private static PointModel innerPosModel;
        public static double FirstArmAngle = 0;
        public static double SecondArmAngle = 0;
        //which indicates the gesture of this two arm mechanism at specific moment
        public static bool CalculateTwoArmAngle()
        {
            if (innerPosModel.Length > (FirstArmLength + SecondArmLength) ||
                innerPosModel.Length < (FirstArmLength - SecondArmLength) || innerPosModel == null)
                return false;
            double tempThetaValue = (innerPosModel.Length * innerPosModel.Length + FirstArmLength * FirstArmLength - SecondArmLength * SecondArmLength) / (2 * innerPosModel.Length * FirstArmLength);
            double tempThetaSecond = Math.Acos(tempThetaValue);
            FirstArmAngle = innerPosModel.GetDegree() - tempThetaSecond;
            tempThetaValue = (SecondArmLength * SecondArmLength + FirstArmLength * FirstArmLength - innerPosModel.Length * innerPosModel.Length) / (2 * SecondArmLength * FirstArmLength);
            double tempThetaPos = Math.Acos(tempThetaValue);
            SecondArmAngle = FirstArmAngle + Math.PI - tempThetaPos;
            return true;
        }
        public static void SetPointModel(PointModel pm)
        {
            innerPosModel = pm;
        }
    }
}