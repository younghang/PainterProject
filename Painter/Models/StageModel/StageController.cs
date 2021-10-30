using Painter.Models.Paint;
using Painter.Models.PhysicalModel;
using Painter.Models.StageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Painter.Models.StageModel
{
    class StageController//物理场的控制及屏幕的刷新，Render是由CanvasModel做的
    {
        CanvasModel render;
        public StageController(Scene scene,CanvasModel canvasModel)
        {
            this.curScene = scene;
            this.render = canvasModel;
        }
        PhysicalField physicalField = new PhysicalField();
        Timer timer = new Timer();
        public static double TIME_SPAN=15;
        Scene curScene;
        public event Action Invalidate;
        public void Start()
        {
            physicalField.AddScene(curScene);
            foreach (var item in curScene.GetSceneObject())
            {
                if (!item.IsHaveTrack)
                {
                    render.GetFreshLayerManager().AddRange(item.GetElements());
                }
                else
                {
                    render.GetHoldLayerManager().AddRange(item.GetElements());
                }
            }
            curScene.AddNewObjectEvent += CurScene_AddNewObjectEvent;
           
            physicalField.Rules += (sceneObject) => {

            };
            physicalField.ApplyField();
            if (Invalidate!=null)
            {
                Invalidate(); 
            }
            timer.Interval = TIME_SPAN;
            timer.Elapsed += Timer_Elapsed; ;
            timer.Start();
        }

        private void CurScene_AddNewObjectEvent(SceneObject obj)
        {
            if (!obj.IsHaveTrack)
            {
                render.GetFreshLayerManager().AddRange(obj.GetElements());
            }
            else
            {
                render.GetHoldLayerManager().AddRange(obj.GetElements());
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Invalidate != null)
            {
                Invalidate();
            }
        }
        public void Resume()
        {
            physicalField.Start();
            timer.Start();
        }
        public void Pause()
        { 
           physicalField.Pause();
           timer.Stop();
        }
        public void Stop()
        {
            timer.Stop();
            physicalField.Dispose();
        }
    }
}
