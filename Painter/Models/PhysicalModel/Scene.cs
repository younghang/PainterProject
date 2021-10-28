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
    class Scene
    {
        public Scene(GraphicsLayerManager fixedManager, GraphicsLayerManager freshManager)
        {
            this.fixedLayerManager = fixedManager;
            freshLayerManager = freshManager;
        }
        private GraphicsLayerManager freshLayerManager;//运动物体
        private GraphicsLayerManager fixedLayerManager;//画静止物体，标题之类的
        public float GroundFrictionRatio = 8f;
        public float AirFrictionRatio = 2f;

        List<SceneObject> sceneObjects = new List<SceneObject>();
        internal void AddObject(SceneObject sob, bool isFresh = true)
        {
            sceneObjects.Add(sob);
            sob.CurrenScene = this;
            if (isFresh)
            {
                freshLayerManager.AddRange(sob.GetElements());
            }
            else
            {
                fixedLayerManager.AddRange(sob.GetElements());
            }
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
            for (int i = sceneObjects.Count-1; i >=0; i--)
            {
                if (sceneObjects[i].IsDisposed)
                {
                    this.sceneObjects.RemoveAt(i);
                }
                else
                {
                    this.sceneObjects[i].SetStatus();
                }
            }
           
        }
        internal IEnumerable<SceneObject> GetSceneObject()
        {
            return sceneObjects;
        }

        internal void Clear()
        {
            sceneObjects.Clear();
            freshLayerManager.Clear();
            fixedLayerManager.Clear();
        }
    }
}
