using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameDev.Core.Managers
{
    public enum GameState
    {
        StartScreen,
        Playing,
        DeathScreen
    }

    public class GameManager
    {
        public GameState CurrentGameState { get; set; } = GameState.StartScreen;

        private Texture2D _startScreenBackground;
        private Texture2D _startButtonTexture;
        private Texture2D _mapBackground;
        private Texture2D _deathScreenBackground;
        private Texture2D _exitButtonTexture;
        private Rectangle _startButtonRectangle;
        private Rectangle _exitButtonRectangle;

        public void LoadContent(Texture2D startScreenBackground, Texture2D startButtonTexture, Texture2D mapBackground, Texture2D deathScreenBackground, Texture2D exitButtonTexture, GraphicsDevice graphicsDevice)
        {
            _startScreenBackground = startScreenBackground;
            _startButtonTexture = startButtonTexture;
            _mapBackground = mapBackground;
            _deathScreenBackground = deathScreenBackground;
            _exitButtonTexture = exitButtonTexture;

            int buttonWidth = _startButtonTexture.Width;
            int buttonHeight = _startButtonTexture.Height;
            int buttonX = (graphicsDevice.Viewport.Width - buttonWidth) / 2;
            int buttonY = (graphicsDevice.Viewport.Height - buttonHeight) / 2 + buttonHeight;

            _startButtonRectangle = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
            _exitButtonRectangle = new Rectangle(buttonX, buttonY + 300, buttonWidth, buttonHeight);
        }


        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (CurrentGameState == GameState.StartScreen)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && _startButtonRectangle.Contains(mouseState.Position))
                {
                    StartGame();
                }
            }
            else if (CurrentGameState == GameState.DeathScreen)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && _exitButtonRectangle.Contains(mouseState.Position))
                {
                    ExitGame();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    ExitGame();
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
            else if (CurrentGameState == GameState.DeathScreen)
            {
                spriteBatch.Draw(_deathScreenBackground, Vector2.Zero, Color.White);
                spriteBatch.Draw(_exitButtonTexture, _exitButtonRectangle, Color.White);
            }
        }

        private void StartGame()
        {
            CurrentGameState = GameState.Playing;
        }

        private void ExitGame()
        {
            System.Environment.Exit(0);
        }
    }
}
