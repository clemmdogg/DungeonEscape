using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static DungeonEscape.Files.HighScore;
using static DungeonEscape.Files.SetupGame;

namespace DungeonEscape.Files
{
    internal class HighScore
    {
        public class HighScoreAchiever
        {
            public string GamerTag { get; set; }
            public int HighScore { get; set; }
            public HighScoreAchiever(string gamerTag, int highScore)
            {
                GamerTag = gamerTag;
                HighScore = highScore;
            }
        }
        /// <summary>
        /// Test if the score is good enough tó the highscore and adds it if it is.
        /// </summary>
        /// <param name="highScore"></param>
        public static void AddHighScore(int highScore)
        {
            List<HighScoreAchiever> highScoreList = new List<HighScoreAchiever>();
            try
            {
                highScoreList = DeserializeHighScoreAchievers();

            }
            catch
            {
                string filePath = "HighScore.json";
                using (FileStream fs = File.Create(filePath)) { }
                HighScoreAchiever highScoreAchiever = new HighScoreAchiever(GetGamerTag(), highScore);
                highScoreList.Add(highScoreAchiever);
                SerializeHighScoreAchievers(highScoreList);
                PrintHighScoreList(highScoreList);
                return;
            }
            if (highScoreList.Count < 21)
            {
                HighScoreAchiever highScoreAchiever = new HighScoreAchiever(GetGamerTag(), highScore);
                highScoreList.Add(highScoreAchiever);
                SerializeHighScoreAchievers(highScoreList);
                PrintHighScoreList(highScoreList);
                return;
            }
            if (highScore <= highScoreList.OrderBy(highScoreAchiever => highScoreAchiever.HighScore).FirstOrDefault().HighScore)
            {
                highScoreList.Remove(highScoreList.OrderBy(highScoreAchiever => highScoreAchiever.HighScore).FirstOrDefault());
                HighScoreAchiever highScoreAchiever = new HighScoreAchiever(GetGamerTag(), highScore);
                highScoreList.Add(highScoreAchiever);
                SerializeHighScoreAchievers(highScoreList);
                PrintHighScoreList(highScoreList);
                return;
            }
        }
        /// <summary>
        /// Prints the highscore.
        /// </summary>
        /// <param name="highScoreList"></param>
        public static void PrintHighScoreList(List<HighScoreAchiever> highScoreList)
        {
            Console.Clear();
            Console.WriteLine("Highscoreliste..");
            Console.WriteLine("-------------------------------------------------------");
            highScoreList.OrderBy(highScoreAchiever => highScoreAchiever.HighScore).ToList();
            foreach (HighScoreAchiever highScoreAchiever in highScoreList)
            {
                Console.WriteLine($"Gamer tag: {highScoreAchiever.GamerTag}\t\t\t\t\t Antal træk: {highScoreAchiever.HighScore}");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Gets the gamer tag from the user.
        /// </summary>
        /// <returns></returns>
        public static string GetGamerTag()
        {
            string gamerTag = "";
            while (true)
            {
                Console.Clear();
                Console.Write("Tillykke, du er på highscoren. Indtast gamertag: ");
                gamerTag = Console.ReadLine();
                if (gamerTag.Length < 5 || gamerTag.Length > 15)
                {
                    Console.WriteLine("Gamer tag skal være mellem 5 og 15 karakterer langt. Prøv Igen!!");
                    Console.ReadKey();
                    continue;
                }
                break;
            }
            return gamerTag;
        }
        /// <summary>
        /// Writes to the highscore file.
        /// </summary>
        /// <param name="persons"></param>
        public static void SerializeHighScoreAchievers(List<HighScoreAchiever> persons)
        {
            string jsonString = JsonSerializer.Serialize(persons, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("HighScore.json", jsonString);
        }
        /// <summary>
        /// Reads from the highscore file.
        /// </summary>
        /// <returns></returns>
        public static List<HighScoreAchiever> DeserializeHighScoreAchievers()
        {
            string jsonString = File.ReadAllText("HighScore.json");
            return JsonSerializer.Deserialize<List<HighScoreAchiever>>(jsonString);
        }
    }
}
