using GameDev.Core.Input;
using GameDev.Core.Player;
using GameDev.Core.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Level3 _level3;



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
                        wizard.futureHitbox.X = (int)MapManager.PlayerSpawn.X;
                        wizard.futureHitbox.Y = (int)MapManager.PlayerSpawn.Y;
                        break;
                }
            }
            switch (_currentState)
            {
                case GameState.StartScreen:
                    _startScreen.Update(gameTime);
                    break;
                case GameState.Level1:
                    _level1.Update(gameTime);
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
                case GameState.Level1:
                    _level1.Draw(spritebatch);
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
