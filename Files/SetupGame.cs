using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonEscape.Files
{
    internal class SetupGame
    {
        public enum BlockType
        {
            Empty,
            Rock,
            Door,
            Monster,
            Key,
            Sword,
            TreasureChest
        }
        public class Block
        {
            public BlockType BlockType { get; set; }
            public bool IsPlayerOnField { get; set; }
            public bool IsDiscovered { get; set; }
            public int[] Coordinates { get; set; }
            public Block(BlockType blockType, bool isPlayerOnField, bool Isdiscovered, int[] coordinates)
            {
                BlockType = blockType;
                IsPlayerOnField = isPlayerOnField;
                IsDiscovered = Isdiscovered;
                Coordinates = coordinates;
            }
        }
        public static Block[] SetupGameMethod()
        {
            Block[] blocks = [];
            bool userHasChosenLevel = false;
            while (!userHasChosenLevel)
            {
                string userInput = "";
                Console.Clear();
                Console.WriteLine("Vælg en bane..");
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("Tast [1] for at vælge \"Bekæmp Monstret\".");
                userInput = Console.ReadLine();
                if (userInput.ToLower() == "1")
                {
                    blocks = FightTheMonster();
                    userHasChosenLevel = true;
                }
                else
                {
                    Console.WriteLine("Forkert input. Prøv igen!!");
                    Console.ReadKey();
                }
            }
            return blocks;
        }
        public static Block[] FightTheMonster()
        {
            Block[] fightTheMonster = [
            new Block(BlockType.Empty, true, true, [0, 0]),
            new Block(BlockType.Empty, false, false, [1, 0]),
            new Block(BlockType.Rock, false, false, [2, 0]),
            new Block(BlockType.Sword, false, false, [3, 0]),
            new Block(BlockType.Empty, false, false, [4, 0]),
            new Block(BlockType.Rock, false, false, [0, 1]),
            new Block(BlockType.Empty, false, false, [1, 1]),
            new Block(BlockType.Rock, false, false, [2, 1]),
            new Block(BlockType.Rock, false, false, [3, 1]),
            new Block(BlockType.Empty, false, false, [4, 1]),
            new Block(BlockType.Empty, false, false, [0, 2]),
            new Block(BlockType.Empty, false, false, [1, 2]),
            new Block(BlockType.Empty, false, false, [2, 2]),
            new Block(BlockType.Empty, false, false, [3, 2]),
            new Block(BlockType.Empty, false, false, [4, 2]),
            new Block(BlockType.Monster, false, false, [0, 3]),
            new Block(BlockType.Rock, false, false, [1, 3]),
            new Block(BlockType.Empty, false, false, [2, 3]),
            new Block(BlockType.Rock, false, false, [3, 3]),
            new Block(BlockType.Rock, false, false, [4, 3]),
            new Block(BlockType.Key, false, false, [0, 4]),
            new Block(BlockType.Rock, false, false, [1, 4]),
            new Block(BlockType.Empty, false, false, [2, 4]),
            new Block(BlockType.Door, false, false, [3, 4]),
            new Block(BlockType.TreasureChest, false, false, [4, 4])
            ];

            return fightTheMonster;
        }


    }
}
