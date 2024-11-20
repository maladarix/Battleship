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
        public static bool Coord(string x, string yString, out int letterNum, out int outY)
        {
            outY = -1; 
            letterNum = -1;
            int y;
            if (int.TryParse(yString, out int number))
            {
                y = number;
                outY = number;
                if (x.Length == 1 && char.IsLetter(x[0]))
                {
                    char letter = Convert.ToChar(x);
                    letterNum = (int)letter - 65;

                    if (letterNum < 10 && letterNum >= 0 && y < 10 && y >= 0)
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
                if (Game.PlayerBoard[x, y] == 'X')
                {
                    Console.WriteLine("You've already attacked that coordinate");
                    return false;
                }
            }
            else if (Game.BotBoard[x, y] == 'X')
            {
                return false;
            }
            return true;
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
