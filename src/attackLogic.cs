using WMPLib;

namespace Battleship.src
{
    internal class attackLogic
    {
        private static readonly Random rnd = new Random();

        public static void Attack(int x, int y, bool playerTurn)
        {
            WindowsMediaPlayer player = new WindowsMediaPlayer();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string audioPath = Path.Combine(basePath, "..", "..", "..", "src", "audio", "miss.mp3");
            var board = playerTurn ? Game.BotBoard : Game.PlayerBoard;

            if (board[y, x] == 'S')
            {
                audioPath = Path.Combine(basePath, "..", "..", "..", "src", "audio", "hit.mp3");
                Boat boat = HitBoat(x, y, playerTurn);

                if(playerTurn)
                {
                    Game.HiddenBoard[y, x] = 'x';
                }
                else
                {
                    if(Game.LastBotHitX == -1 && Game.LastBotHitY == -1)
                    {
                        Game.FirstBotHitX = x;
                        Game.FirstBotHitY = y;
                    }
                    else
                    {
                        if (x != Game.LastBotHitX) Game.BoatDirection = 1;
                        else if (y != Game.LastBotHitY) Game.BoatDirection = 2;
                    }
                    Game.LastBotHitX = x;
                    Game.LastBotHitY = y;
                }
                board[y, x] = 'x';

                Game.ShowGame();

                if (boat.Hp <= 0)
                {
                    Console.WriteLine($"The {boat.Name} is sunk !");
                    
                    if (!playerTurn)
                    {
                        Game.BoatDirection = -1;
                        Game.FirstBotHitX = -1;
                        Game.FirstBotHitY = -1;
                        Game.LastBotHitX = -1;
                        Game.LastBotHitY = -1;
                    }
                }
                else
                {
                    Console.WriteLine("Hit !");
                }
            }
            else
            {
                if (playerTurn)
                {
                    Game.HiddenBoard[y, x] = 'X';
                }
                board[y, x] = 'X';

                Game.ShowGame();
                Console.WriteLine("Missed !");
            }
            player.URL = Path.GetFullPath(audioPath);
            player.controls.play();
        }

