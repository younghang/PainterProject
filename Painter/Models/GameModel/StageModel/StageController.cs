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
        public StageController(CanvasModel canvasModel)
        { 
            this.render = canvasModel;
        }
        PhysicalField physicalField = new PhysicalField();
        public static bool EnableMomenta = false;
        Timer timer=null ;
        public static double TIME_SPAN=15;
        Scene curScene;
        public event Action Invalidate;
        public void Start(Scene scene)
        {
           
            this.curScene = scene;
            this.render.Background = scene.Background;
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
            if (timer==null)
            {
                timer = new Timer();
                timer.Interval = TIME_SPAN;
                timer.Elapsed += Timer_Elapsed; ;
               
            }
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
            if (timer!=null)
            {   
                physicalField.Pause();
                timer.Stop();
            } 
        }
        public void Stop()
        {
            //timer.Stop();
            //timer.Dispose();
            physicalField.Dispose();
            if (curScene!=null)
            {
                curScene.Clear(); 
            }
            render.Clear();
        }
        public void Dispose()
        {
            if (timer!=null)
            {
                timer.Stop();
                timer.Dispose(); 
            }
            physicalField.Dispose();
            curScene.Clear();
            render.Clear();
        }
    }
}
