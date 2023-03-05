using Painter.Models.GameModel.StageModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void CorrugatedSheetForm_Load(object sender, EventArgs e)
        {
            CorrugatedSheetStage corugatedSheetStage = new CorrugatedSheetStage();
            this.canvasView.SetCurStage(corugatedSheetStage);
        }
    }
}
