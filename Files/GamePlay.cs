using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using static DungeonEscape.Files.SetupGame;

namespace DungeonEscape.Files
{
    internal class GamePlay
    {
        /// <summary>
        /// Takes the array of blocks as parameter and run the whole game play from that. Returns the highscore in the form of moves.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int RunGamePlay(Block[] level)
        {
            bool isKeyFound = false;
            bool isSwordFound = false;
            bool isGameOver = false;
            int[] position = [0, 0];
            int highScore = 0;
            while (!isGameOver)
            {
                highScore++;
                VisualPrint(level);
                MoveText();
                int[] newPosition = PlayersTurn(position, level, isKeyFound);
                level = MoveFromOldPosition(level, position);
                MoveToNewPosition(level, newPosition);
                Block newBlock = GetNewBlock(level, newPosition);
                position = newPosition;
                if (!newBlock.IsDiscovered && newBlock.BlockType == BlockType.Key)
                {
                    Console.WriteLine("Tillykke, du har fundet nøglen..");
                    isKeyFound = true;
                    Console.WriteLine("Tryk på en tast for at fortsætte..");
                    Console.ReadKey();
                }
                else if (!newBlock.IsDiscovered && newBlock.BlockType == BlockType.Sword)
                {
                    Console.WriteLine("Tillykke, du har fundet sværdet. Nu kan du dræbe monstret..");
                    isSwordFound = true;
                    Console.WriteLine("Tryk på en tast for at fortsætte..");
                    Console.ReadKey();
                }
                else if (newBlock.BlockType == BlockType.Monster && isSwordFound == false)
                {
                    Console.WriteLine("Du møder monstret, men har ikke noget sværd. Du bliver dræbt!!");
                    isSwordFound = true;
                    isGameOver = true;
                    Console.WriteLine("Tryk på en tast for at fortsætte..");
                    Console.ReadKey();
                }
                else if (!newBlock.IsDiscovered && newBlock.BlockType == BlockType.Monster && isSwordFound == true)
                {
                    Console.WriteLine("Du møder monstret, og dræber det med sværdet!!");
                    isSwordFound = true;
                    Console.WriteLine("Tryk på en tast for at fortsætte..");
                    Console.ReadKey();

                }
                else if (!newBlock.IsDiscovered && newBlock.BlockType == BlockType.Door && isKeyFound == true)
                {
                    Console.WriteLine("Du låser døren op!!");
                    isSwordFound = true;
                    Console.WriteLine("Tryk på en tast for at fortsætte..");
                    Console.ReadKey();
                }
                else if (newBlock.BlockType == BlockType.TreasureChest)
                {
                    Console.WriteLine("Du har fundet skatten. Du har vundet!!");
                    isGameOver = true;
                    isSwordFound = true;
                    Console.WriteLine("Tryk på en tast for at fortsætte..");
                    Console.ReadKey();
                }
                level = MakesNewBlockVisible(level, position);
            }
            return highScore;
        }
        /// <summary>
        /// Makes the new block visible.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Block[] MakesNewBlockVisible(Block[] level, int[] position)
        {
            for (int i = 0; i < level.Length; i++)
            {
                if (level[i].Coordinates[0] == position[0])
                {
                    if (level[i].Coordinates[1] == position[1])
                    {
                        level[i].IsDiscovered = true;
                        break;
                    }
                }
            };
            return level;
        }
        /// <summary>
        /// Gets the new block from level and the new position.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        public static Block GetNewBlock(Block[] level, int[] newPosition)
        {
            Block errorBlock = null;
            for (int i = 0; i < level.Length; i++)
            {
                if (level[i].Coordinates[0] == newPosition[0])
                {
                    if (level[i].Coordinates[1] == newPosition[1])
                    {
                        return level[i];
                    }
                }
            };
            return errorBlock;

        }
        /// <summary>
        /// Do so the player is standing on the new position.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        public static Block[] MoveToNewPosition(Block[] level, int[] newPosition)
        {
            for (int i = 0; i < level.Length; i++)
            {
                if (level[i].Coordinates[0] == newPosition[0])
                {
                    if (level[i].Coordinates[1] == newPosition[1])
                    {
                        level[i].IsPlayerOnField = true;
                        break;
                    }
                }
            };
            return level;
        }
        /// <summary>
        /// Do so the player is not standing on the old position.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Block[] MoveFromOldPosition(Block[] level, int[] position)
        {
            for (int i  = 0; i < level.Length; i++)
            {
                if (level[i].Coordinates[0] == position[0])
                {
                    if (level[i].Coordinates[1] == position[1])
                    {
                        level[i].IsPlayerOnField = false;
                        break;
                    }
                }
            };
            return level;
        }
        /// <summary>
        /// Let player take turn.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="level"></param>
        /// <param name="isKeyFound"></param>
        /// <returns></returns>
        public static int[] PlayersTurn(int[] position, Block[] level, bool isKeyFound)
        {
            int[] newPosition = [0, 0];
            bool isAbleToMove = false;
            while (!isAbleToMove)
            {
                ConsoleKeyInfo playersMove = Console.ReadKey(true);
                if (playersMove.Key == ConsoleKey.UpArrow)
                {
                    newPosition = [position[0], position[1] - 1];
                    isAbleToMove = IsAbleToMove(newPosition, level, isKeyFound);
                }
                else if (playersMove.Key == ConsoleKey.DownArrow)
                {
                    newPosition = [position[0], position[1] + 1];
                    isAbleToMove = IsAbleToMove(newPosition, level, isKeyFound);
                }
                else if (playersMove.Key == ConsoleKey.LeftArrow)
                {
                    newPosition = [position[0] - 1, position[1]];
                    isAbleToMove = IsAbleToMove(newPosition, level, isKeyFound);
                }
                else if (playersMove.Key == ConsoleKey.RightArrow)
                {
                    newPosition = [position[0] + 1, position[1]];
                    isAbleToMove = IsAbleToMove(newPosition, level, isKeyFound);
                }
                else { continue; }
            }
            return newPosition;
        }
        /// <summary>
        /// Control is player is able to move to the block the player is trying to move to.
        /// </summary>
        /// <param name="newPosition"></param>
        /// <param name="level"></param>
        /// <param name="isKeyFound"></param>
        /// <returns></returns>
        public static bool IsAbleToMove(int[] newPosition, Block[] level, bool isKeyFound)
        {
            bool isAbleToMove = false;
            if (newPosition[0] >= 0 && newPosition[0] <= 4 && newPosition[1] >= 0 && newPosition[1] <= 4)
            {
                foreach (Block block in level)
                {
                    if (block.Coordinates[0] == newPosition[0])
                    {
                        if (block.Coordinates[1] == newPosition[1])
                        {
                            if (block.BlockType != BlockType.Rock)
                            {
                                if (block.BlockType == BlockType.Door && isKeyFound == false)
                                {
                                    Console.WriteLine("Der er en låst dør, du ikke kan komme forbi!");
                                }
                                else
                                {
                                    isAbleToMove = true;
                                    break;
                                }
                            }

                        }
                    }
                }
            }
            return isAbleToMove;
        }
        /// <summary>
        /// Prints out the omve text.
        /// </summary>
        public static void MoveText()
        {
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Brug piletasterne til at flytte dig..");
            Console.WriteLine("-------------------------------------------");
        }
        /// <summary>
        /// Translate the visual from the block objects.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static char FieldTranslator(Block block)
        {
            if (!block.IsDiscovered) return ' ';
            if (block.IsPlayerOnField) return '■';
            if (block.BlockType == BlockType.Monster) return 'M';
            if (block.BlockType == BlockType.TreasureChest) return 'X';
            if (block.BlockType == BlockType.Rock) return 'K';
            if (block.BlockType == BlockType.Empty) return 'X';
            if (block.BlockType == BlockType.Sword) return 'S';
            if (block.BlockType == BlockType.Door) return 'D';
            if (block.BlockType == BlockType.Key) return 'N';
            else return 'E';
        }
        /// <summary>
        /// Prints out the board.
        /// </summary>
        /// <param name="level"></param>
        public static void VisualPrint(Block[] level)
        {
            Console.Clear();
            char p1 = FieldTranslator(level[0]);
            char p2 = FieldTranslator(level[1]);
            char p3 = FieldTranslator(level[2]);
            char p4 = FieldTranslator(level[3]);
            char p5 = FieldTranslator(level[4]);
            char p6 = FieldTranslator(level[5]);
            char p7 = FieldTranslator(level[6]);
            char p8 = FieldTranslator(level[7]);
            char p9 = FieldTranslator(level[8]);
            char p10 = FieldTranslator(level[9]);
            char p11 = FieldTranslator(level[10]);
            char p12 = FieldTranslator(level[11]);
            char p13 = FieldTranslator(level[12]);
            char p14 = FieldTranslator(level[13]);
            char p15 = FieldTranslator(level[14]);
            char p16 = FieldTranslator(level[15]);
            char p17 = FieldTranslator(level[16]);
            char p18 = FieldTranslator(level[17]);
            char p19 = FieldTranslator(level[18]);
            char p20 = FieldTranslator(level[19]);
            char p21 = FieldTranslator(level[20]);
            char p22 = FieldTranslator(level[21]);
            char p23 = FieldTranslator(level[22]);
            char p24 = FieldTranslator(level[23]);
            char p25 = FieldTranslator(level[24]);
            int v = 5, h = 2; Console.SetCursorPosition(v, h); Console.Write("┌─────┬─────┬─────┬─────┬─────┐"); 
            Console.SetCursorPosition(v, h + 1); Console.Write($"│ {p1} {p1} │ {p2} {p2} │ {p3} {p3} │ {p4} {p4} │ {p5} {p5} │");
            Console.SetCursorPosition(v, h + 2); Console.Write($"│  {p1}  │  {p2}  │  {p3}  │  {p4}  │  {p5}  │");
            Console.SetCursorPosition(v, h + 3); Console.Write($"│ {p1} {p1} │ {p2} {p2} │ {p3} {p3} │ {p4} {p4} │ {p5} {p5} │");
            Console.SetCursorPosition(v, h + 4); Console.Write($"├─────┼─────┼─────┼─────┼─────┤");
            Console.SetCursorPosition(v, h + 5); Console.Write($"│ {p6} {p6} │ {p7} {p7} │ {p8} {p8} │ {p9} {p9} │ {p10} {p10} │");
            Console.SetCursorPosition(v, h + 6); Console.Write($"│  {p6}  │  {p7}  │  {p8}  │  {p9}  │  {p10}  │");
            Console.SetCursorPosition(v, h + 7); Console.Write($"│ {p6} {p6} │ {p7} {p7} │ {p8} {p8} │ {p9} {p9} │ {p10} {p10} │");
            Console.SetCursorPosition(v, h + 8); Console.Write($"├─────┼─────┼─────┼─────┼─────┤");
            Console.SetCursorPosition(v, h + 9); Console.Write($"│ {p11} {p11} │ {p12} {p12} │ {p13} {p13} │ {p14} {p14} │ {p15} {p15} │");
            Console.SetCursorPosition(v, h + 10); Console.Write($"│  {p11}  │  {p12}  │  {p13}  │  {p14}  │  {p15}  │");
            Console.SetCursorPosition(v, h + 11); Console.Write($"│ {p11} {p11} │ {p12} {p12} │ {p13} {p13} │ {p14} {p14} │ {p15} {p15} │");
            Console.SetCursorPosition(v, h + 12); Console.Write($"├─────┼─────┼─────┼─────┼─────┤");
            Console.SetCursorPosition(v, h + 13); Console.Write($"│ {p16} {p16} │ {p17} {p17} │ {p18} {p18} │ {p19} {p19} │ {p20} {p20} │");
            Console.SetCursorPosition(v, h + 14); Console.Write($"│  {p16}  │  {p17}  │  {p18}  │  {p19}  │  {p20}  │");
            Console.SetCursorPosition(v, h + 15); Console.Write($"│ {p16} {p16} │ {p17} {p17} │ {p18} {p18} │ {p19} {p19} │ {p20} {p20} │");
            Console.SetCursorPosition(v, h + 16); Console.Write($"├─────┼─────┼─────┼─────┼─────┤");
            Console.SetCursorPosition(v, h + 17); Console.Write($"│ {p21} {p21} │ {p22} {p22} │ {p23} {p23} │ {p24} {p24} │ {p25} {p25} │");
            Console.SetCursorPosition(v, h + 18); Console.Write($"│  {p21}  │  {p22}  │  {p23}  │  {p24}  │  {p25}  │");
            Console.SetCursorPosition(v, h + 19); Console.Write($"│ {p21} {p21} │ {p22} {p22} │ {p23} {p23} │ {p24} {p24} │ {p25} {p25} │");
            Console.SetCursorPosition(v, h + 20); Console.Write($"└─────┴─────┴─────┴─────┴─────┘");
        }
    }
}
