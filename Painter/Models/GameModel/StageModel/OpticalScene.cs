using Newtonsoft.Json;
using Painter.Models.GameModel.SceneModel.OpticalModel;
using Painter.Models.Paint;
using Painter.Models.StageModel;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models.GameModel.StageModel
{
    class OpticalScene : StageScene
    {
        public OpticalScene()
        {
            MaxX = 300;
            MinX = -200;
            MinY = -200;
            MaxY = 200;
            width = 800;
            height = 600;
        }
        MainCharacter character = new MainCharacter();
        public void ResetRays()
        {
            var items = this.scene.GetSceneObject();
            for (int i = 0; i < items.Count; i++)
            { 
                if (items[i] is RayLight)
                {
                    (items[i] as RayLight).Reset();
                }
            }
        }
        private string SCENE_FILE_NAME = "./data/optical_scene.json";
        public bool LoadScene()
        { 
            if (File.Exists(SCENE_FILE_NAME) ==false)
            {
                return false;
            }
            try
            { 
                string text = File.ReadAllText(SCENE_FILE_NAME);
                Dictionary<string, string> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                string lensStr = keyValuePairs["lens"];
                string raysStr = keyValuePairs["rays"];
                List<RayLight> rays = new List<RayLight>();
                List<LensObject> lens = new List<LensObject>();

                rays = JsonConvert.DeserializeObject<List<RayLight>>(raysStr);
                foreach (var item in rays)
                {
                    this.scene.AddObject(item);
                }
                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                lens = JsonConvert.DeserializeObject<List<LensObject>>(lensStr , jsonSerializerSettings);
                foreach (var item in lens)
                {
                    item.InitElement();
                    this.scene.AddObject(item);
                } 
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public void SaveScene()
        {
            List<RayLight> rays = new List<RayLight>();
            List<LensObject> lens = new List<LensObject>();
            var objs = this.GetScene().GetSceneObject();
            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i].IsDisposed == false && objs[i] is RayLight)
                {
                    RayLight ray = objs[i] as RayLight;
                    ray.Selected = false;
                    rays.Add(ray); 
                }
                if (objs[i].IsDisposed == false && objs[i] is LensObject)
                {
                    LensObject len = objs[i] as LensObject;
                    len.Selected = false; 
                    lens.Add(len);
                }
            }

            string raysStr = JsonConvert.SerializeObject(rays);
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            string lensStr = JsonConvert.SerializeObject(lens, jsonSerializerSettings);

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs["rays"] = raysStr;
            keyValuePairs["lens"] = lensStr;
            string text = JsonConvert.SerializeObject(keyValuePairs);
            if (!Directory.Exists("./data"))
            {
                Directory.CreateDirectory("./data");
            }
            File.WriteAllText(SCENE_FILE_NAME, text);
        }
        public override Scene CreateScene()
        {
            scene.Clear();
            CanvasModel.EnableTrack = false;
            scene.Background = Color.White;
            PhysicalModel.PhysicalField.TICK_TIME = 10;
            if (LoadScene()==false)
            {
                RayLight redRay = new RayLight(new PointGeo(0, 0), 0, 660);
                RayLight redR1Ray = new RayLight(new PointGeo(0, 10), 0, 660);
                RayLight redR2Ray = new RayLight(new PointGeo(0, 10), 3, 660);
                RayLight redR3Ray = new RayLight(new PointGeo(0, 5), 0, 660);
                RayLight greenRay = RayLight.CreatRayByColor(new PointGeo(0, 0), 5, RayLight.RAY_COLOR.VIOLET);
                MirrorObject mirrorObject33 = new MirrorObject(new PointGeo(100, -30), new PointGeo(150, 30), "Mirror");
                LensObject mirrorObject = new LensObject(new PointGeo(100, -30), new PointGeo(150, 30));
                MirrorObject mirrorObject2 = new MirrorObject(new PointGeo(50, 100), new PointGeo(161, 115), "Mirror");
                MirrorObject mirrorObject3 = new MirrorObject(new PointGeo(200, -51), new PointGeo(270, -6), "Mirror");
                PositiveLens positiveLens = new PositiveLens();
                NegtiveLens negtiveLens = new NegtiveLens();

                scene.AddObject(redRay);
                scene.AddObject(redR1Ray);
                scene.AddObject(redR2Ray);
                scene.AddObject(redR3Ray);
                scene.AddObject(greenRay);


                //scene.AddObject(mirrorObject);
                //scene.AddObject(mirrorObject2);
                //scene.AddObject(mirrorObject33);
                scene.AddObject(positiveLens);
                scene.AddObject(negtiveLens);
            } 
            return scene;
        }

        public override MainCharacter GetMainCharacter()
        {
            return character;
        }
    }
}
