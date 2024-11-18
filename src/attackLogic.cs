using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src
{
    internal class attackLogic
    {
        public static void Attack(int x, int y, bool playerTurn)
        {
            if(playerTurn)
            {
                if (Game.BotBoard[x, y] == 'S')
                {
                    
                }
            }
        }

        public static Boat getBoat(int x, int y, bool playerTurn)
        {
            var boats = playerTurn ? Game.BotBoats : Game.PlayerBoats;
            for (int i = 0; i < boats.lenght; i++)
            {
                Boat boat = boats[i];
                if() 
            }
        }
    }
}
