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
        internal void AddObject(SceneObject sob,bool isFresh=true)
        {
            sceneObjects.Add(sob);
            sob.CurrenScene = this;
            if (isFresh)
            {
                freshLayerManager.AddRange(sob.GetElements()); 
            }else
            {
                fixedLayerManager.AddRange(sob.GetElements());
            }
        }
        private object obj = new object();
        public void CheckState()
        {
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                if (sceneObjects[i] is Role)
                {
                    lock (obj)
                    {
                        Role character = sceneObjects[i] as Role;
                        character.CalMaxMin();
                        character.Status = SCENE_OBJECT_STATUS.IN_AIR;
                        character.Interfer = SCENE_OBJECT_INTERFER.NONE;
                        float width = character.MaxX - character.MinX;
                        width = width / 2;
                        float flash = Math.Abs(PhysicalField.TICK_TIME * character.Speed.Y);
                        if (flash < 5)
                        {
                            flash = 5;
                        }
                        flash = 5;
                        for (int j = 0; j < sceneObjects.Count; j++)
                        {
                            switch (sceneObjects[j].OBJECT_TYPE)
                            {
                                case SCENE_OBJECT_TYPE.ROLE:
                                    if (sceneObjects[j].OBJECT_TYPE == SCENE_OBJECT_TYPE.ROLE)
                                    {
                                        if (sceneObjects[j] is Enemy)
                                        {
                                            Enemy enemy = sceneObjects[j] as Enemy;
                                            enemy.CalMaxMin();
                                            character.CheckCollision(enemy);

                                        }
                                    }
                                    break;
                                case SCENE_OBJECT_TYPE.GROUND:
                                    GroundObject ground = sceneObjects[j] as GroundObject;
                                    ground.Interfer = SCENE_OBJECT_INTERFER.NONE;
                                    ((ground.GetOutShape()).GetDrawMeta() as ShapeMeta).IsFill = false;
                                    ground.CalMaxMin();
                                    if (character.MinY - ground.MaxY > -flash && character.MinY - ground.MaxY < flash)
                                    {
                                        if (character.MaxX < ground.MaxX + width && character.MinX >= ground.MinX - width)
                                        {
                                            character.Status = SCENE_OBJECT_STATUS.IN_GROUND;
                                            character.Interfer = SCENE_OBJECT_INTERFER.INTERFER;
                                            (ground.GetOutShape().GetDrawMeta() as ShapeMeta).IsFill = true;
                                            if (character.Speed.Y < 0)
                                            {
                                                character.Speed.Y *= -ground.ReflectResistance;

                                            }
                                            break;
                                        }
                                    }
                                    break;
                                case SCENE_OBJECT_TYPE.PICTURE:
                                    break;
                                case SCENE_OBJECT_TYPE.OBSTACLE:
                                    break;
                                case SCENE_OBJECT_TYPE.WEAPON:
                                    break;
                                default:
                                    break;
                            }
                        }
                         
                    }
                }
                sceneObjects[i].SetStatus();
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
