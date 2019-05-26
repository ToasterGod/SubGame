using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    class HelicopterElement : MovingElement
    {
        public HelicopterElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager) 
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager)
        {

        }

        public override void LoadContent(ContentManager aContentManager, string anAsset)
        {
            base.LoadContent(aContentManager, anAsset);
        }

        public override void Update(GameTime aGameTime)
        {
            base.Update(aGameTime);
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            base.Draw(aSpriteBatch);
        }
    }
}
