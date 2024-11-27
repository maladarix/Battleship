using WMPLib;

namespace Battleship.src
{
    internal class attackLogic
    {
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
                board[y, x] = 'x';

                Game.ShowGame();

                if (boat.Hp <= 0)
                {
                    Console.WriteLine($"The {boat.Name} is sunk !");
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

        public static void AttackRandom()
        {
            Random random = new Random();
            int x = 0;
            int y = 0;
            do
            {
                x = random.Next(10);
                y = random.Next(10);
            }
            while (Game.PlayerBoard[x, y] == 'S' || Game.PlayerBoard[x, y] == 'X');
            Attack(x, y, false);
        }
    }
}
