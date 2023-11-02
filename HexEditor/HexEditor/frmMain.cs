using HexEditor.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace HexEditor
{
    public partial class frmMain : Form
    {
        private readonly WorldService WorldService = new WorldService();
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnWorldsDat_Click(object sender, EventArgs e)
        {
            this.WorldService.ImportDat();
        }

        

        private void btnWorldsJson_Click(object sender, EventArgs e)
        {
            this.WorldService.ImportJson();
        }

    }
}
