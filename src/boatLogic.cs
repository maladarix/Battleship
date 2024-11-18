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

            for (int i = 0; i < boatLenght; i++)
            {
                board[vertical ? (x, y + i) : (x + i, y)] = "S";
            }
            boats[boatNum].FirstCoord = new[] {x, y};
            boats[boatNum].LastCoord = new[] {vertical ? (x, y + boatLenght) : (x + boatLenght, y)};
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
            for (int i = (vertical ? y : x); i <= (vertical ? y : x) + boatLenght; i++)
            {
                if (Game.PlayerBoard[vertical ? (x, i) : (i, y)] != ".")
                {
                    Console.WriteLine("The ship is in conflit with another ship");
                    return false;
                }
            }
            return true;
        }

        public static void PlaceRandomBoat()
        {
            for (int i = 0; i < Game.Boats.lenght; i++)
            {
                Random random = new Random();
                int x = 0;
                int y = 0;
                bool vertical = false;
                do
                {
                    x = random.Next(10);
                    y = random.Next(10);
                    vertical = random.Next(2) == 1 ? true : false;
                }
                while (CheckBoatPlacementInGrid(x, y, vertical, Game.Boats[i].lenght) && CheckBoatPlacementConflict(x, y, vertical, Game.Boats[i].lenght));
                PlaceBoat(x, y, vertical, Game.Boats[i].lenght, true);
            }
        }
    }
}
