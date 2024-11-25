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

            if (int.TryParse(xString, out int number))
            {
                x = number - 1;
                x = number - 1;
                if (yString.Length == 1 && char.IsLetter(yString[0]))
                {
                    char letter = Convert.ToChar(yString);
                    y = (int)letter - 65;

                    if (y < 10 && y >= 0 && x < 10 && x >= 0)
                    {
                        return true;
                    }
                }
            }         
            Console.WriteLine("Invalid Input");
            return false;
        }
        public static bool AlreadyHit(int x, int y, bool PlayerTurn)
        {
            if (PlayerTurn)
            {
                if (Game.BotBoard[x, y] == 'X')
                {
                    Console.WriteLine("You've already attacked that coordinate");
                    return true;
                }
            }
            else if (Game.PlayerBoard[x, y] == 'X')
            {
                return true;
            }
            return false;
        }
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