        public static Boat HitBoat(int x, int y, bool playerTurn)
        {
            var boats = playerTurn ? Game.BotBoats : Game.PlayerBoats;
            for (int i = 0; i < boats.Length; i++)
            {
                if (x >= boats[i].FirstX && x <= boats[i].LastX && y >= boats[i].FirstY && y <= boats[i].LastY)
                {
                    boats[i].Hp--;
                    return boats[i];
                }
            }

            throw new InvalidOperationException("No boat found. This should never happen!");
        }
        public static void BotAttack()
        {
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

        public static void EasyAttack()
        {
            int x = -1;
            int y = -1;
            RandomCoords(out x, out y);
            Attack(x, y, false);
        }

        public static void MediumAttack()
        {

            int x = -1;
            int y = -1;
            if (Game.LastBotHitX == -1 && Game.LastBotHitY == -1)
            {
                RandomCoords(out x, out y);
            }
            else
            {
                TargetAttack(out x, out y);
            }

            Attack(x, y, false);
        }

        public static void HardAttack()
        {
            int x = -1;
            int y = -1;
            if (Game.LastBotHitX == -1 && Game.LastBotHitY == -1)
            {
                MostProbableCoord(out x, out y);
            }
            else
            {
                TargetAttack(out x, out y);
            }
            Attack(x, y, false);
        }

        public static void TargetAttack(out int x, out int y)
        {
            if (Game.BoatDirection == -1)
            {
                Side4Coords(out x, out y);
            }
            else
            {
                DirectionCoord(out x, out y);
                if (x == -1 && y == -1)
                {
                    RandomCoords(out x, out y);
                }
            }
        }

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

        public static void Side4Coords(out int x, out int y)
        {
            x = -1;
            y = -1;

            for (int i = 0; i < 4; i++)
            {
                x = Game.LastBotHitX + (i == 0 ? 1 : (i == 1 ? -1 : 0));
                y = Game.LastBotHitY + (i == 2 ? 1 : (i == 3 ? -1 : 0));

                if (y <= 9 && x <= 9 && y >= 0 && x >= 0 && !Verification.AlreadyHit(x, y, false))
                {
                    return;
                }
            }
        }


        public static void DirectionCoord(out int x, out int y)
        {
            x = -1;
            y = -1;

            int nextX = Game.LastBotHitX;
            int nextY = Game.LastBotHitY;

            if (Game.BoatDirection == 1) // Horizontal
            {
                if(Game.FirstBotHitX - Game.LastBotHitX < 0) //if direction == right
                {
                    if (Game.LastBotHitX + 1 <= 9 && !Verification.AlreadyHit(Game.LastBotHitX + 1, Game.LastBotHitY, false))
                    {
                        nextX = Game.LastBotHitX + 1;
                    }
                    else
                    {
                        if(Game.FirstBotHitX - 1 >= 0 && !Verification.AlreadyHit(Game.FirstBotHitX - 1, Game.FirstBotHitY, false))
                        {
                            nextX = Game.FirstBotHitX - 1;

                        }
                    }
                }
                else //if direction == left
                {
                    if (Game.LastBotHitX - 1 >= 0 && !Verification.AlreadyHit(Game.LastBotHitX - 1, Game.LastBotHitY, false))
                    {
                        nextX = Game.LastBotHitX - 1;
                    }
                    else
                    {
                        if (Game.FirstBotHitX + 1 <= 9 && !Verification.AlreadyHit(Game.FirstBotHitX + 1, Game.FirstBotHitY, false))
                        {
                            nextX = Game.FirstBotHitX + 1;

                        }
                    }
                }

                if(nextX == Game.LastBotHitX)
                {
                    Game.BoatDirection = -1;
                    Side4Coords(out x, out y);
                }
            }
            else if (Game.BoatDirection == 2) // Vertical
            {
                if (Game.FirstBotHitY - Game.LastBotHitY < 0) //if direction == down
                {
                    if (Game.LastBotHitY + 1 <= 9 && !Verification.AlreadyHit(Game.LastBotHitX, Game.LastBotHitY + 1, false))
                    {
                        nextY = Game.LastBotHitY + 1;
                    }
                    else
                    {
                        if (Game.FirstBotHitX - 1 >= 0 && !Verification.AlreadyHit(Game.FirstBotHitX, Game.FirstBotHitY - 1, false))
                        {
                            nextY = Game.FirstBotHitY - 1;

                        }
                    }
                }
                else //if direction == up
                {
                    if (Game.LastBotHitX - 1 >= 0 && !Verification.AlreadyHit(Game.LastBotHitX, Game.LastBotHitY - 1, false))
                    {
                        nextY = Game.LastBotHitY - 1;
                    }
                    else
                    {
                        if (Game.FirstBotHitX + 1 <= 9 && !Verification.AlreadyHit(Game.FirstBotHitX, Game.FirstBotHitY + 1, false))
                        {
                            nextY = Game.FirstBotHitY + 1;

                        }
                    }
                }

                if (nextY == Game.LastBotHitY)
                {
                    Game.BoatDirection = -1;
                    Side4Coords(out x, out y);
                }
            }

            x = nextX;
            y = nextY;
        }

        public static void MostProbableCoord(out int x, out int y)
        {
            int[,] ProbaBoard = new int[10, 10];

            for (int orientation = 0; orientation < 2; orientation++)
            {
                foreach (var boat in Game.PlayerBoats)
                {
                    if (boat.Hp > 0)
                    {
                        for (int row = 0; row < ProbaBoard.GetLength(0); row++)
                        {
                            for (int col = 0; col < ProbaBoard.GetLength(1); col++)
                            {
                                bool canPlace = true;

                                for (int offset = 0; offset < boat.Length; offset++)
                                {
                                    if ((orientation == 0 && col + offset >= 10) || (orientation == 1 && row + offset >= 10) || Game.PlayerBoard[row + (orientation == 1 ? offset : 0), col + (orientation == 0 ? offset : 0)].ToString().ToUpper() == "X")
                                    {
                                        canPlace = false;
                                        break;
                                    }
                                }

                                if (canPlace)
                                {
                                    for (int offset = 0; offset < boat.Length; offset++)
                                    {
                                        int markRow = orientation == 1 ? row + offset : row;
                                        int markCol = orientation == 0 ? col + offset : col;
                                        ProbaBoard[markRow, markCol]++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            int maxProbability = 0;
            x = 0;
            y = 0;

            for (int row = 0; row < ProbaBoard.GetLength(0); row++)
            {
                for (int col = 0; col < ProbaBoard.GetLength(1); col++)
                {
                    Console.Write($"{ProbaBoard[row, col],3}");
                    if (ProbaBoard[row, col] > maxProbability && !Verification.AlreadyHit(col, row, false))
                    {
                        maxProbability = ProbaBoard[row, col];
                        x = col;
                        y = row;
                    }
                }
                Console.WriteLine();
            }
        }
    }
}