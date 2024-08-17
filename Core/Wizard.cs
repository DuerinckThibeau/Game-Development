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

        public int Health { get; private set; }
        public bool isFlashing;
       
        private float flashTime;
        private float flashInterval = 0.5f; 
        private float flashTimer;

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

            Health = 3;
            isFlashing = false;
            flashTime = 0f;
            flashTimer = 0f;
        }

        public void Update(GameTime gameTime)
        {
            Move();
            ApplyGravity();
            UpdateFlashing(gameTime);
            currentAnimation.Update(gameTime);
            UpdateHitbox();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D currentTexture = currentAnimation == animations[0] ? wizardRunTexture : wizardIdleTexture;
            SpriteEffects spriteEffects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Vector2 drawPosition = Position;
            Color drawColor = Color.White;

            if (isFlashing)
            {
                if (flashTimer % flashInterval < flashInterval / 2)
                {
                    drawColor = Color.Black;
                }
                else
                {
                    drawColor = Color.White; 
                }
            }

            spriteBatch.Draw(currentTexture, drawPosition, currentAnimation.currentFrame.sourceRectangle, drawColor, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
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

        public void TakeDamage()
        {
            if (isFlashing) return;

            Health--;
            isFlashing = true;
            flashTime = 3f; 
            flashTimer = 0f;
        }

        private void UpdateFlashing(GameTime gameTime)
        {
            if (isFlashing)
            {
                flashTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (flashTimer >= flashTime)
                {
                    isFlashing = false;
                    flashTimer = 0f;
                }
            }
        }

        
    }
}
