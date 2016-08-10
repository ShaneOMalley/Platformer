﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SimpleGameLibrary;
using SimpleGameLibrary.Components;
using SimpleGameLibrary.Sprites;

using Platformer.Levels;

namespace Platformer.Components
{
    enum Direction
    {
        Left, Right
    }

    class EntityPlayer : Entity
    {
        #region Field Region

        private Level levelRef;
        
        private float xSpeed = 0;
        private float ySpeed = 0;
        private const float topSpeed = 6;
        private const float acceleration = 1f;
        private const float deceleration = 1.5f;
        private const float decelerationAir = 0.25f;
        private const float gravity = 0.75f;
        private const float terminalVelocity = 12f;
        private const float jumpPower = 24f;

        private const float rocketSpeed = 20f;
        private const float shootKnockBack = 12f;
        private const int shootCoolDownMax = 1;
        private int shootCoolDown = 0;

        private bool moving = false;
        private bool grounded = false;
        private bool groundedPrevious = false;
        private Direction direction = Direction.Right;

        private Color debugColor = Color.Red;

        #endregion

        #region Property Region

        public Vector2 BoundPosition
        {
            get
            {
                //if (!grounded)
                //    return position + new Vector2(7, 0 + 2f) * 3;
                //else
                    return position + new Vector2(7, 8f) * 3;
            }
        }

        public Vector2 BoundSize
        {
            get
            {
                //if (!grounded)
                //    return new Vector2(12, 30 - 4f*2) * 3;
                //else
                    return new Vector2(12, 24 - 8f) * 3;
            }
        }


        /* The position of the bounds after movement by speed */
        public Vector2 ProjectedBounds
        {
            get { return BoundPosition + new Vector2((int)xSpeed, (int)ySpeed); }
        }

        public Vector2 GroundCollisionLeft
        {
            get { return position + new Vector2(9, 30 - 0.1f) * 3; }
        }

        public Vector2 GroundCollisionRight
        {
            get { return position + new Vector2(17, 30 - 0.1f) * 3; }
        }

        public Vector2 CeilingCollisionLeft
        {
            get
            {
                if (!grounded && false)
                    return position + new Vector2(9, 0) * 3;
                return position + new Vector2(9, 5) * 3;
            }
        }

        public Vector2 CeilingCollisionRight
        {
            get
            {
                if (!grounded && false)
                    return position + new Vector2(17, 0) * 3;
                return position + new Vector2(17, 5) * 3;
            }
        }

        #endregion

        #region Constructor Region

        public EntityPlayer(Texture2D texture, Vector2 position, Vector2 size, Vector2? origin, int maxHp, Level levelRef)
            : base(position, size, null, origin, 0f, maxHp)
        {
            this.levelRef = levelRef;

            SetupSprite(texture);
        }

        #endregion

        #region Method Region

        public void SetupSprite(Texture2D texture)
        {
            sprite = new Sprite(texture, 26, 30, 7);
            sprite.AddSequence("idle", 200, new int[] { 2, 0, 0, 0, 0, 0, 0, 1 }, 1);
            sprite.AddSequence("run", 140, new int[] { 3, 4, 5, 4 });
            sprite.AddSequence("jump", 20, new int[] { 6 });
        }

        #endregion

        #region Virtual Method region

