using Newtonsoft.Json;
using SubGame.Levels;
using System;
using System.IO;

namespace SubGame
{
#if WINDOWS || LINUX
    public static class Program
    {
        private static readonly string fileName = @"Levels.json";
        private static readonly string savePath = AppDomain.CurrentDomain.BaseDirectory;

        [STAThread]
        private static void Main(string[] para)
        {
#if DEBUG
            //If DEBUG is set as compiler option it will allow to use a commandline parameter fx "6" 
            //and it will start the game at that level for debugging
            int level = para.Length == 1 && int.TryParse(para[0], out level) ? level : 1;
#else
            //In release version it will use "1" as start level all the time
            int level = 1;
#endif
            using (MainGame myGame = new MainGame(new LevelFactory(Path.Combine(savePath, fileName)), level))
            {
                //LoadState(savePath, myGame);
                myGame.Run();
                //SaveState(savePath, myGame);
            }
        }

        internal static void LoadState(string savePath, MainGame myGame)
        {
            if (File.Exists(Path.Combine(savePath, "CurrentState.json")))
            {
                dynamic content = JsonConvert.DeserializeObject(File.ReadAllText("CurrentState.json"));
            }
        }

        internal static void SaveState(string savePath, MainGame myGame)
        {
            string json = JsonConvert.SerializeObject(myGame);
            File.WriteAllText("CurrentState.json", json);
        }
    }
#endif
}