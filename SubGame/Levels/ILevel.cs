using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubGame.Elements;

namespace SubGame.Levels
{
    public interface ILevel
    {
        int AccessBoatHits { get; }
        int AccessSubHits { get; }
        int AccessBoatHitsAllowed { get; }
        int AccessSubHitsRequired { get; }
        PlayerElement AccessBoat { get; }
        int AccessSinkingSinkBombsThatAreSinking { get; }

        void Initialize();
        void LoadContent();
        void Update(GameTime aGameTime);
        void Draw(SpriteBatch mySpriteBatch, GameTime gameTime);
    }
}
