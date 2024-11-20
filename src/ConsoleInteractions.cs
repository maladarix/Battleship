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
                    
                    AskBoatPlacement();
                    break;

                case "2":
                    endGame = true;
                    break;
            }
        }
        public static void AskBoatPlacement()
        {
            Game.ShowBoard(true);
            for (int i = 0; i < Game.PlayerBoats.Length; i++)
            {
                string boatDirection;
                bool vertical = false;
                string x;
                string userInput;
                {
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

                        x = userInput.Substring(0, 1).ToUpper();
                          
                        
                    } while (!Verification.Coord(x, userInput.Substring(1), out int letterNum, out int y) || !boatLogic.CheckBoatPlacementInGrid(letterNum, y, vertical, Game.PlayerBoats[i].Length) || !boatLogic.CheckBoatPlacementConflict(letterNum, y, vertical, Game.PlayerBoats[i].Length));

                }
            }


        }
    }
}
