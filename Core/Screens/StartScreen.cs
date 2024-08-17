using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameDev.Core.Interfaces;
using GameDev.Core.Managers;


namespace GameDev.Core.Screens
{
    internal class StartScreen : IGameObject
    {
        private Texture2D _backgroundTexture;
        private Texture2D _startButtonTexture;
        private Rectangle _startButtonRectangle;
        private ContentLoader _contentLoader;

        public StartScreen()
        {
            _backgroundTexture = ContentLoader.BackgroundTexture;
            _startButtonTexture = ContentLoader.StartButtonTexture;

            int buttonWidth = _startButtonTexture.Width;
            int buttonHeight = _startButtonTexture.Height;
            int buttonX = (1920 - buttonWidth) / 2;
            int buttonY = (1080 - buttonHeight) / 2 + buttonHeight;

            _startButtonRectangle = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
        }

        public void Update(GameTime gameTime) 
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && _startButtonRectangle.Contains(mouseState.Position))
            {
                GameManager.getInstance().UpdateGameState(GameState.Level1);
            }
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(_startButtonTexture, _startButtonRectangle, Color.White);
        }
    }
}
