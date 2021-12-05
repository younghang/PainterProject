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
using Utils;

namespace Painter.Models.GameModel.StageModel
{
    class MinerStage : StageScene
    {
        
        public override Scene CreateScene()
        {
            return this.scene;
        }

        public override MainCharacter GetMainCharacter()
        {
            throw new NotImplementedException();
        }
         
        public override void OnKeyDown(Keys keyData)
        {
            throw new NotImplementedException();
        }
    }
}
