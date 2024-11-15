using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src
{
    internal class boatLogic
    {
        public static void PlaceBoat(int x, int y, bool vertical, int boatLenght, bool bot = false)
        {
            for (int i = 0; i < boatLenght; i++)
            {
                (bot == true ? Game.BotBoard : Game.PlayerBoard)[vertical ? (x, i) : (i, y)] = "S";
            }
        }

        public static bool CheckBoatPlacementInGrid(int x, int y, bool vertical, int boatLenght)
        {
            if((vertical ? y + boatLenght : x + boatLenght) < 9)
            {
                return true;
            }
            else
            {
                Console.WriteLine("The ship placement is out of bound");
                return false;
            }
        }

        public static bool CheckBoatPlacementConflict(int x, int y, bool vertical, int boatLenght)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Game.PlayerBoard[vertical ? (x, i) : (i, y)] != ".")
                {
                    Console.WriteLine("The ship is in conflit with another ship");
                    return false;
                }
            }
        }
    }
}
