using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SimpleGameLibrary.Components;
using SimpleGameLibrary.Sprites;

namespace SimpleGameLibrary.Controls
{
    public abstract class Control : Component
    {
        #region Field Region

        protected string text;
        protected SpriteFont spriteFont;
        protected Color defaultColor;
        protected Color selectedColor;
        protected bool hasFocus;
        protected bool enabled;

        protected ControlManager controlManager;

        /* This will have 4 values (0 = up, 1 = down, 2 = left, 3 = right) */
        protected string[] adjacentControls;

        #endregion

        #region Event Region

        public EventHandler Clicked;

        #endregion

        #region Property Region

        public string[] AdjacentControls
        {
            get { return adjacentControls; }
        }

        public bool HasFocus
        {
            get { return hasFocus; }
            set { hasFocus = value; }
        }

        public string Text
        {
            get { return text; }
        }

        #endregion

        #region Constructor Region

        public Control(Vector2 position, Vector2 size, Sprite sprite, SpriteFont spriteFont, Color defaultColor, Color selectedColor, string[] adjacentControls)
            : base(position, size, sprite, null, 0)
        {
            this.spriteFont = spriteFont;
            this.defaultColor = defaultColor;
            this.selectedColor = selectedColor;
            this.adjacentControls = adjacentControls;
        }

        #endregion

        #region Method Region

        public virtual void HandleInput()
        {
            if (InputHandler.ActionPressed(Actions.Select, PlayerIndex.One))
                if (Clicked != null)
                    Clicked(this, null);
        }

        #endregion

        #region Virtual Method region

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch, hasFocus ? selectedColor : defaultColor);

            spriteBatch.DrawString(spriteFont, text, position - origin * size, Color.Black);
        }

        #endregion

    }
}
