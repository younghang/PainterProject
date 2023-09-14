using CalculatorDll;
using Painter.Controller;
using Painter.MyUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models
{
    /// <summary>
    /// 提取一个零件的完整G代码，转换为多个GeoProcess对象
    /// nc.txt文件
    /// </summary>
    class GCodeParser
    {
        private GCodeParser()
        {
            InitGCodeParser();
        }
        public static GCodeParser Instance = new GCodeParser();
        //忽略的行
        public List<string> IgnoreLines = new List<string>() { "%", "**" };
        //有效行
        public List<string> GeoSplitter = new List<string>() { "==" };
        public List<string> ExecuteLines = new List<string>() { "G90", "G91", "G00", "G01", "G02", "G03" };
        public List<string> GeoCmds = new List<string>() { "CUT_CONT_START", "CUT_CONT_END", "LAST_LOOP_TACK_START", "LAST_LOOP_TACK_END" };
        public List<string> GeoParams = new List<string>() { "JUDGE", "ASPEED", "CUTSPEED1", "RAPIDSPEED", "LOOPGAP", "HEAD1LIMIT" };
        private List<List<string>> strProcessCmdLines = new List<List<string>>();
        private bool isRelative = false;
        private PointGeo curPos = new PointGeo(0, 0);
        private GGodeInterperter gCodeInterpreter;
        private void InitGCodeParser()
        {
            if (parseVariable == null)
            {
                parseVariable = (line) =>
                {
                    if (line.Contains(","))
                    {
                        string[] param = line.Split(',');
                        foreach (var item in param)
                        {
                            if (item.Contains("="))
                            {
                                CalService.Input(item);
                            }
                        }
                    }
                    else if (line.Contains("="))
                    {
                        CalService.Input(line.Trim());
                    }
                };
            }
            if (actionAddMoveProcessor == null)
            {
                actionAddMoveProcessor = (rapidLine, isLoop) =>
                {
                    MoveGeoProcessor moveGeoProcessor = new MoveGeoProcessor();
                    Geo geo = new Geo();
                    geo.AddShape(rapidLine);
                    moveGeoProcessor.CutGeo = geo;
                    moveGeoProcessor.StartPoint = rapidLine.FirstPoint;
                    moveGeoProcessor.EndPoint = rapidLine.SecondPoint;
                    moveGeoProcessor.cutSpeed = double.Parse(CalService.Input("RAPIDSPEED"));
                    moveGeoProcessor.cutDistance = rapidLine.GetPerimeter();
                    moveGeoProcessor.cutDistanceInXDirection = rapidLine.FirstPoint.X - rapidLine.SecondPoint.X;
                    if (isLoop)
                    {
                        this.gCodeInterpreter.LoopProcessor.Add(moveGeoProcessor);
                    }
                    else
                    {
                        this.gCodeInterpreter.AddGeoProcessor(moveGeoProcessor);
                    }
                };
            }
            parseG00Code = (line) =>
            {

                if (line.Contains("G00"))
                {
                    line = line.Split(';')[0];
                    //(FIRST)G00X1200.780Y1098.000
                    int index = line.IndexOf("G00");
                    int xindex = line.IndexOf("X");
                    int yindex = line.IndexOf("Y");
                    string strX = line.Substring(xindex + 1, yindex - xindex - 1);
                    string strY = line.Substring(yindex + 1, line.Length - yindex - 1);
                    float x = float.Parse(strX);
                    float y = float.Parse(strY);
                    PointGeo point = new PointGeo(x, y);
                    LineGeo rapidLine = new LineGeo();
                    rapidLine.FirstPoint = curPos;
                    if (isRelative)
                    {
                        rapidLine.SecondPoint = point + curPos;
                    }
                    else
                    {
                        rapidLine.SecondPoint = point;
                    }
                    curPos = rapidLine.SecondPoint;
                    if (rapidLine.SecondPoint.X < rapidLine.FirstPoint.X && rapidLine.GetPerimeter() > double.Parse(CalService.Input("HEAD1LIMIT")))
                    {
                        int breakCount = 3;
                        for (int i = 0; i < breakCount; i++)
                        {
                            LineGeo lineGeo = new LineGeo();
                            lineGeo.FirstPoint = (rapidLine.SecondPoint - rapidLine.FirstPoint) * (1.0f * (i + 0) / breakCount) + rapidLine.FirstPoint;
                            lineGeo.SecondPoint = (rapidLine.SecondPoint - rapidLine.FirstPoint) * (1.0f * (i + 1) / breakCount) + rapidLine.FirstPoint;
                            actionAddMoveProcessor(lineGeo, false);
                        }
                    }
                    else
                    {
                        actionAddMoveProcessor(rapidLine, false);
                    }
                }

            };
            parseG01Code = (line) =>
            {
                LineGeo shape = null;
                if (line.Contains("G01"))
                {
                    //G01X0.000Y-323.268
                    int index = line.IndexOf("G01");
                    int xindex = line.IndexOf("X");
                    int yindex = line.IndexOf("Y");
                    string strX = line.Substring(xindex + 1, yindex - xindex - 1);
                    string strY = line.Substring(yindex + 1, line.Length - yindex - 1);
                    float x = float.Parse(strX);
                    float y = float.Parse(strY);
                    PointGeo point = new PointGeo(x, y);
                    shape = new LineGeo();
                    shape.FirstPoint = curPos;
                    if (isRelative)
                    {
                        shape.SecondPoint = point + curPos;
                    }
                    else
                    {
                        shape.SecondPoint = point;
                    }
                    shape.FirstPoint = shape.FirstPoint;
                    shape.SecondPoint = shape.SecondPoint;
                    curPos = shape.SecondPoint;
                }
                return shape;
            };
            parseG02Code = (line) =>
            {
                ArcGeo shape = null;
                if (line.Contains("G02"))//顺时针
                {
                    //G02X-399.732Y0.000I-199.866J7.323
                    int index = line.IndexOf("G02");
                    int xindex = line.IndexOf("X");
                    int yindex = line.IndexOf("Y");
                    int iIndex = line.IndexOf("I");
                    int jIndex = line.IndexOf("J");
                    string strX = line.Substring(xindex + 1, yindex - xindex - 1);
                    string strY = line.Substring(yindex + 1, iIndex - yindex - 1);
                    string strI = line.Substring(iIndex + 1, jIndex - iIndex - 1);
                    string strJ = line.Substring(jIndex + 1, line.Length - jIndex - 1);

                    float x = float.Parse(strX);
                    float y = float.Parse(strY);
                    float offsetI = float.Parse(strI);
                    float offsetJ = float.Parse(strJ);

                    PointGeo endPoint = new PointGeo(x, y);
                    PointGeo centerPoint = new PointGeo(offsetI, offsetJ);

                    shape = new ArcGeo();
                    shape.IsClockwise = true;
                    shape.FirstPoint = curPos;

                    if (isRelative)
                    {
                        shape.SecondPoint = endPoint + curPos;
                    }
                    else
                    {
                        shape.SecondPoint = endPoint;
                    }
                    shape.Radius = (float)Math.Sqrt(offsetI * offsetI + offsetJ * offsetJ);
                    centerPoint += curPos;
                    shape.CenterX = centerPoint.X;
                    shape.CenterY = centerPoint.Y;

                    //y,x 返回角度 θ，以弧度为单位，满足 - π≤θ≤π，且 tan(θ) = y / x
                    float startAngle = (float)(Math.Atan2(shape.SecondPoint.Y - shape.CenterY, shape.SecondPoint.X - shape.CenterX) / Math.PI * 180);
                    float endAngle = (float)(Math.Atan2(shape.FirstPoint.Y - shape.CenterY, shape.FirstPoint.X - shape.CenterX) / Math.PI * 180);
                    shape.StartAngle = startAngle;
                    shape.EndAngle = endAngle;
                    shape.IsFinishedInitial = true;
                    curPos = shape.SecondPoint;
                }
                return shape;
            };
            parseG03Code = (line) =>
            {
                ArcGeo shape = null;
                if (line.Contains("G03"))//逆时针
                {
                    //G02X-399.732Y0.000I-199.866J7.323
                    int index = line.IndexOf("G03");
                    int xindex = line.IndexOf("X");
                    int yindex = line.IndexOf("Y");
                    int iIndex = line.IndexOf("I");
                    int jIndex = line.IndexOf("J");
                    string strX = line.Substring(xindex + 1, yindex - xindex - 1);
                    string strY = line.Substring(yindex + 1, iIndex - yindex - 1);
                    string strI = line.Substring(iIndex + 1, jIndex - iIndex - 1);
                    string strJ = line.Substring(jIndex + 1, line.Length - jIndex - 1);

                    float x = float.Parse(strX);
                    float y = float.Parse(strY);
                    float offsetI = float.Parse(strI);
                    float offsetJ = float.Parse(strJ);

                    PointGeo endPoint = new PointGeo(x, y);
                    PointGeo centerPoint = new PointGeo(offsetI, offsetJ);

                    shape = new ArcGeo();
                    shape.IsClockwise = false;
                    shape.FirstPoint = curPos;

                    if (isRelative)
                    {
                        shape.SecondPoint = endPoint + curPos;
                    }
                    else
                    {
                        shape.SecondPoint = endPoint;
                    }
                    shape.Radius = (float)Math.Sqrt(offsetI * offsetI + offsetJ * offsetJ);
                    centerPoint += curPos;
                    shape.CenterX = centerPoint.X;
                    shape.CenterY = centerPoint.Y;
                    //返回角度 θ，以弧度为单位，满足 - π≤θ≤π，且 tan(θ) = y / x
                    float startAngle = (float)(Math.Atan2(shape.FirstPoint.Y - shape.CenterY, shape.FirstPoint.X - shape.CenterX) / Math.PI * 180);
                    float endAngle = (float)(Math.Atan2(shape.SecondPoint.Y - shape.CenterY, shape.SecondPoint.X - shape.CenterX) / Math.PI * 180);
                    shape.StartAngle = startAngle;
                    shape.EndAngle = endAngle;
                    shape.IsFinishedInitial = true;
                    curPos = shape.SecondPoint;
                }
                return shape;
            };
        }

        internal GCodeParser SetGCodeInterperter(GGodeInterperter gCodeInterpreter)
        {
            this.gCodeInterpreter = gCodeInterpreter;
            return this;
        }

        public GCodeParser SeperateGCodes(List<string> lines)
        {
            strProcessCmdLines.Clear();
            List<string> code = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].ToUpper();
                bool isSeperatorLine = false;
                foreach (var item in GeoSplitter)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.Contains(item))
                    {
                        isSeperatorLine = true;
                        break;
                    }
                }
                if (isSeperatorLine)
                {
                    strProcessCmdLines.Add(MyUtils.MyUtils.Clone<string>(code));
                    code.Clear();
                }
                else
                {
                    code.Add(line);
                }
            }
            if (code.Count > 0)
            {
                strProcessCmdLines.Add(MyUtils.MyUtils.Clone<string>(code));
                code.Clear();
            }
            return this;
        }
        public bool IsValidLine(string line)
        {
            line = line.ToUpper();
            List<List<string>> list = new List<List<string>> { ExecuteLines, GeoSplitter, GeoCmds, GeoParams };
            foreach (var item in IgnoreLines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.Contains(item))
                {
                    return false;
                }
            }
            foreach (var item in list)
            {
                foreach (var item2 in item)
                {
                    if (line.Contains(item2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public Action<string> parseVariable = null;
        public Action<string> parseG00Code = null;
        public Func<string, Shape> parseG01Code = null;
        public Func<string, Shape> parseG02Code = null;
        public Func<string, Shape> parseG03Code = null;
        public Action<LineGeo, bool> actionAddMoveProcessor = null;
        public bool InitializeGCodeInterperter()
        {
            bool isNotFirstPart = false;
            gCodeInterpreter.ClearGeoProcessor();
            curPos = new PointGeo(0, 0);
            for (int i = 0; i < strProcessCmdLines.Count; i++)
            {
                List<string> singleProcess = strProcessCmdLines[i];
                GeoProcessorBase processor = null;
                Geo cutGeo = null;

                if (i == 0)//这里是第一个加工段前面的部分，这就不处理了
                {
                    isNotFirstPart = false;
                }
                else
                {
                    isNotFirstPart = true;
                }
                bool isCutProcess = false;
                bool isPassStart = false;
                for (int j = 0; j < singleProcess.Count; j++)
                {
                    string line = singleProcess[j];
                    if (line.Contains("LAST_LOOP_TACK_END"))
                    {
                        isPassStart = false;
                    }
                    if (line.Contains("LAST_LOOP_TACK_START"))
                    {
                        isPassStart = true;
                    }
                    if (isPassStart)
                    {
                        continue;
                    }
                    if (line.Contains("G91"))
                    {
                        isRelative = true;
                    }
                    if (line.Contains("G90"))
                    {
                        isRelative = false;
                    }
                    parseVariable(line);//前面部分也要提取变量，切割部分也有变量
                    if (!isNotFirstPart)//这里是第一个加工段前面的部分，这就不处理了
                    {
                        continue;
                    }

                    if (line.Contains("G00"))
                    {
                        parseG00Code(line);
                    }
                    Action<Shape> addCutShape = (shape) =>
                    {
                        if (shape != null)
                        {
                            cutGeo.AddShape(shape);
                        }
                    };

                    if (line.Contains("CUT_CONT_START"))
                    {
                        isCutProcess = true;
                        cutGeo = new Geo();
                        processor = new CutGeoProcessor();
                        continue;
                    }
                    if (line.Contains("CUT_CONT_END"))
                    {
                        isCutProcess = false;
                        processor = new CutGeoProcessor();
                        processor.CutGeo = cutGeo;
                        processor.StartPoint = cutGeo.GetShapes()[0].FirstPoint;
                        processor.EndPoint = cutGeo.GetShapes()[cutGeo.GetShapes().Count - 1].SecondPoint;
                        processor.cutSpeed = double.Parse(CalService.Input("CUTSPEED1"));
                        processor.cutDistance = double.Parse(CalService.Input("JUDGELIMITPERE"));
                        processor.cutDistanceInXDirection = double.Parse(CalService.Input("MINOUTERYGAP"));
                        gCodeInterpreter.AddGeoProcessor(processor);
                        continue;
                    }
                    if (isCutProcess)
                    {
                        addCutShape(parseG01Code(line));
                        addCutShape(parseG02Code(line));
                        addCutShape(parseG03Code(line));
                    }
                }
            }
            try
            {
                Settings.A_SHAFT_SPEED = double.Parse(CalService.Input("ASPEED"));
                Settings.CYCLE_LENGTH = double.Parse(CalService.Input("LOOPGAP"));
                Settings.SavetSettingFile();
            }
            catch (Exception)
            {
                throw new LaserException("ERROR", "文件读取异常LOOPGAP，GCodeParser初始化InitializeGCodeInterperter失败");

            }

            if (gCodeInterpreter.GetProcessors().Count == 0)
            {
                return false;
            }
            foreach (var item in gCodeInterpreter.GetProcessors())
            {
                if (item is CutGeoProcessor)
                {
                    gCodeInterpreter.StartPoint = item.CutGeo.GetShapes()[0].FirstPoint.Clone();
                    break;
                }
            }
            gCodeInterpreter.EndPoint = gCodeInterpreter.GetProcessors()[gCodeInterpreter.GetProcessors().Count - 1].CutGeo.GetShapes()[this.gCodeInterpreter.GetProcessors()[gCodeInterpreter.GetProcessors().Count - 1].CutGeo.GetShapes().Count - 1].SecondPoint.Clone();
            //设定循环G00 
            Geo loopRapidGeo = new Geo();
            LineGeo loopG00Line = new LineGeo();
            loopG00Line.FirstPoint = gCodeInterpreter.EndPoint;
            if (ProgramController.Instance.IsNoWait)
            {
                loopG00Line.SecondPoint = new PointGeo((float)(gCodeInterpreter.StartPoint.X + Settings.CYCLE_LENGTH), gCodeInterpreter.StartPoint.Y);
            }
            else
            {
                loopG00Line.SecondPoint = new PointGeo((float)(gCodeInterpreter.StartPoint.X - Settings.CYCLE_LENGTH), gCodeInterpreter.StartPoint.Y);
            }
            if (loopG00Line.SecondPoint.X < loopG00Line.FirstPoint.X && loopG00Line.GetPerimeter() > double.Parse(CalService.Input("HEAD1LIMIT")))
            {
                int breakCount = 3;
                for (int i = 0; i < breakCount; i++)
                {
                    LineGeo lineGeo = new LineGeo();
                    lineGeo.FirstPoint = (loopG00Line.SecondPoint - loopG00Line.FirstPoint) * (1.0f * (i + 0) / breakCount) + loopG00Line.FirstPoint;
                    lineGeo.SecondPoint = (loopG00Line.SecondPoint - loopG00Line.FirstPoint) * (1.0f * (i + 1) / breakCount) + loopG00Line.SecondPoint;
                    actionAddMoveProcessor(lineGeo, true);
                }
            }
            else
            {
                actionAddMoveProcessor(loopG00Line, true);
            }
            return true;
        }

    }
    /// <summary>
    /// xx.MPF文件
    /// </summary>
    class UniversalGCodeParser
    {
        private UniversalGCodeParser()
        {
            InitGCodeParser();
        }
        public Dictionary<int, GeoProcessorBase> LinesIndexedProcessor = new Dictionary<int, GeoProcessorBase>();
        public static UniversalGCodeParser Instance = new UniversalGCodeParser();
        //忽略的行
        public List<string> IgnoreLines = new List<string>() { "ID", "L", "R", "AA0" };
        //有效行
        public List<string> GeoSplitter = new List<string>() { "AA" };
        public List<string> ExecuteLines = new List<string>() { "G90", "G91", "G00", "G01", "G02", "G03", "G1", "G2", "G3" };
        public List<string> GeoCmds = new List<string>() { "CUT_CONT_START", "CUT_CONT_END" };
        public List<string> GeoParams = new List<string>() { "JUDGE", "ASPEED", "CUTSPEED1", "RAPIDSPEED", "LOOPGAP", "HEAD1LIMIT" };
        private List<string> strProcessCmdLines = new List<string>();
        private bool isRelative = false;
        private PointGeo curPos = new PointGeo(0, 0);
        private GGodeInterperter gCodeInterpreter;
        private int curLineIndex = 0;
        private void InitGCodeParser()
        {
            if (actionAddMoveProcessor == null)
            {
                actionAddMoveProcessor = (rapidLine) =>
                {
                    MoveGeoProcessor moveGeoProcessor = new MoveGeoProcessor();
                    Geo geo = new Geo();
                    geo.AddShape(rapidLine);
                    moveGeoProcessor.CutGeo = geo;
                    moveGeoProcessor.StartPoint = rapidLine.FirstPoint;
                    moveGeoProcessor.EndPoint = rapidLine.SecondPoint;
                    moveGeoProcessor.cutSpeed = Settings.DEFAULT_MOVE_SPEED;
                    moveGeoProcessor.cutDistance = rapidLine.GetPerimeter();
                    moveGeoProcessor.cutDistanceInXDirection = rapidLine.FirstPoint.X - rapidLine.SecondPoint.X;
                    this.gCodeInterpreter.AddGeoProcessor(moveGeoProcessor);
                    this.LinesIndexedProcessor.Add(curLineIndex, moveGeoProcessor);
                };
                actionAddCutProcessor = (cutLine) =>
                {
                    CutGeoProcessor processor = new CutGeoProcessor();
                    Geo geo = new Geo();
                    geo.AddShape(cutLine);
                    processor.CutGeo = geo;
                    processor.StartPoint = cutLine.FirstPoint;
                    processor.EndPoint = cutLine.SecondPoint;
                    processor.cutSpeed = Settings.DEFAULT_CUT_SPEED;
                    processor.cutDistance = cutLine.GetPerimeter();
                    processor.cutDistanceInXDirection = cutLine.FirstPoint.X - cutLine.SecondPoint.X;
                    this.gCodeInterpreter.AddGeoProcessor(processor);
                    this.LinesIndexedProcessor.Add(curLineIndex, processor);
                };
            }
            parseG00Code = (line) =>
            {
                if (line.Contains("G00"))
                {
                    //(FIRST)G00X1200.780Y1098.000
                    int index = line.IndexOf("G00");
                    int xindex = line.IndexOf("X");
                    int yindex = line.IndexOf("Y");
                    if (xindex == -1 || yindex == -1)
                    {
                        return;
                    }
                    int endIndex = line.Length;
                    for (int i = yindex + 1; i < line.Length; i++)
                    {
                        if (!(line[i] >= '0' && line[i] <= '9' || line[i] == '.' || line[i] == '-' || line[i] == '+'))
                        {
                            endIndex = i;
                            break;
                        }
                    }
                    string strX = line.Substring(xindex + 1, yindex - xindex - 1);
                    string strY = line.Substring(yindex + 1, endIndex - yindex - 1);
                    try
                    {
                        float x = float.Parse(strX);
                        float y = float.Parse(strY);
                        PointGeo point = new PointGeo(x, y);
                        LineGeo rapidLine = new LineGeo();
                        rapidLine.FirstPoint = curPos;
                        if (isRelative)
                        {
                            rapidLine.SecondPoint = point + curPos;
                        }
                        else
                        {
                            rapidLine.SecondPoint = point;
                        }
                        if (Math.Abs(curPos.X)< 1E-3 && Math.Abs(curPos.Y) < 1E-3)
                        {
                            curPos = rapidLine.SecondPoint;
                            return;
                        }
                        curPos = rapidLine.SecondPoint;
                        actionAddMoveProcessor(rapidLine);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            };
            parseG01Code = (line) =>
            {
                LineGeo shape = null;
                if (line.Contains("G01") || line.Contains("G1"))
                {
                    //G01X0.000Y-323.268
                    int index = 2;
                    if (line.Contains("G01"))
                    {
                        index = line.IndexOf("G01");
                    }
                    else
                    {
                        index = line.IndexOf("G1");
                    }
                    int xindex = line.IndexOf("X");
                    int yindex = line.IndexOf("Y");
                    if (xindex == -1 || yindex == -1)
                    {
                        return;
                    }
                    int endIndex = line.Length;
                    for (int i = yindex + 1; i < line.Length; i++)
                    {
                        if (!(line[i] >= '0' && line[i] <= '9' || line[i] == '.' || line[i] == '-' || line[i] == '+'))
                        {
                            endIndex = i;
                            break;
                        }
                    }
                    string strX = line.Substring(xindex + 1, yindex - xindex - 1);
                    string strY = line.Substring(yindex + 1, endIndex - yindex - 1);
                    try
                    {
                        float x = float.Parse(strX);
                        float y = float.Parse(strY);
                        PointGeo point = new PointGeo(x, y);
                        shape = new LineGeo();
                        shape.FirstPoint = curPos;
                        if (isRelative)
                        {
                            shape.SecondPoint = point + curPos;
                        }
                        else
                        {
                            shape.SecondPoint = point;
                        }
                        shape.FirstPoint = shape.FirstPoint;
                        shape.SecondPoint = shape.SecondPoint;

                        if (Math.Abs(curPos.X) < 1E-3 && Math.Abs(curPos.Y) < 1E-3)
                        {
                            curPos = shape.SecondPoint;
                            return;
                        }
                        curPos = shape.SecondPoint;
                        actionAddCutProcessor(shape);
                    }
                    catch (Exception)
                    {
                        return;// throw new LaserException("GCode Parse Error", "G01读取异常\n" + line); 
                    }
                }
            };
            parseG02Code = (line) =>
            {
                ArcGeo shape = null;
                if (line.Contains("G02") || line.Contains("G2"))//顺时针
                {
                    //G02X-399.732Y0.000I-199.866J7.323
                    int index = 0;
                    if (line.Contains("G02"))
                    {
                        index = line.IndexOf("G02");
                    }
                    else
                    {
                        index = line.IndexOf("G2");
                    }
                    int xindex = line.IndexOf("X");
                    int yindex = line.IndexOf("Y");
                    int iIndex = line.IndexOf("I");
                    int jIndex = line.IndexOf("J");
                    if (xindex == -1 || yindex == -1 || iIndex == -1 || jIndex == -1)
                    {
                        return;
                    }
                    string strX = line.Substring(xindex + 1, yindex - xindex - 1);
                    string strY = line.Substring(yindex + 1, iIndex - yindex - 1);
                    string strI = line.Substring(iIndex + 1, jIndex - iIndex - 1);
                    string strJ = line.Substring(jIndex + 1, line.Length - jIndex - 1);
                    try
                    {
                        float x = float.Parse(strX);
                        float y = float.Parse(strY);
                        float offsetI = float.Parse(strI);
                        float offsetJ = float.Parse(strJ);

                        PointGeo endPoint = new PointGeo(x, y);
                        PointGeo centerPoint = new PointGeo(offsetI, offsetJ);

                        shape = new ArcGeo();
                        shape.IsClockwise = true;
                        shape.FirstPoint = curPos;

                        if (isRelative)
                        {
                            shape.SecondPoint = endPoint + curPos;
                        }
                        else
                        {
                            shape.SecondPoint = endPoint;
                        }
                        shape.Radius = (float)Math.Sqrt(offsetI * offsetI + offsetJ * offsetJ);
                        centerPoint += curPos;
                        shape.CenterX = centerPoint.X;
                        shape.CenterY = centerPoint.Y;
                        //y,x 返回角度 θ，以弧度为单位，满足 - π≤θ≤π，且 tan(θ) = y / x
                        float startAngle = (float)(Math.Atan2(shape.SecondPoint.Y - shape.CenterY, shape.SecondPoint.X - shape.CenterX) / Math.PI * 180);
                        float endAngle = (float)(Math.Atan2(shape.FirstPoint.Y - shape.CenterY, shape.FirstPoint.X - shape.CenterX) / Math.PI * 180);
                        shape.StartAngle = startAngle;
                        shape.EndAngle = endAngle;
                        shape.IsFinishedInitial = true;
                        curPos = shape.SecondPoint;
                        actionAddCutProcessor(shape);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }

            };
            parseG03Code = (line) =>
            {
                ArcGeo shape = null;
                if (line.Contains("G03") || line.Contains("G3"))//逆时针
                {
                    //G02X-399.732Y0.000I-199.866J7.323
                    int index = 0;
                    if (line.Contains("G03"))
                    {
                        index = line.IndexOf("G03");
                    }
                    else
                    {
                        index = line.IndexOf("G3");
                    }
                    int xindex = line.IndexOf("X");
                    int yindex = line.IndexOf("Y");
                    int iIndex = line.IndexOf("I");
                    int jIndex = line.IndexOf("J");
                    if (xindex == -1 || yindex == -1 || iIndex == -1 || jIndex == -1)
                    {
                        return;
                    }
                    string strX = line.Substring(xindex + 1, yindex - xindex - 1);
                    string strY = line.Substring(yindex + 1, iIndex - yindex - 1);
                    string strI = line.Substring(iIndex + 1, jIndex - iIndex - 1);
                    string strJ = line.Substring(jIndex + 1, line.Length - jIndex - 1);
                    try
                    {
                        float x = float.Parse(strX);
                        float y = float.Parse(strY);
                        float offsetI = float.Parse(strI);
                        float offsetJ = float.Parse(strJ);

                        PointGeo endPoint = new PointGeo(x, y);
                        PointGeo centerPoint = new PointGeo(offsetI, offsetJ);

                        shape = new ArcGeo();
                        shape.IsClockwise = false;
                        shape.FirstPoint = curPos;

                        if (isRelative)
                        {
                            shape.SecondPoint = endPoint + curPos;
                        }
                        else
                        {
                            shape.SecondPoint = endPoint;
                        }
                        shape.Radius = (float)Math.Sqrt(offsetI * offsetI + offsetJ * offsetJ);
                        centerPoint += curPos;
                        shape.CenterX = centerPoint.X;
                        shape.CenterY = centerPoint.Y;
                        //返回角度 θ，以弧度为单位，满足 - π≤θ≤π，且 tan(θ) = y / x
                        float startAngle = (float)(Math.Atan2(shape.FirstPoint.Y - shape.CenterY, shape.FirstPoint.X - shape.CenterX) / Math.PI * 180);
                        float endAngle = (float)(Math.Atan2(shape.SecondPoint.Y - shape.CenterY, shape.SecondPoint.X - shape.CenterX) / Math.PI * 180);
                        shape.StartAngle = startAngle;
                        shape.EndAngle = endAngle;
                        shape.IsFinishedInitial = true;
                        curPos = shape.SecondPoint;
                        actionAddCutProcessor(shape);
                    }
                    catch (Exception)
                    {
                        return;//throw new LaserException("GCode Parse Error","G03读取异常\n"+line);
                    }
                }

            };
        }

        internal UniversalGCodeParser SetGCodeInterperter(GGodeInterperter gCodeInterpreter)
        {
            this.gCodeInterpreter = gCodeInterpreter;
            return this;
        }
        public bool IsValidLine(string line)
        {
            line = line.ToUpper();
            List<List<string>> list = new List<List<string>> { ExecuteLines };
            foreach (var item in IgnoreLines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.Contains(item))
                {
                    return false;
                }
            }
            foreach (var item in list)
            {
                foreach (var item2 in item)
                {
                    if (line.Contains(item2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public Action<string> parseVariable = null;
        public Action<string> parseG00Code = null;
        public Action<string> parseG01Code = null;
        public Action<string> parseG02Code = null;
        public Action<string> parseG03Code = null;
        public Action<LineGeo> actionAddMoveProcessor = null;
        public Action<Shape> actionAddCutProcessor = null;
        public bool InitializeGCodeInterperter(List<string> lines)
        {
            gCodeInterpreter.ClearGeoProcessor();
            curLineIndex = -1;
            curPos = new PointGeo(0, 0);
            LinesIndexedProcessor.Clear();
            CalService.Input("CUTSPEED1=" + Settings.DEFAULT_CUT_SPEED.ToString("f3"));
            CalService.Input("RAPIDSPEED=" + Settings.DEFAULT_MOVE_SPEED.ToString("f3"));
            CalService.Input("LOOPGAP=" + 0);
            CalService.Input("ASPEED=" + 0);
            CalService.Input("HEAD1LIMIT=" + 1000000);
            Settings.BEAMON = 0.1;
            Settings.BEAMOFF = 0.1;
            Settings.A_SHAFT_SPEED = double.Parse(CalService.Input("ASPEED"));
            Settings.CYCLE_LENGTH = double.Parse(CalService.Input("LOOPGAP"));
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                curLineIndex++;
                if (line.Contains("G91"))
                {
                    isRelative = true;
                }
                if (line.Contains("G90"))
                {
                    isRelative = false;
                }
                parseG00Code(line);
                parseG01Code(line);
                parseG02Code(line);
                parseG03Code(line);
            }

            Settings.SavetSettingFile();
            if (gCodeInterpreter.GetProcessors().Count == 0)
            {
                return false;
            }
            gCodeInterpreter.StartPoint = gCodeInterpreter.GetProcessors()[0].CutGeo.GetShapes()[0].FirstPoint.Clone();
            gCodeInterpreter.EndPoint = gCodeInterpreter.GetProcessors()[gCodeInterpreter.GetProcessors().Count - 1].CutGeo.GetShapes()[this.gCodeInterpreter.GetProcessors()[gCodeInterpreter.GetProcessors().Count - 1].CutGeo.GetShapes().Count - 1].SecondPoint.Clone();
            //设定循环G00
            Geo loopRapidGeo = new Geo();
            LineGeo loopG00Line = new LineGeo();
            loopG00Line.FirstPoint = gCodeInterpreter.EndPoint;
            if (ProgramController.Instance.IsNoWait)
            {
                loopG00Line.SecondPoint = new PointGeo((float)(gCodeInterpreter.StartPoint.X + Settings.CYCLE_LENGTH), gCodeInterpreter.StartPoint.Y);
            }
            else
            {
                loopG00Line.SecondPoint = new PointGeo((float)(gCodeInterpreter.StartPoint.X - Settings.CYCLE_LENGTH), gCodeInterpreter.StartPoint.Y);
            }
            actionAddMoveProcessor(loopG00Line);
            return true;
        }
    }
}
