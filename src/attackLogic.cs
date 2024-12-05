using Battleship.src;
using WMPLib;

namespace Battleship.src
{
    internal class attackLogic
    {
        private static readonly Random rnd = new Random();


        // Cette fonction permet d'attaquer les bateaux
        public static void Attack(int x, int y, bool playerTurn)
        {
            //Déterminer le board a utiliser
            var board = playerTurn ? Game.BotBoard : Game.PlayerBoard;

            //Déterminer les chemins d'accès des sons
            var player = new WindowsMediaPlayer();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            var missPath = Path.Combine(basePath, "..", "..", "..", "src", "audio", "miss.mp3");
            var hitPath = Path.Combine(basePath, "..", "..", "..", "src", "audio", "hit.mp3");

            if (board[y, x] == 'S') //Si il y a un bateau
            {
                player.URL = Path.GetFullPath(hitPath);

                //Attaquer le bateau et changer la valeur du tableau par un 'x' (hit)
                Boat boat = HitBoat(x, y, playerTurn);
                board[y, x] = 'x';

                
                if (playerTurn) Game.HiddenBoard[y, x] = 'x'; 
                else UpdateBotHitTracking(x, y); //sinon, mettre a jour le tracking de l'ordinateur

                //Montrer le jeu avec les valeurs a jour
                Game.ShowGame();

                if (boat.Hp <= 0) SinkBoat(boat, x, y, playerTurn, board); //Si le HP du bateau est plus petit ou égal a 0, faire couler le bateau
                else Console.WriteLine("Hit !"); //Sinon, écrire "Hit !"
            }
            else //Si il n'y a pas de bateaux
            {
                player.URL = Path.GetFullPath(missPath);

                if (playerTurn) Game.HiddenBoard[y, x] = 'X'; //Si c'est le tour du joueur, changer la valeure du tableau montré au joueur par un 'X' (miss)
                board[y, x] = 'X';

                //Montrer le jeu avec les valeurs a jour
                Game.ShowGame();

                //écrire "Missed !"
                Console.WriteLine("Missed !");
            }
            //Jouer le son selon hit ou miss
            player.controls.play();
        }

        //Cette fonction permet de garder a jour l'ordinateur sur ses dernières attaques
        public static void UpdateBotHitTracking(int x, int y)
        {
            if (Game.LastBotHitX == -1) //Si il n'y a pas de précédents hit, mettre a jour le premier hit
            {
                //Mettre a jour le premier hit
                Game.FirstBotHitX = x;
                Game.FirstBotHitY = y;
            }
            else
            {
                if (x != Game.LastBotHitX) Game.BoatDirection = 1; //Si le dernier hit est sur le même x que le précédent, la direction est horizontale
                else if (y != Game.LastBotHitY) Game.BoatDirection = 2; //Sinon si le dernier hit est sur le même y que le précédent, la direction est verticale
            }

            //Mettre a jour le dernier hit
            Game.LastBotHitX = x;
            Game.LastBotHitY = y;
        }

        //Cette fonction permet de faire couler le bateau
        public static void SinkBoat(Boat boat, int x, int y, bool playerTurn, char[,] board)
        {
            Console.WriteLine($"The {boat.Name} is sunk !");

            //Mettre a jour les valeurs du tableau par des 'd' (destroyed)
            for (int i = 0; i < boat.Length; i++)
            {
                var target = (boat.FirstX == boat.LastX) ? (boat.FirstY + i, x) : (y, boat.FirstX + i);
                if (playerTurn) 
                {
                    Game.HiddenBoard[target.Item1, target.Item2] = 'd';
                } 
                else
                {
                    board[target.Item1, target.Item2] = 'd';
                }
            }

            //Mettre a jour les coordonées et la direction a -1
            if (!playerTurn)
            {
                Game.BoatDirection = -1;
                Game.FirstBotHitX = -1;
                Game.FirstBotHitY = -1;
                Game.LastBotHitX = -1;
                Game.LastBotHitY = -1;
            }
        }

