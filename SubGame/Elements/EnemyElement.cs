using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SubGame.Types;

namespace SubGame.Elements
{
    internal class EnemyElement : MovingElement
    {
        //private List<Texture> textures = new List<Texture>();
        private readonly int[] depthLevels = { 450, 650, 850 };
        private readonly float[] directionLevels = { -1.0f, 1.0f };
        private readonly int mySubTop = 450; // Where the shallowest sub will show up
        private readonly int mySubSpacing = 100; // Constant value for space between subs
        private Random random;
        private int subSpeed;

        public int Depth { get; private set; }

        private int startPoint;
        private ContentManager contentManager;
        private string[] assets;
        private string weaponAsset;

        private int behindLeftEdge;
        private int behindRightEdge;

        public EnemyElement(float scale, float direction, float rotation, float speed, Vector2 position, GraphicsDeviceManager manager)
            : base(scale, direction, rotation, speed, position, manager)
        {
            GenerateNewEnemy();
        }

        public void LoadContent(ContentManager contentManager, string[] assets, string weaponAsset)
        {
            this.contentManager = contentManager;
            this.assets = assets;
            this.weaponAsset = weaponAsset;
            if (subSpeed < 70)
            {
                //slow sub
                Speed = 0.5f;
                LoadContent(contentManager, assets[0]);
            }
            else if (subSpeed < 100)
            {
                //fast sub
                Speed = 0.8f;
                LoadContent(contentManager, assets[1]);
            }
            else
            {
                // fastest sub
                Speed = 1.2f;
                LoadContent(contentManager, assets[2]);
            }
        }

        public override void LoadContent(ContentManager contentManager, string asset)
        {
            base.LoadContent(contentManager, asset);

            //Set left and right startpoints
            behindLeftEdge = 0 - Size.Width;
            behindRightEdge = manager.PreferredBackBufferWidth;
            if (Direction > 0.0f)
            {
                Position = new Vector2(behindLeftEdge, Position.Y);
            }
            else if (Direction < 0.0f)
            {
                Position = new Vector2(behindRightEdge, Position.Y);
            }
        }

        public void LoadWeapon()
        {
        }

        public override void Update(GameTime gameTime)
        {
            // Reset the subs if any of them is outside the left edge
            if (Position.X + Size.Width < 0 && Direction < 0.0f)
            {
                //Sub going left outside of left edge
                GenerateNewEnemy();
                LoadContent(contentManager, assets, weaponAsset);
            }
            else if (Position.X > manager.PreferredBackBufferWidth && Direction > 0.0f)
            {
                //Sub going right outside of right edge
                GenerateNewEnemy();
                LoadContent(contentManager, assets, weaponAsset);
            }

            // Calculate the movement of all the subs (to the left)
            CalcMovement(Speed);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void GenerateNewEnemy()
        {
            subSpeed = RandomNumber.Between(40, 130);
            //Depth = depthLevels[RandomNumber.Between(1, 3) - 1];
            Depth = RandomNumber.Between(450, 850);
            Direction = directionLevels[RandomNumber.Between(1, 2) - 1];
            //Only initial position for Depth value, it will be final after LoadContent
            Position = new Vector2(manager.PreferredBackBufferWidth, Depth);
        }

        public void BoatIsFoundAt(Rectangle location)
        {
            //throw new NotImplementedException();
        }
    }
}
