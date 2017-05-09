using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleGameLibrary
{
    public static class DebugDrawing
    {
        #region Field Region

        private static Texture2D pixel;

        #endregion

        #region Method Region

        public static void Initialize(GraphicsDeviceManager graphics)
        {
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color, float alpha)
        {
            spriteBatch.Draw(pixel, rect, color * alpha);
        }

        #endregion
    }
}
