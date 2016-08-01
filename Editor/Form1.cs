using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Platformer;
using Platformer.Components;
using Platformer.Levels;

using SimpleGameLibrary;

namespace Editor
{
    public partial class Form1 : Form
    {
        private LevelData levelData;
        private Image tileSheet;

        private Graphics panelGraphics;

        private int entitySize = 24;
        private float zoom = 1f;
        
        // The source rectangles for each of the tiles in the tileSheet
        private Rectangle[] sourceRects;

        // Camera
        private int cameraX = 0;
        private int cameraY = 0;

        public Form1()
        {
            InitializeComponent();
            
            // set up the graphics for drawing the level preview
            panelGraphics = pnlPreview.CreateGraphics();
            panelGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            // load the tileSheet
            tileSheet = new Bitmap("images/tilesheet.png");

            // initialize the sourceRects
            sourceRects = new Rectangle[9];
            sourceRects[0] = new Rectangle(0, 0, 32, 32);
            sourceRects[1] = new Rectangle(0, 32, 32, 32);
            sourceRects[2] = new Rectangle(32, 32, 32, 32);
            sourceRects[3] = new Rectangle(64, 32, 32, 32);
            sourceRects[4] = new Rectangle(96, 32, 32, 32);
            sourceRects[5] = new Rectangle(128, 32, 32, 32);
            sourceRects[6] = new Rectangle(160, 32, 32, 32);
            sourceRects[7] = new Rectangle(96, 0, 32, 32);
            sourceRects[8] = new Rectangle(160, 0, 32, 32);

            // initialize the levelData
            levelData = new LevelData(20, 20);

            levelData.tiles[3][3] = new IndividualTileData(1, false);

            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(Key_press);

            Shown += new EventHandler((sender, e) => Draw());
        }

        private void lvEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvTiles.SelectedIndices.Clear();
        }

        private void lvTiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvEntities.SelectedIndices.Clear();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Shane O' Malley Files (*.som)|*.som|All Files (*.*)|*.*";

