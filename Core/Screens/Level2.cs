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
        
        public Level2()
        {
            MapManager.Colliders.Clear();
            _mapManager = new MapManager(ContentLoader.Level2, ContentLoader.TilesetTexture);
            
        }

        public void Update(GameTime gameTime)
        {
            GameManager.wizard.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentLoader.CaveBackground, Vector2.Zero, Color.White);
            _mapManager.Draw();
            GameManager.wizard.Draw(spriteBatch);

            for (int i = 0; i < GameManager.wizard.Health; i++)
            {
                spriteBatch.Draw(ContentLoader.HeartTexture, new Vector2(10 + i * 40, 10), Color.White);
            }
        }
    }
}
