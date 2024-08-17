using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameDev.Core.Managers
{
    public enum GameState
    {
        StartScreen,
        Playing
    }

    public class GameManager
    {
        public GameState CurrentGameState { get; private set; } = GameState.StartScreen;

        private Texture2D _startScreenBackground;
        private Texture2D _startButtonTexture;
        private Texture2D _mapBackground;
        private Rectangle _startButtonRectangle;

        public void LoadContent(Texture2D startScreenBackground, Texture2D startButtonTexture, Texture2D mapBackground, GraphicsDevice graphicsDevice)
        {
            _startScreenBackground = startScreenBackground;
            _startButtonTexture = startButtonTexture;
            _mapBackground = mapBackground;

            int buttonWidth = _startButtonTexture.Width;
            int buttonHeight = _startButtonTexture.Height;
            int buttonX = (graphicsDevice.Viewport.Width - buttonWidth) / 2;
            int buttonY = (graphicsDevice.Viewport.Height - buttonHeight) / 2 + buttonHeight;

            _startButtonRectangle = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentGameState == GameState.StartScreen)
            {
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed && _startButtonRectangle.Contains(mouseState.Position))
                {
                    StartGame();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentGameState == GameState.StartScreen)
            {
                spriteBatch.Draw(_startScreenBackground, Vector2.Zero, Color.White);
                spriteBatch.Draw(_startButtonTexture, _startButtonRectangle, Color.White);
            }
            else if (CurrentGameState == GameState.Playing)
            {
                spriteBatch.Draw(_mapBackground, Vector2.Zero, Color.White);
            }
        }

        private void StartGame()
        {
            CurrentGameState = GameState.Playing;
            _startScreenBackground = null;
        }
    }
}
