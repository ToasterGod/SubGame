using System;

namespace ConfigLibrary
{
    public class LevelData
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public int Subs { get; set; }
        public int Mines { get; set; }
        public int Sinkbombs { get; set; }
        public int Health { get; set; }
        public int RequiredSubHits { get; set; }
        public LevelDifficulty MovementDifficulty { get; set; }
        public LevelDifficulty WeaponDifficulty { get; set; }
    }
}
