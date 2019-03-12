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

        public Vector2 RoundedPosition
        {
            get { return new Vector2((int)position.X, (int)position.Y); }
        }

        public Vector2 RoundedSpeed
        {
            get { return RoundedPosition + new Vector2((int)xSpeed, (int)ySpeed); }
        }

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
            if (InputHandler.KeyPressed(Microsoft.Xna.Framework.Input.Keys.X)
                || InputHandler.ButtonPressed(Microsoft.Xna.Framework.Input.Buttons.Y, PlayerIndex.One))
            {
                destroyed = true;
                return;
            }

            int nextGridX = (int)((position.X + xSpeed) / Globals.TileSize);
            int nextGridY = (int)((position.Y + ySpeed) / Globals.TileSize);
            int gridX = (int)(position.X / Globals.TileSize);
            int gridY = (int)(position.Y / Globals.TileSize);

            Vector2? intersectionPoint = null;
            float wallNormal = 0;

            Tile tileCollided = null;

            /* Update the bullet's speed */
            xSpeed = (float)Math.Cos(MathHelper.ToRadians(rotation)) * speed;
            ySpeed = (float)Math.Sin(MathHelper.ToRadians(rotation)) * speed;

            if (Utils.IsInRange(nextGridY, 0, levelRef.Tiles.GetLength(0))
                && Utils.IsInRange(nextGridX, 0, levelRef.Tiles.GetLength(1))
                && Utils.IsInRange(gridY, 0, levelRef.Tiles.GetLength(0))
                && Utils.IsInRange(gridX, 0, levelRef.Tiles.GetLength(1)))
            {
                int x = gridX;
                int y = gridY;
                int xSign = Math.Sign(gridX - nextGridX);
                int ySign = Math.Sign(gridY - nextGridY);

                List<Tile> tilesToCheck = new List<Tile>();

                // this is a temp solution, which will work most of the time
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                    {
                        if (Utils.IsInRange(nextGridY + j, 0, levelRef.Tiles.GetLength(0))
                            && Utils.IsInRange(nextGridX + i, 0, levelRef.Tiles.GetLength(1)))
                            tilesToCheck.Add(levelRef.Tiles[nextGridY + j, nextGridX + i]);
                    }

                /* Sort the tiles based on their distance from the bullet's starting pos */
                tilesToCheck.Sort((t1, t2) =>
                    Vector2.Distance(new Vector2(gridX, gridY), t1.Position / Globals.TileSize).CompareTo(
                        Vector2.Distance(new Vector2(gridX, gridY), t2.Position / Globals.TileSize)));

                foreach (Tile tile in tilesToCheck)
                    tile.DebugDraw = true;

                Color[] colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple, Color.White, Color.Gray, Color.Black };
                int colorNum = 0;

                foreach (Tile tile in tilesToCheck)
                {
                    tile.DebugColor = colors[colorNum++];
                    tile.DebugDraw = true;

                    float angle;
                    tile.getIntersectionData(position, position + new Vector2(xSpeed, ySpeed), out intersectionPoint, out angle, out wallNormal);

                    if (intersectionPoint != null)
                    {
                        this.rotation = angle;
                        tile.DebugDraw = true;

                        tileCollided = tile;
                        bounces--;

                        break;
                    }
                }
            }

            /* Update the bullet's position based on its speed */
            if (intersectionPoint == null)
                position += new Vector2(xSpeed, ySpeed);
            else
            {
                position = Vector2.Lerp(position, (Vector2)intersectionPoint, 0.2f);

                if (wallNormal > 0 && wallNormal < 180)
                    position.Y += 3f;

                else if (wallNormal > 180 && wallNormal < 360)
                    position.Y -= 3f;

                else if (wallNormal > 90 && wallNormal < 270)
                    position.X -= 3f;

                else if (wallNormal > 270 || wallNormal < 90)
                    position.X += 3f;
            }

            if (bounces <= 0)
            {
                levelRef.InstantiateExplosion(position, wallNormal);
                destroyed = true;
                visible = false;
            }

            base.Update(gameTime);
        }        

        #endregion
    }
}
