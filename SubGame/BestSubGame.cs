using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SubGame.Objects;

namespace SubGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BestSubGame : Game
    {
        private GraphicsDeviceManager myGraphics;
        private SpriteBatch mySpriteBatch;

        private readonly int mySkyTop = 0; // Top position of the Sky graphics
        private readonly int myOceanTop = 280; // Top position of the Ocean graphics

        private readonly int mySubTop = 450; // Where the shallowest sub will show up
        private readonly int mySubSpacing = 100; // Constant value for space between subs

        private readonly int myMineTop = 300; // 20 below OceanTop

        private readonly int myBoatFloatLevel = 120; // Controls how far down the boat will show
        private readonly int myBoatLeftEdge = 60; // Left edge for the boat
        private readonly int myBoatRightEdge = 1860; // Right edge for the boat

        // Set the direction of the background movement.
        private readonly Vector2 myGameDirection = new Vector2(-1, 0); //-1 = move background left

        // Set the direction of mine movement.
        private readonly Vector2 myMineDirection = new Vector2(0, -1); //-1 = move upwards

        // Set the direction of boat movement.
        private Vector2 myBoatDirection = new Vector2(0, 0); //0 = Stand still

        // Set the speed of the background movement.
        private Vector2 myBackgroundSpeed = new Vector2(120, 0); //120 = speed of movement

        private AnimatedBackground[] mySkies = new AnimatedBackground[7];

        private AnimatedBackground[] myOceans = new AnimatedBackground[7];

        private AnimatedObject myBoat;

        private AnimatedObject[] mySubs = new AnimatedObject[3];
        private AnimatedObject[] myMines = new AnimatedObject[3];// One mine for each sub
        //private readonly AnimatedObject mySinkBomb;

        StatusPanel statusPanel;

        private Random myRNG = new Random();

        public BestSubGame()
        {
            myGraphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920, // set this value to the desired width of your window, 1280 is better on lower resolution screen
                PreferredBackBufferHeight = 1080 // set this value to the desired height of your window, 1024 is better on lower resolution screen
            };
            myGraphics.ApplyChanges();
            myBoatRightEdge = myGraphics.PreferredBackBufferWidth - 60;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Instantiate a new AnimatedBackground object for each element in the skies array and set scale for the size of each
            for (int i = 0; i < mySkies.Length; i++)
            {
                mySkies[i] = new AnimatedBackground(0.15f);
            }

            // Instantiate a new AnimatedBackground object for each element in the oceans array and set scale for the size of each
            for (int i = 0; i < myOceans.Length; i++)
            {
                myOceans[i] = new AnimatedBackground(0.5f);
            }

            // Instantiate three subs
            for (int i = 0; i < mySubs.Length; i++)
            {
                mySubs[i] = new AnimatedObject(0.6f, new Vector2(myRNG.Next(40, 130), 0));
            }

            // Instantiate three mines
            for (int i = 0; i < myMines.Length; i++)
            {
                myMines[i] = new AnimatedObject(0.6f, new Vector2(0, 50));
            }

            myBoat = new AnimatedObject(1.0f, new Vector2(120, 0));

            statusPanel = new StatusPanel(new Rectangle(myGraphics.PreferredBackBufferWidth - 410, 10, 400, 110), myGraphics.GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load all the skies images by calling the AnimatedBackground.LoadContent for each one of them
            for (int i = 0; i < mySkies.Length; i++)
            {
                LoadBackground(mySkies[i], "Backgrounds/Himmel",
                    new Vector2(i == 0 ? 0.0f : mySkies[i - 1].AccessPosition.X + mySkies[i - 1].AccessSize.Width, mySkyTop));
            }

            // Load all the oceans images by calling the AnimatedBackground.LoadContent for each one of them
            for (int i = 0; i < myOceans.Length; i++)
            {
                LoadBackground(myOceans[i], "Backgrounds/Ocean",
                    new Vector2(i == 0 ? 0.0f : myOceans[i - 1].AccessPosition.X + myOceans[i - 1].AccessSize.Width, myOceanTop));
            }

            // Load subs
            for (int i = 0; i < mySubs.Length; i++)
            {
                LoadSub(mySubs[i], i, mySubTop);
            }

            // Load mines
            for (int i = 0; i < myMines.Length; i++)
            {
                myMines[i].LoadContent(Content, "Backgrounds/Mine");
                // Set initial position of the mine to the width and height of 2 mines beyond the right, bottom of the screen
                myMines[i].AccessPosition = new Vector2(myGraphics.PreferredBackBufferWidth + myMines[i].AccessSize.Width * 2,
                        myGraphics.PreferredBackBufferHeight + myMines[i].AccessSize.Height * 2);
            }

            // Load the boat from the content stream
            myBoat.LoadContent(Content, "Backgrounds/Boat");
            myBoat.AccessPosition = new Vector2(myBoatLeftEdge, myBoatFloatLevel);

            statusPanel.LoadContent(Content, "Status");
        }

        private void LoadBackground(AnimatedBackground aBackground, string aResource, Vector2 aPosition)
        {
            aBackground.LoadContent(Content, aResource);
            aBackground.AccessPosition = aPosition;
        }

        private void LoadSub(AnimatedObject aSub, int aIndex, int aTop)
        {
            if (aSub.AccessSpeed.X < 70)
            {
                //slow sub
                aSub.LoadContent(Content, "Backgrounds/SlowSub");
            }
            else if (aSub.AccessSpeed.X < 100)
            {
                //fast sub
                aSub.LoadContent(Content, "Backgrounds/MediumSub");
            }
            else
            {
                // fastest sub
                aSub.LoadContent(Content, "Backgrounds/FastSub");
            }
            // Start outside the right edge of the screen
            aSub.AccessPosition = new Vector2(myGraphics.PreferredBackBufferWidth + aSub.AccessSize.Width * 2,
                aTop + aIndex * (aSub.AccessSize.Height + mySubSpacing));
            aSub.AccessFireAt = myRNG.Next(100, myGraphics.PreferredBackBufferWidth - 100);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime aGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                myBoatDirection = new Vector2(1, 0);
            }
            else
                myBoatDirection = new Vector2(-1, 0);

            // Reset the skies array if the first image is fully outside of the screen to the left
            // then put it as the last element. That way we will feed the first image in as the last image once it has passed by
            // Gives a kind of rotating function
            for (int target = 0; target < mySkies.Length; target++)
            {
                int tempSource = mySkies.Length - 1;
                if (target > 0)
                {
                    tempSource = target - 1;
                }

                if (mySkies[target].AccessPosition.X < -mySkies[target].AccessSize.Width)
                {
                    mySkies[target].AccessPosition = new Vector2(mySkies[tempSource].AccessPosition.X + mySkies[tempSource].AccessSize.Width,
                        mySkies[target].AccessPosition.Y);
                }
            }

            // Reset the oceans array if the first image is fully outside of the screen to the left
            // then put it as the last element. That way we will feed the first image in as the last image once it has passed by
            // Gives a kind of rotating function
            for (int target = 0; target < myOceans.Length; target++)
            {
                int tempSource = myOceans.Length - 1;
                if (target > 0)
                {
                    tempSource = target - 1;
                }

                if (myOceans[target].AccessPosition.X < -myOceans[target].AccessSize.Width)
                {
                    myOceans[target].AccessPosition = new Vector2(myOceans[tempSource].AccessPosition.X + myOceans[tempSource].AccessSize.Width,
                        myOceans[target].AccessPosition.Y);
                }
            }

            // Reset the subs if any of them is outside the left edge
            for (int target = 0; target < mySubs.Length; target++)
            {
                // If submarine has moved outside of the screen to the far left
                if (mySubs[target].AccessPosition.X < -mySubs[target].AccessSize.Width)
                {
                    // Create a new submarine outside the far right side of the screen
                    mySubs[target] = new AnimatedObject(mySubs[target].AccessScale, new Vector2(myRNG.Next(40, 130), 0));
                    LoadSub(mySubs[target], target, mySubTop);
                    //mySubs[target].AccessPosition = new Vector2(myGraphics.PreferredBackBufferWidth + mySubs[target].AccessSize.Width * 2,
                    //    mySubTop + target * mySubs[target].AccessSize.Height);
                    // Reset the CollisionBox for that new submarine
                    //mySubs[target].AccessCollisionBox = new Rectangle(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
                }
            }

            // If submarine has moved outside of the screen to the far left
            if (myBoat.AccessPosition.X <= myBoatLeftEdge &&
                myBoatDirection.X == -1)
            {
                myBoatDirection = new Vector2(0, 0);
            }
            else if (myBoat.AccessPosition.X > (myBoatRightEdge - myBoat.AccessSize.Width) &&
                myBoatDirection.X == 1)
            {
                myBoatDirection = new Vector2(0, 0);
            }

            // Reset the mine if it reach water surface without hit
            for (int target = 0; target < myMines.Length; target++)
            {
                // If the mine is at sealevel
                if (myMines[target].AccessPosition.Y < myMineTop)
                {
                    myMines[target].AccessPosition = new Vector2(myGraphics.PreferredBackBufferWidth + myMines[target].AccessSize.Width * 2,
                        myGraphics.PreferredBackBufferHeight + myMines[target].AccessSize.Height * 2);
                    myMines[target].AccessCollisionDetected = false;
                    myMines[target].AccessCollisionRegistered = false;
                }
            }

            // Calculate the movement of all the skies elements (to the left)
            for (int i = 0; i < mySkies.Length; i++)
            {
                mySkies[i].AccessPosition += myGameDirection * myBackgroundSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }

            // Calculate the movement of all the oceans elements (to the left)
            for (int i = 0; i < myOceans.Length; i++)
            {
                myOceans[i].AccessPosition += myGameDirection * myBackgroundSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }

            // Calculate the movement of all the subs (to the left)
            for (int i = 0; i < mySubs.Length; i++)
            {
                mySubs[i].AccessPosition += myGameDirection * mySubs[i].AccessSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;

                // Let the sub drop a mine randomly, each sub got one mine
                if (mySubs[i].AccessPosition.X > mySubs[i].AccessFireAt - 50 && mySubs[i].AccessPosition.X < mySubs[i].AccessFireAt + 50 && myMines[i].AccessPosition.Y > myGraphics.PreferredBackBufferHeight)
                {
                    myMines[i].AccessPosition = new Vector2(mySubs[i].AccessPosition.X, mySubs[i].AccessPosition.Y);
                }
            }

            for (int i = 0; i < myMines.Length; i++)
            {
                if (myMines[i].AccessPosition.X < myGraphics.PreferredBackBufferWidth && myMines[i].AccessPosition.Y < myGraphics.PreferredBackBufferHeight)
                {
                    // Calculate the movement of the mine (to the top)
                    myMines[i].AccessPosition += myMineDirection * myMines[i].AccessSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            //if (myBoat.AccessPosition.X >= 60 || myBoat.AccessPosition.X >= myGraphics.PreferredBackBufferWidth - myBoat.AccessSize.Width - 60)
            myBoat.AccessPosition += myBoatDirection * myBoat.AccessSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;

            base.Update(aGameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // Begin your drawing code here
            mySpriteBatch.Begin();

            // Call Draw for each sky element in the skies array
            for (int i = 0; i < mySkies.Length; i++)
            {
                mySkies[i].Draw(mySpriteBatch);
            }

            // Call draw for the boat
            myBoat.Draw(mySpriteBatch);

            for (int i = 0; i < mySubs.Length; i++)
            {
                mySubs[i].Draw(mySpriteBatch);
            }

            for (int i = 0; i < myMines.Length; i++)
            {
                myMines[i].AccessCollisionDetected = myMines[i].AccessCollisionBox.Intersects(myBoat.AccessCollisionBox);

                if (myMines[i].AccessCollisionDetected && !myMines[i].AccessCollisionRegistered)
                {
                    statusPanel.BoatsHit++;
                    myMines[i].AccessCollisionRegistered = true;
                }

                myMines[i].Draw(mySpriteBatch);
            }

            // Call Draw for each ocean object in the oceans array
            for (int i = 0; i < myOceans.Length; i++)
            {
                myOceans[i].Draw(mySpriteBatch);
            }

            statusPanel.Draw(mySpriteBatch);

            // End your drawing code here
            mySpriteBatch.End();

            // Call the Draw method in the base
            base.Draw(gameTime);
        }
    }
}