using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src
{
    internal class ConsoleInteractions
    {
        //First menu
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
                char boatDirection;
                do
                {
                    Console.WriteLine($"You have to place the {Game.PlayerBoats[i].Name} with a length of {Game.PlayerBoats[i].Length}");
                    do
                    {
                        Console.WriteLine("Do you want your boat to be placed vertically or horizontally? (Type V or H)");
                        boatDirection = Convert.ToChar(Console.ReadLine());
                    } while (Verification.VOrH(boatDirection));

                    do
                    {
                    Console.WriteLine("Enter the coordinates that correspond to the top-left corner of the boat (Exemple: A1)");
                    String userInput = Console.ReadLine();
                    }
                    string x = userInput.Substring(0,1);
                    int y = Convert.ToInt16(userInput.Substring(1));
                }while()
            }


        }
    }
}
