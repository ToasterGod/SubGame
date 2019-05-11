using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using SubGame.Elements;
using SubGame.Levels;

namespace SubGame
{
    public class MainGame : Game
    {
        private readonly GraphicsDeviceManager myGraphics;
        private SpriteBatch mySpriteBatch;

        // All the additional
        private readonly LevelFactory myLevelFactory;
        private PausedBanner pausedBanner;
        public GraphicsDeviceManager Graphics => myGraphics;

        public bool AccessPaused { get; set; }
        public int AccessCurrentLevel { get; set; }
        public ILevel AccessCurrentLevelInstance { get; set; }

        public MainGame(LevelFactory aLevelFactory)
        {
            myLevelFactory = aLevelFactory;
            aLevelFactory.AccessGame = this;
            AccessCurrentLevel = 1;

            myGraphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920, // set this value to the desired width of your window, 1280 is better on lower resolution screen
                PreferredBackBufferHeight = 1080 // set this value to the desired height of your window, 1024 is better on lower resolution screen
            };
            myGraphics.ApplyChanges();

            Content.RootDirectory = "Content";

            AccessCurrentLevelInstance = myLevelFactory.GetLevelInstance(AccessCurrentLevel);
        }

        protected override void Initialize()
        {
            AccessCurrentLevelInstance.Initialize();
            pausedBanner = new PausedBanner(new Vector2(0,myGraphics.PreferredBackBufferHeight / 2), new Vector2(myGraphics.PreferredBackBufferWidth, 80), myGraphics);

            // base.Initialize will call LoadContent so we need to call AccessCurrentLevel.Initialize() before that
            base.Initialize();
        }

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            AccessCurrentLevelInstance.LoadContent();
            pausedBanner.LoadContent(Content, "Banner");
        }

        protected override void Update(GameTime aGameTime)
        {
            if (AccessPaused && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                // Resume game
                AccessPaused = false;
            }
            else if (!AccessPaused && Keyboard.GetState().IsKeyDown(Keys.P))
            {
                // Pause game
                AccessPaused = true;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                // Exit game
                Exit();
            }

            if (AccessPaused)
            {
                // Don't update if paused, Draw will still be called to keep screen updated with current state
                return;
            }

            if (AccessCurrentLevelInstance.AccessBoatHits == AccessCurrentLevelInstance.AccessBoatHitsAllowed)
            {
                RestartLevel();
                return;
            }

            if (AccessCurrentLevelInstance.AccessSubHits == AccessCurrentLevelInstance.AccessSubHitsRequired)
            {
                // Next level
                NextLevel();
            }

            if (AccessCurrentLevelInstance.AccessBoat.AccessSinkBombsLeft + AccessCurrentLevelInstance.AccessSubHits + AccessCurrentLevelInstance.AccessSinkingSinkBombsThatAreSinking < AccessCurrentLevelInstance.AccessSubHitsRequired)
            {
                RestartLevel();
                return;
            }

            AccessCurrentLevelInstance.Update(aGameTime);

            base.Update(aGameTime);
        }

        protected override void Draw(GameTime aGameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            mySpriteBatch.Begin();

            AccessCurrentLevelInstance.Draw(mySpriteBatch, aGameTime);
            if(AccessPaused)
            {
                pausedBanner.Draw(mySpriteBatch, $"Paused... Press Esc to quit or R to resume...");
            }

            mySpriteBatch.End();
            base.Draw(aGameTime);
        }

        protected virtual void RestartLevel()
        {
            AccessCurrentLevelInstance = myLevelFactory.GetLevelInstance(AccessCurrentLevel);
            AccessCurrentLevelInstance.Initialize();
            AccessCurrentLevelInstance.LoadContent();
        }

        protected virtual void NextLevel()
        {
            AccessCurrentLevel++;
            AccessCurrentLevelInstance = myLevelFactory.GetLevelInstance(AccessCurrentLevel);
            AccessCurrentLevelInstance.Initialize();
            AccessCurrentLevelInstance.LoadContent();
        }

        internal void LoadState(string savePath)
        {
            //string json = "";
            //if (File.Exists("CurrentState.json"))
            //{
            //    json = File.ReadAllText("CurrentState.json");
            //}
            //var currentState = JsonConvert.DeserializeObject(json);
        }

        internal void SaveState(string savePath)
        {
            //var currentState = new
            //{
            //    Level = AccessCurrentLevel,
            //    Instance = AccessCurrentLevelInstance
            //};
            //string json = JsonConvert.SerializeObject(currentState);
            //File.WriteAllText("CurrentState.json", json);
        }
    }
}