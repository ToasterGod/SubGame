using Microsoft.Xna.Framework;

namespace SubGame.Elements
{
    public delegate void WhereIsTheBoatDelegate(Rectangle aLocation, GameTime aGameTime);
    public delegate void MineReleasedDelegate(MineElement aMine);
    public delegate void SinkBombReleasedDelegate(SinkBombElement aSinkBomb);
}
