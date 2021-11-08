using Painter.Models;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.DisplayManger
{
    public static class ScreenElementCreator
    {
        public static IScreenPrintable CreateElement(DRAW_TYPE dt)
        {
            switch (dt)
            {
                case DRAW_TYPE.CIRCLE:
                    return new CircleGeo();
                case DRAW_TYPE.RECTANGLE:
                    return new RectangleGeo();
                case DRAW_TYPE.LINE:
                    return new LineGeo();
                case DRAW_TYPE.TEXT:
                    return new DrawableText();
                case DRAW_TYPE.RANDOMLINES:
                    return new RandomLines();
                case DRAW_TYPE.IMAGE:
                    return new DrawableImage();
                case DRAW_TYPE.ELLIPSE:
                    return new EllipseGeo();
                default:
                    return new RectangleGeo();
            }
        }
    }
    public class GraphicsLayerManager
    {
        public void Dispose()
        {
            if (this.Painter != null)
            {
                Painter.Dispose();
            }
        }

        public GraphicsLayerManager() { }
        public GraphicsLayerManager(PainterBase p)
        {
            SetPainter(p);
        }
        public void SetPainter(PainterBase p)
        {
            Painter = p;
        }
        public PainterBase GetPainter()
        {
            return this.Painter;
        }
        private PainterBase Painter;//包含绘制图形的方法  
        private readonly List<IScreenPrintable> drawList = new List<IScreenPrintable>();
        public event Action Invalidate;
        public List<IScreenPrintable> GetDatas()
        {
            return this.drawList;
        }
        private object LockObj = new object();
        public void AddRange(List<IScreenPrintable> isps, bool isReverse = false)
        {
            foreach (var item in isps)
            {
                Add(item, isReverse);
            }
        }
        public void Add(IScreenPrintable ips, bool isReverse = false)
        {
            lock (lockObj)
            {
                if (this.drawList.Count > CurrentPos)
                {
                    this.drawList.RemoveRange(CurrentPos, this.drawList.Count - CurrentPos);
                }
                if (!this.drawList.Contains(ips))
                {
                    if (isReverse)
                    {
                        this.drawList.Insert(0, ips);
                    }
                    else
                    {
                        this.drawList.Add(ips);
                    }
                    CurrentPos++;
                }
            }
        }
        private int CurrentPos = 0;
        public int GetCurPos()
        {
            return CurrentPos;
        }

        public void Draw()
        { 
            //倒着刷新，方便能够删除元素。
            int size = Math.Min(CurrentPos, this.drawList.Count);
            for (int i = size - 1; i >= 0; i--)
            {
                drawList[i].Draw(Painter);
                if (drawList[i] is Shape)
                {
                    if ((drawList[i] as Shape).IsDisposed == true)
                    {
                        drawList.RemoveAt(i);
                        CurrentPos--;
                    }
                }
                else if (drawList[i] is RandomLines)
                {
                    if ((drawList[i] as RandomLines).IsDisposed == true)
                    {
                        drawList.RemoveAt(i);
                        CurrentPos--;
                    }
                }
            }

        }
        private object lockObj = new object();
        public void Clear()
        {
            lock (lockObj)
            {
                drawList.Clear();
                CurrentPos = 0;
                this.Painter.Clear(Color.Transparent);
                if (this.Invalidate != null)
                    Invalidate();
            }
        }
        public void Undo()
        {
            if (CurrentPos > 0)
            {
                CurrentPos--;
                Draw();
            }
        }
        public void Redo()
        {
            if (CurrentPos < drawList.Count)
            {
                CurrentPos++;
                Draw();
            }
        }
        private void Remove(int index)
        {
            drawList.RemoveAt(index);
            if (this.Invalidate != null)
                Invalidate();
        }
        public void LoadShape(List<Shape> shapes)
        {
            CurrentPos += shapes.Count;
            this.drawList.AddRange(shapes);
        }
        public void OffsetShapes(float x, float y)
        {
            foreach (var item in this.drawList)
            {
                Shape shape = item as Shape;
                if (shape == null)
                {
                    continue;
                }
                if (shape is LineGeo)
                {
                    (shape as LineGeo).Translate(new PointGeo(x, y));
                }
                else
                if (shape is ArcGeo)
                {
                    (shape as ArcGeo).Translate(new PointGeo(x, y));
                }
                else
                {
                    shape.Translate(new PointGeo(x, y));
                }

            }
        }
        public void SaveOrLoadFile(bool IsSaveFile)
        {
            if (IsSaveFile)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = "dat";
                sfd.AddExtension = true;
                sfd.Filter = "二进制文件|*.dat";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string FileName = sfd.FileName;
                    using (FileStream fs = new FileStream(FileName, FileMode.Create))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(fs, drawList);
                        //string msg=JsonConvert.SerializeObject(this.drawList);
                        //byte[] datas = Encoding.UTF8.GetBytes(msg);
                        //fs.Write(datas, 0, datas.Length);
                    }
                }
                sfd.Dispose();
            }
            else
            {
                using (OpenFileDialog ofd = new OpenFileDialog()
                {
                    DefaultExt = "dat",
                    AddExtension = true,
                    Filter = "二进制文件|*.dat"
                })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string FileName = ofd.FileName;
                        using (FileStream fs = new FileStream(FileName, FileMode.Open))
                        {
                            IFormatter formatter = new BinaryFormatter();
                            var list = ((List<IScreenPrintable>)formatter.Deserialize(fs));
                            CurrentPos += list.Count;
                            this.drawList.AddRange(list);
                        }
                    }
                }
            }
        }
    }
}
