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

        private Texture2D boat;

        private AnimatedObject[] subs = new AnimatedObject[3];

        private Random rng = new Random();

        public BestSubGame()
        {
            myGraphics = new GraphicsDeviceManager(this);

            myGraphics.PreferredBackBufferWidth = 1920; // set this value to the desired width of your window, 1280 is better on lower resolution screen
            myGraphics.PreferredBackBufferHeight = 1080; // set this value to the desired height of your window, 1024 is better on lower resolution screen
            myGraphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Instantiate a new AnimatedBackground object for each element in the skies array and set scale for the size of each
            for (var i = 0; i < skies.Length; i++)
            {
                skies[i] = new AnimatedBackground();
                // Scale size of each image
                skies[i].Scale = 0.15f;
            }

            // Instantiate a new AnimatedBackground object for each element in the oceans array and set scale for the size of each
            for (var i = 0; i < oceans.Length; i++)
            {
                oceans[i] = new AnimatedBackground();
                // Scale size of each image
                oceans[i].Scale = 0.3f;
            }

            for (int i = 0; i < subs.Length; i++)
            {
                subs[i] = new AnimatedObject();
                subs[i].Scale = 1.0f;
                subs[i].Speed = new Vector2(rng.Next(40, 130), 0);
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
                float adjust = 0.0f;
                if (i == 0)
                    adjust = 0.0f;
                else
                    adjust = skies[i - 1].Position.X + skies[i - 1].Size.Width;

                skies[i].Position = new Vector2(adjust, 0);
            }

            // Load all the oceans images by calling the AnimatedBackground.LoadContent for each one of them
            for (var i = 0; i < oceans.Length; i++)
            {
                oceans[i].LoadContent(this.Content, "Backgrounds/Ocean");
                float adjust = 0.0f;
                if (i == 0)
                    adjust = 0.0f;
                else
                    adjust = oceans[i - 1].Position.X + oceans[i - 1].Size.Width;

                oceans[i].Position = new Vector2(adjust, 280);
            }

            for (int i = 0; i < subs.Length; i++)
            {
                /*
                 * den långsamma ubåten är en yellow submarine
                 * den snabba är samma fast med påmålade flammor
                 */
                if (subs[i].Speed.X < 70)
                {
                    //slow sub
                    subs[i].LoadContent(this.Content, "Backgrounds/Sub");
                }
                else if (subs[i].Speed.X < 100)
                {
                    //fast sub
                    subs[i].LoadContent(this.Content, "Backgrounds/Sub");
                }
                else
                {
                    // fastestest sub
                    subs[i].LoadContent(this.Content, "Backgrounds/Sub");
                }
                subs[i].Position = new Vector2(myGraphics.PreferredBackBufferWidth + subs[i].Size.Width + i * subs[i].Size.Width,
                    350 + i * subs[i].Size.Height);
            }

            // Load the boat from the content stream
            boat = Content.Load<Texture2D>("Backgrounds/Boat");

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
                int source = skies.Length - 1;
                if (target > 0)
                    source = target - 1;

                if (skies[target].Position.X < -skies[target].Size.Width)
                {
                    skies[target].Position.X = skies[source].Position.X + skies[source].Size.Width;
                }
            }

            // Move all the images in the oceans array and if the first image is fully outside of the screen to the left 
            // then put it as the last element. That way we will feed the first image in as the last image once it has passed by
            // Gives a kind of rotating function
            for (var target = 0; target < oceans.Length; target++)
            {
                int source = oceans.Length - 1;
                if (target > 0)
                    source = target - 1;

                if (oceans[target].Position.X < -oceans[target].Size.Width)
                {
                    oceans[target].Position.X = oceans[source].Position.X + oceans[source].Size.Width;
                }
            }

            for (var target = 0; target < subs.Length; target++)
            {

                if (subs[target].Position.X < -subs[target].Size.Width)
                {
                    subs[target].Position = new Vector2(myGraphics.PreferredBackBufferWidth + subs[target].Size.Width + target * subs[target].Size.Width,
                    350 + target * subs[target].Size.Height);
                }
            }


            // Set the direction of the background movement
            Vector2 aDirection = new Vector2(-1, 0); //-1 = move background left
            
            // Set the speed of the background movement
            Vector2 aSpeed = new Vector2(120, 0); //120 = speed of movement

            // Calculate the movement of the oceans elements (to the left)
            for (var i = 0; i < skies.Length; i++)
            {
                skies[i].Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Calculate the movement of the oceans elements (to the left)
            for (var i = 0; i < oceans.Length; i++)
            {
                oceans[i].Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            for (int i = 0; i < subs.Length; i++)
            {
                subs[i].Position += aDirection * subs[i].Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

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
            mySpriteBatch.Draw(boat, new Vector2(60, 130), Color.White); // Make random y(130) to move boat up and down a couple of pixels

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
