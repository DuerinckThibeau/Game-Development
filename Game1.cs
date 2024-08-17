using GameDev.Core;
using GameDev.Core.Input;
using GameDev.Core.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TiledSharp;

namespace GameDev
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _runTexture;
        private Texture2D _idleTexture;
        Wizard wizard;

        private TmxMap _map;
        private MapManager mapManager;
        private Texture2D _tilesetTexture;

        private int _tileWidth;
        private int _tileHeight;
        private int _tilesetTilesWide;
        private int _tilesetTilesHeight;


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
            var tileset = Content.Load<Texture2D>("Tiles/Assets");
            var tileWidth = _map.Tilesets[0].TileWidth;
            var tileHeight = _map.Tilesets[0].TileHeight;
            var TileSetTilesWide = tileset.Width / tileWidth;
            mapManager = new MapManager(_spriteBatch, _map, tileset, TileSetTilesWide, tileWidth, tileHeight);


            _idleTexture = Content.Load<Texture2D>("Wizard/Idle-Sheet");
            _runTexture = Content.Load<Texture2D>("Wizard/Run-Sheet");

            InitializeGameObjects();
        }

        private void InitializeGameObjects()
        {
            wizard = new Wizard(_runTexture, _idleTexture, new KeyboardReader(), new MovementManager(), mapManager);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            wizard.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            mapManager.Draw(_spriteBatch);
            wizard.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetFullScreen()
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Window.IsBorderless = true;
            _graphics.ApplyChanges();
        }
    }
}
