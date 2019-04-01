
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    internal class MineElement : MovingElement
    {
        public bool Released { get; set; }

        public MineElement(float scale, float direction, float rotation, float speed, Vector2 position, GraphicsDeviceManager manager)
            : base(scale, direction, rotation, speed, position, manager)
        {
            //Released = false;
        }

        public override void LoadContent(ContentManager contentManager, string asset)
            => base.LoadContent(contentManager, asset);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CalcVerticalMovement(Speed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(0, 0);
            SpriteEffects effects = SpriteEffects.None;
            if (Direction < 0.0f)
                effects = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, Position, sourceRectangle, Color.White, Rotation, origin, Scale, effects, 1);

            //base.Draw(spriteBatch);
        }
    }
}
