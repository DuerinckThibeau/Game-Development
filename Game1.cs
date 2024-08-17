using GameDev.Core;
using GameDev.Core.Enemies;
using GameDev.Core.Input;
using GameDev.Core.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace GameDev
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _runTexture;
        private Texture2D _idleTexture;
        private Texture2D _deathTexture;
        private Texture2D _backgroundTexture;
        private Texture2D _startButtonTexture;
        private Texture2D _heartTexture;
        private Texture2D _mapBackground;
        private Wizard _wizard;
        private List<Orc> _orcs;
        private Texture2D _orcTexture;
        private GameManager _gameManager;

        private TmxMap _map;
        private MapManager _mapManager;
        private Texture2D _tilesetTexture;

        private float _scale;
        private Matrix _transformMatrix;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _gameManager = new GameManager();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SetFullScreen();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backgroundTexture = Content.Load<Texture2D>("UI/StartScreen");
            _startButtonTexture = Content.Load<Texture2D>("UI/Startbutton");
            _mapBackground = Content.Load<Texture2D>("Tiles/World1");
            _orcTexture = Content.Load<Texture2D>("Enemies/Orc-Idle");
            _heartTexture = Content.Load<Texture2D>("UI/Heart");

            _map = new TmxMap("Content/Maps/Level_1.tmx");
            _tilesetTexture = Content.Load<Texture2D>("Tiles/Assets");
            var tileWidth = _map.Tilesets[0].TileWidth;
            var tileHeight = _map.Tilesets[0].TileHeight;
            var tileSetTilesWide = _tilesetTexture.Width / tileWidth;
            _mapManager = new MapManager(_spriteBatch, _map, _tilesetTexture, tileSetTilesWide, tileWidth, tileHeight);

            _idleTexture = Content.Load<Texture2D>("Wizard/Idle-Sheet");
            _runTexture = Content.Load<Texture2D>("Wizard/Run-Sheet");
            _deathTexture = Content.Load<Texture2D>("Wizard/Death-Sheet");

            InitializeGameObjects();

            _orcs = new List<Orc>();
            foreach (var obj in _map.ObjectGroups["Orc"].Objects)
            {
                if (obj.Name == "OrcSpawn")
                {
                    Vector2 spawnPosition = new Vector2((float)obj.X, (float)obj.Y);
                    Orc newOrc = new Orc(_orcTexture, spawnPosition, 1f);
                    _orcs.Add(newOrc);
                }
            }

            _gameManager.LoadContent(_backgroundTexture, _startButtonTexture, _mapBackground, GraphicsDevice);
        }

        private void InitializeGameObjects()
        {
            _wizard = new Wizard(_runTexture, _idleTexture, _deathTexture, new KeyboardReader(), new MovementManager(), _mapManager);
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
                _spriteBatch.Draw(_mapBackground, Vector2.Zero, Color.White);
                _mapManager.Draw(_spriteBatch);
                _wizard.Draw(_spriteBatch);
                foreach (var orc in _orcs)
                {
                    orc.Draw(_spriteBatch);
                }

                for (int i = 0; i < _wizard.Health; i++)
                {
                    _spriteBatch.Draw(_heartTexture, new Vector2(10 + i * 40, 10), Color.White);
                }
            }
            else if (_gameManager.CurrentGameState == GameState.StartScreen)
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
