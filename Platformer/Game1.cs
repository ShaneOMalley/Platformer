using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SimpleGameLibrary;
using SimpleGameLibrary.States;
using SimpleGameLibrary.Drawing;

using Platformer.States;
using Platformer.Components;


namespace Platformer
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public StateManager stateManager;

        /* The constructor */
        public Game1()
        {
            /* Initialize the Graphics Device Manager */
            graphics = new GraphicsDeviceManager(this);

            /* Set the window dimensions */
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;

            /* Point to the root directory for the game content */
            Content.RootDirectory = "Content";
        }

        /* Initializen any non-content related stuff here */
        protected override void Initialize()
        {
            TileDataManager.LoadData("Data/tile_data.txt");

            /* Initialize the state manager */
            stateManager = new StateManager();

            /* Add States */
            stateManager.AddState("blank", new BlankState());
            stateManager.AddState("game", new StateGame());
            stateManager.AddState("mainMenu", new StateMainMenu());
            stateManager.GoToState("mainMenu", false, "");

            /* Initialize input handler and debug drawer */
            InputHandler.Initialize();
            DebugDrawing.Initialize(graphics);

            base.Initialize();
        }

        /* Initialize content related stuff here */
        protected override void LoadContent()
        {
            /* Create a new SpriteBatch, which can be used to draw textures */
            spriteBatch = new SpriteBatch(GraphicsDevice);

            /* Load the game's content */
            GameContent.LoadContent(Content);
        }

        /* Unload the content here */
        protected override void UnloadContent()
        {
            GameContent.UnloadContent(Content);
        }

        /* Update the logic of the game */
        protected override void Update(GameTime gameTime)
        {
            /* Update the input handler and current state */
            InputHandler.Update();

            /* Update the game logic, but pause the game if 'R' is held, and step one frame if 'E' is pressed when 'R' is held */
            if (!InputHandler.KeyDown(Keys.R) || (InputHandler.KeyDown(Keys.R) && InputHandler.KeyPressed(Keys.E)))
                stateManager.Update(gameTime);

            /* toggle debug drawing mode */
            if (InputHandler.KeyPressed(Keys.T))
                Globals.DebugMode = !Globals.DebugMode;

            base.Update(gameTime);

            if (Window != null)
                Window.Title = Globals.nBullets + " Bullets on screen, " + gameTime.ElapsedGameTime.Milliseconds + "ms frame update";
        }

        /* Handle the drawing of the game */
        protected override void Draw(GameTime gameTime)
        {
            /* Clear the screen */
            GraphicsDevice.Clear(stateManager.CurrentState.BackgroundColor);

            Camera camera = stateManager.CurrentState.Camera;

            if (camera != null)
            {
                spriteBatch.Begin(
                SpriteSortMode.Deferred,
                null,
                SamplerState.PointClamp,
                null,
                null,
                null,
                camera.Transformation);
            }
            else
            {
                /* Draw the current state */
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            }

            stateManager.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
