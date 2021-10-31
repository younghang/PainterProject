using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace Painter.Models.CmdControl
{
    public enum CMDS { NONE,AUTO_FOCUS,DISABLE_FOCUS,INTERFER_ON,INTERFER_OFF,MOMENTA_ON,MOMENTA_OFF,TRACK_ON,TRACK_OFF,NEXT_SCENE,FOR_SCENE}

    class InputCmds
    {
        CMDS curCMD = CMDS.NONE;
        static List<string> cmds = new List<string>() { "NONE","FOCUS","DEFOCUS","INTERFER","DEINTERFER","MOMENTA","DEMOMENTA","TRACK","DETRACK","NEXT","FOR"};
        string strWords = "";
        bool isTyping = false;
        public event Action<CMDS> CMDEvent;
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
            {
                strWords = "";
                return;
            }
            strWords += (char)e.KeyValue;
            isTyping = false;
            for (int i = 0; i < cmds.Count; i++)
            {
                if (strWords==cmds[i])
                {
                    strWords = "";
                    curCMD =(CMDS)i;
                    CMDEvent?.Invoke(curCMD);
                    return;
                }
                if (cmds[i].StartsWith(strWords))
                {
                    isTyping = true;
                    break;
                } 
            }
            if (isTyping==false)
            {
                strWords = "";
            } 
        } 
    }
}
