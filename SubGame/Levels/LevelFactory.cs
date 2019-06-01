using ConfigModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SubGame.Levels
{
    public class LevelFactory
    {
        public MainGame AccessGame { get; set; }
        public List<LevelData> LevelConfigs { get; set; }

        public LevelFactory(string aConfigFile) => LevelConfigs = JsonConvert.DeserializeObject<List<LevelData>>(File.ReadAllText(aConfigFile));

        // This is the main reason wthat interface is used on these two classes.
        public ILevel GetLevelInstance(int level)
        {
            switch (level)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    return new Level1To5(AccessGame.Graphics, AccessGame.Content, LevelConfigs.FirstOrDefault(l => l.Level == level));
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    return new Level6To10(AccessGame.Graphics, AccessGame.Content, LevelConfigs.FirstOrDefault(l => l.Level == level));
                default:
                    // Any other level above 10 returns empty game...
                    return new Level11To15(AccessGame.Graphics, AccessGame.Content, LevelConfigs.FirstOrDefault(l => l.Level == 10));
            }
            //return null;
        }
    }
}
