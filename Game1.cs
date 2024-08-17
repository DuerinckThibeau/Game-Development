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
        private SpriteBatch _spriteBatch;
        private ContentLoader _contentLoader;

        private Wizard _wizard;
        private List<Orc> _orcs;
        private GameManager _gameManager;

        private MapManager _mapManager;

        private float _scale;
        private Matrix _transformMatrix;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _gameManager = new GameManager();
            _contentLoader = new ContentLoader(Content);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SetFullScreen();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _contentLoader.LoadContent();

            var tileWidth = _contentLoader.Level1.Tilesets[0].TileWidth;
            var tileHeight = _contentLoader.Level1.Tilesets[0].TileHeight;
            var tileSetTilesWide = _contentLoader.TilesetTexture.Width / tileWidth;
            _mapManager = new MapManager(_spriteBatch, _contentLoader.Level1, _contentLoader.TilesetTexture, tileSetTilesWide, tileWidth, tileHeight);

            InitializeGameObjects();

            _orcs = new List<Orc>();
            foreach (var obj in _contentLoader.Level1.ObjectGroups["Orc"].Objects)
            {
                if (obj.Name == "OrcSpawn")
                {
                    Vector2 spawnPosition = new Vector2((float)obj.X, (float)obj.Y);
                    Orc newOrc = new Orc(_contentLoader.OrcTexture, spawnPosition, 1f);
                    _orcs.Add(newOrc);
                }
            }

            _gameManager.LoadContent(_contentLoader.BackgroundTexture, _contentLoader.StartButtonTexture, _contentLoader.MapBackground, _contentLoader.DeathScreenTexture, _contentLoader.ExitButtonTexture, GraphicsDevice);
        }

        private void InitializeGameObjects()
        {
            _wizard = new Wizard(_contentLoader.RunTexture, _contentLoader.IdleTexture, _contentLoader.DeathTexture, new KeyboardReader(), new MovementManager(), _mapManager, _gameManager);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _gameManager.Update(gameTime);

            if (_gameManager.CurrentGameState == GameState.Playing)
            {
                _wizard.Update(gameTime);
                foreach (var orc in _orcs)
                {
                    orc.Update(gameTime);
                }

                foreach (var orc in _orcs)
                {
                    if (_wizard.Hitbox.Intersects(orc.Hitbox) && !_wizard.IsFlashing)
                    {
                        _wizard.TakeDamage();
                        break;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: _gameManager.CurrentGameState == GameState.Playing ? _transformMatrix : Matrix.Identity);

            if (_gameManager.CurrentGameState == GameState.Playing)
            {
                _spriteBatch.Draw(_contentLoader.MapBackground, Vector2.Zero, Color.White);
                _mapManager.Draw(_spriteBatch);
                _wizard.Draw(_spriteBatch);
                foreach (var orc in _orcs)
                {
                    orc.Draw(_spriteBatch);
                }

                for (int i = 0; i < _wizard.Health; i++)
                {
                    _spriteBatch.Draw(_contentLoader.HeartTexture, new Vector2(10 + i * 40, 10), Color.White);
                }
            }
            else if (_gameManager.CurrentGameState == GameState.StartScreen)
            {
                _gameManager.Draw(_spriteBatch);
            }
            else if (_gameManager.CurrentGameState == GameState.DeathScreen)
            {
                _gameManager.Draw(_spriteBatch);
            }

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
