using System;
using System.Collections.Generic;
using TcgClone.Entities;

namespace TcgClone
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Player> players = GetPlayers();
            Gameplay game = new Gameplay(players);
            game.RunGame();
        }

        private static List<Player> GetPlayers()
        {
            int playerCountForGame = 2;
            List<Player> playerList = new List<Player>();

            for (int i = 0; i < playerCountForGame; i++)
            {
                Console.WriteLine($"Please enter Player {i + 1} name");
                var name = Console.ReadLine();

                Player player = PlayerFactory.CreatePlayer(name);
                playerList.Add(player);
            }

            return playerList;
        }
    }
}
