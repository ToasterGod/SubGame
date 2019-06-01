using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Levels
{
    public class Level11To15 : LevelBase, ILevel
    {
        public Level11To15(GraphicsDeviceManager aGraphics, ContentManager aContent, LevelData aLevelConfig)
            : base(aGraphics, aContent, aLevelConfig)
        {
        }

        public int AccessBoatHits => myBoatHits;
        public int AccessSubHits => mySubHits;
        public int AccessBoatHitsAllowed => myConfig.Health;
        public int AccessSubHitsRequired => myConfig.RequiredSubHits;

        public void Initialize()
        {
        }

        public void LoadContent()
        {
        }

        public void Update(GameTime aGameTime)
        {
        }

        public void Draw(SpriteBatch mySpriteBatch, GameTime gameTime)
        {
        }

    }
}
