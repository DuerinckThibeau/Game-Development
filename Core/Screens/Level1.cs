using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDev.Core.Enemies;
using GameDev.Core.Interfaces;
using GameDev.Core.Managers;
using GameDev.Core.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDev.Core.Screens
{
    public class Level1 : IGameObject
    {
        private MapManager _mapManager;
        private List<Orc> _orcs;

        public Level1() {

            MapManager.Colliders.Clear();
            _mapManager = new MapManager(ContentLoader.Level1, ContentLoader.TilesetTexture);
            _orcs = new List<Orc>();
            foreach (var obj in ContentLoader.Level1.ObjectGroups["Orc"].Objects)
            {
                if (obj.Name == "OrcSpawn")
                {
                    Vector2 spawnPosition = new Vector2((float)obj.X, (float)obj.Y);
                    Orc newOrc = new Orc(ContentLoader.OrcTexture, spawnPosition, 1f);
                    _orcs.Add(newOrc);
                }
            }
        }

        public void Update(GameTime gameTime) 
        {
            GameManager.wizard.Update(gameTime);

            foreach (var orc in _orcs)
            {
                orc.Update(gameTime);
            }

            foreach (var orc in _orcs)
            {
                if (GameManager.wizard.Hitbox.Intersects(orc.Hitbox) && !GameManager.wizard.IsFlashing)
                {
                    GameManager.wizard.TakeDamage();
                    break;
                }
            }

            if (MapManager.CheckNextLevelTrigger(GameManager.wizard.Hitbox))
            {
                GameManager.getInstance().UpdateGameState(GameState.Level2);
            }
        }
        public void Draw (SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(ContentLoader.MapBackground, Vector2.Zero, Color.White);
            _mapManager.Draw();
            GameManager.wizard.Draw(spriteBatch);
            foreach (var orc in _orcs)
            {
                orc.Draw(spriteBatch);
            }

            for (int i = 0; i < GameManager.wizard.Health; i++)
            {
                spriteBatch.Draw(ContentLoader.HeartTexture, new Vector2(10 + i * 40, 10), Color.White);
            }
        }
    }
}
