using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    internal class MovingElement : Element
    {
        protected GraphicsDeviceManager manager;
        protected Texture2D texture;

        public float Direction { get; set; }
        public float Rotation { get; set; }
        public float Speed { get; set; }
        public float Scale { get; set; }
        public Texture2D Texture => texture;

        public MovingElement(float scale, float direction, float rotation, float speed, Vector2 position, GraphicsDeviceManager manager)
            : base(position)
        {
            Scale = scale;
            Direction = direction;
            Rotation = rotation;
            Speed = speed;
            this.manager = manager;
        }

        public virtual void LoadContent(ContentManager contentManager, string asset)
        {
            texture = contentManager.Load<Texture2D>(asset);
            Size = new Rectangle(0, 0, (int)Math.Round(texture.Width * Scale), (int)Math.Round(texture.Height * Scale));
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(0, 0);
            SpriteEffects effects = SpriteEffects.None;
            if (Direction < 0.0f)
                effects = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, Position, sourceRectangle, Color.White, Rotation, origin, Scale, effects, 1);
        }

        protected void CalcHorizontalMovement(float speed)
        {
            if (Direction < 0.0f || Direction > 0.0f)
            {
                Vector2 direction = new Vector2(Direction, 0);
                direction.Normalize();
                Position += direction * speed;
            }
        }

        protected void CalcVerticalMovement(float speed)
        {
            if (Direction < 0.0f || Direction > 0.0f)
            {
                Vector2 direction = new Vector2(0, Direction);
                direction.Normalize();
                Position += direction * speed;
            }
        }
    }
}