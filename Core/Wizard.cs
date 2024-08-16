using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameDev.Core.Animations;
using GameDev.Core.Interfaces;
using GameDev.Core.Managers;
using Microsoft.Xna.Framework.Input;

namespace GameDev.Core
{
    public class Wizard : IGameObject, IMovable
    {
        Texture2D wizardRunTexture;
        Texture2D wizardIdleTexture;
        Animation runAnimation;
        Animation idleAnimation;
        Animation currentAnimation;
        Animation[] animations;

        MovementManager playerMovementManager;

        public Vector2 Position { get; set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 Direction { get; private set; }
        public Rectangle Hitbox { get; private set; }

        public IInputReader InputReader { get; set; }

        private bool isFacingRight = true;
        private float runAnimationHeight = 32f;
        private float idleAnimationHeight = 32f;

        private float verticalSpeed;
        private bool isGrounded;
        private float gravity = 0.5f;
        private float jumpStrength = 12f;

        public Wizard(Texture2D runTexture, Texture2D idleTexture, IInputReader inputReader, MovementManager movementManager)
        {
            wizardRunTexture = runTexture;
            wizardIdleTexture = idleTexture;
            InputReader = inputReader;
            playerMovementManager = movementManager;

            Acceleration = new Vector2(0.1f, 0.1f);
            Position = new Vector2(1, 1);
            Speed = new Vector2(2, 2);

            animations = new Animation[]
            {
                new Animation(), // run
                new Animation() // idle
            };

            animations[0].AddAnimation(6, 64, 32);
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

            // Hitbox tekenen voor testing
            Texture2D hitboxTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            hitboxTexture.SetData(new[] { Color.White });
            spriteBatch.Draw(hitboxTexture, Hitbox, Color.Red * 0.5f);
        }


        private void Move()
        {
            Direction = InputReader.ReadInput();
            playerMovementManager.Move(this);

            if (Direction != Vector2.Zero)
            {
                currentAnimation = animations[0];
                if (Direction.X < 0)
                {
                    isFacingRight = false;
                }
                else if (Direction.X > 0)
                {
                    isFacingRight = true;
                }
            }
            else
            {
                currentAnimation = animations[1];
            }


            if (isGrounded && Keyboard.GetState().IsKeyDown(Keys.Space) || isGrounded && Keyboard.GetState().IsKeyDown(Keys.Up))
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
                Position = new Vector2(Position.X, Position.Y + verticalSpeed);
            }

            
            if (Position.Y >= 300)
            {
                Position = new Vector2(Position.X, 300);
                verticalSpeed = 0;
                isGrounded = true;
            }
        }

        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 32, 32);
        }
    }
}
