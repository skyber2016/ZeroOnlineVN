using HexEditor.Services;
using System;
using System.Windows.Forms;

namespace HexEditor
{
    public partial class frmMain : Form
    {
        private readonly WorldService WorldService = new WorldService();
        private readonly ItemTypeService ItemTypeService = new ItemTypeService();
        private readonly ShopService ShopService = new ShopService();
        private readonly RobotTypeService RobotTypeService = new RobotTypeService();
        private readonly UploadService UploadService = new UploadService();
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

        private void btnItemTypeDat_Click(object sender, EventArgs e)
        {
            this.ItemTypeService.ImportDat();
        }

        private void btnItemTypeJson_Click(object sender, EventArgs e)
        {
            this.ItemTypeService.ImportJson();
        }

        private void btnShopDat_Click(object sender, EventArgs e)
        {
            this.ShopService.ImportDat();
        }

        private void btnShopJson_Click(object sender, EventArgs e)
        {
            this.ShopService.ImportJson();
        }

        private void btnRobotTypeDat_Click(object sender, EventArgs e)
        {
            RobotTypeService.ImportDat();
        }

        private void btnRobotTypeJson_Click(object sender, EventArgs e)
        {
            RobotTypeService.ImportJson();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            UploadService.DoUpload();
        }
    }
}
