using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SubGame.Elements;
using System.Collections.Generic;

namespace SubGame.Levels
{
    public class LevelBase
    {
        protected readonly int mySurfaceLevel = 280;
        protected List<CloudElement> myClouds;
        protected StaticElement myOcean;
        protected PlayerElement myBoat;
        protected List<EnemyElement> mySubs;
        protected List<MineElement> myMines;
        protected List<SinkBombElement> mySinkBombs;
        protected List<StaticElement> myBooms;
        protected StaticText myStatusPanelLeft;
        protected StaticText myStatusPanelRight;

        protected int myBoatHits;
        protected int mySubHits;
        protected int myLatestAddedCloud;

        protected readonly GraphicsDeviceManager myGraphics;
        protected ContentManager myContent;
        protected readonly LevelConfig myConfig;

        public LevelBase(GraphicsDeviceManager aGraphics, ContentManager aContent, LevelConfig aLevelConfig)
        {
            myGraphics = aGraphics;
            myContent = aContent;
            myConfig = aLevelConfig;
        }
    }
}
