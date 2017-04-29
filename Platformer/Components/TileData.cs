using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Components
{
    public class TileData
    {
        #region Field Region

        public readonly int[] frames;
        public readonly float startHeight;
        public readonly float angle;
        public readonly bool solid;
        public readonly bool visible;
        public readonly bool oneWay;

        #endregion

        #region Constructor Region

        public TileData(int[] frames, float startHeight, float angle, bool solid, bool visible, bool oneWay)
        {
            this.frames = frames;
            this.startHeight = startHeight;
            this.angle = angle;
            this.solid = solid;
            this.visible = visible;
            this.oneWay = oneWay;
        }

        #endregion
    }
}
