﻿using System;
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
        private readonly int mySurfaceLevel = 280;
        private List<SkyElement> mySkies;
        private StaticElement myOcean;
        private PlayerElement myBoat;
        private List<EnemyElement> mySubs;
        private List<MineElement> myMines;
        private List<SinkBombElement> mySinkBombs;
        StaticText myStatusPanelLeft;
        StaticText myStatusPanelRight;

        private readonly int myBoatHits;
        private readonly int mySubHits; 
        private int myLatestAddedSky;

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
            myOcean = new StaticElement(1.0f, 0.0f, new Vector2(0, mySurfaceLevel));
            myBoat = new PlayerElement(1.0f, 0.01f, 0.0f, 1.5f, new Vector2(0, mySurfaceLevel), myGraphics);
            myBoat.AccessSinkBombReleased += SinkBombReleased;
            mySubs = new List<EnemyElement>();
            myMines = new List<MineElement>();
            mySinkBombs = new List<SinkBombElement>();
            //Level1 = three subs at the time, each having one mine
            for (int i = 0; i < 3; i++)
            {
                mySubs.Add(new EnemyElement(mySurfaceLevel, 0.6f, 0.0f, 0.0f, 1.0f, new Vector2(0, 0), myGraphics));
                mySubs[i].AccessMineReleased += MineReleased;
                myBoat.AccessWhereIsTheBoat += mySubs[i].BoatIsFoundAt;
            }
            var staticTextTop = myGraphics.PreferredBackBufferHeight - 100;
            myStatusPanelLeft = new StaticText(new Vector2(20, staticTextTop), new Vector2(300, 80), myGraphics);
            myStatusPanelRight = new StaticText(new Vector2(myGraphics.PreferredBackBufferWidth - 320, staticTextTop), new Vector2(300, 80), myGraphics);

            base.Initialize();
        }

        private void SinkBombReleased(SinkBombElement aSinkBomb)
        {
            mySinkBombs.Add(aSinkBomb);
        }

        private void MineReleased(MineElement aMine)
        {
            myMines.Add(aMine);
        }

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            myOcean.LoadContent(Content, "Backgrounds/SolidOcean");
            foreach (var sub in mySubs)
            {
                sub.LoadContent(Content, new string[] { "Elements/SlowSub", "Elements/MediumSub", "Elements/FastSub" }, "Elements/Mine");
            }
            myBoat.LoadContent(Content, "Elements/Boat", "Elements/Sinkbomb");
            myStatusPanelLeft.LoadContent(Content, "Status");
            myStatusPanelRight.LoadContent(Content, "Status");
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
                
            }

            myBoat.Update(aGameTime);

            foreach (var mine in myMines)
            {
                mine.Update(aGameTime);
                //if (mine.AccessTimeout == true)
                //{
                //    mine.AccessPosition = new Vector2(AccessPosition.X + AccessSize.Width / 2, AccessPosition.Y + AccessSize.Height / 2);
                //    mine.AccessTimeout = false;
                //}
            }
            foreach (var sinkBomb in mySinkBombs)
            {
                sinkBomb.Update(aGameTime);
            }

            base.Update(aGameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            mySpriteBatch.Begin();
            // Begin your drawing code here

            if ((gameTime.TotalGameTime.Seconds % 20) == 0 && gameTime.TotalGameTime.Seconds != myLatestAddedSky)
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

            foreach (var mine in myMines)
            {
                mine.Draw(mySpriteBatch);
            }
            foreach (var sinkBomb in mySinkBombs)
            {
                sinkBomb.Draw(mySpriteBatch);
            }

            myOcean.Draw(mySpriteBatch);

            myStatusPanelLeft.Draw(mySpriteBatch, $"Boat hits: {myBoatHits}");
            myStatusPanelRight.Draw(mySpriteBatch, $"Sub hits: {mySubHits}");

            // End your drawing code here
            mySpriteBatch.End();
            base.Draw(gameTime);
        }

        private void GenerateRandomSkies(GameTime aGameTime)
        {
            float tempRandomSpeed = (float)RandomNumber.Between(3, 8) / 10.0f;
            myLatestAddedSky = aGameTime.TotalGameTime.Seconds;
            var aSkyElement = new SkyElement(0.6f, -1.0f, 0.0f, tempRandomSpeed, new Vector2(0, 0), myGraphics);
            aSkyElement.LoadContent(Content, new string[] { "Elements/cloud1", "Elements/cloud2", "Elements/cloud3", "Elements/cloud4" });
            mySkies.Add(aSkyElement);
            foreach (var sky in mySkies.Where(s => s.AccessOutOfBounds).ToList())
            {
                mySkies.Remove(sky);
            }
        }
    }
}