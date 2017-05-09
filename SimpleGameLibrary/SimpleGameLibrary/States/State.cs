using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using SimpleGameLibrary.Components;
using SimpleGameLibrary.Drawing;

namespace SimpleGameLibrary.States
{
    public abstract class State
    {
        #region Field Region

        /* A reference to the state's state manager */
        protected StateManager stateManager;

        /* A list of the state's components */
        protected List<Component> components;

        /* The color to clear the screen with when this state is active */
        protected Color backgroundColor = Color.MistyRose;

        /* The camera to use when drawing */
        protected Camera camera;

        #endregion

        #region Property Region

        public List<Component> Components
        {
            get { return components; }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
        }

        public Camera Camera
        {
            get { return camera; }
        }

        public StateManager StateManager
        {
            get { return stateManager; }
            set { stateManager = value; }
        }

        #endregion

        #region Constructor Region

        /* State constructor */
        public State()
        {
            /* Initialize the components list */
            components = new List<Component>();

            /* Set up the state */
            LoadContent();
            Initialize();
        }

        #endregion

        #region Virtual Method region

        /* Load content and initialize the state */
        protected abstract void LoadContent();
        protected abstract void Initialize();

        /* Reset the state (reset entity poisions, score, etc.) */
        public abstract void Reset();

        /* Events which happen when the state is entered or left */
        public abstract void OnEnter(string args);
        public abstract void OnLeave();
        
        public virtual void Update(GameTime gameTime)
        {
            /* Update each of the state's components */
            //foreach (Component component in components)
            //    component.Update(gameTime);
            for (int i = 0; i < components.Count; i++)
            {
                Component c = components[i];
                
                if (c.Destroyed)
                {                    
                    components.RemoveAt(i);
                    i--;
                    continue;
                }

                c.Update(gameTime);
            }

            /* Update the camera */
            if (camera != null)
                camera.Update(gameTime);
        }

        /* Draw each of the state's components */
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Component component in components)
                component.Draw(gameTime, spriteBatch);
        }

        #endregion
    }
}
