using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDev.Core.Animations;
using GameDev.Core.Interfaces;
using GameDev.Core.GameManagers;

namespace GameDev.Core
{
    public class Wizard : IGameObject, IMovable
    {
        Texture2D wizardTexture;
        Animation animation;

        MovementManager playerMovementManager;

        public Vector2 Position { get; set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 Speed { get; set; }
        public IInputReader InputReader { get; set; }


        public Wizard(Texture2D texture, IInputReader inputReader, MovementManager movementManager)
        {
            wizardTexture = texture;
            InputReader = inputReader;
            playerMovementManager = movementManager;

            Acceleration = new Vector2(0.1f, 0.1f);
            Position = new Vector2(1, 1);
            Speed = new Vector2(2, 2);

            animation = new Animation();
            animation.AddFrame(new AnimationFrame(new Rectangle(0, 0, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(64, 0, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(128, 0, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(192, 0, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(256, 0, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(320, 0, 64, 64)));
        }

        public void Update(GameTime gameTime) 
        {
            Move();
            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(wizardTexture, Position, animation.currentFrame.sourceRectangle, Color.White);
        }

        private void Move()
        {
            playerMovementManager.Move(this);
        }

    }
}
