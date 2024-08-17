using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameDev.Core.Animations;
using GameDev.Core.Interfaces;
using GameDev.Core.Managers;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameDev.Core
{
    public class Wizard : IGameObject, IMovable
    {
        Texture2D wizardRunTexture;
        Texture2D wizardIdleTexture;
        Animation currentAnimation;
        Animation[] animations;

        MovementManager playerMovementManager;
        private MapManager mapManager;

        public Vector2 Position { get; set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 Direction { get; private set; }
        public Rectangle Hitbox { get; private set; }

        public IInputReader InputReader { get; set; }

        private bool isFacingRight = true;

        private float verticalSpeed;
        private bool isGrounded;
        private float gravity = 0.5f;
        private float jumpStrength = 12f;

        public Wizard(Texture2D runTexture, Texture2D idleTexture, IInputReader inputReader, MovementManager movementManager, MapManager mapManager)
        {
            wizardRunTexture = runTexture;
            wizardIdleTexture = idleTexture;
            InputReader = inputReader;
            playerMovementManager = movementManager;
            this.mapManager = mapManager;

            Acceleration = new Vector2(0.1f, 0.1f);
            Position = new Vector2(1, 1);
            Speed = new Vector2(2, 2);

            animations = new Animation[]
            {
                new Animation(), // run
                new Animation() // idle
            };

            animations[0].AddAnimation(6, 32, 32);
            animations[1].AddAnimation(4, 32, 32);
            currentAnimation = animations[1];
        }


        public void Update(GameTime gameTime)
        {
            Move();
            ApplyGravity();
            currentAnimation.Update(gameTime);
            UpdateHitbox();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D currentTexture = currentAnimation == animations[0] ? wizardRunTexture : wizardIdleTexture;
            SpriteEffects spriteEffects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Vector2 drawPosition = Position;
            spriteBatch.Draw(currentTexture, drawPosition, currentAnimation.currentFrame.sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);

            /*// Hitbox tekenen voor testing
            Texture2D hitboxTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            hitboxTexture.SetData(new[] { Color.White });
            spriteBatch.Draw(hitboxTexture, Hitbox, Color.Red * 0.5f);

            // Tiles (hitbox) tekenen voor testing
            foreach (var collider in mapManager.Colliders)
            {
                spriteBatch.Draw(hitboxTexture, collider, Color.Green * 0.5f);
            }*/
        }


        private void Move()
        {
            Vector2 previousPosition = Position;

            Direction = InputReader.ReadInput();
            playerMovementManager.Move(this);
            bool collisionDetected = false;

            if (Direction != Vector2.Zero)
            {
                currentAnimation = animations[0];
                isFacingRight = Direction.X > 0;

                Rectangle futureHitbox = new Rectangle((int)(Position.X + Direction.X * Speed.X), (int)Position.Y, Hitbox.Width, Hitbox.Height);

                foreach (var collider in mapManager.Colliders)
                {
                    if (futureHitbox.Intersects(collider))
                    {
                        Position = new Vector2(previousPosition.X, Position.Y);
                        collisionDetected = true;
                        break;
                    }
                }

                if (!collisionDetected)
                {
                    Position = new Vector2(Position.X + Direction.X * Speed.X, Position.Y);
                }
            }
            else
            {
                currentAnimation = animations[1];
            }

            if (isGrounded && (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up)))
            {
                verticalSpeed = -jumpStrength;
                isGrounded = false;
            }
        }

        private void ApplyGravity()
        {
            if (!isGrounded)
            {
                verticalSpeed += gravity;
            }

            Vector2 futurePosition = new Vector2(Position.X, Position.Y + verticalSpeed);
            Rectangle futureHitbox = new Rectangle((int)futurePosition.X, (int)futurePosition.Y, Hitbox.Width, Hitbox.Height);

            bool collisionDetected = false;

            foreach (var collider in mapManager.Colliders)
            {
                if (futureHitbox.Intersects(collider))
                {
                    if (verticalSpeed > 0)
                    {
                        Position = new Vector2(Position.X, collider.Top - Hitbox.Height);
                        verticalSpeed = 0;
                        isGrounded = true;
                    }
                    else if (verticalSpeed < 0)
                    {
                        Position = new Vector2(Position.X, collider.Bottom);
                        verticalSpeed = 0;
                    }
                    collisionDetected = true;
                    break;
                }
            }

            if (!collisionDetected)
            {
                Position = futurePosition;
                isGrounded = false;
            }
        }
        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 25, 32);
        }
    }
}
