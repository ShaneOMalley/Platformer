using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Components
{
    public class EntityData
    {
        #region Field Region

        public int id;

        public int positionX;
        public int positionY;

        #endregion

        #region Constructor Region

        public EntityData(int id, int positionX, int positionY)
        {
            this.id = id;
            this.positionX = positionX;
            this.positionY = positionY;
        }

        public EntityData()
        {
        }

        #endregion
    }
}
