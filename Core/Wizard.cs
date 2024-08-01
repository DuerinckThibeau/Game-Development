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

        MovementManager playerMovementManager;

        public Vector2 Position { get; set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 Direction { get; private set; }
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

            runAnimation = new Animation();
            runAnimation.AddFrame(new AnimationFrame(new Rectangle(0, 0, 64, 32)));
            runAnimation.AddFrame(new AnimationFrame(new Rectangle(64, 0, 64, 32)));
            runAnimation.AddFrame(new AnimationFrame(new Rectangle(128, 0, 64, 32)));
            runAnimation.AddFrame(new AnimationFrame(new Rectangle(192, 0, 64, 32)));
            runAnimation.AddFrame(new AnimationFrame(new Rectangle(256, 0, 64, 32)));
            runAnimation.AddFrame(new AnimationFrame(new Rectangle(320, 0, 64, 32)));

            idleAnimation = new Animation();
            idleAnimation.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 32)));
            idleAnimation.AddFrame(new AnimationFrame(new Rectangle(32, 0, 32, 32)));
            idleAnimation.AddFrame(new AnimationFrame(new Rectangle(64, 0, 32, 32)));
            idleAnimation.AddFrame(new AnimationFrame(new Rectangle(96, 0, 32, 32)));

            currentAnimation = idleAnimation;
        }

        public void Update(GameTime gameTime)
        {
            Move();
            ApplyGravity();
            currentAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D currentTexture = currentAnimation == runAnimation ? wizardRunTexture : wizardIdleTexture;
            SpriteEffects spriteEffects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Calculate the vertical offset based on the height of the current animation
            float verticalOffset = currentAnimation == runAnimation ? runAnimationHeight - idleAnimationHeight : 0;

            // Adjust the drawing position to keep the wizard at the same visual height
            Vector2 drawPosition = Position;
            if (currentAnimation == idleAnimation)
            {
                drawPosition.Y += verticalOffset;
            }

            spriteBatch.Draw(currentTexture, drawPosition, currentAnimation.currentFrame.sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
        }

        private void Move()
        {
            Direction = InputReader.ReadInput();
            playerMovementManager.Move(this);

            if (Direction != Vector2.Zero)
            {
                currentAnimation = runAnimation;
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
                currentAnimation = idleAnimation;
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

            // Simple ground collision detection
            if (Position.Y >= 300) // Assuming ground level is at Y = 300
            {
                Position = new Vector2(Position.X, 300);
                verticalSpeed = 0;
                isGrounded = true;
            }
        }
    }
}
