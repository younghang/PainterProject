using Painter.DisplayManger;
using Painter.Models.PhysicalModel;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models.Paint
{
    public class Scene
    {
        public Scene()
        {
 
        }
        private Color background = Color.Transparent;
        public Color Background { get { return background; } set { this.background = value; } }
        public float AirFrictionRatio = 2f;
        public event Action<SceneObject,bool> AddNewObjectEvent;
        List<SceneObject> sceneObjects = new List<SceneObject>();
        internal void AddObject(SceneObject sob, bool isFresh = true )
        {
            if (!sceneObjects.Contains(sob))
            {
                sceneObjects.Add(sob);
            } 
            sob.CurrenScene = this;
            sob.IsHaveTrack = !isFresh;
            AddNewObjectEvent?.Invoke(sob,false);
        }
        internal void InsertObject(SceneObject sob, bool isFresh = true)
        {
            if (!sceneObjects.Contains(sob))
            {
                sceneObjects.Insert(0, sob);
            }  
            sob.CurrenScene = this;
            sob.IsHaveTrack = !isFresh;
            AddNewObjectEvent?.Invoke(sob,true);
        }
        private object obj = new object();
        public void CheckState()
        {
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                lock (obj)
                {
                    switch (sceneObjects[i].OBJECT_TYPE)
                    {
                        case SCENE_OBJECT_TYPE.ROLE:
                            if (sceneObjects[i] is Role)
                            { 
                                Role character = sceneObjects[i] as Role;
                                character.CalMaxMin();
                                character.Status = SCENE_OBJECT_STATUS.IN_AIR;
                                character.Interfer = SCENE_OBJECT_INTERFER.NONE; 
                                for (int j = 0; j < sceneObjects.Count; j++)
                                {
                                    character.CheckInterfer(sceneObjects[j]);
                                } 
                            }
                            break;
                        case SCENE_OBJECT_TYPE.GROUND: 
                            break;
                        case SCENE_OBJECT_TYPE.PICTURE:
                            break;
                        case SCENE_OBJECT_TYPE.OBSTACLE:
                            break;
                        case SCENE_OBJECT_TYPE.WEAPON:
                            if (sceneObjects[i] is Weapon)
                            {
                                Weapon weapon = sceneObjects[i] as Weapon;
                                for (int j = 0; j < sceneObjects.Count; j++)
                                {
                                    weapon.CheckInterfer(sceneObjects[j]);
                                    if (weapon.Interfer==SCENE_OBJECT_INTERFER.INTERFER)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                        case SCENE_OBJECT_TYPE.EFFECT:
                            break;
                        default:
                            break;
                    } 
                }
            }
            
           
        }
        internal List<SceneObject> GetSceneObject()
        {
            return sceneObjects;
        }

        internal void Clear()
        {
            sceneObjects.Clear(); 
        }
    }
}
