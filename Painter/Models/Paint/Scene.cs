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
       

        List<SceneObject> sceneObjects = new List<SceneObject>();
        internal void AddObject(SceneObject sob)
        { 
            sceneObjects.Add(sob);
            freshLayerManager.AddRange(sob.GetElements());
        }

        internal void Clear()
        {
            sceneObjects.Clear();
            freshLayerManager.Clear();
            fixedLayerManager.Clear();
        }
    }
}
