using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDev.Core.Enemies;
using GameDev.Core.Interfaces;
using GameDev.Core.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDev.Core.Screens
{
    internal class Level2 : IGameObject
    {
        private MapManager _mapManager;
        private List<Snake> _snakes;
        
        public Level2()
        {
            MapManager.Colliders.Clear();
            _mapManager = new MapManager(ContentLoader.Level2, ContentLoader.TilesetTexture);
            _snakes = new List<Snake>();
            foreach (var obj in ContentLoader.Level2.ObjectGroups["Snake"].Objects)
            {
                if(obj.Name == "SnakeSpawn")
                {
                    Vector2 spawnPosition = new Vector2((float)obj.X, (float)obj.Y);
                    Snake newSnake = new Snake(ContentLoader.SnakeTexture, spawnPosition, 1f);
                    _snakes.Add(newSnake);
                }
            }
            
        }

        public void Update(GameTime gameTime)
        {
            GameManager.wizard.Update(gameTime);

            foreach(var snake in _snakes)
            {
                snake.Update(gameTime);
            }

            foreach(var snake in _snakes)
            {
                if (GameManager.wizard.Hitbox.Intersects(snake.Hitbox) && !GameManager.wizard.IsFlashing)
                {
                    GameManager.wizard.TakeDamage();
                    break;
                }
            }

            if (MapManager.CheckNextLevelTrigger(GameManager.wizard.Hitbox))
            {
                GameManager.getInstance().UpdateGameState(GameState.VictoryScreen);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentLoader.CaveBackground, Vector2.Zero, Color.White);
            _mapManager.Draw();
            GameManager.wizard.Draw(spriteBatch);
            foreach(var snake in _snakes) 
            {
                snake.Draw(spriteBatch);
            }
            for (int i = 0; i < GameManager.wizard.Health; i++)
            {
                spriteBatch.Draw(ContentLoader.HeartTexture, new Vector2(10 + i * 40, 10), Color.White);
            }
        }
    }
}
