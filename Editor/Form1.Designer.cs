namespace Editor
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("", 0);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("", 1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("", 2);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("", 3);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("", 4);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("", 5);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("", 6);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("", 7);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("", 8);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("", 9);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("", 0);
            this.pnlPreview = new System.Windows.Forms.Panel();
            this.lvTiles = new System.Windows.Forms.ListView();
            this.ilTiles = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvEntities = new System.Windows.Forms.ListView();
            this.ilEntities = new System.Windows.Forms.ImageList(this.components);
            this.cbEntitySnap = new System.Windows.Forms.CheckBox();
            this.cbFlipTile = new System.Windows.Forms.CheckBox();
            this.btnClearEntities = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPreview
            // 
            this.pnlPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlPreview.Location = new System.Drawing.Point(12, 27);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Size = new System.Drawing.Size(743, 529);
            this.pnlPreview.TabIndex = 0;
            this.pnlPreview.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlPreview_Click);
            // 
            // lvTiles
            // 
            this.lvTiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTiles.GridLines = true;
            this.lvTiles.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10});
            this.lvTiles.LargeImageList = this.ilTiles;
            this.lvTiles.Location = new System.Drawing.Point(761, 289);
            this.lvTiles.Name = "lvTiles";
            this.lvTiles.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lvTiles.Size = new System.Drawing.Size(280, 242);
            this.lvTiles.SmallImageList = this.ilTiles;
            this.lvTiles.TabIndex = 1;
            this.lvTiles.TileSize = new System.Drawing.Size(128, 128);
            this.lvTiles.UseCompatibleStateImageBehavior = false;
            this.lvTiles.SelectedIndexChanged += new System.EventHandler(this.lvTiles_SelectedIndexChanged);
            // 
            // ilTiles
            // 
            this.ilTiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTiles.ImageStream")));
            this.ilTiles.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTiles.Images.SetKeyName(0, "B.png");
            this.ilTiles.Images.SetKeyName(1, "C.png");
            this.ilTiles.Images.SetKeyName(2, "D.png");
            this.ilTiles.Images.SetKeyName(3, "E.png");
            this.ilTiles.Images.SetKeyName(4, "F.png");
            this.ilTiles.Images.SetKeyName(5, "G.png");
            this.ilTiles.Images.SetKeyName(6, "H.png");
            this.ilTiles.Images.SetKeyName(7, "I.png");
            this.ilTiles.Images.SetKeyName(8, "J.png");
            this.ilTiles.Images.SetKeyName(9, "K.png");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.newLevelToolStripMenuItem,
            this.resizeLevelToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1053, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadToolStripMenuItem.Text = "&Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // newLevelToolStripMenuItem
            // 
            this.newLevelToolStripMenuItem.Name = "newLevelToolStripMenuItem";
            this.newLevelToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.newLevelToolStripMenuItem.Text = "&New Level";
            this.newLevelToolStripMenuItem.Click += new System.EventHandler(this.newLevelToolStripMenuItem_Click);
            // 
            // resizeLevelToolStripMenuItem
            // 
            this.resizeLevelToolStripMenuItem.Name = "resizeLevelToolStripMenuItem";
            this.resizeLevelToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.resizeLevelToolStripMenuItem.Text = "&Resize Level";
            this.resizeLevelToolStripMenuItem.Click += new System.EventHandler(this.resizeLevelToolStripMenuItem_Click);
            // 
            // lvEntities
            // 
            this.lvEntities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lvEntities.GridLines = true;
            this.lvEntities.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem11});
            this.lvEntities.LargeImageList = this.ilEntities;
            this.lvEntities.Location = new System.Drawing.Point(761, 27);
            this.lvEntities.Name = "lvEntities";
            this.lvEntities.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lvEntities.Size = new System.Drawing.Size(280, 233);
            this.lvEntities.SmallImageList = this.ilEntities;
            this.lvEntities.TabIndex = 1;
            this.lvEntities.TileSize = new System.Drawing.Size(128, 128);
            this.lvEntities.UseCompatibleStateImageBehavior = false;
            this.lvEntities.SelectedIndexChanged += new System.EventHandler(this.lvEntities_SelectedIndexChanged);
            // 
            // ilEntities
            // 
            this.ilEntities.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilEntities.ImageStream")));
            this.ilEntities.TransparentColor = System.Drawing.Color.Transparent;
            this.ilEntities.Images.SetKeyName(0, "player_spawn.png");
            // 
            // cbEntitySnap
            // 
            this.cbEntitySnap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEntitySnap.AutoSize = true;
            this.cbEntitySnap.Location = new System.Drawing.Point(761, 266);
            this.cbEntitySnap.Name = "cbEntitySnap";
            this.cbEntitySnap.Size = new System.Drawing.Size(135, 17);
            this.cbEntitySnap.TabIndex = 4;
            this.cbEntitySnap.Text = "Snap entities to tile grid";
            this.cbEntitySnap.UseVisualStyleBackColor = true;
            // 
            // cbFlipTile
            // 
            this.cbFlipTile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFlipTile.AutoSize = true;
            this.cbFlipTile.Location = new System.Drawing.Point(761, 539);
            this.cbFlipTile.Name = "cbFlipTile";
            this.cbFlipTile.Size = new System.Drawing.Size(119, 17);
            this.cbFlipTile.TabIndex = 4;
            this.cbFlipTile.Text = "Flip Tile Horizontally";
            this.cbFlipTile.UseVisualStyleBackColor = true;
            // 
            // btnClearEntities
            // 
            this.btnClearEntities.Location = new System.Drawing.Point(923, 262);
            this.btnClearEntities.Name = "btnClearEntities";
            this.btnClearEntities.Size = new System.Drawing.Size(118, 23);
            this.btnClearEntities.TabIndex = 5;
            this.btnClearEntities.Text = "Clear Entities";
            this.btnClearEntities.UseVisualStyleBackColor = true;
            this.btnClearEntities.Click += new System.EventHandler(this.btnClearEntities_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 566);
            this.Controls.Add(this.btnClearEntities);
            this.Controls.Add(this.cbFlipTile);
            this.Controls.Add(this.cbEntitySnap);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lvEntities);
            this.Controls.Add(this.lvTiles);
            this.Controls.Add(this.pnlPreview);
            this.Location = new System.Drawing.Point(1006, 605);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1069, 605);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1069, 605);
            this.Name = "Form1";
            this.Text = "Level Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlPreview;
        private System.Windows.Forms.ListView lvTiles;
        private System.Windows.Forms.ImageList ilTiles;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newLevelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resizeLevelToolStripMenuItem;
        private System.Windows.Forms.ListView lvEntities;
        private System.Windows.Forms.ImageList ilEntities;
        private System.Windows.Forms.CheckBox cbEntitySnap;
        private System.Windows.Forms.CheckBox cbFlipTile;
        private System.Windows.Forms.Button btnClearEntities;
    }
}

