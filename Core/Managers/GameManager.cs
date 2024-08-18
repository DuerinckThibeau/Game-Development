using GameDev.Core.Input;
using GameDev.Core.Player;
using GameDev.Core.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace GameDev.Core.Managers
{
    public enum GameState
    {
        StartScreen,
        Level1,
        Level2,
        Level3,
        DeathScreen,
        VictoryScreen
    }

    internal class GameManager
    {
        // Singleton pattern
        private static GameManager Instance;

        private GameState _currentState = GameState.StartScreen;
        private GameState _nextState;
        private GameState _currentLevel;
        public bool changeState = false;

        // Screens
        private StartScreen _startScreen = new StartScreen();
        private DeathScreen _deathScreen = new DeathScreen();
        private VictoryScreen _victoryScreen = new VictoryScreen();

        private Level1 _level1;
        private Level2 _level2;

        // Player
        public static Wizard wizard;

        private GameManager() { }

        public static GameManager getInstance()
        {
            if(Instance == null)
            {
                Instance = new GameManager();
            }
            return Instance;
        }

        public void UpdateGameState(GameState newState)
        {
            if (newState == GameState.Level1)
            {
                resetWizard();
            }
            _nextState = newState;
            changeState = true;
        }

        public void Update(GameTime gameTime)
        {
            if(changeState = true)
            {
                _currentState = _nextState;
                changeState = false;
                switch(_currentState)
                {
                    case GameState.Level1:
                        _currentLevel = GameState.Level1;
                        _level1 = new Level1();
                        wizard.Hitbox.X = (int)MapManager.PlayerSpawn.X;
                        wizard.Hitbox.Y = (int)MapManager.PlayerSpawn.Y;
                        break;
                    case GameState.Level2:
                        _currentLevel = GameState.Level2;
                        _level2 = new Level2();
                        wizard.Hitbox.X = (int)MapManager.PlayerSpawn.X;
                        wizard.Hitbox.Y = (int)MapManager.PlayerSpawn.Y;
                        break;
                }
            }
            switch (_currentState)
            {
                case GameState.StartScreen:
                    _startScreen.Update(gameTime);
                    break;
                case GameState.DeathScreen:
                    _deathScreen.Update(gameTime);
                    break;
                case GameState.VictoryScreen:
                    _victoryScreen.Update(gameTime);
                    break;
                case GameState.Level1:
                    _level1.Update(gameTime);
                    break;
                case GameState.Level2:
                    _level2.Update(gameTime);
                    break;

            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            switch (_currentState)
            {
                case GameState.StartScreen:
                    _startScreen.Draw(spritebatch);
                    break;
                case GameState.DeathScreen:
                    _deathScreen.Draw(spritebatch);
                    break;
                case GameState.VictoryScreen:
                    _victoryScreen.Draw(spritebatch);
                    break;
                case GameState.Level1:
                    _level1.Draw(spritebatch);
                    break;
                case GameState.Level2:
                    _level2.Draw(spritebatch);
                    break;
            }
        }

        private void resetWizard()
        {
            if(wizard == null)
            {
                wizard = new Wizard(ContentLoader.RunTexture, ContentLoader.IdleTexture, ContentLoader.DeathTexture, new KeyboardReader(), new MovementManager());
            }
            else
            {
                wizard.Health = 3;
            }
        }

        public GameState getCurrentState()
        {
            return _currentState;
        }
        
    }
}
