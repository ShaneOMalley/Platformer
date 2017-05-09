using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SimpleGameLibrary.Components;

namespace SimpleGameLibrary.Controls
{
    public class ControlManager : Component
    {
        #region Field Region

        private Dictionary<string, Control> controls;
        private Control selectedControl;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public ControlManager()
            : base()
        {
            controls = new Dictionary<string, Control>();

            Console.WriteLine("special  = " + special);
        }

        #endregion

        #region Method Region

        public void AddControl(string name, Control control)
        {
            controls.Add(name, control);

            /* set the selected control to be the first one added */
            if (selectedControl == null)
            {
                selectedControl = control;
                selectedControl.HasFocus = true;
            }
        }

        public void SetSelectedControl(string name)
        {
            if (controls.ContainsKey(name))
            {
                selectedControl.HasFocus = false;
                selectedControl = controls[name];
                selectedControl.HasFocus = true;
            }
            else
                Console.WriteLine("Control \"{0}\" not found", name);
        }

        private void MoveSelection(int direction)
        {
            string nextControl = selectedControl.AdjacentControls[direction];

            /* Don't move if there is no adjacent control in that direction */
            if (nextControl.Equals(""))
                return;

            /* Move to the adjacent control if the name exists */
            if (controls.ContainsKey(nextControl))
            {
                selectedControl.HasFocus = false;
                selectedControl = controls[nextControl];
                selectedControl.HasFocus = true;
            }
            else
                Console.WriteLine("Control \"{0}\" not found", nextControl);
        }

        public override void Update(GameTime gameTime)
        {
            /* Update each of the controls */
            foreach (Control control in controls.Values)
                control.Update(gameTime);

            /* The keys / buttons to control moving between controls */
            bool up = InputHandler.KeyPressed(Keys.Up) || InputHandler.ButtonPressed(Buttons.DPadUp, PlayerIndex.One) || InputHandler.ButtonPressed(Buttons.LeftThumbstickUp, PlayerIndex.One);

            /* Handle input (moving between controls) */
            if (InputHandler.ActionPressed(Actions.Up, PlayerIndex.One))
                MoveSelection(0);
            else if (InputHandler.ActionPressed(Actions.Down, PlayerIndex.One))
                MoveSelection(1);
            else if (InputHandler.ActionPressed(Actions.Left, PlayerIndex.One))
                MoveSelection(2);
            else if (InputHandler.ActionPressed(Actions.Right, PlayerIndex.One))
                MoveSelection(3);

            /* Let selected control handle input */
            selectedControl.HandleInput();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            /* Draw each of the controls */
            foreach (Control control in controls.Values)
                control.Draw(gameTime, spriteBatch);
        }

        #endregion
    }
}
