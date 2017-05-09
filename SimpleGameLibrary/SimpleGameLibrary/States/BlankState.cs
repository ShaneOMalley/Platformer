using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace SimpleGameLibrary.States
{
    public class BlankState : State
    {
        #region Constructor Region

        public BlankState()
            : base()
        {
            backgroundColor = Color.CornflowerBlue;
        }

        #endregion

        #region Virtual Method region

        public override void OnEnter(string args)
        { }
        public override void OnLeave()
        { }
        public override void Reset()
        { }
        protected override void Initialize()
        { }
        protected override void LoadContent()
        { }

        #endregion
    }
}
