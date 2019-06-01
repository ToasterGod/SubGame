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
#if DEBUG
            //If DEBUG is set as compiler option it will allow to use a commandline parameter fx "6" 
            //and it will start the game at that level for debugging
            int level = para.Length == 1 && int.TryParse(para[0], out level) ? level : 1;
#else
            //In release version it will use "1" as start level all the time
            int level = 1;
#endif
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