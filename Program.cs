using System;
using System.IO;

using Toad;

public static class Program
{
    [STAThread]
    static void Main()
    {
        string logPath = "game_log.txt";
        File.WriteAllText(logPath, "Starting the game...\n");

        try
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
        catch (Exception e)
        {
            File.AppendAllText(logPath, $"An error occurred: {e.Message}\n");
            File.AppendAllText(logPath, e.StackTrace + "\n");
        }

        File.AppendAllText(logPath, "Game has exited.\n");
    }
}