using SubGame.Levels;
using System;

namespace SubGame
{
#if WINDOWS || LINUX

    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            string savePath = "";
            using (MainGame myGame = new MainGame(new LevelFactory("Levels.json")))
            {
                myGame.LoadState(savePath);
                myGame.Run();
                myGame.SaveState(savePath);
            }
        }
    }

#endif
}