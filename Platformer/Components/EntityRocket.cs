using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SimpleGameLibrary.Components;
using SimpleGameLibrary.Sprites;

using Platformer.Levels;
using SimpleGameLibrary;

namespace Platformer.Components
{
    class EntityRocket : Entity
    {
        /* Common rocket attributes */
        private static readonly Vector2 rocketSize = new Vector2(7, 4) * 3;
        private static readonly int rocketMaxHp = 1;

        private float xSpeed;
        private float ySpeed;

        private Level levelRef;
        private int bounces;
        private float speed;

        #region Field Region
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public EntityRocket(Vector2 position, float rotation, float speed, int bounces, Level levelRef)
            : base(position, rocketSize, GameContent.RocketSprite, new Vector2(3.5f, 2f), rotation, rocketMaxHp)
        {
            this.speed = speed;
            this.bounces = bounces;

            this.levelRef = levelRef;

            Globals.nBullets++;
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region

        public override void Update(GameTime gameTime)
        {
            int nextGridX = (int)((position.X + xSpeed) / Globals.TileSize);
            int nextGridY = (int)((position.Y + ySpeed) / Globals.TileSize);
            int gridX = (int)(position.X / Globals.TileSize);
            int gridY = (int)(position.Y / Globals.TileSize);

            Vector2? poi = null;

            //if (gridY >= 0 && gridY < levelRef.Tiles.GetLength(0)
            //    && gridX >= 0 && gridX < levelRef.Tiles.GetLength(1))
            if (Utils.IsInRange(nextGridY, 0, levelRef.Tiles.GetLength(0))
                && Utils.IsInRange(nextGridX, 0, levelRef.Tiles.GetLength(1))
                && Utils.IsInRange(gridY, 0, levelRef.Tiles.GetLength(0))
                && Utils.IsInRange(gridX, 0, levelRef.Tiles.GetLength(1)))
            {
                /*
                Vector2? poi;
                float angle;

                Tile tile = levelRef.Tiles[gridY, gridX];
                tile.getIntersectionData(position, position + new Vector2(xSpeed, ySpeed), out poi, out angle);

                if (poi != null)
                {
                    tile.DebugDraw = true;
                    this.rotation = angle;
                }
                */

                int x = gridX;
                int y = gridY;
                int xSign = Math.Sign(gridX - nextGridX);
                int ySign = Math.Sign(gridY - nextGridY);

                List<Tile> tilesToCheck = new List<Tile>();

                /* nested do-while loops to populate list of tiles to check */
                // TODO Fix This
                //do
                //{
                //    do
                //    {
                //        /* Add the tile to the list of tiles to check */
                //        Tile tile = levelRef.Tiles[y, x];
                //        tile.DebugDraw = true;
                //        tilesToCheck.Add(tile);

                //        y -= ySign;
                //    }
                //    while (y != nextGridY);

                //    x -= xSign;
                //}
                //while (x != nextGridX);

                // this is a temp solution, which will work most of the time
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                    {
                        if (Utils.IsInRange(gridY + j, 0, levelRef.Tiles.GetLength(0))
                            && Utils.IsInRange(gridX + i, 0, levelRef.Tiles.GetLength(1)))
                            tilesToCheck.Add(levelRef.Tiles[gridY + j, gridX + i]);
                    }

                /* Sort the tiles based on their distance from the bullet's starting pos */
                tilesToCheck.Sort((t1, t2) => 
                    Vector2.Distance(new Vector2(gridX, gridY), t1.Position / Globals.TileSize).CompareTo(
                        Vector2.Distance(new Vector2(gridX, gridY), t2.Position / Globals.TileSize)));
                
                foreach (Tile tile in tilesToCheck)
                {
                    //Console.WriteLine("tile {0}, pos = {1}", n++, tile.Center);

                    tile.DebugDraw = true;

                    float angle;

                    tile.getIntersectionData(position, position + new Vector2(xSpeed, ySpeed), out poi, out angle);

                    if (poi != null)
                    {
                        this.rotation = angle;

                        break;
                    }
                }
            }

            /* Update the bullet's speed */
            xSpeed = (float)Math.Cos(MathHelper.ToRadians(rotation)) * speed;
            ySpeed = (float)Math.Sin(MathHelper.ToRadians(rotation)) * speed;

            /* Update the bullet's position based on its speed */
            if (poi == null)
                position += new Vector2(xSpeed, ySpeed);
            else
                //position = (Vector2)poi;
                position = Vector2.Lerp(position, position + new Vector2(xSpeed, ySpeed), 0.95f);

            base.Update(gameTime);
        }

        /* Return the tile closest to (0, 0) */
        

        #endregion
    }
}
