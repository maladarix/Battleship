using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src
{

    /*Cette section sert à initialiser une structure pour le jeu, afin de pouvoir utiliser ses composantes de 
    manière plus efficace. Il y a d'abbord le tableau qui est créer dans un array de 10X10, il y en a un pour 
    le joueur, un pour le bot et un qui est caché. Ensuite, les bateaux sont initialisés,
    5 pour le joueur et 5 pour le bot. C'est ensuite les coordonnées de tirs du bot qui sont initialisées à 
    l'extérieur de la grille de jeu pour permettre de faire des vérifications plus tard. La direction du bateau 
    est aussi à -1 pour les mêmes raisons. Finalement, la difficulté est initialisé pour que le joueur puisse choisir.*/
    struct Game {
        public static char[,] PlayerBoard = new char[10, 10];
        public static char[,] BotBoard = new char[10, 10];
        public static char[,] HiddenBoard = new char[10, 10];
        public static Boat[] PlayerBoats = new Boat[5];
        public static Boat[] BotBoats = new Boat[5];
        public static int FirstBotHitX = -1;
        public static int FirstBotHitY = -1;
        public static int LastBotHitX = -1;
        public static int LastBotHitY = -1;
        public static int BoatDirection = -1;
        public static int Difficulty = 0;


        
        /*Cette partie du code donne un nom et une longueur à chacun des bateaux autant pour le joueur
        que pour le bot dans l'ordre qu'ils sont écrits.*/
        public static void InitBoat()
        {
            string[] BoatName = { "Carrier", "Batleship", "Destroyer", "Submarine", "Patrolboat" };
            int[] BoatLength = {5, 4, 3, 3, 2};

            for (int i = 0; i < BoatName.Length; i++)
            {
                PlayerBoats[i] = new Boat(BoatName[i], BoatLength[i]);
                BotBoats[i] = new Boat(BoatName[i], BoatLength[i]);
            }
        }

        /* Cette partie de code fait en sorte que les cases dans les tableaux de jeu soient des points "." 
        au lieu d'être des espaces vides ou des chiffres.*/
        public static void InitBoard()
        {
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    PlayerBoard[i, j] = '.';
                    BotBoard[i, j] = '.';
                    HiddenBoard[i, j] = '.';
                }
            }
        }

        
        /* Cette section met les lettres et le numéros des rangées et des colonnes afin de pouvoir choisir des 
        coordonnées dans les tableaux. Elle donne aussi la couleur aux cases selon si c'est de l'eau ou si c'est touché
        et va afficher les lettres dans les cases des tableaux en remplaçant les points. Ces modifications seront fait
        uniquement sur le tableau du joueur et celui qui est "caché" puisque celui-ce sert à montrer ce que le joueur
        connait du tableau du bot. Le tableau du bot reste invisible pour ne pas voir ses bateaux.*/
       
        public static void ShowBoard(bool Player)
        {
            if (Player)
            {
                Console.WriteLine("   A B C D E F G H I J");

            }

            for (int i = 0; i <= 9; i++)
            {
                Console.Write($"{(i + 1).ToString().PadLeft(2)} ");
                for (int j = 0; j <= 9; j++)
                {
                    if (Player)
                    {
                        if(PlayerBoard[i, j] == 'x' || PlayerBoard[i, j] == 'd')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if(PlayerBoard[i, j] == 'X')
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        Console.Write(($"{PlayerBoard[i, j]} ").ToUpper());
                    }
                    else
                    {
                        if (HiddenBoard[i, j] == 'x' || HiddenBoard[i, j] == 'd')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (HiddenBoard[i, j] == 'X')
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        Console.Write(($"{HiddenBoard[i, j]} ").ToUpper());
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
          
        }

        /* Cette fonction sert a effacer ce qui était présent sur l'affichage du débuggeur pour éviter
        de surcharger le visuel. C'est aussi ici que les noms d'à qui appartiennent les tableaux sont affichés*/
        public static void ShowGame()
        {
            Console.Clear();
            Console.WriteLine("        Computer");
            ShowBoard(false);
            ShowBoard(true);
            Console.WriteLine("          You");
        }

        /*Ceci est la fontion du programme qui sert de boucle de jeu, elle montre le jeu et appelle les foncions
         nécéssaires pour jouer le jeu. Tant qu'il y a des bateaux en vie sur les deux tableaux, le jeu continue.
         Par contre, si le joueur ou le bot n'a plus de bateau en vie, un vainqueur est déclaré.*/
        public static void Play()
        {
            do
            {
                ShowGame();
                ConsoleInteractions.AskAttack();
                if(!boatLogic.AreBotBoatsAlive())
                {
                    break;
                }
                System.Threading.Thread.Sleep(3000);
                attackLogic.BotAttack();
                if (!boatLogic.ArePlayerBoatsAlive())
                {
                    break;
                }
                System.Threading.Thread.Sleep(3000);
            }
            while (true);

            if(boatLogic.ArePlayerBoatsAlive())
            {
                Console.WriteLine("You have won! GG");
            }
            else
            {
                Console.WriteLine("Get gud the bot has won");
            }
        }
    }

}