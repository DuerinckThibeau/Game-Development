using GameDev.Core;
using GameDev.Core.Input;
using GameDev.Core.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Metadata;
using TiledSharp;

namespace GameDev
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _runTexture;
        private Texture2D _idleTexture;
        private Wizard _wizard;

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
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {

        SetFullScreen();

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _map = new TmxMap("Content/Maps/Level_1.tmx");
        _tilesetTexture = Content.Load<Texture2D>("Tiles/Assets");
        var tileWidth = _map.Tilesets[0].TileWidth;
        var tileHeight = _map.Tilesets[0].TileHeight;
        var tileSetTilesWide = _tilesetTexture.Width / tileWidth;
        _mapManager = new MapManager(_spriteBatch, _map, _tilesetTexture, tileSetTilesWide, tileWidth, tileHeight);

        _idleTexture = Content.Load<Texture2D>("Wizard/Idle-Sheet");
        _runTexture = Content.Load<Texture2D>("Wizard/Run-Sheet");

        InitializeGameObjects();
        }

        private void InitializeGameObjects()
        {
        _wizard = new Wizard(_runTexture, _idleTexture, new KeyboardReader(), new MovementManager(), _mapManager);
        }

        protected override void Update(GameTime gameTime)
            {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _wizard.Update(gameTime);
        base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
            {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: _transformMatrix);
        _mapManager.Draw(_spriteBatch);
        _wizard.Draw(_spriteBatch);
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
