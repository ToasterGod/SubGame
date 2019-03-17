using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SubGame.Animations;

namespace SubGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BestSubGame : Game
    {
        private GraphicsDeviceManager myGraphics;
        private SpriteBatch mySpriteBatch;

        private AnimatedBackground[] mySkies = new AnimatedBackground[7];

        private AnimatedBackground[] myOceans = new AnimatedBackground[7];

        private Texture2D myBoat;
        private Texture2D myBoom;

        private AnimatedObject[] mySubs = new AnimatedObject[3];
        private AnimatedObject myMine;
        private AnimatedObject mySinkBomb;

        private Random myRNG = new Random();
        private Rectangle test = new Rectangle(200, 200, 20, 1000);

        public BestSubGame()
        {
            myGraphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920, // set this value to the desired width of your window, 1280 is better on lower resolution screen
                PreferredBackBufferHeight = 1080 // set this value to the desired height of your window, 1024 is better on lower resolution screen
            };
            myGraphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Instantiate a new AnimatedBackground object for each element in the skies array and set scale for the size of each
            for (var i = 0; i < mySkies.Length; i++)
            {
                mySkies[i] = new AnimatedBackground
                {
                    // Scale size of each image
                    AccessScale = 0.15f
                };
            }

            // Instantiate a new AnimatedBackground object for each element in the oceans array and set scale for the size of each
            for (var i = 0; i < myOceans.Length; i++)
            {
                myOceans[i] = new AnimatedBackground
                {
                    // Scale size of each image
                    AccessScale = 0.5f
                };
            }

            for (int i = 0; i < mySubs.Length; i++)
            {
                mySubs[i] = new AnimatedObject
                {
                    AccessScale = 0.6f,
                    AccessSpeed = new Vector2(myRNG.Next(40, 130), 0)
                };
            }

            myMine = new AnimatedObject
            {
                AccessScale = 1.0f,
                AccessSpeed = new Vector2(0, 100)
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load all the skies images by calling the AnimatedBackground.LoadContent for each one of them
            for (var i = 0; i < mySkies.Length; i++)
            {
                mySkies[i].LoadContent(this.Content, "Backgrounds/Himmel");
                float tempAdjust;
                if (i == 0)
                    tempAdjust = 0.0f;
                else
                    tempAdjust = mySkies[i - 1].AccessPosition.X + mySkies[i - 1].AccessSize.Width;

                mySkies[i].AccessPosition = new Vector2(tempAdjust, 0);
            }

            // Load all the oceans images by calling the AnimatedBackground.LoadContent for each one of them
            for (var i = 0; i < myOceans.Length; i++)
            {
                myOceans[i].LoadContent(this.Content, "Backgrounds/Ocean");
                float tempAdjust;
                if (i == 0)
                    tempAdjust = 0.0f;
                else
                    tempAdjust = myOceans[i - 1].AccessPosition.X + myOceans[i - 1].AccessSize.Width;

                myOceans[i].AccessPosition = new Vector2(tempAdjust, 225);
            }

            for (int i = 0; i < mySubs.Length; i++)
            {
                /*
                 * den långsamma ubåten är en yellow submarine
                 * den snabba är samma fast med påmålade flammor
                 */
                if (mySubs[i].AccessSpeed.X < 70)
                {
                    //slow sub
                    mySubs[i].LoadContent(this.Content, "Backgrounds/SlowSub");
                }
                else if (mySubs[i].AccessSpeed.X < 100)
                {
                    //fast sub
                    mySubs[i].LoadContent(this.Content, "Backgrounds/MediumSub");
                }
                else
                {
                    // fastestest sub
                    mySubs[i].LoadContent(this.Content, "Backgrounds/FastSub");
                }
                mySubs[i].AccessPosition = new Vector2(myGraphics.PreferredBackBufferWidth + mySubs[i].AccessSize.Width + i * mySubs[i].AccessSize.Width,
                    360 + i * mySubs[i].AccessSize.Height);
            }

            myMine.LoadContent(this.Content, "Backgrounds/Mine");
            myMine.AccessPosition = new Vector2(-1, -1);

            // Load the boat from the content stream
            myBoat = Content.Load<Texture2D>("Backgrounds/Boat");
            myBoom = Content.Load<Texture2D>("Backgrounds/Boom");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime aGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Move all the images in the skies array and if the first image is fully outside of the screen to the left 
            // then put it as the last element. That way we will feed the first image in as the last image once it has passed by
            // Gives a kind of rotating function
            for (var target = 0; target < mySkies.Length; target++)
            {
                int tempSource = mySkies.Length - 1;
                if (target > 0)
                    tempSource = target - 1;

                if (mySkies[target].AccessPosition.X < -mySkies[target].AccessSize.Width)
                {
                    mySkies[target].AccessPosition = new Vector2(mySkies[tempSource].AccessPosition.X + mySkies[tempSource].AccessSize.Width,
                        mySkies[target].AccessPosition.Y);
                }
            }

            // Move all the images in the oceans array and if the first image is fully outside of the screen to the left 
            // then put it as the last element. That way we will feed the first image in as the last image once it has passed by
            // Gives a kind of rotating function
            for (var target = 0; target < myOceans.Length; target++)
            {
                int tempSource = myOceans.Length - 1;
                if (target > 0)
                    tempSource = target - 1;

                if (myOceans[target].AccessPosition.X < -myOceans[target].AccessSize.Width)
                {
                    myOceans[target].AccessPosition = new Vector2(myOceans[tempSource].AccessPosition.X + myOceans[tempSource].AccessSize.Width,
                        myOceans[target].AccessPosition.Y);
                }
            }

            for (var target = 0; target < mySubs.Length; target++)
            {

                // If submarine has moved outside of the screen to the far left
                if (mySubs[target].AccessPosition.X < -mySubs[target].AccessSize.Width)
                {
                    // Create a new submarine outside the far right side of the screen
                    mySubs[target].AccessPosition = new Vector2(myGraphics.PreferredBackBufferWidth + mySubs[target].AccessSize.Width + target * mySubs[target].AccessSize.Width,
                    350 + target * mySubs[target].AccessSize.Height);
                    // Reset the CollisionBox for that new submarine
                    mySubs[target].AccessCollisionBox = new Rectangle(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
                }
            }



            // Set the direction of the background movement
            Vector2 aDirection = new Vector2(-1, 0); //-1 = move background left
            Vector2 aMineDirection = new Vector2(0, -1); //-1 = move upwards

            // Set the speed of the background movement
            Vector2 aSpeed = new Vector2(120, 0); //120 = speed of movement

            // Calculate the movement of the oceans elements (to the left)
            for (var i = 0; i < mySkies.Length; i++)
            {
                mySkies[i].AccessPosition += aDirection * aSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }

            // Calculate the movement of the oceans elements (to the left)
            for (var i = 0; i < myOceans.Length; i++)
            {
                myOceans[i].AccessPosition += aDirection * aSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }

            for (int i = 0; i < mySubs.Length; i++)
            {
                mySubs[i].AccessPosition += aDirection * mySubs[i].AccessSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;

            }

            if (myMine.AccessPosition.X != -1 && myMine.AccessPosition.Y != -1)
            {
                myMine.AccessPosition += aMineDirection * myMine.AccessSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }

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
            for (var i = 0; i < mySkies.Length; i++)
            {
                mySkies[i].Draw(this.mySpriteBatch);
            }

            // Call draw for the boat, since it's a simple object it will be drawn by the spriteBatch itself
            mySpriteBatch.Draw(myBoat, new Vector2(60, 110), Color.White); // Make random y(130) to move boat up and down a couple of pixels

            for (int i = 0; i < mySubs.Length; i++)
            {
                // Do collisiondetection
                if (mySubs[i].AccessCollisionBox.Intersects(test))
                {
                    myMine.AccessPosition = new Vector2(mySubs[i].AccessCollisionBox.X, mySubs[i].AccessCollisionBox.Y);
                    //Bamalamadingdong-woopdidoopdicrashelicrash
                    mySpriteBatch.Draw(myBoom, new Vector2(mySubs[i].AccessCollisionBox.X, mySubs[i].AccessCollisionBox.Y), Color.White);
                    // Move submarine outside far left edge of the screen
                    if (mySubs[i].AccessCollisionTime > TimeSpan.MinValue)
                    {
                        if (mySubs[i].AccessCollisionTime.Add(new TimeSpan(0, 0, 2)) < gameTime.ElapsedGameTime)
                        {

                        }
                        else
                            mySubs[i].AccessPosition = new Vector2(0 - mySubs[i].AccessSize.Width - 1, mySubs[i].AccessSize.Y);
                    }
                    else
                        mySubs[i].AccessCollisionTime = gameTime.ElapsedGameTime;
                }
                else
                {
                    mySubs[i].Draw(this.mySpriteBatch);
                }

            }

            myMine.Draw(this.mySpriteBatch);
            

            // Call Draw for each ocean object in the oceans array
            for (var i = 0; i < myOceans.Length; i++)
            {
                myOceans[i].Draw(this.mySpriteBatch);
            }

            // End your drawing code here
            mySpriteBatch.End();

            // Call the Draw method in the base
            base.Draw(gameTime);
        }
    }
}
