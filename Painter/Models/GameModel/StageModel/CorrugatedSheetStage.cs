using Painter.Models.Paint;
using Painter.Models.StageModel;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models.GameModel.StageModel
{

    class CorrugatedSheet
    {
        public PointGeo OriPos = new PointGeo();
        public int LineCount = 16;
        public int LineMargin = 86;
        public int LineLength = 1000;
        public float SlotWidth = 80;
        public float SlotHeight = 30;
    }
    class WeldObj : Obstacle
    {
        private string labelName = "1";
        public string LabelName
        {
            get { return labelName; }
            set
            {
                this.labelName = value;
                this.elements.Clear();
                InitElement();
            }
        }
        public int Index = 0;
        public virtual void InitElement() { }
        protected PointGeo centerPos = new PointGeo(0, 0); 
        public PointGeo CenterPos
        {
            get { return centerPos; }
            set
            {
                centerPos = value;
                this.elements.Clear();
                InitElement();
            }
        }
    }
    class LineObj : WeldObj
    {
        PointGeo startPos = new PointGeo(0, 0);
        PointGeo endPos = new PointGeo(0, 100);
       
        public PointGeo StartPos { 
            get { return startPos; }  
            set { startPos = value;
                this.elements.Clear();
                InitElement();
            }
        }
        public PointGeo EndPos
        {
            get { return endPos; }
            set
            {
                endPos = value;
                this.elements.Clear();
                InitElement();
            }
        } 
        public string ToGCode(bool isRel=false)
        {
            if (isRel)
            {
                return "X" + (EndPos-StartPos).X.ToString("f3") + " " + (EndPos - StartPos).Y.ToString("f3") + "\n";

            }
            else
            {
                return "X" + this.EndPos.X.ToString("f3") + " " + this.EndPos.Y.ToString("f3")+"\n"  ;
            }
        }
        public override void InitElement()
        {
            ShapeMeta lineMeta=new ShapeMeta() { ForeColor = Color.White, BackColor = Color.White, LineWidth = 3 };
            LineGeo lineGeo = new LineGeo();
            lineGeo.FirstPoint = StartPos;
            lineGeo.SecondPoint = EndPos;
            lineGeo.SetDrawMeta(lineMeta);
            PointGeo dir = EndPos - StartPos;
            dir.Normalized();

            PointGeo nor = dir.Clone();
            nor.RotateAroundOrigin(90,new PointGeo(0,0));
            nor.Normalized();

            LineGeo arrowLine1 = new LineGeo();
            arrowLine1.FirstPoint = (StartPos+EndPos)/2;
            float arrowLength = 15;
            arrowLine1.SecondPoint = (StartPos + EndPos) / 2- arrowLength * dir+nor*(arrowLength / 1.73f);
            arrowLine1.SetDrawMeta(lineMeta);

            LineGeo arrowLine2 = new LineGeo();
            arrowLine2.FirstPoint = (StartPos + EndPos) / 2;
            arrowLine2.SecondPoint = (StartPos + EndPos) / 2 - arrowLength * dir - nor * arrowLength / 1.73f;
            arrowLine2.SetDrawMeta(lineMeta);

            DrawableText labelText = new DrawableText();
            labelText.pos = (StartPos+EndPos)/2+nor.Abs()*40;
            labelText.SetDrawMeta(new TextMeta(LabelName) { IsScaleble = true, ForeColor = Color.LimeGreen, TEXTFONT = new Font("Consolas Bold", 24f), stringFormat = new StringFormat() { Alignment = StringAlignment.Center } });

            this.Add(lineGeo);
            this.Add(arrowLine2);
            this.Add(arrowLine1);
            this.Add(labelText);
        }
    }
    class SlotObj : WeldObj
    {
        float length = 50;
        float width = 20;
        float angle = 0;
        public bool isLeft = false;
        public float Length{
            get { return length; }
            set {
                length = value;
                this.elements.Clear();
                InitElement();
            }
        }
        public float Width
        {
            get { return width; }
            set
            {
                width = value;
                this.elements.Clear();
                InitElement();
            }
        }
        public float Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                this.elements.Clear();
                InitElement();
            }
        } 
        public override void InitElement()
        {
            ShapeMeta lineMeta = new ShapeMeta() { ForeColor = Color.White, BackColor = Color.White, LineWidth = 3 };
          
            PointGeo dir=new PointGeo(1f,0f);
            dir.RotateAroundOrigin(angle, new PointGeo(0, 0));
            dir.Normalized();

            PointGeo nor = dir.Clone();
            nor.RotateAroundOrigin(90, new PointGeo(0, 0));
            nor.Normalized();

            LineGeo lineGeo = new LineGeo();
            lineGeo.FirstPoint = centerPos+ dir*(length-width)/2+nor*width/2;
            lineGeo.SecondPoint = centerPos - dir * (length - width) / 2 + nor * width / 2; ;
            lineGeo.SetDrawMeta(lineMeta);

            ArcGeo arcGeo = new ArcGeo();
            arcGeo.StartAngle = 90;
            arcGeo.EndAngle = 270;
            arcGeo.CenterX = (centerPos - dir * (length - width) / 2).X;
            arcGeo.CenterY = (centerPos - dir * (length - width) / 2).Y;
            arcGeo.Radius = width/2;
            arcGeo.SetDrawMeta(lineMeta);

            LineGeo lineGeo2 = new LineGeo();
            lineGeo2.FirstPoint = centerPos - dir * (length - width) / 2 - nor * width / 2; ;
            lineGeo2.SecondPoint = centerPos + dir * (length - width) / 2 - nor * width / 2;
            lineGeo2.SetDrawMeta(lineMeta);

            ArcGeo arcGeo2 = new ArcGeo();
            arcGeo2.StartAngle = -90;
            arcGeo2.EndAngle = 90;
            arcGeo2.CenterX = (centerPos + dir * (length - width) / 2).X;
            arcGeo2.CenterY = (centerPos + dir * (length - width) / 2).Y;
            arcGeo2.Radius = width / 2;
            arcGeo2.SetDrawMeta(lineMeta);

            DrawableText labelText = new DrawableText();
            if (isLeft)
            {
                labelText.pos = centerPos -dir*(length+0)  + nor.Abs() * (15);
            }else
            {
                labelText.pos = centerPos +dir*(length+0)  + nor.Abs() * (15);
            }
            labelText.SetDrawMeta(new TextMeta(LabelName) { IsScaleble = true, ForeColor = Color.Red, TEXTFONT = new Font("Consolas Bold", 24f), stringFormat = new StringFormat() { Alignment = StringAlignment.Center } });

            this.Add(lineGeo);
            this.Add(arcGeo);
            this.Add(lineGeo2);
            this.Add(arcGeo2);

            this.Add(labelText);
        }
    }
    

    class CorrugatedSheetStage : StageScene
    {
        public CorrugatedSheet SheetData = new CorrugatedSheet();
        public CorrugatedSheetStage()
        {
            MaxX = 600;
            MinX = 0;
            MinY = 0;
            MaxY = 800;
            width = 600;
            height = 800;
        }
        DrawableText scoreText = new DrawableText();
        MainCharacter character = new MainCharacter();
        public override Scene CreateScene()
        {
            CanvasModel.EnableTrack = false;
            scene.Background = Color.Black;
            Obstacle TextObject = new Obstacle();
            scoreText.pos = new PointGeo(300, 400);
            scoreText.SetDrawMeta(new TextMeta("你好呀 Hello") { IsScaleble = true, ForeColor = Color.LimeGreen, TEXTFONT = new Font("Consolas Bold", 36f), stringFormat = new StringFormat() { Alignment = StringAlignment.Center } });
            RectangleGeo rect = new RectangleGeo(scoreText.pos, scoreText.pos - new PointGeo(100, 100));
            rect.SetDrawMeta(new ShapeMeta() { ForeColor=Color.White,BackColor=Color.Red });
            TextObject.Add(rect);
            TextObject.Add(scoreText);
            scene.AddObject(TextObject, false);
             
            Obstacle sheetObj = new Obstacle();
            int index = 1;
            List<WeldObj> listObj = new List<WeldObj>();
            for (int i = 0; i < SheetData.LineCount; i++)
            {
                LineObj lineObj = new LineObj();
                lineObj.Index = (index++);
                lineObj.LabelName = (lineObj.Index).ToString();
                //if (i%2==0)
                //{
                //    lineObj.StartPos = new PointGeo(0, 0 - i * SheetData.LineMargin);
                //    lineObj.EndPos = new PointGeo(-SheetData.LineLength, 0 - i * SheetData.LineMargin);
                //}else
                //{
                //    lineObj.EndPos = new PointGeo(0, 0 - i * SheetData.LineMargin);
                //    lineObj.StartPos = new PointGeo(-SheetData.LineLength, 0 - i * SheetData.LineMargin);
                //}
                    lineObj.StartPos = new PointGeo(0, 0 - i * SheetData.LineMargin);
                    lineObj.EndPos = new PointGeo(-SheetData.LineLength, 0 - i * SheetData.LineMargin);

                lineObj.CenterPos = (lineObj.EndPos + lineObj.StartPos) / 2;
                SlotObj slot = new SlotObj();
                slot.Index = (index++);
                slot.LabelName = (slot.Index).ToString();
                slot.isLeft = false;
                slot.Width = SheetData.SlotHeight;
                slot.Length = SheetData.SlotWidth;
                slot.CenterPos = new PointGeo(-SheetData.SlotWidth/2, 0 - i * SheetData.LineMargin);
                SlotObj slot2 = new SlotObj();
                slot2.Index = (index++);
                slot2.LabelName = slot2.Index.ToString();
                slot2.isLeft = true;
                slot2.Width = SheetData.SlotHeight;
                slot2.Length = SheetData.SlotWidth;
                slot2.CenterPos = new PointGeo(SheetData.SlotWidth / 2 - SheetData.LineLength, 0 - i * SheetData.LineMargin);
                listObj.Add(lineObj);
                listObj.Add(slot);
                listObj.Add(slot2); 
            }
            int firstIndex = 16;
            firstIndex = firstIndex * 3 - 2;
            Func<WeldObj, WeldObj,bool> compare = (WeldObj w1, WeldObj w2) =>{
                bool result = true;
                if (w1 is LineObj)
                {
                    if (w2 is SlotObj)
                    {
                        return true;
                    }else
                    {
                        if (Math.Abs(w1.Index-firstIndex)< Math.Abs(w2.Index - firstIndex))
                        {
                            return true;
                        }else
                        {
                            return false;
                        }
                    }
                }else
                {
                    if (w2 is LineObj)
                    {
                        return false;
                    }else
                    {
                        
                    }
                }
                return result;
            };



            for (int i = 0; i < listObj.Count; i++)
            { 
                for (int j = i+1; j < listObj.Count; j++)
                { 
                    WeldObj weldObj1 = listObj[i];
                    WeldObj weldObj2 = listObj[j];
                    if(!compare(weldObj1, weldObj2))
                    {
                        WeldObj temp = listObj[i];
                        listObj[i] = listObj[j];
                        listObj[j] = temp;
                    }
                }
            }
            PointGeo lastPoint = new PointGeo();
            for (int i = 0; i < listObj.Count; i++)
            {
                var welObj = listObj[i];
                if (welObj is LineObj)
                {
                    LineObj line = welObj as LineObj;
                    if (i%2==0)
                    {
                        var p = line.StartPos;
                        line.StartPos = line.EndPos;
                        line.EndPos = p;
                    }
                    lastPoint = line.EndPos;
                }
            }
            List<SlotObj> tempObjs = new List<SlotObj>();
            List<LineObj> lines = new List<LineObj>();

            for (int i = 0; i < listObj.Count; i++)
            {
                var welObj = listObj[i];
                if (welObj is LineObj)
                {
                    lines.Add(welObj as LineObj);
                }else 
                if (welObj is  SlotObj)
                {
                    tempObjs.Add(welObj as SlotObj);
                }
            }

            List<SlotObj> tempObjs2 = new List<SlotObj>();
            int size = tempObjs.Count;
            
            for (int i = 0; i < size; i++)
            {
                int minIndex = 0;
                float MinDistance = 1E6f;
                for (int j = 0; j < tempObjs.Count; j++)
                {
                    if (tempObjs[j].CenterPos.DistanceTo(lastPoint)< MinDistance)
                    {
                        MinDistance = tempObjs[j].CenterPos.DistanceTo(lastPoint);
                        minIndex = j;
                    }
                   
                }
                var obj = tempObjs[minIndex];
                tempObjs2.Add(obj);
                lastPoint = obj.CenterPos;
                tempObjs.RemoveAt(minIndex);
            }
             
            listObj.Clear();

            listObj.AddRange(lines);
            listObj.AddRange(tempObjs2);

            for (int i = 0; i < listObj.Count; i++)
            {
                var welObj = listObj[i];
                //welObj.LabelName = (i+1)+"("+welObj.Index+")";
                welObj.LabelName = (i + 1)+"";
                scene.AddObject(welObj, false); 
                this.MinY = welObj.MinY < this.MinY ? welObj.MinY : this.MinY;
                this.MinX = welObj.MinX < this.MinX ? welObj.MinX : this.MinX;
                this.MaxX = welObj.MaxX > this.MaxX ? welObj.MaxX : this.MaxX;
                this.MaxY = welObj.MaxY > this.MaxY ? welObj.MaxY : this.MaxY;
            }
            return scene;
        }

        public override MainCharacter GetMainCharacter()
        {
            return character;
        }
    }
}
