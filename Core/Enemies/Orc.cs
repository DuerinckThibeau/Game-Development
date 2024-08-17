using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameDev.Core.Animations;
using GameDev.Core.Interfaces;

namespace GameDev.Core.Enemies
{
    public class Orc : IGameObject
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float _scale;
        private Animation _animation;

        public Rectangle Hitbox { get; private set; }

        public Orc(Texture2D texture, Vector2 position, float scale)
        {
            _texture = texture;
            _position = position;
            _scale = scale;
            _animation = new Animation();
            _animation.AddAnimation(4, 32, 32);
            UpdateHitbox();
        }

        public void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
            UpdateHitbox();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var currentFrame = _animation.currentFrame;
            spriteBatch.Draw(_texture, _position, currentFrame.sourceRectangle, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)_position.X, (int)_position.Y, 25, 32);
        }
    }
}
