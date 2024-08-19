using GameDev.Core.Enemies;
using GameDev.Core.Input;
using GameDev.Core.Managers;
using GameDev.Core.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameDev
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        private float _scale;
        private Matrix _transformMatrix;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            SetFullScreen();
            base.Initialize();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentLoader contentLoader = new ContentLoader(Content);
            ContentLoader.LoadContent();
            
            InitializeGameObjects();
        }

        private void InitializeGameObjects()
        {
            /*_wizard = new Wizard(_contentLoader.RunTexture, _contentLoader.IdleTexture, _contentLoader.DeathTexture, new KeyboardReader(), new MovementManager(), _mapManager, _gameManager);*/
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameManager.getInstance().Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: GameManager.getInstance().getCurrentState() == GameState.Level1 || GameManager.getInstance().getCurrentState() == GameState.Level2 ? _transformMatrix : Matrix.Identity);
            GameManager.getInstance().Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetFullScreen()
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            int screenHeight = GraphicsDevice.Viewport.Height;
            int screenWidth = GraphicsDevice.Viewport.Width;

            int mapWidth = 800;
            int mapHeight = 480;

            _scale = Math.Min(screenWidth / (float)mapWidth, screenHeight / (float)mapHeight);
            _transformMatrix = Matrix.CreateScale(_scale);
        }
    }
}
