using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SimpleGameLibrary.States;
using SimpleGameLibrary.Controls;
using SimpleGameLibrary;

namespace Platformer.States
{
    class StateMainMenu : State
    {
        #region Field Region

        private SpriteFont font;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public StateMainMenu()
            : base()
        {
            backgroundColor = Color.DarkTurquoise;
        }

        #endregion

        #region Virtual Method region

        protected override void LoadContent()
        {
            /* Load the font */
            font = Program.game.Content.Load<SpriteFont>("fonts/arial");
        }

        protected override void Initialize()
        {
            /* Initialize the control manager */
            ControlManager controlManager = new ControlManager();            

            /* Initialize controls */
            ControlLabel lGreeting = new ControlLabel(
                "Please select the level you wish to play",
                new Vector2(Globals.WindowWidth * 0.5f, Globals.WindowHeight * 0.2f),
                true,
                font,
                Color.Black,
                Color.Red,
                new string[] { "", "", "", "" });
            controlManager.AddControl("lGreeting", lGreeting);

            string[] files = Directory.GetFiles("Data/levels", "*.som", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < files.Length; i++)
            {
                string name = "l" + i;
                string[] adj = new string[] { "", "", "", "" };

                if (files.Length == 1)
                {
                    adj[0] = "l0";
                    adj[1] = "l0";
                }
                else
                {
                    adj[0] = "l" + (i - 1);
                    adj[1] = "l" + (i + 1);
                }

                if (i == 0)
                {
                    adj[0] = "l" + (files.Length - 1);
                }
                else if (i == files.Length - 1)
                {
                    adj[1] = "l0";
                }

                string fileName = files[i].Split('\\')[1];
                string fileNameWithoutExtension = fileName.Substring(0, fileName.Length - 4);

                ControlLabel label = new ControlLabel(
                    fileNameWithoutExtension,
                    new Vector2(Globals.WindowWidth * 0.5f, Globals.WindowHeight * 0.2f + 50 + i * 20),
                    true,
                    font,
                    Color.Black,
                    Color.Red,
                    adj);
                label.Clicked += OnLevelSelect;

                controlManager.AddControl(name, label);
            }

            controlManager.SetSelectedControl("l0");

            /* Add the control manager component to the state's components */
            components.Add(controlManager);
        }

        private void OnLevelSelect(object sender, EventArgs e)
        {
            stateManager.GoToState("game", true, Directory.GetCurrentDirectory() + "/Data/levels/" + ((ControlLabel)sender).Text + ".som");
        }

        public override void OnEnter(string args) { }
        public override void OnLeave() { }
        public override void Reset(){ }

        public override void Update(GameTime gameTime)
        {
            /* Exiting the game */
            if (InputHandler.KeyPressed(Keys.Escape) || InputHandler.ButtonDown(Buttons.Back, PlayerIndex.One))
                Program.game.Exit();

            base.Update(gameTime);
        }

        #endregion
    }
}
