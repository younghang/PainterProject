using Painter.Models.Paint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models.StageModel
{
    //为多场景下准备的
    class StageManager
    {
        List<StageScene> scenes = new List<StageScene>();
        public int CurStageIndex = 0;
        internal StageScene GetCurScene()
        {
            return scenes[CurStageIndex]; 
        }
        internal StageScene GetStageAt(int index)
        {
            if (index<this.scenes.Count&&index>-1)
            {
                this.CurStageIndex = index;
                return scenes[index]; 
            }
            return null;
        }
        public void AddStage(StageScene scene)
        {
            if (!this.scenes.Contains(scene))
            {
                this.scenes.Add(scene); 
            }
        }
        public void SwitchStage()
        {

        }
        public StageScene GetNextStage()
        {
            return GetStageAt(++CurStageIndex);
        }

        internal StageScene GetPreViewStage()
        {
            return GetStageAt(--CurStageIndex);
        }
    }
}
