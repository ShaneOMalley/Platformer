using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SimpleGameLibrary.Components;
using SimpleGameLibrary.Sprites;

using Platformer.Levels;

namespace Platformer.Components
{
    class EntityExplosion : Entity
    {
        #region Field Region

        private float direction;

        #endregion

        #region Property Region

        public float EffectiveRadius
        {
            get { return Size.X * 0.75f; }
        }

        public float Direction
        {
            get { return direction; }
        }

        #endregion

        #region Constructor Region

        public EntityExplosion(Vector2 position, float size, float direction, Level levelRef)
            : base(position, new Vector2(size), (Sprite)GameContent.ExplosionSprite.Clone(), new Vector2(16), 0, 1)
        {
            this.direction = direction;

            sprite.OnAnimComplete += new EventHandler(ExplosionAnimEnded);
        }

        private void ExplosionAnimEnded(object sender, EventArgs e)
        {
            destroyed = true;
            visible = false;
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region
        #endregion
    }
}
