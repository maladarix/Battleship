using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src
{
    internal class boatLogic
    {
        public static void PlaceBoat(int x, int y, bool vertical, int boatLenght, int boatNum, bool bot = false)
        {
            var boats = bot ? Game.BotBoats : Game.PlayerBoats;
            var board = bot ? Game.BotBoard : Game.PlayerBoard;

            boats[boatNum].FirstX = x;
            boats[boatNum].FirstY = y;
            for (int i = 0; i < boatLenght; i++)
            {
                if(vertical)
                {
                    board[y + i, x] = 'S';
                    boats[boatNum].LastX = x;
                    boats[boatNum].LastY = y + boatLenght;
                }
                else
                {
                    board[y, x + i] = 'S';
                    boats[boatNum].LastX = x + boatLenght;
                    boats[boatNum].LastY = y;
                }
            }
            
        }

        public static bool CheckBoatPlacementInGrid(int x, int y, bool vertical, int boatLenght, bool bot = false)
        {
            if((vertical ? y + boatLenght : x + boatLenght) <= 10)
            {
                return true;
            }
            else
            {
                if(!bot)
                {
                    Console.WriteLine("The ship placement is out of bound");
                } 
                return false;
            }
        }

        public static bool CheckBoatPlacementConflict(int x, int y, bool vertical, int boatLenght, bool bot = false)
        {
            for (int i = (vertical ? y : x); i < (vertical ? y : x) + boatLenght; i++)
            {
                int col = vertical ? x : i;
                int row = vertical ? i : y;
                if(bot)
                {
                    if (Game.BotBoard[row, col] != '.')
                    {
                        return false;
                    }
                }
                else
                {
                    if (Game.PlayerBoard[row, col] != '.')
                    {
                        Console.WriteLine("The ship is in conflict with another ship");
                        return false;
                    }
                }
            }
            return true;
        }

        public static void PlaceRandomBoat()
        {
            for (int i = 0; i < Game.BotBoats.Length; i++)
            {
                Random random = new Random();
                int x = 0;
                int y = 0;
                bool vertical = false;
                bool exitLoop = false;
                do
                {
                    x = random.Next(10);
                    y = random.Next(10);
                    vertical = random.Next(2) == 1 ? true : false;
                    if(CheckBoatPlacementInGrid(x, y, vertical, Game.BotBoats[i].Length, true) == true)
                    {
                        if(CheckBoatPlacementConflict(x, y, vertical, Game.BotBoats[i].Length,  true))
                        {
                            exitLoop = true;
                        }
                    }
                }
                while (exitLoop == false);
                PlaceBoat(x, y, vertical, Game.BotBoats[i].Length, i, true);
            }
        }

        public static bool ArePlayerBoatsAlive()
        {
            foreach (var boat in Game.PlayerBoats)
            {
                if (boat.Hp > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AreBotBoatsAlive()
        {
            foreach (var boat in Game.BotBoats)
            {
                if (boat.Hp > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
