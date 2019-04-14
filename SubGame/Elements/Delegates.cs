using Microsoft.Xna.Framework;

namespace SubGame.Elements
{
    public delegate void WhereIsTheBoatDelegate(Rectangle location, GameTime gameTime);
    public delegate void MineReleasedDelegate(MineElement mine);
    public delegate void SinkBombReleasedDelegate(SinkBombElement sinkBomb);
}
