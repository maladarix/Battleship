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
                    for (int i = 0; i < boat.Length; i++)
                    {
                        if(boat.FirstX - boat.LastX == 0) //vertical
                        {
                            if(playerTurn)
                            {
                                Game.HiddenBoard[boat.FirstY + i, x] = 'd';
                            }
                            else
                            {
                                board[boat.FirstY + i, x] = 'd';
                            }
                        }
                        else //horizontal
                        {
                            if(playerTurn)
                            {
                                Game.HiddenBoard[y, boat.FirstX + i] = 'd';
                            }
                            else
                            {
                                board[y, boat.FirstX + i] = 'd';
                            }
                        }
                    }

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
            MostProbableCoord(out x, out y);
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

            if (Game.BoatDirection == 1) // Horizontal
            {
                if(Game.FirstBotHitX - Game.LastBotHitX < 0) //if direction == right
                {
                    if (Game.LastBotHitX + 1 <= 9 && !Verification.AlreadyHit(Game.LastBotHitX + 1, Game.LastBotHitY, false))
                    {
                        x = Game.LastBotHitX + 1;
                        y = Game.LastBotHitY;
                    }
                    else
                    {
                        if(Game.FirstBotHitX - 1 >= 0 && !Verification.AlreadyHit(Game.FirstBotHitX - 1, Game.FirstBotHitY, false))
                        {
                            x = Game.FirstBotHitX - 1;
                            y = Game.LastBotHitY;


                        }
                    }
                }
                else //if direction == left
                {
                    if (Game.LastBotHitX - 1 >= 0 && !Verification.AlreadyHit(Game.LastBotHitX - 1, Game.LastBotHitY, false))
                    {
                        x = Game.LastBotHitX - 1;
                        y = Game.LastBotHitY;

                    }
                    else
                    {
                        if (Game.FirstBotHitX + 1 <= 9 && !Verification.AlreadyHit(Game.FirstBotHitX + 1, Game.FirstBotHitY, false))
                        {
                            x = Game.FirstBotHitX + 1;
                            y = Game.LastBotHitY;


                        }
                    }
                }

                if(x == Game.LastBotHitX)
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
                        x = Game.LastBotHitX;
                        y = Game.LastBotHitY + 1;
                    }
                    else
                    {
                        if (Game.FirstBotHitY - 1 >= 0 && !Verification.AlreadyHit(Game.FirstBotHitX, Game.FirstBotHitY - 1, false))
                        {
                            x = Game.LastBotHitX;
                            y = Game.FirstBotHitY - 1;

                        }
                    }
                }
                else //if direction == up
                {
                    if (Game.LastBotHitY - 1 >= 0 && !Verification.AlreadyHit(Game.LastBotHitX, Game.LastBotHitY - 1, false))
                    {
                        x = Game.LastBotHitX;
                        y = Game.LastBotHitY - 1;
                    }
                    else
                    {
                        if (Game.FirstBotHitY + 1 <= 9 && !Verification.AlreadyHit(Game.FirstBotHitX, Game.FirstBotHitY + 1, false))
                        {
                            x = Game.LastBotHitX;
                            y = Game.FirstBotHitY + 1;

                        }
                    }
                }

                if (y == Game.LastBotHitY)
                {
                    Game.BoatDirection = -1;
                    Side4Coords(out x, out y);
                }
            }
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
                                bool canPlace = false;
                                bool boosted = false;
                                for (int offset = 0; offset < boat.Length; offset++)
                                {
                                    int targetRow = orientation == 0 ? row : row + offset;
                                    int targetCol = orientation == 0 ? col + offset : col;

                                    if (targetRow >= 10 || targetCol >= 10 ||
                                        Game.PlayerBoard[targetRow, targetCol].ToString() == "X" ||
                                        Game.PlayerBoard[targetRow, targetCol].ToString().ToUpper() == "D")
                                    {
                                        canPlace = false;
                                        break;
                                    }

                                    if (Game.PlayerBoard[targetRow, targetCol].ToString() == "x")
                                    {
                                        boosted = true;
                                    }

                                    if (offset == boat.Length - 1)
                                    {
                                        canPlace = true;
                                        break;
                                    }
                                }

                                if (canPlace)
                                {
                                    for (int offset = 0; offset < boat.Length; offset++)
                                    {
                                        int markRow = orientation == 0 ? row : row + offset;
                                        int markCol = orientation == 0 ? col + offset : col;

                                        if (Game.PlayerBoard[markRow, markCol].ToString().ToLower() != "x")
                                        {
                                            ProbaBoard[markRow, markCol] += boosted ? 10 : 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            List<(int x, int y)> maxProbabilities = new List<(int x, int y)>();
            int maxProbability = 0;
            x = 0;
            y = 0;

            for (int row = 0; row < ProbaBoard.GetLength(0); row++)
            {
                for (int col = 0; col < ProbaBoard.GetLength(1); col++)
                {
                    if (ProbaBoard[row, col] >= maxProbability)
                    {
                        if(ProbaBoard[row, col] > maxProbability)
                        {
                            maxProbabilities.Clear();
                        }

                        maxProbability = ProbaBoard[row, col];
                        maxProbabilities.Add((col, row));
                    }
                }
            }

            Random rnd = new Random();
            var randomCase = maxProbabilities[rnd.Next(maxProbabilities.Count)];
            x = randomCase.x;
            y = randomCase.y;
        }
    }
}