using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SubGame.Elements
{
    public delegate void WhereIsTheBoatDelegate(Rectangle location);
    public delegate void MineReleasedDelegate(MineElement mine);
    public delegate void SinkBombReleasedDelegate(SinkBombElement sinkBomb);
}
