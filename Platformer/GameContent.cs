using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SimpleGameLibrary.Sprites;

namespace Platformer
{
    class GameContent
    {
        public static Texture2D PlayerTexture;
        public static Texture2D TerrainTexture;
        public static Texture2D RocketTexture;
        public static Texture2D ExplosionTexture;

        public static Sprite PlayerSprite;
        public static Sprite RocketSprite;
        public static Sprite ExplosionSprite;

        public static void LoadContent(ContentManager content)
        {
            /* Load the textures */
            PlayerTexture = content.Load<Texture2D>("images/entities/player");
            TerrainTexture = content.Load<Texture2D>("images/terrain/terrain");
            RocketTexture = content.Load<Texture2D>("images/entities/rocket");
            ExplosionTexture = content.Load<Texture2D>("images/entities/explosion");

            /* Create the sprites */
            PlayerSprite = new Sprite(PlayerTexture, 26, 30, 7);
            PlayerSprite.AddSequence("idle", 200, new int[] { 2, 0, 0, 0, 0, 0, 0, 1 }, 1);
            PlayerSprite.AddSequence("run", 140, new int[] { 3, 4, 5, 4 });
            PlayerSprite.AddSequence("jump", 20, new int[] { 6 });

            RocketSprite = new Sprite(RocketTexture, 7, 4, 1);
            RocketSprite.AddSequence("default", 1, new int[] { 0 });

            ExplosionSprite = new Sprite(ExplosionTexture, 32, 32, 4);
            ExplosionSprite.AddSequence("default", 80, new int[] { 0, 1, 2, 3 });
        }

        public static void UnloadContent(ContentManager content)
        {
            content.Unload();
        }
    }
}
