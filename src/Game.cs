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
        public static int FirstBotHitX = -1;
        public static int FirstBotHitY = -1;
        public static int LastBotHitX = -1;
        public static int LastBotHitY = -1;
        public static int BoatDirection = -1;

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
            if (Player)
            {
                Console.WriteLine("   A B C D E F G H I J");

            }

            for (int i = 0; i <= 9; i++)
            {
                Console.Write($"{(i + 1).ToString().PadLeft(2)} ");
                for (int j = 0; j <= 9; j++)
                {
                    if (Player)
                    {
                        if(PlayerBoard[i, j] == 'x')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if(PlayerBoard[i, j] == 'X')
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        Console.Write(($"{PlayerBoard[i, j]} ").ToUpper());
                    }
                    else
                    {
                        if (HiddenBoard[i, j] == 'x')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (HiddenBoard[i, j] == 'X')
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        Console.Write(($"{HiddenBoard[i, j]} ").ToUpper());
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
          
        }

        public static void ShowGame()
        {
            Console.Clear();
            Console.WriteLine("        Computer");
            ShowBoard(false);
            ShowBoard(true);
            Console.WriteLine("          You");
        }

        public static void Play()
        {
            do
            {
                ShowGame();
                ConsoleInteractions.AskAttack();
                if(!boatLogic.AreBotBoatsAlive())
                {
                    break;
                }
                System.Threading.Thread.Sleep(3000);
                attackLogic.BotAttack();
                if (!boatLogic.ArePlayerBoatsAlive())
                {
                    break;
                }
                System.Threading.Thread.Sleep(3000);
            }
            while (true);

            if(boatLogic.ArePlayerBoatsAlive())
            {
                Console.WriteLine("You have won! GG");
            }
            else
            {
                Console.WriteLine("Get gud the bot has won");
            }
        }
    }

}