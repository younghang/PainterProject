using Painter.Models;
using Painter.Models.GameModel.StageModel;
using Painter.Models.PainterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter
{
    public partial class CorrugatedSheetForm : Form
    {
        public CorrugatedSheetForm()
        {
            InitializeComponent();

        }

        GeoLabel geoLabel = null;
        private static string configFile = "./CorrugatedSheet.txt";
        private void LoadConfig()
        {
            if (File.Exists(configFile))
            {
                SheetData = CorrugatedSheet.LoadFromJson(File.ReadAllText(configFile)); ;
            }
            this.txtLineCount.Value = SheetData.LineCount;
            this.txtStartNum.Value = SheetData.StartLine;
            this.txtLineLength.Value = (decimal)SheetData.LineLength;
            this.txtLineMargin.Value = (decimal)SheetData.LineMargin;
            this.txtSlotHeight.Value = (decimal)SheetData.SlotHeight;
            this.txtSlotWidth.Value = (decimal)SheetData.SlotWidth;
            this.OriX.Value = (decimal)SheetData.OriPos.X;
            this.OriY.Value = (decimal)SheetData.OriPos.Y;
        }
        private void CorrugatedSheetForm_Load(object sender, EventArgs e)
        {
            LoadConfig();
            corugatedSheetStage = new CorrugatedSheetStage(SheetData);
            this.canvasView.SetCurStage(corugatedSheetStage);
            geoLabel = new GeoLabel(100, 20, new RectangleGeo());
            canvasView.AddControl(geoLabel);
            canvasView.MouseMove += CanvasView_MouseMove;
        }

        private void CanvasView_MouseMove(object sender, MouseEventArgs e)
        {
            geoLabel.Text = canvasView.GetCanvasModel().CurObjectPoint.ToString();
        }

        CorrugatedSheet SheetData = new CorrugatedSheet();
        CorrugatedSheetStage corugatedSheetStage = null;
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (corugatedSheetStage!=null)
            {
                corugatedSheetStage.Clear();
            }
            SheetData.LineCount = (int)this.txtLineCount.Value;
            SheetData.StartLine = (int)this.txtStartNum.Value;
            SheetData.LineLength = (float)this.txtLineLength.Value;
            SheetData.LineMargin = (float)this.txtLineMargin.Value;
            SheetData.SlotHeight = (float)this.txtSlotHeight.Value;
            SheetData.SlotWidth = (float)this.txtSlotWidth.Value;
            SheetData.OriPos.X= (float)this.OriX.Value;
            SheetData.OriPos.Y= (float)this.OriY.Value;
            this.canvasView.SetCurStage(corugatedSheetStage);
            File.WriteAllText(configFile, CorrugatedSheet.ToJsonStr(SheetData));
        }
   
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (corugatedSheetStage!=null)
            {
                corugatedSheetStage.GenerateGCode();
                MessageBox.Show("Done");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new GCodeLaserHeadForm().Show();
        }

        private void canvasView_Load(object sender, EventArgs e)
        {

        }
    }
}
