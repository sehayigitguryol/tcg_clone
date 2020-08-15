using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone
{
    public class Gameplay
    {
        public Player ActivePlayer { get; private set; }

        public Player OpponentPlayer { get; private set; }

        public int TurnCount { get; private set; }

        private readonly Random random = new Random();

        public Gameplay(Player player1, Player player2)
        {
            TurnCount = 0;
            DecideActivePlayer(player1, player2);
            ActivePlayer.GetStartingHand();
            OpponentPlayer.GetStartingHand();
        }

        private void DecideActivePlayer(Player player1, Player player2)
        {
            int decider = random.Next(2);

            ActivePlayer = decider == 0 ? player1 : player2;
            OpponentPlayer = decider == 0 ? player2 : player1;
        }

    }
}
