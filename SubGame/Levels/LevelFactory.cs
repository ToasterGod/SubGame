﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SubGame.Levels
{
    public class LevelFactory
    {
        public MainGame AccessGame { get; set; }
        public List<LevelConfig> LevelConfigs { get; set; }

        public LevelFactory(string aConfigFile)
        {
            LevelConfigs = JsonConvert.DeserializeObject<List<LevelConfig>>(File.ReadAllText(aConfigFile));
        }

        public ILevel GetLevelInstance(int level)
        {
            switch (level)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    return new Level1To5(AccessGame.Graphics, AccessGame.Content, LevelConfigs.FirstOrDefault(l => l.Id == level));
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    return new Level6To10(AccessGame.Graphics, AccessGame.Content, LevelConfigs.FirstOrDefault(l => l.Id == level));
                default:
                    break;
            }
            return null;
        }
    }
}
