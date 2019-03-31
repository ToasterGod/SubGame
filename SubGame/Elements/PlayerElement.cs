using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SubGame.Elements
{
    public delegate void WhereIsTheBoatDelegate(Rectangle location);

    internal class PlayerElement : MovingElement
    {
        private int leftEdge;
        private int rightEdge;
        private Texture2D textureSinkbomb;

        public Rectangle CollisionBox { get; internal set; }
        public WhereIsTheBoatDelegate WhereIsTheBoat { get; set; }

        public PlayerElement(float scale, float direction, float rotation, float speed, Vector2 position, GraphicsDeviceManager manager)
            : base(scale, direction, rotation, speed, position, manager)
        {
        }

        public void LoadContent(ContentManager contentManager, string asset, string weaponAsset)
        {
            LoadContent(contentManager, asset);
            //Skapa och Ladda vapen
            textureSinkbomb = contentManager.Load<Texture2D>(weaponAsset);
        }

        public override void LoadContent(ContentManager contentManager, string asset)
        {
            base.LoadContent(contentManager, asset);
            //Level boat at sealevel and set left and right endpoints
            leftEdge = 60;
            rightEdge = manager.PreferredBackBufferWidth - 60 - Size.Width;
            Position = new Vector2(leftEdge, Position.Y - Size.Height * 0.66f);
        }

        public override void Update(GameTime gameTime)
        {
            Direction = 0.0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Direction = -1.0f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Direction = 1.0f;
            }

            //Keep the boat within screens left and right edge
            if (Position.X <= leftEdge && Direction < 0.0f)
            {
                Direction = 0.0f;
            }
            else if (Position.X > (rightEdge) && Direction > 0.0f)
            {
                Direction = 0.0f;
            }

            CalcMovement(Speed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            WhereIsTheBoat?.Invoke(new Rectangle(Position.ToPoint(), Size.Size));

            base.Draw(spriteBatch);
        }
    }
}
