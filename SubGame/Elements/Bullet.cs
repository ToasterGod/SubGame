using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubGame.Elements
{
    public class Bullet
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 angularVelocity;
        public Rectangle Size => new Rectangle(0, 0, texture.Width, texture.Height);
        public Rectangle MyHitBox => new Rectangle(position.ToPoint(), new Point(Size.Width, Size.Height));

        /// <summary>
        /// Defines a bullet.
        /// </summary>
        /// <param name="bulletTexture">Texture for the bullet.</param>
        /// <param name="bulletOrigin">Position the bullet will originate from.</param>
        /// <param name="bulletSpeed">Speed of the bullet.</param>
        /// <param name="bulletAngle">Angle of the bullet (in radians).</param>
        public Bullet(Texture2D bulletTexture, Vector2 bulletOrigin, float bulletSpeed, float bulletAngle)
        {
            texture = bulletTexture;
            position = bulletOrigin;
            // create our angle from the passed in angle
            Vector2 angleVector = new Vector2((float)Math.Cos(bulletAngle), -(float)Math.Sin(bulletAngle));
            // multiply the angle vector by the bullet to get its angular velocity (velocity on some angle*)
            angularVelocity = angleVector * bulletSpeed;
        }

        public void Update(GameTime gameTime)
        {
            // time since last update
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // multiply the angular velocity by the time elapsed for delta correction (unstable framerate)
            // then add it to the position
            position += angularVelocity * delta;
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            Rectangle mySourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 myOrigin = new Vector2(0, 0);
            SpriteEffects myEffects = SpriteEffects.None;
            //if (AccessDirection < 0.0f)
            //{
            //    myEffects = SpriteEffects.FlipHorizontally;
            //}
            aSpriteBatch.Draw(texture, position, mySourceRectangle, Color.White, 0.3f, myOrigin, 1.0f, myEffects, 1);
            //base.Draw(aSpriteBatch);
        }
    }
}
