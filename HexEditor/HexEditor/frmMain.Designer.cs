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
            this.tab_worlds_dat = new System.Windows.Forms.TabPage();
            this.tabShop = new System.Windows.Forms.TabPage();
            this.btnWorldsDat = new System.Windows.Forms.Button();
            this.btnWorldsJson = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tab_worlds_dat.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tab_worlds_dat);
            this.tabControl.Controls.Add(this.tabShop);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(307, 94);
            this.tabControl.TabIndex = 0;
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
            // tabShop
            // 
            this.tabShop.Location = new System.Drawing.Point(4, 23);
            this.tabShop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabShop.Name = "tabShop";
            this.tabShop.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabShop.Size = new System.Drawing.Size(299, 67);
            this.tabShop.TabIndex = 1;
            this.tabShop.Text = "shop.dat";
            this.tabShop.UseVisualStyleBackColor = true;
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
            this.tab_worlds_dat.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tab_worlds_dat;
        private System.Windows.Forms.Button btnWorldsDat;
        private System.Windows.Forms.TabPage tabShop;
        private System.Windows.Forms.Button btnWorldsJson;
    }
}

