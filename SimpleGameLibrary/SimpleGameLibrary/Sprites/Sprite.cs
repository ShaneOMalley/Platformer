using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleGameLibrary.Sprites
{
    /* A Sequence defines the current animation (attacking, walking, idle etc.) */
    class Sequence
    {
        public readonly string name;
        public readonly int frameTime;
        public readonly int[] frames;
        public readonly int loopStart;

        public Sequence(string name, int frameTime, int[] frames, int loopStart = 0)
        {
            this.name = name;
            this.frameTime = frameTime;
            this.frames = frames;
            this.loopStart = loopStart;
        }
    }

    /* The Sprite class */
    public class Sprite : ICloneable
    {
        #region Field Region

        /* Sequence related fields */
        private Dictionary<string, Sequence> sequences;
        private Sequence currentSequence;
        private int currentFrame;
        private int frameTimeLeft;

        public float alpha;

        /* Texture related fields */
        private Rectangle[] sourceRectangles;
        private Texture2D texture;

        private EventHandler onAnimComplete;

        #endregion

        #region Field Region

        public int CellWidth
        {
            get { return sourceRectangles[0].Width; }
        }

        public int CellHeight
        {
            get { return sourceRectangles[0].Height; }
        }

        public EventHandler OnAnimComplete
        {
            get { return onAnimComplete; }
            set { onAnimComplete = value; }
        }

        #endregion

        #region Constructor Region

        /* The constructor for the sprite */
        public Sprite(Texture2D texture, int cellWidth, int cellHeight, int numFrames)
        {
            this.texture = texture;

            /* Set up the source rectangles for the textures given cell dimensions */
            sourceRectangles = new Rectangle[numFrames];

            int maxColumns = texture.Width / cellWidth;
            int maxRows = texture.Height / cellHeight;

            for (int frame = 0; frame < numFrames; ++frame)
            {
                int column = frame % maxColumns;
                int row = frame / maxColumns;

                /* Only add the source rect if it is inside the bounds of the texture */
                if (row < maxRows)
                    sourceRectangles[frame] = new Rectangle(column * cellWidth, row * cellHeight, cellWidth, cellHeight);
                else
                    break;
            }

            /* Initialize fields */
            sequences = new Dictionary<string, Sequence>();
            alpha = 1f;
        }

        /* empty constructor for cloning */
        private Sprite()
        {
        }

        #endregion

        #region Method Region

        /* Add a sequence to the dictionary of sequences */
        public void AddSequence(string name, int frameTime, int[] frames, int loopStart = 0)
        {
            Sequence sequence = new Sequence(name, frameTime, frames, loopStart);
            sequences.Add(name, sequence);

            /* Set the current sequence to be the first sequence to be added */
            if (currentSequence == null)
                currentSequence = sequence;
        }

        /* Set the current sequene */
        public void SetSequence(string name)
        {
            if (!sequences.ContainsKey(name))
            {
                Console.WriteLine("Sequence \"" + name + "\" not found");
                return;
            }

            /* do nothing if the sequence is the same */
            if (currentSequence.name == name)
                return;


            currentSequence = sequences[name];
            currentFrame = 0;
        }

        public string GetSequence()
        {
            return currentSequence.name;
        }

        private void AnimComplete()
        {
            if (onAnimComplete != null)
                OnAnimComplete(this, null);
        }

        /* Update the current frame of the sprite */
        public void Update(GameTime gameTime)
        {
            /* Store timePassed and frameTime in temp variables */
            int timePassed = gameTime.ElapsedGameTime.Milliseconds;
            int frameTime = currentSequence.frameTime;

            /* Advance to the next frame if needed */
            frameTimeLeft -= timePassed;
            if (frameTimeLeft <= 0)
            {
                /* This code will work accurately for small values of frameTime */
                int framesAdvanced = 1 + (-frameTimeLeft / frameTime);
                currentFrame += framesAdvanced;
                if (currentFrame >= currentSequence.frames.Length)
                {
                    AnimComplete();
                    currentFrame = currentSequence.loopStart;
                }

                /* This code is more accurate than setting "frameTimeLeft = frameTime" */
                frameTimeLeft = frameTime - (timePassed % frameTime);
            }
        }

        /* Draw the current frame of the sprite in the given position */
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destinationRectangle, Vector2 origin, float rotation, SpriteEffects effects)
        {
            Draw(gameTime, spriteBatch, destinationRectangle, origin, rotation, effects, Color.White);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destinationRectangle, Vector2 origin, float rotation, SpriteEffects effects, Color color)
        {
            if (currentSequence == null)
                Console.WriteLine("hhs");
            Rectangle sourceRectangle = sourceRectangles[currentSequence.frames[currentFrame]];

            spriteBatch.Draw(
                texture,
                destinationRectangle,
                sourceRectangle,
                color * alpha,
                rotation,
                origin,
                effects,
                0);
        }

        public object Clone()
        {
            Sprite result = new Sprite();

            result.sequences = new Dictionary<string, Sequence>();
            result.currentSequence = currentSequence;
            result.currentFrame = this.currentFrame;
            result.frameTimeLeft = this.frameTimeLeft;
            result.alpha = this.alpha;
            result.sourceRectangles = (Rectangle[])this.sourceRectangles.Clone();
            result.texture = this.texture;

            return result;
        }

        #endregion
    }
}
