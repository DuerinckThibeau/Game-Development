using System;
using System.Text;
using System.Threading.Tasks;
using GameDev.Core.Interfaces;
using GameDev.Core.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameDev.Core.Screens
{
    internal class DeathScreen : IGameObject 
    {
        private Texture2D _backgroundTexture;
        private Texture2D _exitButtonTexture;
        private Rectangle _exitButtonRectangle;

        public DeathScreen()
        {
            _backgroundTexture = ContentLoader.DeathScreenTexture;
            _exitButtonTexture = ContentLoader.ExitButtonTexture;

            int buttonWidth = _exitButtonTexture.Width;
            int buttonHeight = _exitButtonTexture.Height;
            int buttonX = (1920 - buttonWidth) / 2;
            int buttonY = (1080 - buttonHeight) / 2 + buttonHeight;

            _exitButtonRectangle = new Rectangle(buttonX, buttonY + 250, 168, 88);
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && _exitButtonRectangle.Contains(mouseState.Position))
            {
                System.Environment.Exit(0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(_exitButtonTexture, _exitButtonRectangle, Color.White);
        }


    }
}