        //Cette fonction permet de retirer la vie au bateau
        public static Boat HitBoat(int x, int y, bool playerTurn)
        {
            //Détermier a qui le bateau appartien
            var boats = playerTurn ? Game.BotBoats : Game.PlayerBoats;

            //Trouver le bateau qui est touché dans la liste des bateaux
            for (int i = 0; i < boats.Length; i++)
            {
                //Si les coordonées sont dans les coordonées du bateau, retirer 1 HP
                if (x >= boats[i].FirstX && x <= boats[i].LastX && y >= boats[i].FirstY && y <= boats[i].LastY)
                {
                    boats[i].Hp--;
                    return boats[i];
                }
            }
            //Cette ligne ne devrait jamais s'acctiver, il y aura toujours un bateau quand cette fonction est demandé
            throw new InvalidOperationException("No boat found. This should never happen!");
        }

        //Cette fonction est la fonction qui permet l'ordinateur d'attaquer
        public static void BotAttack()
        {
            //Changer la stratégie d'attaque selon la difficultée
            switch(Game.Difficulty)
            {
                case 1:
                    EasyAttack();
                    break;

                case 2:
                    MediumAttack();
                    break;

                case 3: 
                    HardAttack();
                    break;
            }
        }

        //Cette fonction est la stratégie qui attaque aléatoirement
        public static void EasyAttack()
        {
            int x = -1;
            int y = -1;
            RandomCoords(out x, out y);
            Attack(x, y, false);
        }

        //Cette fonction est la stratégie qui attaque aléatoirement tant qu'il n'a pas de hit
        public static void MediumAttack()
        {

            int x = -1;
            int y = -1;
            if (Game.LastBotHitX == -1 && Game.LastBotHitY == -1) //Si il n'a pas hit, attaquer aléatoirement
            {
                RandomCoords(out x, out y);
            }
            else //Sinon, attaquer en mode "destroy"
            {
                TargetAttack(out x, out y);
            }

            Attack(x, y, false);
        }

        //Cette fonction est la stratégie qui attaque selon les probabilitées qu'il y ai un bateau sur les coordonées
        public static void HardAttack()
        {
            int x = -1;
            int y = -1;
            MostProbableCoord(out x, out y);
            Attack(x, y, false);
        }

        //Cette fonction est le mode "destroy" de l'ordinateur
        public static void TargetAttack(out int x, out int y)
        {
            if (Game.BoatDirection == -1) //Si il n'a pas de direction, attaquer les 4 coordonnées autour du hit
            {
                Side4Coords(out x, out y);
            }
            else //Si il a un direction, attaquer dans le sens de la direction
            {
                DirectionCoord(out x, out y);
                if (x == -1 && y == -1) //Si aucune coordonée est trouvée, attaquer aléatoirement
                {
                    RandomCoords(out x, out y);
                }
            }
        }

        //Cette fonction permet de choisir une coordonée aléatoire
        public static void RandomCoords(out int x, out int y)
        {
            x = -1;
            y = -1;
            do
            {
                x = rnd.Next(10);
                y = rnd.Next(10);
            }
            while (Verification.AlreadyHit(x, y, false));
        }

        //Cette fonction permet de trouver les coordonées autour du hit
        public static void Side4Coords(out int x, out int y)
        {
            x = -1;
            y = -1;

            for (int i = 0; i < 4; i++)
            {
                //Ordre: Droite, gauche, bas, haut
                x = Game.LastBotHitX + (i == 0 ? 1 : (i == 1 ? -1 : 0));
                y = Game.LastBotHitY + (i == 2 ? 1 : (i == 3 ? -1 : 0));

                //Vérifier si la coordonée est valide
                if (y <= 9 && x <= 9 && y >= 0 && x >= 0 && !Verification.AlreadyHit(x, y, false))
                {
                    return;
                }
            }
        }

