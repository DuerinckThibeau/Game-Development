using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace GameDev.Core.Managers
{
    internal class ContentLoader
    {
        private static ContentManager _content;

        // Startscreen
        public static Texture2D BackgroundTexture;
        public static Texture2D StartButtonTexture;

        // Deathscreen
        public static Texture2D DeathScreenTexture;
        public static Texture2D ExitButtonTexture;

        // Enemy textures
        public static Texture2D OrcTexture;

        // Player textures
        public static Texture2D HeartTexture;
        public static Texture2D IdleTexture;
        public static Texture2D RunTexture;
        public static Texture2D DeathTexture;

        // Level 1 
        public static TmxMap Level1;
        public static Texture2D MapBackground;
        public static Texture2D TilesetTexture;

        // Level 2
        public static TmxMap Level2;
        public static Texture2D CaveBackground;


        public ContentLoader(ContentManager contentManager)
        {
            _content = contentManager;
        }

        public static void LoadContent()
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

            // Level 2
            Level2 = new TmxMap("Content/Maps/Level_2.tmx");
            CaveBackground = _content.Load<Texture2D>("Tiles/World2");
        }
    }
}
