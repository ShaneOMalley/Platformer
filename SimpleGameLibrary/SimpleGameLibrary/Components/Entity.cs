using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SimpleGameLibrary.Sprites;

namespace SimpleGameLibrary.Components
{
    public abstract class Entity : Component
    {
        #region Field Region

        protected int maxHp;
        protected int hp;
        
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public Entity(Vector2 position, Vector2 size, Sprite sprite, Vector2? origin, float rotation, int maxHp)
            : base(position, size, sprite, origin, rotation)
        {
            this.maxHp = maxHp;
            hp = maxHp;
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region
        #endregion
    }
}
