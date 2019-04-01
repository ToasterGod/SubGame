using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SubGame.Types;

namespace SubGame.Elements
{
    internal class SkyElement : MovingElement
    {
        public bool OutOfBounds { get; set; }

        public SkyElement(float scale, float direction, float rotation, float speed, Vector2 position, GraphicsDeviceManager manager) : base(scale, direction, rotation, speed, position, manager)
        {
            Direction = -1.0f;
            Position = new Vector2(manager.PreferredBackBufferWidth, RandomNumber.Between(1, 50));
        }

        public void LoadContent(ContentManager contentManager, string[] assets)
        {
            int random = RandomNumber.Between(1, assets.Length) - 1;
            LoadContent(contentManager, assets[random]);
        }

        public override void Update(GameTime gameTime)
        {
            //// Reset the sky if it is outside the left or right edge
            if (Position.X + Size.Width < 0 && Direction < 0.0f)
            {
                //Sky going left outside of left edge
                OutOfBounds = true;
            }
            else if (Position.X > manager.PreferredBackBufferWidth && Direction > 0.0f)
            {
                //Sky going right outside of right edge
                OutOfBounds = true;
            }

            // Calculate the movement of all the clouds
            CalcHorizontalMovement(Speed);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
