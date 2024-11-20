using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src
{
    struct Game {
        public static char[,] PlayerBoard = new char[10, 10];
        public static char[,] BotBoard = new char[10, 10];
        public static char[,] HiddenBoard = new char[10, 10];
        public static Boat[] PlayerBoats = new Boat[5];
        public static Boat[] BotBoats = new Boat[5];

        public static void InitBoat()
        {
            string[] BoatName = { "Carrier", "Batleship", "Destroyer", "Submarine", "Patrolboat" };
            int[] BoatLength = {5, 4, 3, 3, 2};

            for (int i = 0; i < BoatName.Length; i++)
            {
                PlayerBoats[i] = new Boat(BoatName[i], BoatLength[i]);
                BotBoats[i] = new Boat(BoatName[i], BoatLength[i]);
            }
        }

        public static void InitBoard()
        {
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    PlayerBoard[i, j] = '.';
                    BotBoard[i, j] = '.';
                    HiddenBoard[i, j] = '.';
                }
            }
        }
        public static void ShowBoard(bool Player)
        {
            Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
            Char[] letterList = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            for (int i = 0; i <= 9; i++)
            {
                Console.Write(letterList[i] + " ");
                for (int j = 0; j <= 9; j++)
                {
                    if (Player)
                    {
                        Console.Write($"{PlayerBoard[i, j]} ");
                    }
                    else
                    {
                        Console.Write($"{HiddenBoard[i, j]} ");  
                    }
                }
                Console.WriteLine();
            }
          
        }
    }

}


