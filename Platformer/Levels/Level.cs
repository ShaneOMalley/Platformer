﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SimpleGameLibrary;
using SimpleGameLibrary.Sprites;
using SimpleGameLibrary.Components;

using Platformer.Components;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace Platformer.Levels
{
    public class Level
    {
        #region Field Region

        /* Each tile will have this as a sprite. Their different sequences is what will determine what tile(s) they use */
        private Sprite tileSheetSprite;

        /* The array of tiles */
        private Tile[,] tiles;

        /* the level's entities */
        private List<Entity> entities;
        // entities added to these lists will be added or removed from the 
        // 'entities' list after all entities have been updated each cycle
        private List<Entity> entityAddBuffer;
        private List<Entity> entityRemoveBuffer;
        // a special reference to the 'player spawn' entity
        private EntityPlayerSpawn playerSpawn;

        /* the texture containing all the tiles */
        private Texture2D tileSheetTexture;
        
        #endregion

        #region Property Region

        public Tile[,] Tiles
        {
            get { return tiles; }
        }

        public List<Entity> Entities
        {
            get { return entities; }
        }

        public EntityPlayerSpawn PlayerSpawn
        {
            get { return playerSpawn; }
        }

        #endregion

        #region Constructor Region

        public Level(Texture2D tileSheetTexture, string levelDataFileName)
        {
            this.tileSheetTexture = tileSheetTexture;
            tileSheetSprite = new Sprite(tileSheetTexture, 32, 32, 12);

            LoadLevel(levelDataFileName);
        }

        #endregion

        #region Method Region

        private void LoadLevel(string fileName)
        {
            /* load and deserialize the level */
            LevelData levelData = SimpleGameLibrary.Serialization.Deserialize<LevelData>(fileName);

            /* set up tiles */
            tiles = new Tile[levelData.tiles[0].Length, levelData.tiles.Length];

            for (int x = 0; x < levelData.tiles.Length; x++)
                for (int y = 0; y < levelData.tiles[0].Length; y++)
                {
                    IndividualTileData individualTileData = levelData.tiles[x][y];

                    int id = individualTileData != null ? individualTileData.id : 0;

                    Components.TileData tileData = TileDataManager.tileDataDictionary[id];

                    Sprite tileSprite = (Sprite)tileSheetSprite.Clone();
                    tileSprite.AddSequence("default", 200, tileData.frames);

                    Tile tile = new Tile(x, y, tileSprite, tileData);
                    if (individualTileData != null && individualTileData.flipHorizontal)
                        tile.Flip();

                    tiles[y, x] = tile;
                }

            /* set up entities */
            entities = new List<Entity>();

            foreach (EntityData entityData in levelData.entities)
            {
                if (entityData.id == 0)
                {
                    playerSpawn = new EntityPlayerSpawn(new Vector2(entityData.positionX, entityData.positionY));
                    entities.Add(playerSpawn);
                }
            }

            /* set up the entitiy add and remove buffers */
            entityAddBuffer = new List<Entity>();
            entityRemoveBuffer = new List<Entity>();
        }

        /* Create an explosion at the given position */
        public void InstantiateExplosion(Vector2 position, float wallNormal)
        {
            entityAddBuffer.Add(new EntityExplosion(position, 120f, wallNormal, this));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            /* Draw all the tiles */
            for (int y = 0; y < tiles.GetLength(0); y++)
                for (int x = 0; x < tiles.GetLength(1); x++)
                    tiles[y, x].Draw(gameTime, spriteBatch);

            /* Draw all the entities */
            foreach (Entity entity in entities)
                entity.Draw(gameTime, spriteBatch);

            /* Debug draw the player spawn point */
            if (Globals.DebugMode && PlayerSpawn != null)
            {
                Rectangle rect = new Rectangle(PlayerSpawn.Position.ToPoint(), new Point(10));
                rect.Offset(-5, -5);
                DebugDrawing.DrawRectangle(spriteBatch, rect, Color.Purple, 1);
            }
        }

        public void Update(GameTime gameTime)
        {
            /* Update all the tiles */
            for (int y = 0; y < tiles.GetLength(0); y++)
                for (int x = 0; x < tiles.GetLength(1); x++)
                    tiles[y, x].Update(gameTime);

            /* Update all the entities */
            for (int i = 0; i < entities.Count; i++)
            {
                Entity e = entities[i];

                if (e.Destroyed)
                {
                    entityRemoveBuffer.Add(e);
                    if (e.GetType() == typeof(EntityRocket))
                        Globals.nBullets--;
                    continue;
                }

                e.Update(gameTime);
            }

            /* Add or remove entities in buffers */
            foreach (Entity e in entityAddBuffer)
                entities.Add(e);
            entityAddBuffer.Clear();

            foreach (Entity e in entityRemoveBuffer)
                entities.Remove(e);
            entityRemoveBuffer.Clear();
        }

        #endregion
    }
}
