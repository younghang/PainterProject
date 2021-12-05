using Painter.Models.Paint;
using Painter.Models.StageModel;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.Models.GameModel.StageModel
{
    class SecondStageScene : StageScene
    {
        public override void Clear()
        {
            this.scene.Clear();
            CanvasModel.EnableTrack = false;
        }
        MainCharacter character = new MainCharacter();
        DrawableText scoreText = new DrawableText();
      
        public override Scene CreateScene()
        {
            CanvasModel.EnableTrack = true;
            scene.Background = Color.Black;
            Obstacle TextObject = new Obstacle();
            scoreText.pos = new PointGeo(1200, 500);
            scoreText.SetDrawMeta(new TextMeta("你好呀 Hello") { IsScaleble = true, ForeColor = Color.LimeGreen, TEXTFONT = new Font("Consolas Bold", 36f), stringFormat = new StringFormat() { Alignment = StringAlignment.Center } });
            RectangleGeo rect = new RectangleGeo(scoreText.pos, scoreText.pos - new PointGeo(100, 100));
            TextObject.Add(rect);
            TextObject.Add(scoreText);
            scene.AddObject(TextObject, false);
            return scene;
        }

        public override MainCharacter GetMainCharacter()
        {
            return character;
        }

       
        public override void OnKeyDown(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Space:
                    character.Move(new PointGeo());
                    break; 
            }
        }
         
    }
}
