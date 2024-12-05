using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src
{
    internal class Verification
    {
        //Verification of the play or quit input
        public static bool PlayOrQuit(string input)
        {
            if (input == "1" || input == "2")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid input");
                return false;
            }
        }
        //verification of the input of coordinates
        public static bool Coord(string xString, string yString, out int x, out int y)
        {
            x = -1; 
            y = -1;

            if (int.TryParse(yString, out int number))
            {
                y = number - 1;
                if (xString.Length == 1 && char.IsLetter(xString[0]))
                {
                    char letter = Convert.ToChar(xString);
                    x = (int)letter - 65;

                    if (y < 10 && y >= 0 && x < 10 && x >= 0)
                    {
                        return true;
                    }
                }
            }         
            Console.WriteLine("Invalid Input");
            return false;
        }
        //verification if the coord has already been hit
        public static bool AlreadyHit(int x, int y, bool PlayerTurn)
        {
            if (PlayerTurn)
            {
                if (Game.BotBoard[y, x] == 'X')
                {
                    Console.WriteLine("You've already attacked that coordinate");
                    return true;
                }
            }
            else if (Game.PlayerBoard[y, x] == 'X')
            {
                return true;
            }
            return false;
        }
        //Check if player chose vertical or horizontal
        public static bool VOrH(string input, ref bool vertical)
        {
            if (input.ToUpper() == "V" || input.ToUpper() == "H")
            {
                if (input.ToUpper() == "V")
                {
                    vertical = true;
                }
                else
                {
                    vertical = false;
                }
                return true;
            }
            else
            {
                vertical = false;
                return false;
            }
        }
    }
}