        //Cette fonction détermine la prochaine coordonée selon la direction
        public static void DirectionCoord(out int x, out int y)
        {
            x = Game.LastBotHitX;
            y = Game.LastBotHitY;

            if (Game.BoatDirection == 1) // Si la direction est horizontale
            {
                if(Game.FirstBotHitX - Game.LastBotHitX < 0) //Si la direction est a droite
                {
                    //Vérifier si la coordonée de droite est valide
                    if (Game.LastBotHitX + 1 <= 9 && !Verification.AlreadyHit(Game.LastBotHitX + 1, Game.LastBotHitY, false))
                    {
                        x = Game.LastBotHitX + 1;
                        y = Game.LastBotHitY;
                    }
                    //Vérifier si la coordonée de gauche du premier hit est valide
                    else if (Game.FirstBotHitX - 1 >= 0 && !Verification.AlreadyHit(Game.FirstBotHitX - 1, Game.FirstBotHitY, false))
                    {
                        x = Game.FirstBotHitX - 1;
                        y = Game.LastBotHitY;
                    }
                }
                else //if direction == left
                {
                    //Vérifier si la coordonée de gauche est valide 
                    if (Game.LastBotHitX - 1 >= 0 && !Verification.AlreadyHit(Game.LastBotHitX - 1, Game.LastBotHitY, false))
                    {
                        x = Game.LastBotHitX - 1;
                        y = Game.LastBotHitY;

                    }
                    //Vérifier si la coordonée de droite du premier hit est valide
                    else if (Game.FirstBotHitX + 1 <= 9 && !Verification.AlreadyHit(Game.FirstBotHitX + 1, Game.FirstBotHitY, false))
                    {
                        x = Game.FirstBotHitX + 1;
                        y = Game.LastBotHitY;
                    }
                }

                //Si aucune coordonnée n'a été trouvée, attaquer les 4 cotés
                if(x == Game.LastBotHitX)
                {
                    Game.BoatDirection = -1;
                    Side4Coords(out x, out y);
                }
            }
            else if (Game.BoatDirection == 2) //Si la direction était verticale
            {
                if (Game.FirstBotHitY - Game.LastBotHitY < 0) //Si la direction est en bas
                {
                    //Vérifier si la coordonée du bas est valide
                    if (Game.LastBotHitY + 1 <= 9 && !Verification.AlreadyHit(Game.LastBotHitX, Game.LastBotHitY + 1, false))
                    {
                        x = Game.LastBotHitX;
                        y = Game.LastBotHitY + 1;
                    }
                    //Vérifier si la coordonée du bas du premier hit est valide
                    else if (Game.FirstBotHitY - 1 >= 0 && !Verification.AlreadyHit(Game.FirstBotHitX, Game.FirstBotHitY - 1, false))
                    {
                        x = Game.LastBotHitX;
                        y = Game.FirstBotHitY - 1;
                    }
                }
                else //Si la direction est en haut
                {
                    //Vérifier si la coordonée du haut est valide
                    if (Game.LastBotHitY - 1 >= 0 && !Verification.AlreadyHit(Game.LastBotHitX, Game.LastBotHitY - 1, false))
                    {
                        x = Game.LastBotHitX;
                        y = Game.LastBotHitY - 1;
                    }
                    //Vérifier si la coordonée du haut du premier hit est valide
                    else if (Game.FirstBotHitY + 1 <= 9 && !Verification.AlreadyHit(Game.FirstBotHitX, Game.FirstBotHitY + 1, false))
                    {
                        x = Game.LastBotHitX;
                        y = Game.FirstBotHitY + 1;
                    }
                }

                //Si aucune coordonnée n'a été trouvée, attaquer les 4 cotés
                if (y == Game.LastBotHitY)
                {
                    Game.BoatDirection = -1;
                    Side4Coords(out x, out y);
                }
            }
        }

