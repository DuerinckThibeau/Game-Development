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
        private MapManager mapManager;

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

            // Tiles (hitbox) tekenen voor testing
            foreach (var collider in mapManager.Colliders)
            {
                spriteBatch.Draw(hitboxTexture, collider, Color.Green * 0.5f);
            }
        }


        private void Move()
        {
            Vector2 previousPosition = Position;

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

            
            Rectangle futureHitbox = new Rectangle((int)(Position.X + Direction.X * Speed.X), (int)(Position.Y + verticalSpeed), Hitbox.Width, Hitbox.Height);

            foreach (var collider in mapManager.Colliders)
            {
                if (futureHitbox.Intersects(collider))
                {
                    Position = previousPosition;
                    return;
                }
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
                Vector2 newPosition = new Vector2(Position.X, Position.Y + verticalSpeed);

                // Maak een toekomstige hitbox om te controleren op botsingen
                Rectangle futureHitbox = new Rectangle((int)newPosition.X, (int)newPosition.Y, Hitbox.Width, Hitbox.Height);

                // Check voor botsingen met de kaart
                foreach (var collider in mapManager.Colliders)
                {
                    if (futureHitbox.Intersects(collider))
                    {
                        // Als er een botsing is, stel de speler terug naar een correcte positie
                        verticalSpeed = 0;
                        isGrounded = true;
                        Position = new Vector2(Position.X, collider.Top - Hitbox.Height); // Plaats de speler bovenop de tegel
                        return;
                    }
                }

                Position = newPosition; // Geen botsingen, update positie
            }

            // Controleer of de speler op de grond is
            if (Position.Y >= 300) // Dit is een tijdelijke check, zorg dat je dit vervangt door een correcte waarde
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
