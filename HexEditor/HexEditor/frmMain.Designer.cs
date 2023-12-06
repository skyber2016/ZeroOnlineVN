namespace HexEditor
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPatches = new System.Windows.Forms.TabPage();
            this.btnUpload = new System.Windows.Forms.Button();
            this.tab_worlds_dat = new System.Windows.Forms.TabPage();
            this.btnWorldsJson = new System.Windows.Forms.Button();
            this.btnWorldsDat = new System.Windows.Forms.Button();
            this.tabItemType = new System.Windows.Forms.TabPage();
            this.btnItemTypeJson = new System.Windows.Forms.Button();
            this.btnItemTypeDat = new System.Windows.Forms.Button();
            this.tabShop = new System.Windows.Forms.TabPage();
            this.btnShopJson = new System.Windows.Forms.Button();
            this.btnShopDat = new System.Windows.Forms.Button();
            this.tabRobottype = new System.Windows.Forms.TabPage();
            this.btnRobotTypeJson = new System.Windows.Forms.Button();
            this.btnRobotTypeDat = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPatches.SuspendLayout();
            this.tab_worlds_dat.SuspendLayout();
            this.tabItemType.SuspendLayout();
            this.tabShop.SuspendLayout();
            this.tabRobottype.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPatches);
            this.tabControl.Controls.Add(this.tab_worlds_dat);
            this.tabControl.Controls.Add(this.tabItemType);
            this.tabControl.Controls.Add(this.tabShop);
            this.tabControl.Controls.Add(this.tabRobottype);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(307, 94);
            this.tabControl.TabIndex = 0;
            // 
            // tabPatches
            // 
            this.tabPatches.Controls.Add(this.btnUpload);
            this.tabPatches.Location = new System.Drawing.Point(4, 23);
            this.tabPatches.Name = "tabPatches";
            this.tabPatches.Size = new System.Drawing.Size(299, 67);
            this.tabPatches.TabIndex = 4;
            this.tabPatches.Text = "Auto Patches";
            this.tabPatches.UseVisualStyleBackColor = true;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(8, 3);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(283, 56);
            this.btnUpload.TabIndex = 0;
            this.btnUpload.Text = "Upload file Patches";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // tab_worlds_dat
            // 
            this.tab_worlds_dat.Controls.Add(this.btnWorldsJson);
            this.tab_worlds_dat.Controls.Add(this.btnWorldsDat);
            this.tab_worlds_dat.Location = new System.Drawing.Point(4, 23);
            this.tab_worlds_dat.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tab_worlds_dat.Name = "tab_worlds_dat";
            this.tab_worlds_dat.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tab_worlds_dat.Size = new System.Drawing.Size(299, 67);
            this.tab_worlds_dat.TabIndex = 0;
            this.tab_worlds_dat.Text = "worlds.dat";
            this.tab_worlds_dat.UseVisualStyleBackColor = true;
            // 
            // btnWorldsJson
            // 
            this.btnWorldsJson.Location = new System.Drawing.Point(152, 6);
            this.btnWorldsJson.Name = "btnWorldsJson";
            this.btnWorldsJson.Size = new System.Drawing.Size(138, 51);
            this.btnWorldsJson.TabIndex = 1;
            this.btnWorldsJson.Text = "worlds.json";
            this.btnWorldsJson.UseVisualStyleBackColor = true;
            this.btnWorldsJson.Click += new System.EventHandler(this.btnWorldsJson_Click);
            // 
            // btnWorldsDat
            // 
            this.btnWorldsDat.Location = new System.Drawing.Point(8, 6);
            this.btnWorldsDat.Name = "btnWorldsDat";
            this.btnWorldsDat.Size = new System.Drawing.Size(138, 51);
            this.btnWorldsDat.TabIndex = 0;
            this.btnWorldsDat.Text = "worlds.dat";
            this.btnWorldsDat.UseVisualStyleBackColor = true;
            this.btnWorldsDat.Click += new System.EventHandler(this.btnWorldsDat_Click);
            // 
            // tabItemType
            // 
            this.tabItemType.Controls.Add(this.btnItemTypeJson);
            this.tabItemType.Controls.Add(this.btnItemTypeDat);
            this.tabItemType.Location = new System.Drawing.Point(4, 23);
            this.tabItemType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabItemType.Name = "tabItemType";
            this.tabItemType.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabItemType.Size = new System.Drawing.Size(299, 67);
            this.tabItemType.TabIndex = 1;
            this.tabItemType.Text = "itemtype.dat";
            this.tabItemType.UseVisualStyleBackColor = true;
            // 
            // btnItemTypeJson
            // 
            this.btnItemTypeJson.Location = new System.Drawing.Point(152, 8);
            this.btnItemTypeJson.Name = "btnItemTypeJson";
            this.btnItemTypeJson.Size = new System.Drawing.Size(138, 51);
            this.btnItemTypeJson.TabIndex = 3;
            this.btnItemTypeJson.Text = "itemtype.json";
            this.btnItemTypeJson.UseVisualStyleBackColor = true;
            this.btnItemTypeJson.Click += new System.EventHandler(this.btnItemTypeJson_Click);
            // 
            // btnItemTypeDat
            // 
            this.btnItemTypeDat.Location = new System.Drawing.Point(8, 8);
            this.btnItemTypeDat.Name = "btnItemTypeDat";
            this.btnItemTypeDat.Size = new System.Drawing.Size(138, 51);
            this.btnItemTypeDat.TabIndex = 2;
            this.btnItemTypeDat.Text = "itemtype.dat";
            this.btnItemTypeDat.UseVisualStyleBackColor = true;
            this.btnItemTypeDat.Click += new System.EventHandler(this.btnItemTypeDat_Click);
            // 
            // tabShop
            // 
            this.tabShop.Controls.Add(this.btnShopJson);
            this.tabShop.Controls.Add(this.btnShopDat);
            this.tabShop.Location = new System.Drawing.Point(4, 23);
            this.tabShop.Name = "tabShop";
            this.tabShop.Size = new System.Drawing.Size(299, 67);
            this.tabShop.TabIndex = 2;
            this.tabShop.Text = "shop.dat";
            this.tabShop.UseVisualStyleBackColor = true;
            // 
            // btnShopJson
            // 
            this.btnShopJson.Location = new System.Drawing.Point(152, 8);
            this.btnShopJson.Name = "btnShopJson";
            this.btnShopJson.Size = new System.Drawing.Size(138, 51);
            this.btnShopJson.TabIndex = 3;
            this.btnShopJson.Text = "shop.json";
            this.btnShopJson.UseVisualStyleBackColor = true;
            this.btnShopJson.Click += new System.EventHandler(this.btnShopJson_Click);
            // 
            // btnShopDat
            // 
            this.btnShopDat.Location = new System.Drawing.Point(8, 8);
            this.btnShopDat.Name = "btnShopDat";
            this.btnShopDat.Size = new System.Drawing.Size(138, 51);
            this.btnShopDat.TabIndex = 2;
            this.btnShopDat.Text = "shop.dat";
            this.btnShopDat.UseVisualStyleBackColor = true;
            this.btnShopDat.Click += new System.EventHandler(this.btnShopDat_Click);
            // 
            // tabRobottype
            // 
            this.tabRobottype.Controls.Add(this.btnRobotTypeJson);
            this.tabRobottype.Controls.Add(this.btnRobotTypeDat);
            this.tabRobottype.Location = new System.Drawing.Point(4, 23);
            this.tabRobottype.Name = "tabRobottype";
            this.tabRobottype.Size = new System.Drawing.Size(299, 67);
            this.tabRobottype.TabIndex = 3;
            this.tabRobottype.Text = "robottype.dat";
            this.tabRobottype.UseVisualStyleBackColor = true;
            // 
            // btnRobotTypeJson
            // 
            this.btnRobotTypeJson.Location = new System.Drawing.Point(152, 8);
            this.btnRobotTypeJson.Name = "btnRobotTypeJson";
            this.btnRobotTypeJson.Size = new System.Drawing.Size(138, 51);
            this.btnRobotTypeJson.TabIndex = 5;
            this.btnRobotTypeJson.Text = "robottype.json";
            this.btnRobotTypeJson.UseVisualStyleBackColor = true;
            this.btnRobotTypeJson.Click += new System.EventHandler(this.btnRobotTypeJson_Click);
            // 
            // btnRobotTypeDat
            // 
            this.btnRobotTypeDat.Location = new System.Drawing.Point(8, 8);
            this.btnRobotTypeDat.Name = "btnRobotTypeDat";
            this.btnRobotTypeDat.Size = new System.Drawing.Size(138, 51);
            this.btnRobotTypeDat.TabIndex = 4;
            this.btnRobotTypeDat.Text = "robottype.dat";
            this.btnRobotTypeDat.UseVisualStyleBackColor = true;
            this.btnRobotTypeDat.Click += new System.EventHandler(this.btnRobotTypeDat_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(307, 94);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("JetBrains Mono", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tool Hex Editor";
            this.tabControl.ResumeLayout(false);
            this.tabPatches.ResumeLayout(false);
            this.tab_worlds_dat.ResumeLayout(false);
            this.tabItemType.ResumeLayout(false);
            this.tabShop.ResumeLayout(false);
            this.tabRobottype.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tab_worlds_dat;
        private System.Windows.Forms.Button btnWorldsDat;
        private System.Windows.Forms.TabPage tabItemType;
        private System.Windows.Forms.Button btnWorldsJson;
        private System.Windows.Forms.Button btnItemTypeJson;
        private System.Windows.Forms.Button btnItemTypeDat;
        private System.Windows.Forms.TabPage tabShop;
        private System.Windows.Forms.Button btnShopJson;
        private System.Windows.Forms.Button btnShopDat;
        private System.Windows.Forms.TabPage tabRobottype;
        private System.Windows.Forms.Button btnRobotTypeJson;
        private System.Windows.Forms.Button btnRobotTypeDat;
        private System.Windows.Forms.TabPage tabPatches;
        private System.Windows.Forms.Button btnUpload;
    }
}

