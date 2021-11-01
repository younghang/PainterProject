using Painter.Models.Paint;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models.CameraModel
{
    
    public class Camera
    {
        PointGeo FocusPoint;
        PointGeo ScaleVector;
        MainCharacter FocusObject;
        public bool EnableFucus = false;
        private CanvasModel canvas;
        public Camera(CanvasModel canvasModel)
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
            Shape shape = FocusObject.GetOutShape();
            PointGeo pointGeo = shape.GetShapeCenter().Clone();//这里一定要用Clone
            Point pointInScreen= canvas.ObjectToScreen(pointGeo);
            if (pointInScreen.X>=canvas.Width/3&&pointInScreen.X<=(int)(1.0f*canvas.Width/3*2))
            {

            }
            else
            {
                PointGeo offsetP;
                if (pointInScreen.X <canvas.Width / 3)
                {
                    offsetP = new PointGeo(canvas.Width / 3 - pointInScreen.X, 0);
                }else
                {
                    offsetP = new PointGeo((int)(1.0f * canvas.Width / 3 * 2) - pointInScreen.X, 0);
                }
                canvas.OffsetPoint += offsetP/10;
            }
        }
    }
}
