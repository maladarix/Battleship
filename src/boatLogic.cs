using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src
{
    internal class boatLogic
    {
        //Cette fonction permet de placer les bateaux
        public static void PlaceBoat(int x, int y, bool vertical, int boatLenght, int boatNum, bool player = false)
        {
            //Déterminer entre le joueur et l'ordinateur
            var boats = player ? Game.PlayerBoats : Game.BotBoats;
            var board = player ? Game.PlayerBoard : Game.BotBoard;

            //Enregistrer les premières coordonées du bateau
            boats[boatNum].FirstX = x;
            boats[boatNum].FirstY = y;

            //Placer le bateau sur la grille
            for (int i = 0; i < boatLenght; i++)
            {
                if(vertical)
                {
                    board[y + i, x] = 'S';
                    boats[boatNum].LastX = x;
                    boats[boatNum].LastY = y + boatLenght - 1;
                }
                else
                {
                    board[y, x + i] = 'S';
                    boats[boatNum].LastX = x + boatLenght - 1;
                    boats[boatNum].LastY = y;
                }
            }
            
        }

        //Cette fonction permet de regarder si le bateau dépasse de la grille
        public static bool CheckBoatPlacementInGrid(int x, int y, bool vertical, int boatLenght, bool player = false)
        {
            if((vertical ? y + boatLenght : x + boatLenght) <= 10) //Le bateau est complètement dans la grille
            {
                return true;
            }
            else  //Le bateau dépasse
            {
                if(player)
                {
                    Console.WriteLine("The ship placement is out of bound"); 
                } 
                return false;
            }
        }

        //Cette fonction permet de savoir si le bateau serait a la même coordonée qu'un autre bateau
        public static bool CheckBoatPlacementConflict(int x, int y, bool vertical, int boatLenght, bool player = false)
        {
            for (int i = (vertical ? y : x); i < (vertical ? y : x) + boatLenght; i++)
            {
                int col = vertical ? x : i;
                int row = vertical ? i : y;

                if ((player ? Game.PlayerBoard : Game.BotBoard)[row, col] != '.') //Si ce n'est pas un '.' il y a un bateau
                {
                    if(player) Console.WriteLine("The ship is in conflict with another ship");
                    return true;
                }
            }
            return false;
        }

        //Cette fonction permet de placer les bateau aléatoirement
        public static void PlaceRandomBoat(bool player = false)
        {
            //Pour chaque bateau
            for (int i = 0; i < Game.BotBoats.Length; i++)
            {
                Random random = new Random();
                int x = 0;
                int y = 0;
                bool vertical = false;
                bool exitLoop = false;

                //Faire la loop tant que les vérification ne sont pas validées
                do
                {
                    //Déterminer les coordonées et si c'est vertical
                    x = random.Next(10);
                    y = random.Next(10);
                    vertical = random.Next(2) == 1 ? true : false;

                    //Faire les vérification de placement
                    if (CheckBoatPlacementInGrid(x, y, vertical, player ? Game.PlayerBoats[i].Length : Game.BotBoats[i].Length, player) == true)
                    {
                        if(!CheckBoatPlacementConflict(x, y, vertical, player ? Game.PlayerBoats[i].Length : Game.BotBoats[i].Length, player))
                        {
                            exitLoop = true;
                        }
                    }
                }
                while (exitLoop == false);

                //Placer le bateau aux coordonées
                PlaceBoat(x, y, vertical, player ? Game.PlayerBoats[i].Length : Game.BotBoats[i].Length, i, player);
            }
        }

        //Cette fonction permet de voir si les bateaux du joueurs sont en vie
        public static bool ArePlayerBoatsAlive()
        {
            foreach (var boat in Game.PlayerBoats)
            {
                if (boat.Hp > 0) //Si un bateau a plus de 0 "HP", dire que le joueur est en vie
                {
                    return true;
                }
            }
            return false;
        }

        //Cette fonction permet de voir si les bateaux de l'ordinateur sont en vie
        public static bool AreBotBoatsAlive()
        {
            foreach (var boat in Game.BotBoats)
            {
                if (boat.Hp > 0) //Si un bateau a plus de 0 "HP", dire que l'ordinateur est en vie
                {
                    return true;
                }
            }
            return false;
        }
    }
}
