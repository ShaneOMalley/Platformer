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
            Vector2 q1 = new Vector2(720, 480);
            Vector2 q2 = new Vector2(800, 480);
            Vector2 p1 = new Vector2(747.8922f, 485.6485f);
            Vector2 p2 = new Vector2(753.4494f, 479.8938f);
            //Vector2 p2 = new Vector2(5.557267f, -5.754719f);

            Console.WriteLine("Ya boobay, {0}", Utils.GetIntersection(p1, p2, q1, q2));

            /* Run the game */
            using (game = new Game1())
                game.Run();
        }
    }
#endif
}
