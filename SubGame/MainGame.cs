using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SubGame.Elements;
using SubGame.Types;
using System.Collections.Generic;
using System.Linq;

namespace SubGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        private readonly GraphicsDeviceManager myGraphics;
        private SpriteBatch mySpriteBatch;

        // All the new
        public bool AccessPaused { get; private set; }
        public LevelContainerBase AccessCurrentLevel { get; set; }

        public MainGame()
        {
            myGraphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920, // set this value to the desired width of your window, 1280 is better on lower resolution screen
                PreferredBackBufferHeight = 1080 // set this value to the desired height of your window, 1024 is better on lower resolution screen
            };
            myGraphics.ApplyChanges();
            Content.RootDirectory = "Content";
            AccessCurrentLevel = new Level1(myGraphics, Content);
        }

        protected override void Initialize()
        {
            AccessCurrentLevel.Initialize();
            base.Initialize();
        }



        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);
            AccessCurrentLevel.LoadContent();
        }

        protected override void Update(GameTime aGameTime)
        {
            if (AccessPaused && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                AccessPaused = false;
            }
            else if (!AccessPaused && Keyboard.GetState().IsKeyDown(Keys.P))
            {
                AccessPaused = true;
            }
            if (AccessPaused)
            {
                return;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            AccessCurrentLevel.Update(aGameTime);
            base.Update(aGameTime);
        }

        protected override void Draw(GameTime aGameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            mySpriteBatch.Begin();
            // Begin your drawing code here

            AccessCurrentLevel.Draw(mySpriteBatch, aGameTime);

            // End your drawing code here
            mySpriteBatch.End();
            base.Draw(aGameTime);
        }

        
    }
}