        public override void Update(GameTime gameTime)
        {
            bool left = InputHandler.ActionDown(Actions.Left, PlayerIndex.One);
            bool right = InputHandler.ActionDown(Actions.Right, PlayerIndex.One);
            bool jump = InputHandler.ActionPressed(Actions.Jump, PlayerIndex.One);
            bool shoot = InputHandler.ActionPressed(Actions.Shoot, PlayerIndex.One);

            // reset position
            if (InputHandler.KeyPressed(Keys.Q))
            {
                if (levelRef.PlayerSpawn != null)
                    Center = levelRef.PlayerSpawn.Position;
                else
                    Center = Vector2.Zero;
            }

            /* Player Movement with Acceleration and Deceleration */
            /* Right */
            if (right && !left)
            {
                /* If already moving right accelerate */
                if (xSpeed >= 0)
                    xSpeed += acceleration;
                /* ... otherwise, decelerate */
                else
                    xSpeed += deceleration;

                /* Limit the player's speed */
                if (xSpeed > topSpeed)
                    xSpeed = topSpeed;

                direction = Direction.Right;
                moving = true;
            }
            /* Left */
            else if (left && !right)
            {
                /* If already moving left, accelerate */
                if (xSpeed <= 0)
                    xSpeed -= acceleration;
                /* ... otherwise, decelerate */
                else
                    xSpeed -= deceleration;

                /* Limit the player's speed */
                if (xSpeed < -topSpeed)
                    xSpeed = -topSpeed;

                direction = Direction.Left;
                moving = true;
            }
            /* Not moving */
            else
            {
                int sign = Math.Sign(xSpeed);

                /* decelerate the player */
                xSpeed -= (grounded ? deceleration : decelerationAir) * sign;

                /* set speed to zero to when he decelerated to a halt */
                if (Math.Sign(xSpeed) != sign)
                    xSpeed = 0;

                moving = false;
            }

            /* gravity */
            if (groundedPrevious && !grounded && ySpeed > 0)
                ySpeed = 0;

            if (!grounded || shoot)
            {
                ySpeed += gravity;
                if (ySpeed > terminalVelocity)
                    ySpeed = terminalVelocity;
            }
            else
            {
                ySpeed += gravity * 10f;
            }

            /* Jumping */
            if (jump && grounded)
            {
                ySpeed -= jumpPower;
            }

            /* Collision Detection */
            /* by default, do not highlight each tile in debug drawing mode */
            //foreach (Tile tile in levelRef.Tiles)
            //    tile.DebugDraw = false;

            /* The list of tiles to check for collision with */
            HashSet<Tile> tilesToCheck = new HashSet<Tile>();

            /* The tile where the center of the player is */
            int tileX = (int)(Center.X / Globals.TileSize);
            int tileY = (int)(Center.Y / Globals.TileSize);

            /* Add the surrounding 9 tiles to the tiles to check */
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    tilesToCheck.Add(
                        levelRef.Tiles[
                            MathHelper.Clamp(tileY + j, 0, levelRef.Tiles.GetLength(0) -1),
                            MathHelper.Clamp(tileX + i, 0, levelRef.Tiles.GetLength(1) -1)
                            ]);
                }

            groundedPrevious = grounded;
            grounded = false;

            bool colFloor = false;
            bool colCeil = false;
            bool colWall = false;

            for (int pass = 0; pass < 2; ++pass)
            {
                foreach (Tile tile in tilesToCheck)
                {
                    if (Math.Abs(ySpeed) > Math.Abs(xSpeed))
                    {
                        if (ySpeed > -5.5f)
                            colFloor = HandleFloorCollision(tile) || colFloor;
                        if (ySpeed < 5.5f)
                            colCeil = HandleCeilingCollision(tile) || colCeil;
                        colWall = HandleWallCollision(tile) || colWall;
                    }
                    else
                    {
                        colWall = HandleWallCollision(tile) || colWall;
                        if (ySpeed > -5.5f)
                            colFloor = HandleFloorCollision(tile) || colFloor;
                        if (ySpeed < 5.5f)
                            colCeil = HandleCeilingCollision(tile) || colCeil;
                    }
                }
            }

            /* Update the player's position based on his speed */
            position += new Vector2(xSpeed, ySpeed);

            debugColor = (colFloor || colWall || colCeil) ? Color.Blue : Color.Red;

            /* Set the player's animation sequence */
            if ((!grounded && Math.Abs(ySpeed) > terminalVelocity * 0.2f)
                || (!grounded && sprite.GetSequence().Equals("jump")))
                sprite.SetSequence("jump");
            else
            {
                if (moving)
                    sprite.SetSequence("run");
                else
                    sprite.SetSequence("idle");
            }

            /* Set the player sprite's direction */
            flipHorizontal = (direction == Direction.Left);

            /* Shooting */
            if (shoot && shootCoolDown == 0)
            {
                Vector2 rightStick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;

                float rocketAng = (float)MathHelper.ToDegrees((float)Math.Atan2(-rightStick.Y, rightStick.X));
                if (rocketAng < 0) rocketAng += 360;

                //rocketAng = (int)(rocketAng / 45) * 45f;

                /* knock the player back */
                if (rocketAng >= 40 && rocketAng <= 140)
                {
                    xSpeed += -(float)Math.Cos(MathHelper.ToRadians(rocketAng)) * shootKnockBack;
                    ySpeed = -(float)Math.Sin(MathHelper.ToRadians(rocketAng)) * shootKnockBack;

                    xSpeed = MathHelper.Clamp(xSpeed, -shootKnockBack, shootKnockBack);
                    ySpeed = MathHelper.Clamp(ySpeed, -shootKnockBack, shootKnockBack);
                }

                levelRef.Entities.Add(new EntityRocket(Center, rocketAng, rocketSpeed, 3, levelRef));

                grounded = false;
                shootCoolDown = shootCoolDownMax;
            }

            shootCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            if (shootCoolDown < 0)
                shootCoolDown = 0;

            base.Update(gameTime);
        }

        private bool HandleWallCollision(Tile tile)
        {
            /* dont handle wall collision if the player is already in the tile */
            if (tile.Rectangle.Intersects(new Rectangle(BoundPosition.ToPoint(), BoundSize.ToPoint()))
                || tile.Rectangle.Contains(GroundCollisionLeft) || tile.Rectangle.Contains(GroundCollisionRight))
                return false;

            Rectangle wallCollisionRect = new Rectangle(ProjectedBounds.ToPoint(), BoundSize.ToPoint());
            if (grounded)
                //wallCollisionRect.Y = (int)(position.Y + (BoundPosition.Y - Position.Y));
                wallCollisionRect.Y = (int)BoundPosition.Y;

            if (tile.Collides(wallCollisionRect))
            {
                // if the player came from the right
                if (position.X > tile.Position.X)
                {
                    // place the player at the tile's right wall
                    position.X = tile.Position.X + tile.Size.X - (BoundPosition.X - position.X) + 0.1f;

                    // stop the player
                    xSpeed = 0;

                    //tile.DebugDraw = true;

                    // the player is colliding
                    return true;
                }
                // if the player came from the left
                else
                {
                    // place the player at the tile's left wall
                    position.X = tile.Position.X - size.X + (BoundPosition.X - position.X) - 0.1f;

                    // stop the player
                    xSpeed = 0;

                    //tile.DebugDraw = true;

                    // the player is colliding
                    return true;
                }
            }

            return false;
        }

        private bool HandleFloorCollision(Tile tile)
        {
            float collisionLeft = tile.PointCollision(GroundCollisionLeft + new Vector2(0, ySpeed));
            float collisionRight = tile.PointCollision(GroundCollisionRight + new Vector2(0, ySpeed));

            if (ySpeed >= -(terminalVelocity * 0.2f) || true)
            {
                if (collisionLeft > 0)
                {
                    // move the player to the top of the tile
                    position.Y += ySpeed - collisionLeft - 0.1f;
                    ySpeed = 0;
                    grounded = true;

                    // the player is colliding
                    return true;
                }

                else if (collisionRight > 0)
                {
                    // move the player to the top of the tile
                    position.Y += ySpeed - collisionRight - 0.1f;
                    ySpeed = 0;
                    grounded = true;

                    // the player is colliding
                    return true;
                }
            }

            return false;
        }

        private bool HandleCeilingCollision(Tile tile)
        {
            float collisionLeft = tile.PointCollision(CeilingCollisionLeft + new Vector2(0, ySpeed));
            float collisionRight = tile.PointCollision(CeilingCollisionRight + new Vector2(0, ySpeed));

            if (collisionLeft > 0 || collisionRight > 0)
            {
                position.Y = tile.Position.Y + tile.Size.Y - (CeilingCollisionLeft.Y - position.Y);
                ySpeed = Math.Max(0, ySpeed);

                return true;
            }

            return false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (Globals.DebugMode)
            {
                DebugDrawing.DrawRectangle(spriteBatch, new Rectangle(ProjectedBounds.ToPoint(), BoundSize.ToPoint()), Color.Yellow, 0.5f);

                DebugDrawing.DrawRectangle(spriteBatch, new Rectangle(BoundPosition.ToPoint(), BoundSize.ToPoint()), debugColor, 0.5f);

                Rectangle bottomLeft = new Rectangle(GroundCollisionLeft.ToPoint(), new Point(4));
                bottomLeft.Offset(-2, -2);

                Rectangle bottomRight = new Rectangle(GroundCollisionRight.ToPoint(), new Point(4));
                bottomRight.Offset(-2, -2);

                Rectangle topLeft = new Rectangle(CeilingCollisionLeft.ToPoint(), new Point(4));
                topLeft.Offset(-2, -2);

                Rectangle topRight = new Rectangle(CeilingCollisionRight.ToPoint(), new Point(4));
                topRight.Offset(-2, -2);

                DebugDrawing.DrawRectangle(spriteBatch, bottomLeft, Color.White, 1f);
                DebugDrawing.DrawRectangle(spriteBatch, bottomRight, Color.White, 1f);
                DebugDrawing.DrawRectangle(spriteBatch, topLeft, Color.White, 1f);
                DebugDrawing.DrawRectangle(spriteBatch, topRight, Color.White, 1f);
            }
        }

        #endregion
    }
}
