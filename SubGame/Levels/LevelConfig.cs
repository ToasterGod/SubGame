using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubGame.Levels
{
    public class LevelConfig
    {
        // These properties MUST have the same name as the corresponding values in Levels.json
        // Do not rename them since it will break the serialization/deserialization
        public int Id { get; set; }
        public string Name { get; set; }
        public int Subs { get; set; }
        public int Mines { get; set; }
        public int Sinkbombs { get; set; }
    }
}
