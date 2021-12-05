using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace Painter.Models.CmdControl
{
 
    public enum GAME_CMDS { NONE,AUTO_FOCUS,DISABLE_FOCUS,INTERFER_ON,INTERFER_OFF,MOMENTA_ON,MOMENTA_OFF,TRACK_ON,TRACK_OFF,NEXT_SCENE,FOR_SCENE,EDIT,GCODE,OBJECT_3D}

    class GameInputCmds
    {
        GAME_CMDS curCMD = GAME_CMDS.NONE;
        static List<string> cmds = new List<string>() { "NONE","FOCUS","DEFOCUS","INTERFER","DEINTERFER","MOMENTA","DEMOMENTA","TRACK","DETRACK","NEXT","FOR","EDIT", "GCODE","THREE" };
        string strWords = "";
        bool isTyping = false;
        public event Action<GAME_CMDS> CMDEvent;
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
                    curCMD =(GAME_CMDS)i;
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

    public enum PAINT_CMDS { NONE, SHOWPANEL,HIDEPANEL,LINE,CIRCLE,ARC,RECT,SHOWGAME,CLEAR,POLY,SPLINE,ELLIPSE, TEXT,HELP}

    class PainterInputCmds
    {
        PAINT_CMDS curCMD = PAINT_CMDS.NONE;
        static List<string> cmds = new List<string>() { "NONE" , "SHOW", "HIDE", "LINE", "CIRCLE", "ARC", "RECT", "GAME", "CLEAR", "POLY", "SPLINE", "ELLIPSE", "TEXT", "HELP" };
        string strWords = "";
        bool isTyping = false;
        public event Action<PAINT_CMDS> CMDEvent;
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                strWords = "";
                return;
            }
            strWords += (char)e.KeyValue;
            isTyping = false;
            for (int i = 0; i < cmds.Count; i++)
            {
                if (strWords == cmds[i])
                {
                    strWords = "";
                    curCMD = (PAINT_CMDS)i;
                    CMDEvent?.Invoke(curCMD);
                    return;
                }
                if (cmds[i].StartsWith(strWords))
                {
                    isTyping = true;
                    break;
                }
            }
            if (isTyping == false)
            {
                strWords = "";
            }
        }
    }
}
