using SubGame.Levels;
using System;

namespace SubGame
{
#if WINDOWS || LINUX

    public static class Program
    {
        [STAThread]
        private static void Main(string[] para)
        {
            string savePath = "";
            int level = para.Length == 1 && int.TryParse(para[0], out level) ? level : 1;
            using (MainGame myGame = new MainGame(new LevelFactory("Levels.json"), level))
            {
                myGame.LoadState(savePath);
                myGame.Run();
                myGame.SaveState(savePath);
            }
        }
    }

#endif
}