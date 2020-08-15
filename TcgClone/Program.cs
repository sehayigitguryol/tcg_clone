using System;
using TcgClone.Entities;

namespace TcgClone
{
    class Program
    {
        static void Main(string[] args)
        {
            Player player1 = new Player("selam");
            Player player2 = new Player("naber");

            Gameplay game = new Gameplay(player1, player2);
            game.RunGame();
        }
    }
}
