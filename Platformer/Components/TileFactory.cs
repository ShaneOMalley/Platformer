using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SimpleGameLibrary.Sprites;

namespace Platformer.Components
{
    public class TileFactory
    {
        #region Field Region

        private Texture2D tileSheet;
        private int spriteCellSize;
        private int tileSize;
        int[][] frames;
        int[][] heightMasks;
        Sprite[] sprites;

        #endregion

        #region Constructor Region

        public TileFactory(Texture2D texture, int spriteCellSize, int tileSize, int numSprites, int[] frameTimes, int[][] frames, int[][] heightMasks)
        {
            this.tileSheet = texture;
            this.spriteCellSize = spriteCellSize;
            this.tileSize = tileSize;
            this.frames = frames;
            this.heightMasks = heightMasks;

            sprites = new Sprite[numSprites];

            for (int i = 0; i < numSprites; i++)
            {
                sprites[i] = new Sprite(tileSheet, tileSize, tileSize, frames[i].Length);
                sprites[i].AddSequence("default", frameTimes[i], frames[i]);
            }
        }

        #endregion

        #region Method Region

        public Tile CreateTile(int gridX, int gridY, int tileId)
        {
            //return new Tile(new Vector2(gridX, gridY) * tileSize, new Vector2(tileSize), sprites[tileId], heightMasks[tileId]);
            return null;
        }

        #endregion

        #region Virtual Method region
        #endregion
    }
}
