using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace Painter.Models.ControlUtils
{
    public enum CMDS { NONE,AUTO_FOCUS,DISABLE_FOCUS}

    class InputCmds
    {
        CMDS curCMD = CMDS.NONE;
        static List<string> cmds = new List<string>() { "NONE","FOCUS","DEFOCUS" };
        string strWords = "";
        bool isTyping = false;
        public event Action<CMDS> CMDEvent;
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            strWords += (char)e.KeyValue;
            isTyping = false;
            for (int i = 0; i < cmds.Count; i++)
            {
                if (strWords==cmds[i])
                {
                    curCMD=(CMDS)i;
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
