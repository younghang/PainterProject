using Painter.DisplayManger;
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
        public  float GroundFrictionRatio = 0.2f;
        public float AirFrictionRatio = 0.01f;

        List<SceneObject> sceneObjects = new List<SceneObject>();
        internal void AddObject(SceneObject sob)
        { 
            sceneObjects.Add(sob);
            sob.CurrenScene=this;
            freshLayerManager.AddRange(sob.GetElements());
        }
        public void CheckState()
        {
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                if (sceneObjects[i] is Character)
                {
                    Character character = sceneObjects[i] as Character;
                    character.CalMaxMin();
                    character.Status = SCENE_OBJECT_STATUS.IN_AIR;
                    character.Interfer = SCENE_OBJECT_INTERFER.NONE; 
                    float width = character.MaxX - character.MinX;
                    width = width / 2;
                    for (int j = 0; j < sceneObjects.Count; j++)
                    {
                        if (sceneObjects[j].OBJECT_TYPE==SCENE_OBJECT_TYPE.GROUND)
                        {
                            SceneObject ground = sceneObjects[j];
                            ground.Interfer = SCENE_OBJECT_INTERFER.NONE;
                            ((ground.GetElements()[0] as Shape).GetDrawMeta() as ShapeMeta).IsFill = false;
                            ground.CalMaxMin();
                            if (character.MinY-ground.MaxY>-5&& character.MinY - ground.MaxY <5)
                            {
                                if (character.MaxX<ground.MaxX+ width && character.MinX>=ground.MinX- width)
                                {
                                    character.Status = SCENE_OBJECT_STATUS.IN_GROUND;
                                    character.Interfer = SCENE_OBJECT_INTERFER.INTERFER;
                                    ((ground.GetElements()[0] as Shape).GetDrawMeta() as ShapeMeta).IsFill = true;
                                    break;
                                }
                            } 
                        }
                    }
                    if (character.Interfer == SCENE_OBJECT_INTERFER.NONE)
                    {
                        ((character.GetElements()[2] as Shape).GetDrawMeta() as ShapeMeta).IsFill = false;
                    }
                    else
                    {
                        ((character.GetElements()[2] as Shape).GetDrawMeta() as ShapeMeta).IsFill = true;
                    }
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
