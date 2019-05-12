using ConfigModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SubGame.Extensions;
using SubGame.Types;
using System.Collections.Generic;
using System.Linq;

namespace SubGame.Elements
{
    public class SeaCreatureElement : MovingElement
    {
        private readonly float[] myDirectionLevels = { -1.0f, 1.0f };
        private int myCreatureSpeed;

        public int GetDepth { get; private set; }

        private ContentManager myContentManager;
        private string[] myAssets;

        private int myBehindLeftEdge;
        private int myBehindRightEdge;
        
        private readonly int mySurfaceLevel;

        public SeaCreatureElement(int aSurfaceLevel, float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager)
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager)
        {
            mySurfaceLevel = aSurfaceLevel;
            AccessIsEnemy = false;
            GenerateNewCreature();
        }

        private void GenerateNewCreature()
        {
            myCreatureSpeed = RandomNumber.Between(40, 130);
            GetDepth = RandomNumber.Between(450, 850);
            AccessDirection = myDirectionLevels[RandomNumber.Between(1, 2) - 1];
            //Only initial position for Depth value, it will be final after LoadContent
            AccessPosition = new Vector2(myManager.PreferredBackBufferWidth, GetDepth);
        }

        public void LoadContent(ContentManager aContentManager, string[] anAssets)
        {
            myContentManager = aContentManager;
            myAssets = anAssets;
            if (myCreatureSpeed < 95)
            {
                // Whale
                AccessSpeed = (float)RandomNumber.Between(4, 8) / 10;
                LoadContent(aContentManager, anAssets[0]);
            }
            else
            {
                // Shark
                AccessSpeed = (float)RandomNumber.Between(10, 15) / 10;
                LoadContent(aContentManager, anAssets[1]);
            }
        }

        public override void LoadContent(ContentManager aContentManager, string anAsset)
        {
            base.LoadContent(aContentManager, anAsset);

            //Set left and right startpoints
            myBehindLeftEdge = 0 - AccessSize.Width;
            myBehindRightEdge = myManager.PreferredBackBufferWidth;
            if (AccessDirection > 0.0f)
            {
                AccessPosition = new Vector2(myBehindLeftEdge, AccessPosition.Y);
            }
            else if (AccessDirection < 0.0f)
            {
                AccessPosition = new Vector2(myBehindRightEdge, AccessPosition.Y);
            }
        }

        public override void Update(GameTime aGameTime)
        {
            //Add pre update stuff here:

            //Then call base update
            base.Update(aGameTime);

            //Add post update stuff here:

            // Reset the creatures if any of them is outside the left edge
            if (AccessPosition.X + AccessSize.Width < 0 && AccessDirection < 0.0f)
            {
                // Creatures going left outside of left edge, create a new
                ResetCreature();
            }
            else if (AccessPosition.X > myManager.PreferredBackBufferWidth && AccessDirection > 0.0f)
            {
                // Creatures going right outside of right edge, create a new
                ResetCreature();
            }

            // Calculate the movement
            CalcHorizontalMovement(AccessSpeed);
        }

        public void ResetCreature()
        {
            GenerateNewCreature();
            LoadContent(myContentManager, myAssets);
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            base.Draw(aSpriteBatch);
        }
    }
}