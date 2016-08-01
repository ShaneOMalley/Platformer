using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Components
{
    public class IndividualTileData
    {
        #region Field Region

        public int id;
        public bool flipHorizontal;

        #endregion

        #region Constructor Region
        
        public IndividualTileData(int id, bool flipHorizontal)
        {
            this.id = id;
            this.flipHorizontal = flipHorizontal;
        }

        public IndividualTileData()
        {
        }

        #endregion
    }
}
