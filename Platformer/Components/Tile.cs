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

        protected float startHeight;
        protected float angle;
        protected bool solid;
        protected bool visible;
        protected bool oneWay;

        public bool DebugDraw = false;
        public Color DebugColor = Color.Purple;

        #endregion

        #region Property Region

        public Vector2 LeftWallTop
        {
            get { return position + new Vector2(0, (1 - startHeight) * size.Y); }
        }

        public Vector2 LeftWallBottom
        {
            get { return position + new Vector2(0, size.Y); }
        }

        public Vector2 RightWallTop
        {
            get
            {
                return position + new Vector2(
                    size.X,
                    (float)(1f - startHeight + Math.Tan(MathHelper.ToRadians(angle))) * size.Y
                    );
            }
        }

        public Vector2 RightWallBottom
        {
            get { return position + size; }
        }

        #endregion

        #region Constructor Region

        public Tile(Vector2 position, Vector2 size, Sprite sprite, float startHeight, float angle, bool solid, bool visible, bool oneWay)
            : base(position, size, sprite, null, 0)
        {
            this.startHeight = startHeight;
            this.angle = angle;
            this.solid = solid;
            this.visible = visible;
            this.oneWay = oneWay;
        }

        public Tile(int gridX, int gridY, int tileSize, Sprite sprite, float startHeight, int angle, bool solid, bool visible, bool oneWay)
            : this(new Vector2(gridX, gridY), new Vector2(tileSize), sprite, startHeight, angle, solid, visible, oneWay)
        {
        }

        public Tile(int gridX, int gridY, Sprite sprite, TileData tileData)
            : this(new Vector2(gridX, gridY) * Globals.TileSize, new Vector2(Globals.TileSize), sprite, tileData.startHeight, tileData.angle, tileData.solid, tileData.visible, tileData.oneWay)
        {

        }

        #endregion

        #region Method Region

        private void ReverseAngle()
        {
            angle = 360 - angle;
        }

        /* Changes the tile's properties to be its horizontally flipped counterpart */
        public void Flip()
        {
            flipHorizontal = !flipHorizontal;
            startHeight = startHeight - (float)Math.Tan(MathHelper.ToRadians(angle)) * 1;
            ReverseAngle();
        }

        /* Returns the height of the ground at the given x position, for use in collision methods below */
        private float HeightAtX(float x)
        {
            return (float)(startHeight * size.Y - x * Math.Tan(MathHelper.ToRadians(angle)));
        }

        /* Returns true if the tile collides with the rectangle */
        public bool Collides(System.Drawing.RectangleF rect)
        {
            if (!solid || !Rectangle.IntersectsWith(rect))
                return false;

            /* check collision at left wall */
            /* the x position of the left wall, relative to the tile */
            float x = rect.Left - position.X;

            // the distance from the top of the tile to the floor
            float h = Size.Y - HeightAtX(x);

            float y1 = rect.Top - Rectangle.Top;
            float y2 = rect.Bottom - Rectangle.Top;

            if ((h >= y1 && h <= y2) || (y1 > h && y1 < size.Y))
                return true;            

            /* check collision at right wall */
            /* the x position of the left wall, relative to the tile */
            x = rect.Right - position.X;

            // the distance from the top of the tile to the floor
            h = Size.Y - HeightAtX(x);

            if ((h > y1 && h < y2) || (y1 > h && y1 < size.Y))
                return true;

            return false;
        }

        /* Returns how deep the point is in the tile from the top of the tile at that x position */
        public float PointCollision(Vector2 point)
        {
            if (!solid || !Rectangle.Contains(point.X, point.Y))
                return -1;

            /* The x position of the point relative to the tiles X position */
            float dx = point.X % size.X;

            /* The height of the ground at the point's relative X position (at position.X + dx) */
            float h = HeightAtX(dx);

            float dy = point.Y % size.Y;

            return -(size.Y - dy - h);
        }

        /* Get the point of intersection, angle of reflection, and normal of the wall if |p1p2| intersects with this tile */
        public void getIntersectionData(Vector2 p1, Vector2 p2, out Vector2? poi, out float angle, out float wallNormal)
        {
            if (solid)
            {
                Vector2[,] sides = new Vector2[,]
                    {
                        { LeftWallBottom, LeftWallTop },
                        { RightWallBottom, LeftWallBottom },
                        { RightWallTop, RightWallBottom },
                        { LeftWallTop, RightWallTop},
                    };

                Vector2 ep1, ep2;
                Utils.Extend(p1, p2, out ep1, out ep2, 1.0f);

                float rayAng = Utils.AngleBetweenPoints(ep1, ep2);
                if (rayAng < 0)
                    rayAng += 360;

                for (int sideNo = 0; sideNo < sides.GetLength(0); sideNo++)
                {
                    float normalAng = (Utils.AngleBetweenPoints(sides[sideNo, 0], sides[sideNo, 1]) - 90) % 360;
                    if (normalAng < 0)
                        normalAng += 360;

                    /* only check for collision if the normal of the side opposes the angle of the ray */
                    if (Utils.AngleInRange(rayAng, normalAng - 90, normalAng + 90))
                        continue;

                    Vector2 extendedWallP1, extendedWallP2;
                    Utils.Extend(sides[sideNo, 0], sides[sideNo, 1], out extendedWallP1, out extendedWallP2, 1.0f);
                    
                    Vector2? intersection = Utils.GetIntersection(ep1, ep2, extendedWallP1, extendedWallP2);

                    if (intersection != null)
                    {
                        poi = (Vector2)intersection;
                        angle = Utils.GetAngle(ep1, ep2, extendedWallP1, extendedWallP2);
                        wallNormal = normalAng;
                        return;
                    }
                }
            }

            poi = null;
            angle = -1;
            wallNormal = -1;
        }

        #endregion

        #region Virtual Method region

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DebugDraw = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            
            /* highlight if needed, and if debug drawing mode is enabled */
            if (Globals.DebugMode && DebugDraw)
                DebugDrawing.DrawRectangle(spriteBatch, new Rectangle((int)Rectangle.X, (int)Rectangle.Y, (int)Rectangle.Width, (int)Rectangle.Height), DebugColor, 0.5f);
        }

        #endregion
    }
}
