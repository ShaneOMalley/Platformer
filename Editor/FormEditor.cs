﻿using System;
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
    public partial class FormEditor : Form
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

        public FormEditor()
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

        /* save the level as an .som file on disk */
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* initialize the file dialog */
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Shane O' Malley Files (*.som)|*.som|All Files (*.*)|*.*";

            /* set the path to start in the game's level directory */
            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.Length - 17);
            path += @"\Platformer\bin\Windows\x86\Debug\Data\levels";

            /* set the initial directory of the file dialog, if it exists */
            if (Directory.Exists(path))
                fd.InitialDirectory = path;

            /* show the dialog, and save the level to the file chosen or created by the user */
            if (fd.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = fd.OpenFile())
                    Serialization.Serialize<LevelData>(stream, levelData);
            }

            /* free any resources used by the file dialog */
            fd.Dispose();
        }

        /* load a level from a .som file on disk */
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* initialize the file dialog */
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Shane O' Malley Files (*.som)|*.som|All Files (*.*)|*.*";

            /* set the path to start in the game's level directory */
            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.Length - 17);
            path += @"\Platformer\bin\Windows\x86\Debug\Data\levels";

            /* set the initial directory of the file dialog if it exists */
            if (Directory.Exists(path))
                fd.InitialDirectory = path;

            /* show the dialog, and load the level using the file chosen by the user */
            if (fd.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = fd.OpenFile())
                    levelData = Serialization.Deserialize<LevelData>(stream);
            }

            /* free any resources used by the file dialog */
            fd.Dispose();

            /* draw the level after loading it */
            Draw();
        }

        /* clear all tiles and entities from the level, and set the grid size */
        private void newLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            levelData.ClearTiles();
            levelData.ClearEntities();
            levelData.ResizeTiles(15, 15);

            Draw();
        }

        /* resize the tile grid */
        private void resizeLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Show the resize level dialog */
            using (FormResize formResize = new FormResize(levelData.tiles.Length, levelData.tiles[0].Length))
            {
                DialogResult result = formResize.ShowDialog();
                
                if (result == DialogResult.OK)
                {
                    levelData.ResizeTiles(formResize.GridWidth, formResize.GridHeight);
                }
            }

            Draw();
        }

        /* move all tiles and entities in the level by a certain amount */
        private void shiftLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormShift formShift = new FormShift())
            {
                DialogResult result = formShift.ShowDialog();

                if (result == DialogResult.OK)
                {
                    int right = formShift.RightShift;
                    int down = formShift.DownShift;

                    IndividualTileData[][] oldTiles = levelData.tiles;
                    IndividualTileData[][] newTiles = new IndividualTileData[oldTiles.Length][];
                    for (int i = 0; i < newTiles.Length; i++)
                        newTiles[i] = new IndividualTileData[oldTiles[i].Length];

                    /* Loop through all tiles, replacing each with the one at relative position (-left, -down) */
                    for (int i = 0; i < newTiles.Length; i++)
                    {
                        for (int j = 0; j < newTiles[0].Length; j++)
                        {
                            if (i - right < 0 || j - down < 0
                                || i - right >= oldTiles.Length || j - down >= oldTiles[0].Length)
                                newTiles[i][j] = null;
                            else
                            {
                                newTiles[i][j] = oldTiles[i - right][j - down];
                            }
                        }
                    }

                    /* move each entity the same distance as the tiles are shifted */
                    foreach (EntityData entity in levelData.entities)
                    {
                        entity.positionX += right * Globals.TileSize;
                        entity.positionY += down * Globals.TileSize;
                    }

                    levelData.tiles = newTiles;
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
                        (int)((entity.positionX - cameraX) * zoom),
                        (int)((entity.positionY - cameraY) * zoom),
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
