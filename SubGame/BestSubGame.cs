using System;
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
        GraphicsDeviceManager myGraphics;
        SpriteBatch mySpriteBatch;

        private AnimatedBackground[] skies = new AnimatedBackground[7];

        private AnimatedBackground[] oceans = new AnimatedBackground[7];

        private Texture2D aBoat;

        private AnimatedObject[] subs = new AnimatedObject[3];

        private Random aRNG = new Random();

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
            for (var i = 0; i < skies.Length; i++)
            {
                skies[i] = new AnimatedBackground
                {
                    // Scale size of each image
                    aScale = 0.15f
                };
            }

            // Instantiate a new AnimatedBackground object for each element in the oceans array and set scale for the size of each
            for (var i = 0; i < oceans.Length; i++)
            {
                oceans[i] = new AnimatedBackground
                {
                    // Scale size of each image
                    aScale = 0.5f
                };
            }

            for (int i = 0; i < subs.Length; i++)
            {
                subs[i] = new AnimatedObject
                {
                    aScale = 0.6f,
                    aSpeed = new Vector2(aRNG.Next(40, 130), 0)
                };
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load all the skies images by calling the AnimatedBackground.LoadContent for each one of them
            for (var i = 0; i < skies.Length; i++)
            {
                skies[i].LoadContent(this.Content, "Backgrounds/Himmel");
                float tempAdjust;
                if (i == 0)
                    tempAdjust = 0.0f;
                else
                    tempAdjust = skies[i - 1].aPosition.X + skies[i - 1].aSize.Width;

                skies[i].aPosition = new Vector2(tempAdjust, 0);
            }

            // Load all the oceans images by calling the AnimatedBackground.LoadContent for each one of them
            for (var i = 0; i < oceans.Length; i++)
            {
                oceans[i].LoadContent(this.Content, "Backgrounds/Ocean");
                float tempAdjust;
                if (i == 0)
                    tempAdjust = 0.0f;
                else
                    tempAdjust = oceans[i - 1].aPosition.X + oceans[i - 1].aSize.Width;

                oceans[i].aPosition = new Vector2(tempAdjust, 280);
            }

            for (int i = 0; i < subs.Length; i++)
            {
                /*
                 * den långsamma ubåten är en yellow submarine
                 * den snabba är samma fast med påmålade flammor
                 */
                if (subs[i].aSpeed.X < 70)
                {
                    //slow sub
                    subs[i].LoadContent(this.Content, "Backgrounds/SlowSub");
                }
                else if (subs[i].aSpeed.X < 100)
                {
                    //fast sub
                    subs[i].LoadContent(this.Content, "Backgrounds/MediumSub");
                }
                else
                {
                    // fastestest sub
                    subs[i].LoadContent(this.Content, "Backgrounds/FastSub");
                }
                subs[i].aPosition = new Vector2(myGraphics.PreferredBackBufferWidth + subs[i].aSize.Width + i * subs[i].aSize.Width,
                    350 + i * subs[i].aSize.Height);
            }

            // Load the boat from the content stream
            aBoat = Content.Load<Texture2D>("Backgrounds/Boat");

            //You could frequently free resources by Content.Unload();
            //It might be a good idea to create multiple ContentManager objects to divide up the content. 
            //That way you can call Unload() for the ContentManagers that you are done with.
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Move all the images in the skies array and if the first image is fully outside of the screen to the left 
            // then put it as the last element. That way we will feed the first image in as the last image once it has passed by
            // Gives a kind of rotating function
            for (var target = 0; target < skies.Length; target++)
            {
                int tempSource = skies.Length - 1;
                if (target > 0)
                    tempSource = target - 1;

                if (skies[target].aPosition.X < -skies[target].aSize.Width)
                {
                    skies[target].aPosition.X = skies[tempSource].aPosition.X + skies[tempSource].aSize.Width;
                }
            }

            // Move all the images in the oceans array and if the first image is fully outside of the screen to the left 
            // then put it as the last element. That way we will feed the first image in as the last image once it has passed by
            // Gives a kind of rotating function
            for (var target = 0; target < oceans.Length; target++)
            {
                int tempSource = oceans.Length - 1;
                if (target > 0)
                    tempSource = target - 1;

                if (oceans[target].aPosition.X < -oceans[target].aSize.Width)
                {
                    oceans[target].aPosition.X = oceans[tempSource].aPosition.X + oceans[tempSource].aSize.Width;
                }
            }

            for (var target = 0; target < subs.Length; target++)
            {

                if (subs[target].aPosition.X < -subs[target].aSize.Width)
                {
                    subs[target].aPosition = new Vector2(myGraphics.PreferredBackBufferWidth + subs[target].aSize.Width + target * subs[target].aSize.Width,
                    350 + target * subs[target].aSize.Height);
                }
            }


            // Set the direction of the background movement
            Vector2 aDirection = new Vector2(-1, 0); //-1 = move background left
            
            // Set the speed of the background movement
            Vector2 aSpeed = new Vector2(120, 0); //120 = speed of movement

            // Calculate the movement of the oceans elements (to the left)
            for (var i = 0; i < skies.Length; i++)
            {
                skies[i].aPosition += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Calculate the movement of the oceans elements (to the left)
            for (var i = 0; i < oceans.Length; i++)
            {
                oceans[i].aPosition += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            for (int i = 0; i < subs.Length; i++)
            {
                subs[i].aPosition += aDirection * subs[i].aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }

            base.Update(gameTime);
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
            for (var i = 0; i < skies.Length; i++)
            {
                skies[i].Draw(this.mySpriteBatch);
            }

            // Call draw for the boat, since it's a simple object it will be drawn by the spriteBatch itself
            mySpriteBatch.Draw(aBoat, new Vector2(60, 130), Color.White); // Make random y(130) to move boat up and down a couple of pixels

            for (int i = 0; i < subs.Length; i++)
            {
                subs[i].Draw(this.mySpriteBatch);
            }

            // Call Draw for each ocean object in the oceans array
            for (var i = 0; i < oceans.Length; i++)
            {
                oceans[i].Draw(this.mySpriteBatch);
            }

            // End your drawing code here
            mySpriteBatch.End();

            // Call the Draw method in the base
            base.Draw(gameTime);
        }
    }
}
