using System;

namespace SubGame
{
#if WINDOWS || LINUX

    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (MainGame myGame = new MainGame())
            {
                myGame.Run();
            }
        }
    }

#endif
}