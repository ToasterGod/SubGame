using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Levels
{
    public interface ILevel
    {
        int AccessBoatHits { get; }
        int AccessSubHits { get; }
        int AccessBoatHitsAllowed { get; }
        int AccessSubHitsRequired { get; }
        void Initialize();
        void LoadContent();
        void Update(GameTime aGameTime);
        void Draw(SpriteBatch mySpriteBatch, GameTime gameTime);
    }
}
