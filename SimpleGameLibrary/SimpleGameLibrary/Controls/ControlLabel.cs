using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleGameLibrary.Controls
{
    public class ControlLabel : Control
    {
        #region Field Region
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public ControlLabel(String text, Vector2 position, bool originCenter, SpriteFont spriteFont, Color defaultColor, Color selectedColor, string[] adjacentControls)
            : base(position, Vector2.Zero, null, spriteFont, defaultColor, selectedColor, adjacentControls)
        {
            this.text = text;

            this.size = spriteFont.MeasureString(text);
            if (originCenter)
                this.origin = new Vector2(0.5f, 0f);
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, text, position - origin * size, hasFocus ? selectedColor : defaultColor);
        }

        #endregion
    }
}
