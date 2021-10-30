using Painter.Models.Paint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Painter.Models.PhysicalModel
{
    class PhysicalField
    {
        public PhysicalField()
        {
            Rules += InitialRules;
        }
        public static float Gravity = -9.8f/2000;
        private void ForceAnalyse(SceneObject sceneObject)
        {
            switch (sceneObject.OBJECT_TYPE)
            {
                case SCENE_OBJECT_TYPE.ROLE:
                    Role character = sceneObject as Role;
                    character.Force.Y = Gravity*character.Mass; 
                    character.Force.X = 0;
                    if (character.Status==SCENE_OBJECT_STATUS.IN_AIR)
                    {
                        character.Force.X = -character.Speed.X * character.CurrenScene.AirFrictionRatio ; 
                    }
                    else if(character.Status == SCENE_OBJECT_STATUS.IN_GROUND)
                    {
                        //character.Force.Y = 0;
                        //if (character.Acc.Y<0&& character.Speed.Y<0)
                        //{
                        //    character.Speed.Y =-0.8f *character.Speed.Y;
                        //}
                        //if (Math.Abs(character.Speed.Y) < 0.3)
                        //{
                        //    character.Speed.Y = 0;
                        //}
                        //character.Force.X = -character.Speed.X * character.CurrenScene.GroundFrictionRatio;
                        //if (Math.Abs(character.Force.X) < 0.1)
                        //{
                        //    character.Force.X = 0;
                        //    character.Speed.X = 0;
                        //    if (character is Enemy)
                        //    {
                        //        (character as Enemy).OnStopOnGround();
                        //    }
                        //} 
                    } 
                    break;
                case SCENE_OBJECT_TYPE.GROUND:
                    break;
                case SCENE_OBJECT_TYPE.PICTURE:
                    break;
                default:
                    break;
            }
        }
        private void MotionAnalyse(SceneObject sceneObject)
        {
            switch (sceneObject.OBJECT_TYPE)
            {
                case SCENE_OBJECT_TYPE.ROLE:
                    Role character = sceneObject as Role;  
                    character.Acc.Y = character.Force.Y / character.Mass  ;
                    character.Speed.Y += character.Acc.Y *  TICK_TIME;
                    character.Acc.X = character.Force.X / character.Mass ;
                    character.Speed.X += character.Acc.X * TICK_TIME;
                    character.Move(character.Speed * TICK_TIME / 2);
                    break;
                case SCENE_OBJECT_TYPE.GROUND:
                    break;
                case SCENE_OBJECT_TYPE.PICTURE:
                    break;
                default:
                    break;
            }
        }
        private void InitialRules(SceneObject sceneObject)
        {
            ForceAnalyse(sceneObject); 
        }
        public static float TICK_TIME = 10;
        List<Scene> scenes = new List<Scene>();
        public void AddScene(Scene scene)
        {
            if (this.scenes.Contains(scene)==false)
            {
                this.scenes.Add(scene);
            }
        }
        public void Clear()
        {
            this.scenes.Clear();
        }

        internal void Dispose()
        {
            if (timer!=null)
            {
                timer.Stop();
                timer.Dispose();
            } 
        }
        public void Pause()
        {
            timer.Stop();
        }
        public void Start()
        {
            timer.Start();
        }
        public Action<SceneObject> Rules;
        Timer timer;
        public void ApplyField()
        {
            timer = new Timer(TICK_TIME);
            timer.Elapsed += (e, f) =>
            {
                TravasalObject(Rules);
            };
            timer.Start();
        }
        Stopwatch timeWatcher = new Stopwatch();

        private void TravasalObject(Action<SceneObject> action)
        {
            timeWatcher.Reset();
            timeWatcher.Start(); 
            foreach (var item in this.scenes)
            {
                List<SceneObject> sos = item.GetSceneObject();
                for (int i = sos.Count()-1; i >0; i--)
                {
                    action(sos[i]);//单物体施加物理场：重力 空气阻力 运动学  
                } 
                item.CheckState();//检查场景内物体间作用（干涉） 并设置枚举状态

                //依据状态 设置场景内物体的显示效果
                for (int i = item.GetSceneObject().Count - 1; i >= 0; i--)
                {
                    if (item.GetSceneObject()[i].IsDisposed)
                    {
                        item.GetSceneObject().RemoveAt(i);
                    }
                    else
                    {
                        MotionAnalyse(item.GetSceneObject()[i]);//这里做运动学计算
                        item.GetSceneObject()[i].SetStatus();
                    }
                }
            }
            timeWatcher.Stop();
            Debug.Print(timeWatcher.ElapsedTicks + "");
        }
    }
   
}
