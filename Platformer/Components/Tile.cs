using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SimpleGameLibrary.Components;
using SimpleGameLibrary.Sprites;
using SimpleGameLibrary;

namespace Platformer.Components
{
    public class Tile : Component
    {
        #region Field Region

        protected int[] heightMask;
        protected float startHeight;
        protected float angle;
        protected bool solid;
        protected bool visible;
        protected bool oneWay;

        public bool DebugDraw = false;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public Tile(Vector2 position, Vector2 size, Sprite sprite, int[] heightMask, float startHeight, float angle, bool solid, bool visible, bool oneWay)
            : base(position, size, sprite, null, 0)
        {
            this.heightMask = (int[])heightMask.Clone();
            this.startHeight = startHeight;
            this.angle = angle;
            this.solid = solid;
            this.visible = visible;
            this.oneWay = oneWay;
        }

        public Tile(int gridX, int gridY, int tileSize, Sprite sprite, int[] heightMask, float startHeight, int angle, bool solid, bool visible, bool oneWay)
            : this(new Vector2(gridX, gridY), new Vector2(tileSize), sprite, heightMask, startHeight, angle, solid, visible, oneWay)
        {
        }

        public Tile(int gridX, int gridY, Sprite sprite, TileData tileData)
            : this(new Vector2(gridX, gridY) * Globals.TileSize, new Vector2(Globals.TileSize), sprite, tileData.heightMask, tileData.startHeight, tileData.angle, tileData.solid, tileData.visible, tileData.oneWay)
        {

        }

        #endregion

        #region Method Region

        public void ReverseHeightMask()
        {
            Array.Reverse(heightMask);
        }

        public void ReverseAngle()
        {
            angle = 360 - angle;
        }

        /* Changes the tile's properties to be its horizontally flipped counterpart */
        public void Flip()
        {
            flipHorizontal = !flipHorizontal;
            startHeight = startHeight - (float)Math.Tan(MathHelper.ToRadians(angle)) * 1;
            angle = 360 - angle;
        }

        /* Returns true if the rectangle given intersects with this, taking into account the tile's heightMask */
        public bool CollidesOLD(Rectangle rect)
        {
            /* return false if this tile is not solid */
            if (!solid)
                return false;

            /* return false if the rectangle is completely outside the tile */
            if (!rect.Intersects(Rectangle))
                return false;

            float sizeIncrease = size.X / sprite.CellWidth;

            int h1, h2;
            if (rect.X > position.X && rect.X < position.X + size.X)
                h1 = heightMask[(int)((rect.X - position.X) * (1 / sizeIncrease))];
            else
                h1 = -1;

            if (rect.X + rect.Width > position.X && rect.X + rect.Width < position.X + size.X)
                h2 = heightMask[(int)((rect.X + rect.Width - position.X) * (1 / sizeIncrease))];
            else
                h2 = -1;

            if (h1 != -1 && ((rect.Y + rect.Height) % size.Y) * (1 / sizeIncrease) > (size.Y * (1 / sizeIncrease) - h1))
                return true;

            if (h2 != -1 && ((rect.Y + rect.Height) % size.Y) * (1 / sizeIncrease) > (size.Y * (1 / sizeIncrease) - h2))
                return true;
            
            return false;
        }

        /* Returns the height of the ground at the given x position, for use in collision methods below */
        private float HeightAtX(float x)
        {
            return (float)(startHeight * size.Y - x * Math.Tan(MathHelper.ToRadians(angle)));
        }

        /* Returns true if the tile collides with the rectangle */
        public bool Collides(Rectangle rect)
        {
            if (!solid || !Rectangle.Intersects(rect))
                return false;

            /* check collision at left wall */
            // the x position of the left wall, relative to the tile */
            float x = rect.Left - position.X;

            // the distance from the top of the tile to the floor
            float h = Size.Y - HeightAtX(x);

            float y1 = rect.Top - Rectangle.Top;
            float y2 = rect.Bottom - Rectangle.Top;

            if ((h >= y1 && h <= y2) || (y1 > h && y1 < size.Y))
                return true;            

            /* check collision at right wall */
            // the x position of the left wall, relative to the tile */
            x = rect.Right - position.X;

            // the distance from the top of the tile to the floor
            h = Size.Y - HeightAtX(x);

            if ((h > y1 && h < y2) || (y1 > h && y1 < size.Y))
                return true;

            return false;
        }

        public float PointCollision(Vector2 point)
        {
            if (!solid || !Rectangle.Contains(point))
                return -1;

            /* The x position of the point relative to the tiles X position */
            float dx = point.X % size.X;

            /* The height of the ground at the point's relative X position (at position.X + dx) */
            float h = HeightAtX(dx);

            float dy = point.Y % size.Y;

            return -(size.Y - dy - h);
        }

        #endregion

        #region Virtual Method region

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            
            /* highlight if needed, and if debug drawing mode is enabled */
            if (Globals.DebugMode && DebugDraw)
                DebugDrawing.DrawRectangle(spriteBatch, Rectangle, Color.Purple, 0.5f);
        }

        #endregion
    }
}
