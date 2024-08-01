using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDev.Core.Animations;
using GameDev.Core.Interfaces;
using GameDev.Core.Managers;

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
        runAnimation.AddFrame(new AnimationFrame(new Rectangle(0, 0, 64, 64)));
        runAnimation.AddFrame(new AnimationFrame(new Rectangle(64, 0, 64, 64)));
        runAnimation.AddFrame(new AnimationFrame(new Rectangle(128, 0, 64, 64)));
        runAnimation.AddFrame(new AnimationFrame(new Rectangle(192, 0, 64, 64)));
        runAnimation.AddFrame(new AnimationFrame(new Rectangle(256, 0, 64, 64)));
        runAnimation.AddFrame(new AnimationFrame(new Rectangle(320, 0, 64, 64)));

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
            currentAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D currentTexture = currentAnimation == runAnimation ? wizardRunTexture : wizardIdleTexture;
            SpriteEffects spriteEffects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(currentTexture, Position, currentAnimation.currentFrame.sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
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
        }

    }
}
