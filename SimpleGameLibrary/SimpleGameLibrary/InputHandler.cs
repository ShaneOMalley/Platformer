using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SimpleGameLibrary
{
    /* Each of these correspond to an action which the player can do, or user can input in a menu */
    public enum Actions
    {
        Up, Down, Left, Right, Jump, Shoot, Select
    }

    public static class InputHandler
    {
        #region Field Region

        static KeyboardState keyboardState;
        static KeyboardState lastKeyboardState;
        static GamePadState[] gamePadStates;
        static GamePadState[] lastGamePadStates;
        
        static Dictionary<Actions, object[]> keyMapping;

        #endregion

        #region Property Region

        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }

        public static KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }

        public static GamePadState[] GamePadStates
        {
            get { return gamePadStates; }
        }

        public static GamePadState[] LastGamePadStates
        {
            get { return lastGamePadStates; }
        }

        #endregion

        #region General Method Region

        public static void Initialize()
        {
            keyboardState = Keyboard.GetState();
            lastKeyboardState = Keyboard.GetState();

            Console.WriteLine(typeof(Keys));
            Console.WriteLine(typeof(Buttons));

            int numPlayers = Enum.GetValues(typeof(PlayerIndex)).Length;
            gamePadStates = new GamePadState[numPlayers];
            lastGamePadStates = new GamePadState[numPlayers];

            foreach (PlayerIndex playerIndex in Enum.GetValues(typeof(PlayerIndex)))
            {
                gamePadStates[(int)playerIndex] = GamePad.GetState(playerIndex);
                lastGamePadStates[(int)playerIndex] = GamePad.GetState(playerIndex);
            }

            keyMapping = new Dictionary<Actions, object[]>();
            keyMapping.Add(Actions.Up, new object[] { Keys.Up, Keys.W, Buttons.DPadUp, Buttons.LeftThumbstickUp });
            keyMapping.Add(Actions.Down, new object[] { Keys.Down, Keys.S, Buttons.DPadDown, Buttons.LeftThumbstickDown });
            keyMapping.Add(Actions.Left, new object[] { Keys.Left, Keys.A, Buttons.DPadLeft, Buttons.LeftThumbstickLeft });
            keyMapping.Add(Actions.Right, new object[] { Keys.Right, Keys.D, Buttons.DPadRight, Buttons.LeftThumbstickRight });
            keyMapping.Add(Actions.Jump, new object[] { Keys.Space, Buttons.A });
            keyMapping.Add(Actions.Shoot, new object[] { Keys.Enter, Buttons.RightShoulder });
            keyMapping.Add(Actions.Select, new object[] { Keys.Enter, Buttons.A });
        }

        public static void Update()
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            lastGamePadStates = (GamePadState[])gamePadStates.Clone();
            foreach (PlayerIndex playerIndex in Enum.GetValues(typeof(PlayerIndex)))
                gamePadStates[(int)playerIndex] = GamePad.GetState(playerIndex);
        }

        public static void Flush()
        {
            lastKeyboardState = keyboardState;
        }

        #endregion

        #region Keyboard Region

        public static bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) &&
                lastKeyboardState.IsKeyUp(key);
        }

        public static bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
                lastKeyboardState.IsKeyDown(key);
        }

        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        /* Returns true if any of the buttons associated with the action are down */
        public static bool ActionDown(Actions action, PlayerIndex player)
        {
            foreach (object obj in keyMapping[action])
            {
                if (obj.GetType() == typeof(Keys))
                {
                    if (KeyDown((Keys)obj))
                        return true;
                }

                else if (obj.GetType() == typeof(Buttons))
                {
                    if (ButtonDown((Buttons)obj, player))
                        return true;
                }
            }

            return false;
        }

        /* Returns true if any of the buttons associated with the action are pressed */
        public static bool ActionPressed(Actions action, PlayerIndex player)
        {
            foreach (object obj in keyMapping[action])
            {
                if (obj.GetType() == typeof(Keys))
                {
                    if (KeyPressed((Keys)obj))
                        return true;
                }

                else if (obj.GetType() == typeof(Buttons))
                {
                    if (ButtonPressed((Buttons)obj, player))
                        return true;
                }
            }

            return false;
        }

        #endregion

        #region GamePad Region

        public static bool ButtonPressed(Buttons button, PlayerIndex playerIndex)
        {
            return gamePadStates[(int)playerIndex].IsButtonDown(button) &&
                lastGamePadStates[(int)playerIndex].IsButtonUp(button);
        }

        public static bool ButtonReleased(Buttons button, PlayerIndex playerIndex)
        {
            return gamePadStates[(int)playerIndex].IsButtonUp(button) &&
                lastGamePadStates[(int)playerIndex].IsButtonDown(button);
        }

        public static bool ButtonDown(Buttons button, PlayerIndex playerIndex)
        {
            return gamePadStates[(int)playerIndex].IsButtonDown(button);
        }

        #endregion
    }
}
