using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SimpleGameLibrary.Sprites;

namespace SimpleGameLibrary.Components
{
    public abstract class Component
    {
        #region Field Region
        
        /* If the component is "special", it does not have a sprite, position or dimensions */
        protected bool special;

        protected Vector2 position;
        protected Vector2 size;
        protected Vector2 origin;
        protected float rotation;

        protected Sprite sprite;
        protected bool flipHorizontal = false;
        protected bool flipVertical = false;

        // do not draw if this is false
        protected bool visible = true;

        // if this is true, this Component should be removed from the ICollection<Component> which contains it
        protected bool destroyed = false;

        #endregion

        #region Property Region

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 Size
        {
            get { return size; }
        }

        public Vector2 Center
        {
            get { return position + size / 2; }
            set { position = value - size / 2; }
        }

        public System.Drawing.RectangleF Rectangle
        {
            get { return new System.Drawing.RectangleF(position.X, position.Y, size.X, size.Y); }
        }

        public bool FlipHorizontal
        {
            get { return flipHorizontal; }
            set { flipHorizontal = value; }
        }

        public bool FlipVertical
        {
            get { return flipVertical; }
            set { flipVertical = value; }
        }

        public bool Destroyed
        {
            get { return destroyed; }
        }

        #endregion

        #region Constructor Region

        public Component(Vector2 position, Vector2 size, Sprite sprite, Vector2? origin, float rotation)
        {
            this.position = position;
            this.size = size;
            this.sprite = sprite;
            this.rotation = rotation;

            /* set default value for origin if not given */
            this.origin = origin != null ? (Vector2)origin : Vector2.Zero;

            this.special = false;
        }

        public Component()
        {
            this.special = true;
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region

        public virtual void Update(GameTime gameTime)
        {
            if (special || sprite == null)
                return;

            sprite.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime, spriteBatch, Color.White);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            if (special || sprite == null || !visible)
                return;

            /* Flip the sprite horizontally and/or vertically */
            SpriteEffects effects =
                (flipHorizontal ? SpriteEffects.FlipHorizontally : SpriteEffects.None) |
                (flipVertical ? SpriteEffects.FlipVertically : SpriteEffects.None);

            /* Draw the sprite */
            sprite.Draw(
                gameTime,
                spriteBatch,
                new Rectangle(position.ToPoint(), size.ToPoint()),
                origin,
                (float)MathHelper.ToRadians(rotation),
                effects);
        }

        #endregion
    }
}
