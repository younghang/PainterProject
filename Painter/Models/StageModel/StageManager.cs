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
        List<IStageScene> scenes = new List<IStageScene>();
        public int CurStageIndex = 0;
        internal IStageScene GetCurScene()
        {
            return scenes[CurStageIndex]; 
        }
        internal IStageScene GetStageAt(int index)
        {
            if (index<this.scenes.Count&&index>-1)
            {
                return scenes[index]; 
            }
            return null;
        }
        public void AddStage(IStageScene scene)
        {
            if (!this.scenes.Contains(scene))
            {
                this.scenes.Add(scene); 
            }
        }
        public void SwitchStage()
        {

        }
        public IStageScene GetNextStage()
        {
            return GetStageAt(++CurStageIndex);
        }
    }
}
