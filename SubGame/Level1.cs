using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SubGame.Elements;
using SubGame.Types;

namespace SubGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Level1 : Game
    {
        private GraphicsDeviceManager myGraphics;
        private SpriteBatch mySpriteBatch;

        // All the new
        private readonly int surfaceLevel = 280;
        private List<SkyElement> mySkies;
        private StaticElement ocean;
        private PlayerElement myBoat;
        private List<EnemyElement> mySubs;
        StaticText statusPanelLeft;
        StaticText statusPanelRight;

        private int myBoatHits;
        private int mySubHits;
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
            mySkies = new List<SkyElement>();
            ocean = new StaticElement(1.0f, 0.0f, new Vector2(0, surfaceLevel));
            myBoat = new PlayerElement(1.0f, 0.01f, 0.0f, 1.5f, new Vector2(0, surfaceLevel), myGraphics);
            mySubs = new List<EnemyElement>();
            //Level1 = three subs at the time, each having one mine
            for (int i = 0; i < 3; i++)
            {
                mySubs.Add(new EnemyElement(0.6f, 0.0f, 0.0f, 1.0f, new Vector2(0, 0), myGraphics));
                myBoat.WhereIsTheBoat += mySubs[i].BoatIsFoundAt;
            }
            var staticTextTop = myGraphics.PreferredBackBufferHeight - 100;
            statusPanelLeft = new StaticText(new Vector2(20, staticTextTop), new Vector2(300, 80), myGraphics);
            statusPanelRight = new StaticText(new Vector2(myGraphics.PreferredBackBufferWidth - 320, staticTextTop), new Vector2(300, 80), myGraphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            ocean.LoadContent(Content, "Backgrounds/SolidOcean");
            foreach (var sub in mySubs)
            {
                sub.LoadContent(Content, new string[] { "Elements/SlowSub", "Elements/MediumSub", "Elements/FastSub" }, "Elements/Mine");
            }
            myBoat.LoadContent(Content, "Elements/Boat", "Elements/Sinkbomb");
            statusPanelLeft.LoadContent(Content, "Status");
            statusPanelRight.LoadContent(Content, "Status");
        }

        protected override void Update(GameTime aGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            foreach (var sky in mySkies)
            {
                sky.Update(aGameTime);
            }

            foreach (var sub in mySubs)
            {
                //Sub update will call its mines update
                sub.Update(aGameTime);
                if ((sub.Position.X < 410.0f && sub.Position.X > 409.5f) || (sub.Position.X < 810.0f && sub.Position.X > 809.5f))
                {
                    sub.ReleaseMine();
                }
            }

            myBoat.Update(aGameTime);

            base.Update(aGameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            mySpriteBatch.Begin();
            // Begin your drawing code here

            if ((gameTime.TotalGameTime.Seconds % 20) == 0 && gameTime.TotalGameTime.Seconds != latestSkyAdded)
            {
                GenerateRandomSkies(gameTime);
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

            ocean.Draw(mySpriteBatch);

            statusPanelLeft.Draw(mySpriteBatch, $"Boat hits: {myBoatHits}");
            statusPanelRight.Draw(mySpriteBatch, $"Sub hits: {mySubHits}");

            // End your drawing code here
            mySpriteBatch.End();
            base.Draw(gameTime);
        }

        private void GenerateRandomSkies(GameTime gameTime)
        {
            float randomSpeed = (float)RandomNumber.Between(3, 8) / 10.0f;
            latestSkyAdded = gameTime.TotalGameTime.Seconds;
            var skyElement = new SkyElement(0.6f, -1.0f, 0.0f, randomSpeed, new Vector2(0, 0), myGraphics);
            skyElement.LoadContent(Content, new string[] { "Elements/cloud1", "Elements/cloud2", "Elements/cloud3", "Elements/cloud4" });
            mySkies.Add(skyElement);
            foreach (var sky in mySkies.Where(s => s.OutOfBounds).ToList())
            {
                mySkies.Remove(sky);
            }
        }
    }
}