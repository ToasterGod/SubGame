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
            using (Level1 myGame = new Level1())
            {
                myGame.Run();
            }
        }
    }

#endif
}