using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SubGame.Types;

namespace SubGame.Elements
{
    internal class EnemyElement : MovingElement
    {
        private readonly float[] directionLevels = { -1.0f, 1.0f };
        private int subSpeed;

        public int Depth { get; private set; }

        private ContentManager contentManager;
        private string[] assets;

        private int behindLeftEdge;
        private int behindRightEdge;

        //Mine stuff
        private List<MineElement> mines;
        private string weaponAsset;

        public EnemyElement(float scale, float direction, float rotation, float speed, Vector2 position, GraphicsDeviceManager manager)
            : base(scale, direction, rotation, speed, position, manager)
        {
            GenerateNewEnemy();
        }

        private void GenerateNewEnemy()
        {
            subSpeed = RandomNumber.Between(40, 130);
            //Depth = depthLevels[RandomNumber.Between(1, 3) - 1];
            Depth = RandomNumber.Between(450, 850);
            Direction = directionLevels[RandomNumber.Between(1, 2) - 1];
            //Only initial position for Depth value, it will be final after LoadContent
            Position = new Vector2(manager.PreferredBackBufferWidth, Depth);
            GenerateNewWeapons();
        }

        private void GenerateNewWeapons()
        {
            mines = new List<MineElement>();
            for (int i = 0; i < 4; i++)
            {
                mines.Add(new MineElement(1.0f, Direction, Rotation, 1.0f, Position, manager));
            }
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
            foreach (var mine in mines)
            {
                mine.LoadContent(contentManager, weaponAsset);
                mine.Position = new Vector2(Position.X + Size.Width / 2, Position.Y + Size.Height / 2);
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

        public override void Update(GameTime gameTime)
        {
            //Add pre update stuff here:

            //Then call base update
            base.Update(gameTime);

            //Add post update stuff here:
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

            // Calculate the movement
            CalcHorizontalMovement(Speed);

            foreach (var mine in mines.Where(m => m.Released == false))
            {
                mine.Update(gameTime);
                mine.Position = new Vector2(Position.X + Size.Width / 2, Position.Y + Size.Height / 2);
            }

            foreach (var mine in mines.Where(m => m.Released == true))
            {
                mine.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Mines are drawn before sub, that way they will be hidden behind the sub
            base.Draw(spriteBatch);
            foreach (var mine in mines)
            {
                mine.Draw(spriteBatch);
            }
        }

        public void BoatIsFoundAt(Rectangle location)
        {
            //throw new NotImplementedException();
        }

        public void ReleaseMine()
        {
            var mine = mines.FirstOrDefault(m => m.Released == false);
            if (mine != null)
            {
                mine.Released = true;
                mine.Direction = -1.0f;
                mine.Speed = 10.0f;
                mine.Position = Position;
            }
        }
    }
}
