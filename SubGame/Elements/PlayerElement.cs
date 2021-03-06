﻿using ConfigModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubGame.Elements
{
    public class PlayerElement : ArmedElement
    {
        private bool mySpacePressed;
        private bool myEnterPressed;
        private int myLeftEdge;
        private int myRightEdge;
        private List<SinkBombElement> mySinkBombList;
        private string myWeaponAsset;
        private string myBulletAsset;
        private Texture2D myBulletTexture;
        private ContentManager myContentManager;
        private readonly int mySinkBombCount;
        private readonly int myLevel;

        public SinkBombReleasedDelegate AccessSinkBombReleased { get; set; }
        public BulletFiredDelegate BulletFired { get; set; }
        public WhereIsTheBoatDelegate AccessWhereIsTheBoat { get; set; }
        public int AccessSinkBombsLeft => mySinkBombList.Count();

        public PlayerElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager, int someSinkBombs, int aLevel, LevelDifficulty aMovementDifficulty, LevelDifficulty aWeaponDifficulty)
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager, aMovementDifficulty, aWeaponDifficulty)
        {
            AccessIsEnemy = false;
            mySinkBombCount = someSinkBombs;
            myLevel = aLevel;
        }

        public void LoadContent(ContentManager aContentManager, string anAsset, string aWeaponAsset, string aBulletAsset)
        {
            myContentManager = aContentManager;
            myWeaponAsset = aWeaponAsset;
            myBulletAsset = aBulletAsset;

            LoadContent(myContentManager, anAsset);
            if (!string.IsNullOrWhiteSpace(aBulletAsset))
            {
                myBulletTexture = aContentManager.Load<Texture2D>(myBulletAsset);
            }

            //Skapa och ladda vapen
            ReloadMines();
        }

        public override void LoadContent(ContentManager aContentManager, string anAsset)
        {
            base.LoadContent(aContentManager, anAsset);

            //Level boat at sealevel and set left and right endpoints
            myLeftEdge = 60;
            myRightEdge = myManager.PreferredBackBufferWidth - 60 - AccessSize.Width;
            AccessPosition = new Vector2(myLeftEdge, AccessPosition.Y - AccessSize.Height * 0.7f);
        }

        //Wrapper around Update so we can handle setting of right edge if we have icebergs in this level
        public void Update(int aRightEdge, GameTime aGameTime)
        {
            myRightEdge = aRightEdge; // Initial the right edge to the Iceberg if we have one...
            Update(aGameTime); // Call the one below
        }

        public override void Update(GameTime aGameTime)
        {
            base.Update(aGameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                AccessDirection = -1.0f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                AccessDirection = 1.0f;
            }
            else
            {
                AccessDirection = 0.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //The else statement below will make this into a "one time trigger"
                if (mySpacePressed == false)
                {
                    mySpacePressed = true;
                    SinkBombElement sinkBomb = mySinkBombList.FirstOrDefault();
                    if (sinkBomb != null)
                    {
                        AccessSinkBombReleased?.Invoke(sinkBomb);
                        mySinkBombList.Remove(sinkBomb);
                    }
                }
            }
            else
            {
                //Force to unpress space before it is allowed to drop next sink bomb, "one time trigger"
                mySpacePressed = false;
            }

            if (myLevel > 5 && AccessDirection > 0.0f && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                //The else statement below will make this into a "one time trigger"
                if (myEnterPressed == false)
                {
                    myEnterPressed = true;
                    //TODO! Add proper values
                    Vector2 bulletOrigin = new Vector2(AccessPosition.X + (AccessSize.Width * AccessScale), AccessPosition.Y);
                    float bulletSpeed = 120.0f;
                    float bulletAngle = 50.2f;
                    Bullet bullet = new Bullet(myBulletTexture, bulletOrigin, bulletSpeed, bulletAngle);
                    if (bullet != null)
                    {
                        BulletFired?.Invoke(bullet);
                    }
                }
            }
            else
            {
                //Force to unpress enter before it is allowed to shoot next bullet, "one time trigger"
                myEnterPressed = false;
            }

            //Keep the boat within screens left and right edge
            if (AccessPosition.X <= myLeftEdge && AccessDirection < 0.0f)
            {
                AccessDirection = 0.0f;
            }
            else if (AccessPosition.X > (myRightEdge) && AccessDirection > 0.0f)
            {
                AccessDirection = 0.0f;
            }

            CalcHorizontalMovement(AccessSpeed);
            AccessWhereIsTheBoat?.Invoke(new Rectangle(AccessPosition.ToPoint(), AccessSize.Size), aGameTime);

            foreach (SinkBombElement sinkBomb in mySinkBombList)
            {
                //Make all not dropped sinkbombs follow the boat
                sinkBomb.AccessPosition = new Vector2(AccessPosition.X + AccessSize.Center.X, AccessPosition.Y + AccessSize.Center.Y);
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            base.Draw(aSpriteBatch);

            //Sinkbombs belonging to the boat doesn't have to be drawn, so skip the foreach loop once we are done
            //Drawing sinkbombs that are dropped (no longer belongs to the boat) will be handled in the Level1 class
            //foreach (SinkBombElement sinkBomb in mySinkBombList)
            //{
            //    sinkBomb.Draw(aSpriteBatch);
            //}
        }

        internal void ReloadMines()
        {
            mySinkBombList = new List<SinkBombElement>();
            for (int i = 0; i < mySinkBombCount; i++)
            {
                SinkBombElement sinkBomb = new SinkBombElement(1.0f, 1.0f, AccessRotation, CalcWeaponSpeed(), AccessPosition, myManager);
                mySinkBombList.Add(sinkBomb);
                sinkBomb.LoadContent(myContentManager, myWeaponAsset);
                sinkBomb.AccessPosition = new Vector2(AccessPosition.X + AccessSize.Width / 2, AccessPosition.Y + AccessSize.Height / 2);
            }
        }

        private float CalcWeaponSpeed()
        {
            float tempSinkBombSpeed = 1.0f;
            switch (myWeaponDifficulty)
            {
                case LevelDifficulty.Easy:
                    tempSinkBombSpeed *= 0.75f;
                    break;
                case LevelDifficulty.Normal:
                    tempSinkBombSpeed *= 1.0f;
                    break;
                case LevelDifficulty.Hard:
                    tempSinkBombSpeed *= 1.5f;
                    break;
                default:
                    break;
            }
            return tempSinkBombSpeed;
        }

    }
}
