using ConfigModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SubGame.Elements;

namespace SubGame.Levels
{
    public class Level6To10 : LevelBase, ILevel
    {
        #region Inheritance implementation
        public Level6To10(GraphicsDeviceManager aGraphics, ContentManager aContent, LevelData aLevelConfig)
            : base(aGraphics, aContent, aLevelConfig)
        {}
        #endregion

        #region Interface implementation methods
        public int AccessBoatHits => myBoatHits;
        public int AccessSubHits => mySubHits;
        public int AccessBoatHitsAllowed => myConfig.Health;
        public int AccessSubHitsRequired => myConfig.RequiredSubHits;
        

        public void Draw(SpriteBatch mySpriteBatch, GameTime gameTime)
        {
        }
        public void Initialize()
        {
        }

        public void LoadContent()
        {
        }

        public void Update(GameTime aGameTime)
        {
        }
        #endregion

        #region Internal methods
        #endregion
    }
}
