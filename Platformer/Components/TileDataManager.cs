using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Components
{
    public static class TileDataManager
    {
        /* frame time of the tile animations */
        private const int frameTime = 100;

        public static Dictionary<int, TileData> tileDataDictionary;

        public static void LoadData(string fileName)
        {
            tileDataDictionary = new Dictionary<int, TileData>();

            /* rawlines is all lines in the file including whitespace and comments */
            string[] rawLines = System.IO.File.ReadAllLines(fileName);

            /* lines are only lines which matter */
            List<string> lines = new List<string>();

            /* Filter out comments and whitespace */
            foreach (string line in rawLines)
                if (!line.StartsWith(";") && !line.Equals(""))
                    lines.Add(line);

            int tileId = 0;
            for (int lineNum = 0; lineNum < lines.Count; lineNum += 3)
            {
                int[] frames = Array.ConvertAll(lines[lineNum].Split(' '), int.Parse);
                int[] heightMask = new int[] { 0 };
                float startHeight = float.Parse(lines[lineNum + 1]);

                string[] other = lines[lineNum + 2].Split(' ');

                float angle = float.Parse(other[0]);
                bool solid = other[1].Equals("1");
                bool visible = other[2].Equals("1");
                bool oneWay = other[3].Equals("1");

                TileData td = new TileData(frames, heightMask, startHeight, angle, solid, visible, oneWay);
                tileDataDictionary.Add(tileId++, td);
            }
        }
    }
}
