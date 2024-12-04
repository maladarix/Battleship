using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src
{
    /* Structure des bateaux, elle permet d'initialiser les variables 
     nécéssaires à la création de tous les bateaux. */
    public struct Boat
    {
        public string Name;
        public int Length;
        public int FirstX = -1;
        public int FirstY = -1;
        public int LastX = -1;
        public int LastY = -1;
        public int Hp;

        //Déclaration des variables des composantes d'un bateau
        public Boat(string BoatName, int BoatLegnth)
        {
            Name = BoatName;
            Length = BoatLegnth;
            Hp = BoatLegnth;
        }
    }
}
