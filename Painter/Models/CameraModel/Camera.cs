using Painter.Models.Paint;
using System;
using System.Collections.Generic;
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
            //此处应该用SceneObject的形心位置来判断，但形心的计算还有点问题
            //再不济也应该用SceneObject中心（CalMaxMin() (Max+Min)/2），这个就非常耗时了
            Shape shape = FocusObject.GetOutShape();
            PointGeo pointGeo = shape.GetShapeCenter();
             
        }
    }
}
