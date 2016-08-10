using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    public static class Globals
    {
        /* If this is on, do debug drawing etc. */
        public static bool DebugMode = false;

        /* Window Dimensions */
        public static int WindowWidth = 1366;
        public static int WindowHeight = 768;

        /* Size of tiles, will be referenced by the game and the editor */
        public static int TileSize = 80;

        public static int nBullets = 0;
        public static int BullShit = 0;
    }
}
