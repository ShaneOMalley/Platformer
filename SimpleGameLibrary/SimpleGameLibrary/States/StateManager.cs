using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleGameLibrary.States
{
    public class StateManager
    {
        #region Field Region

        protected Dictionary<string, State> states;
        protected State currentState;

        #endregion

        #region Property Regions

        public Dictionary<string, State> States
        {
            get { return states; }
        }

        public State CurrentState
        {
            get { return currentState; }
        }

        #endregion

        #region Constructor Region

        public StateManager()
        {
            states = new Dictionary<string, State>();
        }

        #endregion

        #region Method Region

        /* Add a state to the dictionary */
        public void AddState(string name, State state)
        {
            /* Add the state */
            states.Add(name, state);

            /* Set this to be the state's state manager */
            state.StateManager = this;

            /* If there is no current state, set it to be the state which was just added */
            if (currentState == null)
                currentState = state;
        }

        /* Change the current state and reset it told to */
        public void GoToState(string name, bool reset, string args)
        {
            /* Do not change state if the state given is the current state */
            if (currentState == states[name])
                return;

            /* Change the state */
            currentState.OnLeave();
            currentState = states[name];
            currentState.OnEnter(args);

            /* Reset the new state if told to */
            if (reset)
                currentState.Reset();
        }

        /* Update the current state */
        public void Update(GameTime gameTime)
        {
            currentState.Update(gameTime);
        }

        /* Draw the current state */
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            currentState.Draw(gameTime, spriteBatch);
        }

        #endregion
    }
}
