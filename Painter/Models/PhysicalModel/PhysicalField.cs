using Painter.Models.Paint;
using System;
using System.Collections.Generic;
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
        public static float Gravity = -9.8f / 2;
        private void ForceAnalyse(SceneObject sceneObject)
        {
            switch (sceneObject.OBJECT_TYPE)
            {
                case SCENE_OBJECT_TYPE.ROLE:
                    Role character = sceneObject as Role;
                    if (character.Status==SCENE_OBJECT_STATUS.IN_AIR)
                    {
                        character.Force.Y = Gravity;
                        character.Force.X = -character.Speed.X * character.CurrenScene.AirFrictionRatio; 
                    }
                    else if(character.Status == SCENE_OBJECT_STATUS.IN_GROUND)
                    {
                        character.Force.Y = 0;
                        //if (character.Acc.Y<0&& character.Speed.Y<0)
                        //{
                        //    character.Speed.Y =-0.8f *character.Speed.Y;
                        //}
                        if (Math.Abs(character.Speed.Y) < 0.3)
                        {
                            character.Speed.Y = 0;
                        }
                        character.Force.X = -character.Speed.X * character.CurrenScene.GroundFrictionRatio;
                        if (Math.Abs(character.Force.X) < 0.1)
                        {
                            character.Force.X = 0;
                            character.Speed.X = 0;
                            if (character is Enemy)
                            {
                                character.Speed.X = ((float)new Random().NextDouble() * 2 - 1) * 10;
                            }
                        } 
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
            MotionAnalyse(sceneObject);
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
        private void TravasalObject(Action<SceneObject> action)
        {
            foreach (var item in this.scenes)
            {
                item.CheckState();
                foreach (var obj in item.GetSceneObject())
                { 
                    action(obj);
                }
            }
        }
    }
   
}