            string gameLevelDirectory = Directory.GetCurrentDirectory() + "/../../../Platformer/bin/Windows/x86/Debug/Data/levels";
            if (Directory.Exists(gameLevelDirectory))
                fd.InitialDirectory = gameLevelDirectory;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = fd.OpenFile())
                    Serialization.Serialize<LevelData>(stream, levelData);
            }

            fd.Dispose();

            Draw();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Shane O' Malley Files (*.som)|*.som|All Files (*.*)|*.*";

            string gameLevelDirectory = Directory.GetCurrentDirectory() + "/../../../Platformer/bin/Windows/x86/Debug/Data/levels";
            if (Directory.Exists(gameLevelDirectory))
                fd.InitialDirectory = gameLevelDirectory;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = fd.OpenFile())
                    levelData = Serialization.Deserialize<LevelData>(stream);
            }

            fd.Dispose();

            Draw();
        }

        private void newLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            levelData.ClearTiles();
            levelData.ResizeTiles(15, 15);

            Draw();
        }

        private void resizeLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Show the resize level dialog */
            using (FormResize formResize = new FormResize())
            {
                DialogResult result = formResize.ShowDialog();
                
                if (result == DialogResult.OK)
                {
                    levelData.ResizeTiles(formResize.GridWidth, formResize.GridHeight);
                }
            }

            Draw();
        }

        private void btnClearEntities_Click(object sender, EventArgs e)
        {
            levelData.ClearEntities();

            Draw();
        }

        private void pnlPreview_Click(object sender, MouseEventArgs e)
        {
            float invZoom = 1 / zoom;

            int gridX = (int)(((e.X * invZoom + cameraX)) / Globals.TileSize);
            int gridY = (int)(((e.Y * invZoom + cameraY)) / Globals.TileSize);

            bool insideGrid = (gridX >= 0 && gridX < levelData.tiles.Length && gridY >= 0 && gridY < levelData.tiles[0].Length);

            if (e.Button == MouseButtons.Left)
            {
                /* If an entity is selected ... */
                if (lvEntities.SelectedIndices.Count != 0)
                {
                    /* ... add an entity at the mouse's position */
                    int posX = (int)((cbEntitySnap.Checked ? gridX * Globals.TileSize + Globals.TileSize / 2 : e.X * invZoom + cameraX));
                    int posY = (int)((cbEntitySnap.Checked ? gridY * Globals.TileSize + Globals.TileSize / 2 : e.Y * invZoom + cameraY));
                    posX -= (int)((entitySize / 2));
                    posY -= (int)((entitySize / 2));

                    levelData.entities.Add(new EntityData(lvEntities.SelectedIndices[0], posX, posY));
                }
                /* If a tile is selected ... */
                if (lvTiles.SelectedIndices.Count != 0)
                {
                    /* ... add a tile at the mouse's position */
                    if (insideGrid)
                    {
                        if (e.Button == MouseButtons.Left)
                            levelData.tiles[gridX][gridY] = new IndividualTileData(lvTiles.SelectedIndices[0] + 1, cbFlipTile.Checked);
                    }
                }
            }

            else if (e.Button == MouseButtons.Right)
            {
                bool entityRemoved = false;

                /* remove entity at mouse */
                for (int ent = 0; ent < levelData.entities.Count; ent++)
                {
                    EntityData entity = levelData.entities[ent];
                    
                    Rectangle rect = new Rectangle(
                        (int)(entity.positionX * zoom - cameraX),
                        (int)(entity.positionY * zoom - cameraY),
                        (int)(entitySize * zoom),
                        (int)(entitySize * zoom));
                    if (rect.Contains(e.X, e.Y))
                    {
                        levelData.entities.Remove(entity);
                        ent--;
                        entityRemoved = true;
                    }
                }

                /* remove tile at mouse, only if there was not an entity in front of it */
                if (insideGrid && !entityRemoved)
                    levelData.tiles[gridX][gridY] = null;
            }

            Draw();
        }

        private void Draw()
        {
            // clear the screen (panel) before drawing
            panelGraphics.Clear(Color.CornflowerBlue);

            float invZoom = zoom;

            // loop through every tile, and draw it
            for (int i = 0; i < levelData.tiles.Length; i++)
                for (int j = 0; j < levelData.tiles[0].Length; j++)
                {
                    IndividualTileData tile = levelData.tiles[i][j];
                    
                    // if the tile is not air, draw it
                    if (tile != null)
                    {
                        Rectangle dstRect = new Rectangle(
                            (int)(((i * Globals.TileSize) - cameraX) * zoom),
                            (int)(((j * Globals.TileSize) - cameraY) * zoom),
                            (int)(Globals.TileSize * invZoom),
                            (int)(Globals.TileSize * invZoom));

                        if (tile.flipHorizontal)
                        {
                            Bitmap flippedImage = (Bitmap)ilTiles.Images[tile.id - 1].Clone();
                            flippedImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            panelGraphics.DrawImage(flippedImage, dstRect);
                        }
                        else
                            panelGraphics.DrawImage(ilTiles.Images[tile.id - 1], dstRect);
                    }
                }

            /* Draw the entities */
            foreach (EntityData entityData in levelData.entities)
            {
                Console.WriteLine("drawing entity");
                Rectangle dstRect = new Rectangle(
                    (int)((entityData.positionX - cameraX) * zoom),
                    (int)((entityData.positionY - cameraY) * zoom), 
                    (int)((entitySize) * invZoom), 
                    (int)((entitySize) * invZoom));
                panelGraphics.DrawImage(ilEntities.Images[entityData.id], dstRect);
            }

            /* Draw the gridlines */
            Pen pen = new Pen(Color.Black, 1);
            // top
            panelGraphics.DrawLine(
                pen, 
                (0 - cameraX) * invZoom, 
                (0 - cameraY) * invZoom, 
                ((levelData.tiles.Length * Globals.TileSize) - cameraX) * invZoom,
                (0 - cameraY) * invZoom);
            // right
            panelGraphics.DrawLine(
                pen,
                ((levelData.tiles.Length * Globals.TileSize) - cameraX) * invZoom,
                (0 - cameraY) * invZoom,
                ((levelData.tiles.Length * Globals.TileSize) - cameraX) * invZoom,
                ((levelData.tiles[0].Length * Globals.TileSize) - cameraY) * invZoom);
            // bottom
            panelGraphics.DrawLine(
                pen,
                (0 - cameraX) * invZoom,
                ((levelData.tiles[0].Length * Globals.TileSize) - cameraY) * invZoom,
                ((levelData.tiles.Length * Globals.TileSize) - cameraX) * invZoom,
                ((levelData.tiles[0].Length * Globals.TileSize) - cameraY) * invZoom);
            // left
            panelGraphics.DrawLine(
                pen,
                (0 - cameraX) * invZoom,
                (0 - cameraY) * invZoom,
                (0 - cameraX) * invZoom,
                ((levelData.tiles[0].Length * Globals.TileSize) - cameraY) * invZoom);

            /* vertical lines */
            for (int i = 0; i < levelData.tiles.Length; i++)
            {
                panelGraphics.DrawLine(
                    pen,
                    ((i * Globals.TileSize) - cameraX) * invZoom,
                    (0 - cameraY) * invZoom,
                    ((i * Globals.TileSize) - cameraX) * invZoom,
                    ((levelData.tiles[0].Length * Globals.TileSize) - cameraY) * invZoom);
            }

            /* horizontal lines */
            for (int i = 0; i < levelData.tiles[0].Length; i++)
            {
                panelGraphics.DrawLine(
                    pen,
                    (0 - cameraX) * invZoom,
                    ((i * Globals.TileSize) - cameraY) * invZoom,
                    ((levelData.tiles.Length * Globals.TileSize) - cameraX) * invZoom,
                    ((i * Globals.TileSize) - cameraY) * invZoom);
            }
        }

        private void Key_press(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                /* camera movement */
                case 'w':
                    cameraY -= 64;
                    break;

                case 's':
                    cameraY += 64;
                    break;

                case 'a':
                    cameraX -= 64;
                    break;

                case 'd':
                    cameraX += 64;
                    break;

                /* zooming in and out */
                case '=':
                    zoom += 0.1f;
                    break;

                case '-':
                    zoom -= 0.1f;
                    break;
            }

            Draw();
        }
    }
}