        //Cette fonction permet de trouver la coordonée la plus probable
        public static void MostProbableCoord(out int x, out int y)
        {
            //Créer un tableau pour enregistrer les probabilitées de chaques cases
            int[,] ProbaBoard = new int[10, 10];

            //Pour chaque orientation du bateau
            for (int orientation = 0; orientation < 2; orientation++)
            {
                //Pour chaque bateau
                foreach (var boat in Game.PlayerBoats)
                {
                    //Si le bateau est en vie
                    if (boat.Hp > 0)
                    {
                        //Regarder chaques coordonées y
                        for (int row = 0; row < ProbaBoard.GetLength(0); row++)
                        {
                            //Regarder chaques coordonées x
                            for (int col = 0; col < ProbaBoard.GetLength(1); col++)
                            {
                                bool canPlace = false;
                                bool boosted = false;
                                //Regarder chaques coordonées où le bateau est placé
                                for (int offset = 0; offset < boat.Length; offset++)
                                {
                                    //Calculer a quelle ligne et quelle colonne les vérification vont se faire
                                    int targetRow = orientation == 0 ? row : row + offset;
                                    int targetCol = orientation == 0 ? col + offset : col;

                                    //Vérification si le bateau est a l'extérieur ou il y a un miss ou un bateau coulé a la coordonée. Si oui,
                                    //le bateau ne doit pas augmenté les probabilitées de son placement
                                    if (targetRow >= 10 || targetCol >= 10 ||
                                        Game.PlayerBoard[targetRow, targetCol].ToString() == "X" ||
                                        Game.PlayerBoard[targetRow, targetCol].ToString().ToUpper() == "D")
                                    {
                                        canPlace = false;
                                        break;
                                    }

                                    //Si la coordonée contien un hit, booster les probabilitée du bateau placé a cet endroit
                                    if (Game.PlayerBoard[targetRow, targetCol].ToString() == "x")
                                    {
                                        boosted = true;
                                    }

                                    //Si tout la longueur du bateau a été testé, il peut placer le bateau pour augmenter les probabilitées a ces coordonées
                                    if (offset == boat.Length - 1)
                                    {
                                        canPlace = true;
                                        break;
                                    }
                                }
                                //Si le bateau peut être placé
                                if (canPlace)
                                {
                                    //Augmenté les probabilitées aux coordonées du bateau
                                    for (int offset = 0; offset < boat.Length; offset++)
                                    {
                                        int markRow = orientation == 0 ? row : row + offset;
                                        int markCol = orientation == 0 ? col + offset : col;

                                        if (Game.PlayerBoard[markRow, markCol].ToString().ToLower() != "x")
                                        {
                                            //Si boosté, ajouté 10 sinon 1
                                            ProbaBoard[markRow, markCol] += boosted ? 10 : 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Créer un liste pour tout les meilleurs probabilitées
            List<(int x, int y)> maxProbabilities = new List<(int x, int y)>();
            int maxProbability = 0;
            x = 0;
            y = 0;

            //Regarder chaques coordonées y
            for (int row = 0; row < ProbaBoard.GetLength(0); row++)
            {
                //Regarder chaques coordonées x
                for (int col = 0; col < ProbaBoard.GetLength(1); col++)
                {
                    //Si la probabilitée est plus grande ou égale a la plus grande existante
                    if (ProbaBoard[row, col] >= maxProbability)
                    {
                        //Si la probabilitée est plus grande que la plus grande existante, supprimer les éléments de la liste
                        if (ProbaBoard[row, col] > maxProbability)
                        {
                            maxProbabilities.Clear();
                        }

                        //Ajuster la meilleure probabilitée et l'ajouter a la liste
                        maxProbability = ProbaBoard[row, col];
                        maxProbabilities.Add((col, row));
                    }
                }
            }
            
            //Choisir une coordonée aléatoire dans la liste des meilleures probabilitées
            Random rnd = new Random();
            var randomCase = maxProbabilities[rnd.Next(maxProbabilities.Count)];
            x = randomCase.x;
            y = randomCase.y;
        }
    }
}