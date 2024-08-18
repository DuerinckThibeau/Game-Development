using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace GameDev.Core.Managers
{
    public class MapManager
    {
        private SpriteBatch spriteBatch;
        TmxMap map;
        Texture2D tileset;
        int tilesetTilesWide;
        int tileWidth;
        int tileHeight;

        public static Vector2 PlayerSpawn;
        public static List<Rectangle> Colliders = new();
        private static Rectangle nextLevelTrigger;

        private Texture2D rectangleTexture;

        public MapManager(TmxMap _map, Texture2D _tileset)
        {
            spriteBatch = Game1._spriteBatch;
            map = _map;
            tileset = _tileset;

            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;
            tilesetTilesWide = tileset.Width / tileWidth;

            // TESTING
            /*rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new[] { Color.White });*/

            PlayerSpawn = new Vector2((int)map.ObjectGroups["Spawn"].Objects["Spawn"].X, (int)map.ObjectGroups["Spawn"].Objects["Spawn"].Y);

            var collisionLayer = map.Layers.FirstOrDefault(layer => layer.Name == "Tiles");

            if (collisionLayer != null)
            {
                for (var i = 0; i < collisionLayer.Tiles.Count; i++)
                {
                    int gid = collisionLayer.Tiles[i].Gid;
                    if (gid == 0)
                    {
                        continue;
                    }

                    int x = (i % map.Width) * map.TileWidth;
                    int y = (i / map.Width) * map.TileHeight;

                    Colliders.Add(new Rectangle(x, y, map.TileWidth, map.TileHeight));
                }
            }
            else
            {
                Console.WriteLine("Collision layer not found!");
            }

            var nextLevelObject = map.ObjectGroups["NextLevel"].Objects.FirstOrDefault(o => o.Name == "NextLevel");
            if (nextLevelObject != null)
            {
                nextLevelTrigger = new Rectangle((int)nextLevelObject.X, (int)nextLevelObject.Y, (int)nextLevelObject.Width, (int)nextLevelObject.Height);
            }
        }

        public void Draw()
        {
            for (var i = 0; i < map.Layers.Count; i++)
            {
                for (var j = 0; j < map.Layers[i].Tiles.Count; j++)
                {
                    int gid = map.Layers[i].Tiles[j].Gid;
                    if (gid == 0)
                    {
                        continue;
                    }
                    int tileFrame = gid - 1;
                    int column = tileFrame % tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);
                    float x = (j % map.Width) * map.TileWidth;
                    float y = (float)Math.Floor(j / (double)map.Width) * map.TileHeight;
                    Rectangle tilesetRec = new Rectangle((tileWidth) * column, (tileHeight) * row, tileWidth, tileHeight);
                    spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White);
                }
            }

            // TESTING
            /*foreach (var collider in Colliders)
            {
                spriteBatch.Draw(rectangleTexture, collider, Color.Green * 0.5f);
            }*/

            // TESTING
            /*spriteBatch.Draw(rectangleTexture, nextLevelTrigger, Color.Red * 0.5f);*/
        }

        public static bool CheckNextLevelTrigger(Rectangle playerHitbox)
        {
            return playerHitbox.Intersects(nextLevelTrigger);
        }
    }
}
