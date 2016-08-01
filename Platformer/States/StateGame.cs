using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SimpleGameLibrary.States;
using SimpleGameLibrary.Drawing;
using SimpleGameLibrary.Components;
using SimpleGameLibrary;

using Platformer.Components;
using Platformer.Levels;

namespace Platformer.States
{
    class StateGame : State
    {
        #region Field Region 

        private Level level;
        private EntityPlayer player;

        private Texture2D tilesTexture;
        private Texture2D playerTexture;
        private string levelFile;

        #endregion

        #region Property Region

        public string LevelFile
        {
            get { return levelFile; }
            set { levelFile = value; }
        }

        #endregion

        #region Constructor Region

        public StateGame()
            : base()
        {
            backgroundColor = Color.LightSkyBlue;
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region

        protected override void LoadContent()
        {
            tilesTexture = Program.game.Content.Load<Texture2D>("images/terrain/terrain");
            playerTexture = Program.game.Content.Load<Texture2D>("images/entities/player");
        }

        private void SetUpLevel()
        {
            level = new Level(tilesTexture, levelFile);
        }

        private void SetUpPlayer()
        {
            /* Find the player's spawn point */
            Vector2 spawnPos = Vector2.Zero;
            foreach (Entity entity in level.Entities)
                if (entity.GetType() == typeof(EntityPlayerSpawn))
                {
                    spawnPos = entity.Position;
                    break;
                }

            /* Set up the player */
            player = new EntityPlayer(playerTexture, Vector2.Zero, new Vector2(26 * 3, 30 * 3), null, 100, level);
            player.Center = spawnPos;
            components.Add(player);
        }

        private void SetUpCamera()
        {
            /* Set up the camera */
            camera = new Camera(player, Globals.WindowWidth, Globals.WindowHeight, CameraMode.Lerp);
        }

        protected override void Initialize()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            level.Draw(gameTime, spriteBatch);

            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            /* Go back to main menu if the player wants to 'go back' */
            if (InputHandler.KeyPressed(Keys.Escape) || InputHandler.ButtonDown(Buttons.Back, PlayerIndex.One))
                stateManager.GoToState("mainMenu", true, "");

            level.Update(gameTime);

            base.Update(gameTime);
        }

        public override void OnEnter(string args)
        {
            levelFile = args;
        }

        public override void OnLeave() { }

        public override void Reset()
        {
            components.Remove(player);
            SetUpLevel();
            SetUpPlayer();
            SetUpCamera();
        }

        #endregion
    }
}
