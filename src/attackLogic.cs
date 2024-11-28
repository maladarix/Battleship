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
            int x = -1;
            int y = -1;
            if(Game.LastBotHitX == -1 && Game.LastBotHitY == -1)
            {
                RandomCoords(out x, out y);
            }
            else
            {
                if(Game.BoatDirection == -1)
                {
                    Side4Coords(out x, out y);
                }
                else
                {
                    DirectionCoord(out x, out y);
                }
            }

            Attack(x, y, false);
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

            int nextX = Game.LastBotHitX, nextY = Game.LastBotHitY;

            if (Game.BoatDirection == 1) // Horizontal
            {
                nextX = (Game.LastBotHitX + 1 <= 9 && !Verification.AlreadyHit(Game.LastBotHitX + 1, Game.LastBotHitY, false))
                    ? Game.LastBotHitX + 1
                    : Game.LastBotHitX - 1;
            }
            else if (Game.BoatDirection == 2) // Vertical
            {
                nextY = (Game.LastBotHitY + 1 <= 9 && !Verification.AlreadyHit(Game.LastBotHitX, Game.LastBotHitY + 1, false))
                    ? Game.LastBotHitY + 1
                    : Game.LastBotHitY - 1;
            }

            if (nextX >= 0 && nextX <= 9 && !Verification.AlreadyHit(nextX, nextY, false))
            {
                x = nextX;
                y = nextY;
            }
        }
    }
}
