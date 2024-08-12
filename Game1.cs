﻿using GameDev.Core;
using GameDev.Core.Input;
using GameDev.Core.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TiledCS;

namespace GameDev
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _runTexture;
        private Texture2D _idleTexture;
        Wizard wizard;

        private TiledMap _map;
        private TiledTileset _tileset;
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Set the "Copy to Output Directory" property of these two files to `Copy if newer`
            // by clicking them in the solution explorer.
            _map = new TiledMap(Content.RootDirectory + "\\Maps/Level_1.tmx");
            _tileset = new TiledTileset(Content.RootDirectory + "\\Tiles/Tiles.tsx");

            // Not the best way to do this but it works. It looks for "exampleTileset.xnb" file
            // which is the result of building the image file with "Content.mgcb".
            _tilesetTexture = Content.Load<Texture2D>("Tiles/Assets");

            _tileWidth = _tileset.TileWidth;
            _tileHeight = _tileset.TileHeight;

            // Amount of tiles on each row (left right)
            _tilesetTilesWide = _tileset.Columns;
            // Amount of tiels on each column (up down)
            _tilesetTilesHeight = _tileset.TileCount / _tileset.Columns;


            _idleTexture = Content.Load<Texture2D>("Wizard/Idle-Sheet");
            _runTexture = Content.Load<Texture2D>("Wizard/Run-Sheetv2");

            InitializeGameObjects();
        }

        private void InitializeGameObjects()
        {
            wizard = new Wizard(_runTexture, _idleTexture, new KeyboardReader(), new MovementManager());
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
            for (var i = 0; i < _map.Layers[0].data.Length; i++)
            {
                int gid = _map.Layers[0].data[i];

                // Empty tile, do nothing
                if (gid == 0)
                {

                }
                else
                {
                    // Tileset tile ID
                    // Looking at the exampleTileset.png
                    // 0 = Blue
                    // 1 = Green
                    // 2 = Dark Yellow
                    // 3 = Magenta
                    int tileFrame = gid - 1;

                    // Print the tile type into the debug console.
                    // This assumes only one (1) `tiled tileset` is being used, so getting the first one.
                    var tile = _map.GetTiledTile(_map.Tilesets[0], _tileset, gid);
                    if (tile != null)
                    {
                        // This should print "Grass" for each grass tile in the map each draw call
                        // so six (6) times.
                        System.Diagnostics.Debug.WriteLine(tile.type);
                    }

                    int column = tileFrame % _tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)_tilesetTilesWide);

                    float x = (i % _map.Width) * _map.TileWidth;
                    float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;

                    Rectangle tilesetRec = new Rectangle(_tileWidth * column, _tileHeight * row, _tileWidth, _tileHeight);

                    _spriteBatch.Draw(_tilesetTexture, new Rectangle((int)x, (int)y, _tileWidth, _tileHeight), tilesetRec, Color.White);
                }
            }
            wizard.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
