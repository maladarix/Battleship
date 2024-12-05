using Battleship.src;
using System;
using WMPLib;

class Program
{
    static void Main()
    {
        //Créé la variable pour sortir de la loop (sortie du programme)
        bool endGame = false;

        //Détermine le chemin d'accès du son
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string audioPath = Path.Combine(basePath, "..", "..", "..", "src", "audio", "background-music.mp3");
        player.URL = Path.GetFullPath(audioPath);

        player.settings.setMode("loop", true);
        player.settings.volume = 25;
        do
        {
            //Active le son en background
            player.controls.play();

            //Demander au joueur si il veut jouer ou quitter le programme
            ConsoleInteractions.AskPlayOrQuit(ref endGame);
        }
        while (endGame == false);
        player.controls.stop();
    }
}
