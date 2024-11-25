using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src
{
    internal class ConsoleInteractions
    {
        public static void AskPlayOrQuit(ref bool endGame)
        {
            String playerInput;
            do
            {
                Console.WriteLine("1-Play");
                Console.WriteLine("2-Quit");
                playerInput = Console.ReadLine();

            } while (!Verification.PlayOrQuit(playerInput));

            switch(playerInput)
            {
                case "1":
                    Game.InitBoat();
                    Game.InitBoard();
                    boatLogic.PlaceRandomBoat();
                    AskBoatPlacement();
                    Game.Play();
                    break;

                case "2":
                    endGame = true;
                    break;
            }
        }
        public static void AskBoatPlacement()
        {
            Console.Clear();
            Game.ShowBoard(true);
            for (int i = 0; i < Game.PlayerBoats.Length; i++)
            {
                string boatDirection;
                bool vertical = false;
                int x = 0;
                int y = 0;
                string userInput;
                
                Console.WriteLine($"You have to place the {Game.PlayerBoats[i].Name} with a length of {Game.PlayerBoats[i].Length}");
                do
                {
                    Console.WriteLine("Do you want your boat to be placed vertically or horizontally? (Type V or H)");
                    boatDirection = Console.ReadLine();
                } while (!Verification.VOrH(boatDirection, ref vertical));

                do
                {
                    Console.WriteLine("Enter the coordinates that correspond to the top-left corner of the boat (Exemple: A1)");
                    userInput = Console.ReadLine();

                } while (!Verification.Coord(userInput.Substring(1).ToUpper(), userInput.Substring(0, 1).ToUpper(), out x, out y) || !boatLogic.CheckBoatPlacementInGrid(x, y, vertical, Game.PlayerBoats[i].Length) || !boatLogic.CheckBoatPlacementConflict(x, y, vertical, Game.PlayerBoats[i].Length));

                boatLogic.PlaceBoat(x, y, vertical, Game.PlayerBoats[i].Length, i, false);
                Console.Clear();
                Game.ShowBoard(true);
            }
        }

        public static void AskAttack()
        {
            Console.Clear();
            Game.ShowBoard(false);
            Console.WriteLine("=================");
            Game.ShowBoard(true);

            string userInput;
            bool exitLoop = false;
            int x = -1;
            int y = -1;
            do
            {
                Console.WriteLine("Enter a coordinate that you want to attack (Exemple:A1)");
                userInput = Console.ReadLine();

                if(Verification.Coord(userInput.Substring(1).ToUpper(), userInput.Substring(0, 1).ToUpper(), out x, out y))
                {
                    if (!Verification.AlreadyHit(x, y, true))
                    {
                        exitLoop = true;
                    }
                }

            } while ( exitLoop == false );
            attackLogic.Attack(x, y, true);
        }

        public static void BotAttack()
        {
            Random rnd = new Random();
            int x = rnd.Next(10);
            int y = rnd.Next(10) + 65;
            bool exitLoop = false;

            do
            {
                if (Verification.Coord(x.ToString(), ((char)y).ToString(), out x, out y))
                {
                    if(Verification.AlreadyHit(x, y, false))
                    {
                        exitLoop = true;
                    }
                }
            }
            while (exitLoop);
            attackLogic.Attack(x, y, false);
        }
    }
}
