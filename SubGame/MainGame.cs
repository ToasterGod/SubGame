using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public bool AccessBetweenLevels { get; set; }
        public int AccessCurrentLevel { get; set; }
        public ILevel AccessCurrentLevelInstance { get; set; }

        public MainGame(LevelFactory aLevelFactory, int level)
        {
            myLevelFactory = aLevelFactory;
            aLevelFactory.AccessGame = this;
            AccessCurrentLevel = level;

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
            pausedBanner = new PausedBanner(new Vector2(0, myGraphics.PreferredBackBufferHeight / 2), new Vector2(myGraphics.PreferredBackBufferWidth, 80), myGraphics);

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
            int boatHealth = AccessCurrentLevelInstance.AccessBoatHitsAllowed - AccessCurrentLevelInstance.AccessBoatHits;
            int subsToBeat = AccessCurrentLevelInstance.AccessSubHitsRequired - AccessCurrentLevelInstance.AccessSubHits;
            int sinkBombsPossibleHits = AccessCurrentLevelInstance.AccessBoat.AccessSinkBombsLeft + AccessCurrentLevelInstance.AccessSubHits + AccessCurrentLevelInstance.AccessSinkingSinkBombsThatAreSinking;
            bool lostLevel = false;
            bool wonLevel = false;

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

            if (boatHealth == 0)
            {
                // Boat has been beaten
                lostLevel = true;
                AccessBetweenLevels = true;
            }

            if (subsToBeat == 0)
            {
                // Next level, all required subs have been beaten
                wonLevel = true;
                AccessBetweenLevels = true;
            }

            // If level is > 5 then this shouldn't happen because the helicopter will refill
            if (AccessCurrentLevel < 6)
            {
                if (sinkBombsPossibleHits < AccessCurrentLevelInstance.AccessSubHitsRequired)
                {
                    // Boat can't sink the subs required
                    lostLevel = true;
                    AccessBetweenLevels = true;
                }
            }

            if (AccessBetweenLevels)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.C))
                {
                    AccessBetweenLevels = false;
                    if (lostLevel)
                    {
                        RestartLevel();
                    }

                    if (wonLevel)
                    {
                        NextLevel();
                    }
                }
                else
                {
                    return;
                }
            }

            if (AccessPaused)
            {
                // Don't update if paused, Draw will still be called to keep screen updated with current state
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
            if (AccessPaused)
            {
                pausedBanner.Draw(mySpriteBatch, $"Paused... Press Esc to quit or R to resume...");
            }

            if (AccessBetweenLevels)
            {
                int boatHealth = AccessCurrentLevelInstance.AccessBoatHitsAllowed - AccessCurrentLevelInstance.AccessBoatHits;
                int subsToBeat = AccessCurrentLevelInstance.AccessSubHitsRequired - AccessCurrentLevelInstance.AccessSubHits;
                int sinkBombsPossibleHits = AccessCurrentLevelInstance.AccessBoat.AccessSinkBombsLeft + AccessCurrentLevelInstance.AccessSubHits + AccessCurrentLevelInstance.AccessSinkingSinkBombsThatAreSinking;
                if (boatHealth == 0 || (AccessCurrentLevel < 6 && sinkBombsPossibleHits < AccessCurrentLevelInstance.AccessSubHitsRequired))
                {
                    pausedBanner.Draw(mySpriteBatch, $"Level lost!!! Press Esc to quit or C to continue...");
                }
                else if (subsToBeat == 0)
                {
                    pausedBanner.Draw(mySpriteBatch, $"Level won!!! Press Esc to quit or C to continue...");
                }
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