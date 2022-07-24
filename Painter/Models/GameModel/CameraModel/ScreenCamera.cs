using Painter.Models.Paint;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models.CameraModel
{
     
    public class ScreenCamera
    {
        PointGeo FocusPoint;
        PointGeo ScaleVector;
        MainCharacter FocusObject;
        public bool EnableFucus = false;
        private CanvasModel canvas;
        public ScreenCamera(CanvasModel canvasModel)
        {
            canvas = canvasModel; 
        }
        public void SetFocusObject(MainCharacter mainCharacter)
        { 
            if (mainCharacter!=null)
            {
                FocusObject = mainCharacter;
            }
            else { return; }
            FocusObject.PositionChange += FocusObject_PositionChange;
            
        }

        private void FocusObject_PositionChange()
        {
            if (!EnableFucus)
            {
                return;
            }
            //此处应该用SceneObject的形心位置来判断，但形心的计算还有点问题
            //再不济也应该用SceneObject中心（CalMaxMin() (Max+Min)/2），这个就非常耗时了
            Shape shape = FocusObject.GetOutShape() as Shape;
            PointGeo pointGeo = shape.GetShapeCenter().Clone();//这里一定要用Clone
            Point pointInScreen= canvas.ObjectToScreen(pointGeo);
            if (pointInScreen.X>=canvas.Width/3&&pointInScreen.X<=(int)(1.0f*canvas.Width/3*2)
                && pointInScreen.Y >= canvas.Height / 3 && pointInScreen.Y <= (int)(1.0f * canvas.Height / 3 * 2))
            {

            }
            else
            {
                PointGeo offsetP;
                float x = 0;
                float y = 0;
                if (pointInScreen.X <canvas.Width / 3)
                {
                    x = (int)(1.0f*canvas.Width / 3) - pointInScreen.X; 
                }else if(pointInScreen.X > (int)(1.0f * canvas.Width / 3 * 2))
                {
                    x=(int)(1.0f * canvas.Width / 3 * 2) - pointInScreen.X ;
                }
                if (pointInScreen.Y < canvas.Height / 3)
                {
                    y = (int)(1.0f * canvas.Height / 3) - pointInScreen.Y;
                }
                else if(pointInScreen.Y > (int)(1.0f * canvas.Height / 3 * 2))
                {
                    y = (int)(1.0f * canvas.Height / 3 * 2) - pointInScreen.Y;
                }
                offsetP = new PointGeo(x, y);
                canvas.OffsetPoint += offsetP/10;
            }
        }
    }
}
