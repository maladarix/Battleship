using Battleship.src;
using System;

class Program
{
    static void Main()
    {
        bool endGame = false;
        do
        {
            ConsoleInteractions.AskPlayOrQuit(ref endGame);
        }
        while (endGame == false);
    }
}
