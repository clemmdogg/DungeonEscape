using static DungeonEscape.Files.SetupGame;
using static DungeonEscape.Files.GamePlay;
using static DungeonEscape.Files.HighScore;

namespace DungeonEscape
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainMenu();
        }
        /// <summary>
        /// Start the menu and the methods inside the menu
        /// </summary>
        public static void MainMenu()
        {
            bool isUserQuittingMenu = false;
            while (!isUserQuittingMenu)
            {
                string userInput = "";
                Console.Clear();
                Console.WriteLine("Halløj og velkommen til spillet Dungeon Escape..");
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("Tast [S] for at vælge bane og starte spil.");
                Console.WriteLine("Tast [H] for at se highscore.");
                Console.WriteLine("Tast [A] for at afslutte spillet.");
                userInput = Console.ReadLine();
                if (userInput.ToLower() == "s")
                {
                    AddHighScore(RunGamePlay(SetupGameMethod()));
                }
                else if (userInput.ToLower() == "h")
                {
                    try
                    {
                        List<HighScoreAchiever> highScoreList = new List<HighScoreAchiever>();
                        highScoreList = DeserializeHighScoreAchievers();
                        PrintHighScoreList(highScoreList);
                    }
                    catch
                    {
                        Console.WriteLine("Der er ingen på higscorelisten endnu!!");
                        Console.ReadKey();
                    }
                }
                else if (userInput.ToLower() == "a")
                {
                    isUserQuittingMenu = true;
                }
                else
                {
                    Console.WriteLine("Forkert input. Prøv igen!!");
                    Console.ReadKey();
                }
            }
            Console.WriteLine("Halløj tak fordi du spillede Dungeon Escape. Farvel!!");
            Console.ReadKey();
        }
    }
}
