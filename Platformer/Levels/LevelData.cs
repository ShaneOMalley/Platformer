using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Platformer.Components;

namespace Platformer.Levels
{
    public class LevelData
    {
        #region Field Region

        public IndividualTileData[][] tiles;
        public List<EntityData> entities;

        #endregion

        #region Constructor Region

        public LevelData(IndividualTileData[][] tiles, List<EntityData> entities)
        {
            this.tiles = tiles;
            this.entities = entities;
        }

        public LevelData(int width, int height)
            : this (new IndividualTileData[width][], new List<EntityData>())
        {
            for (int w = 0; w < width; w++)
                tiles[w] = new IndividualTileData[height];
        }

        public LevelData()
        {
        }

        #region Method Region

        public void ResizeTiles(int width, int height)
        {
            IndividualTileData[][] newTiles = new IndividualTileData[width][];
            for (int w = 0; w < width; w++)
                newTiles[w] = new IndividualTileData[height];

            for (int i = 0; i < tiles.Length && i < width; i++)
            {
                for (int j = 0; j < tiles[0].Length && j < height; j++)
                {
                    newTiles[i][j] = tiles[i][j];
                }
            }

            tiles = newTiles;
        }

        public void ClearTiles()
        {
            for (int i = 0; i < tiles.Length; i++)
                for (int j = 0; j < tiles[0].Length; j++)
                {
                    tiles[i][j] = null;
                }
        }

        public void ClearEntities()
        {
            entities.Clear();
        }

        #endregion

        #endregion
    }
}
