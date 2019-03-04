using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SubGame.Animations;

namespace SubGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class StinkySubGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private AnimatedBackground[] skies = new AnimatedBackground[5];

        private AnimatedBackground[] oceans = new AnimatedBackground[5];

        private Texture2D boat;

        public StinkySubGame()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1920; // set this value to the desired width of your window, 1280 is better on lower resolution screen
            graphics.PreferredBackBufferHeight = 1080; // set this value to the desired height of your window, 1024 is better on lower resolution screen
            graphics.ApplyChanges();

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load all the skies images by calling the AnimatedBackground.LoadContent for each one of them
            for (var i = 0; i < skies.Length; i++)
            {
                skies[i].LoadContent(this.Content, "Backgrounds/Sky");
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
                int source = 4;
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
                int source = 4;
                if (target > 0)
                    source = target - 1;

                if (oceans[target].Position.X < -oceans[target].Size.Width)
                {
                    oceans[target].Position.X = oceans[source].Position.X + oceans[source].Size.Width;
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
            spriteBatch.Begin();

            // Call Draw for each sky element in the skies array
            for (var i = 0; i < skies.Length; i++)
            {
                skies[i].Draw(this.spriteBatch);
            }

            // Call draw for the boat, since it's a simple object it will be drawn by the spriteBatch itself
            spriteBatch.Draw(boat, new Vector2(60, 130), Color.White); // Make random y(130) to move boat up and down a couple of pixels

            // Call Draw for each ocean object in the oceans array
            for (var i = 0; i < oceans.Length; i++)
            {
                oceans[i].Draw(this.spriteBatch);
            }

            // End your drawing code here
            spriteBatch.End();

            // Call the Draw method in the base
            base.Draw(gameTime);
        }
    }
}
