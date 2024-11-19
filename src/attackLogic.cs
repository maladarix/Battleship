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

            if (board[x, y] == 'S')
            {
                Boat boat = GetBoat(x, y, playerTurn);
                boat.Hp--;

                if (boat.Hp <= 0)
                {
                    Console.WriteLine($"The {boat.Name} is sunk !");
                }
                else
                {
                    audioPath = Path.Combine(basePath, "..", "..", "..", "src", "audio", "hit.mp3");
                    Console.WriteLine("Hit !");
                }
            }
            else
            {
                Console.WriteLine("Missed !");
            }
            player.URL = audioPath;
            player.controls.play();
        }

        public static Boat GetBoat(int x, int y, bool playerTurn)
        {
            var boats = playerTurn ? Game.BotBoats : Game.PlayerBoats;
            foreach (Boat boat in boats)
            {
                if (x >= boat.FirstX && x <= boat.LastX && y >= boat.FirstY && y <= boat.LastY) {
                    return boat;
                }
            }
            throw new InvalidOperationException("No boat found. This should never happen!");
        }
    }
}
