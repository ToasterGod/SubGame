using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameWindows.Animations;

namespace MonoGameWindows
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class StinkySubGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private AnimatedSky[] skies = new AnimatedSky[5];

        private AnimatedOcean[] oceans = new AnimatedOcean[5];

        private Texture2D boat;

        public StinkySubGame()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1920; // 1280;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 1080; // 1024;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            for (var i = 0; i < skies.Length; i++)
            {
                skies[i] = new AnimatedSky();
                skies[i].Scale = 0.15f;
            }

            for (var i = 0; i < oceans.Length; i++)
            {
                oceans[i] = new AnimatedOcean();
                oceans[i].Scale = 0.3f;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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

            // TODO: Add your update logic here
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

            Vector2 aDirection = new Vector2(-1, 0);
            Vector2 aSpeed = new Vector2(40, 0);

            for (var i = 0; i < skies.Length; i++)
            {
                skies[i].Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

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

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            for (var i = 0; i < skies.Length; i++)
            {
                skies[i].Draw(this.spriteBatch);
            }

            spriteBatch.Draw(boat, new Vector2(60, 130), Color.White);

            for (var i = 0; i < oceans.Length; i++)
            {
                oceans[i].Draw(this.spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
