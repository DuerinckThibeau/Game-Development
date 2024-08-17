using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace GameDev.Core.Managers
{
    public class ContentLoader
    {
        private readonly ContentManager _content;

        // Startscreen
        public Texture2D BackgroundTexture { get; private set; }
        public Texture2D StartButtonTexture { get; private set; }

        // Deathscreen
        public Texture2D DeathScreenTexture { get; private set; }
        public Texture2D ExitButtonTexture { get; private set; }

        // Enemy textures
        public Texture2D OrcTexture { get; private set; }

        // Player textures
        public Texture2D HeartTexture { get; private set; }
        public Texture2D IdleTexture { get; private set; }
        public Texture2D RunTexture { get; private set; }
        public Texture2D DeathTexture { get; private set; }

        // Level 1 
        public TmxMap Level1 { get; private set; }
        public Texture2D MapBackground { get; private set; }
        public Texture2D TilesetTexture { get; private set; }


        public ContentLoader(ContentManager contentManager)
        {
            _content = contentManager;
        }

        public void LoadContent()
        {
            // Startscreen
            BackgroundTexture = _content.Load<Texture2D>("UI/StartScreen");
            StartButtonTexture = _content.Load<Texture2D>("UI/Startbutton");

            // Deathscreen
            DeathScreenTexture = _content.Load<Texture2D>("UI/DeathScreen");
            ExitButtonTexture = _content.Load<Texture2D>("UI/ExitButton");

            // Enemy textures
            OrcTexture = _content.Load<Texture2D>("Enemies/Orc-Idle");

            // Player
            HeartTexture = _content.Load<Texture2D>("UI/Heart");
            IdleTexture = _content.Load<Texture2D>("Wizard/Idle-Sheet");
            RunTexture = _content.Load<Texture2D>("Wizard/Run-Sheet");
            DeathTexture = _content.Load<Texture2D>("Wizard/Death-Sheet");

            // Level 1
            Level1 = new TmxMap("Content/Maps/Level_1.tmx");
            MapBackground = _content.Load<Texture2D>("Tiles/World1");
            TilesetTexture = _content.Load<Texture2D>("Tiles/Assets");
        }
    }
}
