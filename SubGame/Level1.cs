using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SubGame.Elements;
using SubGame.Objects;

namespace SubGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Level1 : Game
    {
        private GraphicsDeviceManager myGraphics;
        private SpriteBatch mySpriteBatch;

        //All the old, should be replaced with better functionality
        // Set the direction of the background movement.
        private readonly Vector2 myGameDirection = new Vector2(-1, 0); //-1 = move background left
        // Set the speed of the background movement.
        private Vector2 myBackgroundSpeed = new Vector2(120, 0); //120 = speed of movement

        private AnimatedBackground[] myOceans = new AnimatedBackground[7];

        // Set the direction of mine movement.
        private readonly Vector2 myMineDirection = new Vector2(0, -1); //-1 = move upwards
        private AnimatedObject[] myMines = new AnimatedObject[3];// One mine for each sub


        // All the new
        private readonly int surfaceLevel = 280;
        private List<SkyElement> mySkies;
        private List<EnemyElement> mySubs;
        private PlayerElement myBoat;
        StaticText statusPanel;

        private int myBoatsHit;
        private int latestSkyAdded;

        public Level1()
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
            //All the old, should be replaced with better functionality
            // Instantiate a new AnimatedBackground object for each element in the oceans array and set scale for the size of each
            for (int i = 0; i < myOceans.Length; i++)
            {
                myOceans[i] = new AnimatedBackground(0.5f);
            }

            //// Instantiate three mines
            for (int i = 0; i < myMines.Length; i++)
            {
                myMines[i] = new AnimatedObject(0.6f, new Vector2(0, 50));
            }


            // All the new
            mySkies = new List<SkyElement>();
            myBoat = new PlayerElement(1.0f, 0.01f, 0.0f, 1.5f, new Vector2(0, surfaceLevel), myGraphics);
            mySubs = new List<EnemyElement>();
            for (int i = 0; i < 3; i++)
            {
                mySubs.Add(new EnemyElement(0.6f, 0.0f, 0.0f, 1.0f, new Vector2(0, 0), myGraphics));
                myBoat.WhereIsTheBoat += mySubs[i].BoatIsFoundAt;
            }
            statusPanel = new StaticText(new Vector2(20, 20), new Vector2(400, 200), myGraphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            //All the old, should be replaced with better functionality

            // Load all the oceans images by calling the AnimatedBackground.LoadContent for each one of them
            for (int i = 0; i < myOceans.Length; i++)
            {
                LoadBackground(myOceans[i], "Backgrounds/Ocean",
                    new Vector2(i == 0 ? 0.0f : myOceans[i - 1].AccessPosition.X + myOceans[i - 1].AccessSize.Width, surfaceLevel));
            }

            // Load mines
            for (int i = 0; i < myMines.Length; i++)
            {
                myMines[i].LoadContent(Content, "Backgrounds/Mine");
                // Set initial position of the mine to the width and height of 2 mines beyond the right, bottom of the screen
                myMines[i].AccessPosition = new Vector2(myGraphics.PreferredBackBufferWidth + myMines[i].AccessSize.Width * 2,
                        myGraphics.PreferredBackBufferHeight + myMines[i].AccessSize.Height * 2);
            }


            // All the new
            foreach (var sub in mySubs)
            {
                sub.LoadContent(Content, new string[] { "Elements/SlowSub", "Elements/MediumSub", "Elements/FastSub" });
            }
            myBoat.LoadContent(Content, "Elements/Boat", "Elements/Sinkbomb");
            statusPanel.LoadContent(Content, "Status");
        }

        private void LoadBackground(AnimatedBackground aBackground, string aResource, Vector2 aPosition)
        {
            aBackground.LoadContent(Content, aResource);
            aBackground.AccessPosition = aPosition;
        }

        protected override void Update(GameTime aGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //All the old, should be replaced with better functionality
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

            for (int target = 0; target < myMines.Length; target++)
            {
                if (myMines[target].AccessPosition.Y < surfaceLevel)
                {
                    myMines[target].AccessPosition = new Vector2(myGraphics.PreferredBackBufferWidth + myMines[target].AccessSize.Width * 2,
                        myGraphics.PreferredBackBufferHeight + myMines[target].AccessSize.Height * 2);
                    myMines[target].AccessCollisionDetected = false;
                    myMines[target].AccessCollisionRegistered = false;
                }
            }

            for (int i = 0; i < myOceans.Length; i++)
            {
                myOceans[i].AccessPosition += myGameDirection * myBackgroundSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }

            for (int i = 0; i < myMines.Length; i++)
            {
                if (myMines[i].AccessPosition.X < myGraphics.PreferredBackBufferWidth && myMines[i].AccessPosition.Y < myGraphics.PreferredBackBufferHeight)
                {
                    myMines[i].AccessPosition += myMineDirection * myMines[i].AccessSpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
                }
            }


            //All the new
            foreach (var sky in mySkies)
            {
                sky.Update(aGameTime);
            }
            foreach (var sub in mySubs)
            {
                sub.Update(aGameTime);
            }
            myBoat.Update(aGameTime);
            base.Update(aGameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.LightSkyBlue);
            GraphicsDevice.Clear(Color.White);
            mySpriteBatch.Begin();
            // Begin your drawing code here

            //All the old, should be replaced with better functionality
            for (int i = 0; i < myMines.Length; i++)
            {
                myMines[i].AccessCollisionDetected = myMines[i].AccessCollisionBox.Intersects(myBoat.CollisionBox);

                if (myMines[i].AccessCollisionDetected && !myMines[i].AccessCollisionRegistered)
                {
                    myBoatsHit++;
                    myMines[i].AccessCollisionRegistered = true;
                }

                myMines[i].Draw(mySpriteBatch);
            }

            // Call Draw for each ocean object in the oceans array
            for (int i = 0; i < myOceans.Length; i++)
            {
                myOceans[i].Draw(mySpriteBatch);
            }

            //All the new
            if ((gameTime.TotalGameTime.Seconds % 10) == 0)
            {
                if (gameTime.TotalGameTime.Seconds != latestSkyAdded)
                {
                    latestSkyAdded = gameTime.TotalGameTime.Seconds;
                    HandleSkies();
                }
            }
            foreach (var sky in mySkies)
            {
                sky.Draw(mySpriteBatch);
            }
            foreach (var sub in mySubs)
            {
                sub.Draw(mySpriteBatch);
            }
            myBoat.Draw(mySpriteBatch);
            string subs = "";
            foreach (var sub in mySubs)
            {
                subs += $"{sub.Position.X:N1}x{sub.Position.Y:N1}, Speed: {sub.Speed:N1}\n";
            }
            statusPanel.Draw(mySpriteBatch, $"Boats hit: {myBoatsHit}\n{subs}");

            // End your drawing code here
            mySpriteBatch.End();
            base.Draw(gameTime);
        }

        private void HandleSkies()
        {
            var skyElement = new SkyElement(0.6f, -1.0f, 0.0f, 1.0f, new Vector2(0, 0), myGraphics);
            skyElement.LoadContent(Content, new string[] { "Elements/cloud1", "Elements/cloud2", "Elements/cloud3", "Elements/cloud4" });
            mySkies.Add(skyElement);
            foreach (var sky in mySkies.Where(s => s.OutOfBounds).ToList())
            {
                mySkies.Remove(sky);
            }
        }
    }
}