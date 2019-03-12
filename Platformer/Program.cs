using System;
using SimpleGameLibrary;
using Microsoft.Xna.Framework;

namespace Platformer
{
#if WINDOWS || LINUX

    public static class Program
    {
        /* A static instance of Game1 to be used by anywhere */
        public static Game1 game;

        [STAThread]
        static void Main()
        {
            /* Run the game */
            using (game = new Game1())
                game.Run();
        }
    }
#endif
}
