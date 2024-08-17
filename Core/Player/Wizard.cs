using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameDev.Core.Animations;
using GameDev.Core.Interfaces;
using GameDev.Core.Managers;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameDev.Core.Player
{
    public class Wizard : IGameObject, IMovable
    {
        Texture2D wizardRunTexture;
        Texture2D wizardIdleTexture;
        Texture2D wizardDeathTexture;
        Texture2D currentTexture;
        Animation currentAnimation;
        Animation[] animations;

        MovementManager playerMovementManager;
        private MapManager mapManager;
        private GameManager gameManager;

        public Vector2 Position { get; set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 Direction { get; private set; }
        public Rectangle Hitbox;
        public Rectangle futureHitbox;

        public IInputReader InputReader { get; set; }

        private bool isFacingRight = true;

        private float verticalSpeed;
        private bool isGrounded;
        private float gravity = 0.5f;
        private float jumpStrength = 12f;

        public int Health { get;  set; }
        public bool IsFlashing { get; private set; }

        private float flashTime;
        private const float FLASH_INTERVAL = 0.5f;
        private float flashTimer;

        public Wizard(Texture2D runTexture, Texture2D idleTexture, Texture2D deathTexture, IInputReader inputReader, MovementManager movementManager)
        {
            wizardRunTexture = runTexture;
            wizardIdleTexture = idleTexture;
            wizardDeathTexture = deathTexture;
            InputReader = inputReader;
            playerMovementManager = movementManager;

            Acceleration = new Vector2(0.1f, 0.1f);
            Position = new Vector2(1, 1);
            Speed = new Vector2(2, 2);

            animations = new Animation[]
            {
                new Animation(), // run
                new Animation(), // idle
                new Animation()  // death
            };

            animations[0].AddAnimation(6, 32, 32);
            animations[1].AddAnimation(4, 32, 32);
            animations[2].AddAnimation(6, 32, 32);
            currentAnimation = animations[1];

            Health = 3;
            IsFlashing = false;
            flashTime = 0f;
            flashTimer = 0f;
        }

        public void Update(GameTime gameTime)
        {
            if (Health <= 0)
            {
                currentAnimation.Update(gameTime);
                GameManager.getInstance().UpdateGameState(GameState.DeathScreen);
                return;
            }

            Move();
            ApplyGravity();
            UpdateFlashing(gameTime);
            currentAnimation.Update(gameTime);
            UpdateHitbox();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Health <= 0)
            {
                currentTexture = wizardDeathTexture;
            }
            else
            {
                currentTexture = currentAnimation == animations[0] ? wizardRunTexture : wizardIdleTexture;
            }



            SpriteEffects spriteEffects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawPosition = Position;
            Color drawColor = Color.White;

            if (IsFlashing)
            {
                if (flashTimer % FLASH_INTERVAL < FLASH_INTERVAL / 2)
                {
                    drawColor = Color.Red;
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
            if (Health <= 0) return;

            Vector2 previousPosition = Position;

            Direction = InputReader.ReadInput();
            playerMovementManager.Move(this);
            bool collisionDetected = false;

            if (Direction != Vector2.Zero)
            {
                currentAnimation = animations[0];
                isFacingRight = Direction.X > 0;

                futureHitbox = new Rectangle((int)(Position.X + Direction.X * Speed.X), (int)Position.Y, Hitbox.Width, Hitbox.Height);

                foreach (var collider in MapManager.Colliders)
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

            foreach (var collider in MapManager.Colliders)
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
            if (IsFlashing || Health <= 0) return;

            Health--;
            IsFlashing = true;
            flashTime = 3f;
            flashTimer = 0f;

        }

        private void UpdateFlashing(GameTime gameTime)
        {
            if (IsFlashing)
            {
                flashTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (flashTimer >= flashTime)
                {
                    IsFlashing = false;
                    flashTimer = 0f;
                }
            }
        }
    }
}
