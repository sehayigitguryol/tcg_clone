using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone
{
    public class Gameplay
    {
        public List<Player> PlayerList { get; private set; }

        public int ActivePlayerIndex { get; set; }

        public int TurnCount { get; private set; }

        private readonly Random random = new Random();

        public Gameplay(Player firstPlayer, Player secondPlayer, int turnCount, int activePlayerIndex)
        {
            PlayerList = new List<Player>()
            {
                firstPlayer,
                secondPlayer
            };

            TurnCount = turnCount;
            ActivePlayerIndex = activePlayerIndex;
            if (TurnCount == 1)
            {
                foreach (var player in PlayerList)
                {
                    player.GetStartingHand();
                }
            }
        }

        public Gameplay(Player firstPlayer, Player secondPlayer)
        {
            PlayerList = new List<Player>()
            {
                firstPlayer,
                secondPlayer
            };

            TurnCount = 1;
            ActivePlayerIndex = DecideActivePlayer();

            foreach (var player in PlayerList)
            {
                player.GetStartingHand();
            }
        }

        public void RunGame()
        {
            while (true)
            {
                StartTurn();
                GetPlayerAction();
                EndTurn();
            }
        }

        public void StartTurn()
        {
            Player activePlayer = GetActivePlayer();
            activePlayer.IncrementManaCapacity();
            activePlayer.RefillMana();
            if (TurnCount != 1)
            {
                activePlayer.DrawCard();
            }
        }

        public void GetPlayerAction()
        {
            Player activePlayer = GetActivePlayer();
            Player defendingPlayer = GetDefendingPlayer();
            while (activePlayer.CanPlayAnyMove())
            {
                // take input from console
                Card decidedCard = activePlayer.DecideOnCard();
                activePlayer.PlayCard(decidedCard, defendingPlayer);
            }
        }

        public void UseCard(Card card)
        {
            Player activePlayer = GetActivePlayer();
            Player defendingPlayer = GetDefendingPlayer();

            activePlayer.PlayCard(card, defendingPlayer);
        }

        public void EndTurn()
        {
            SwitchActivePlayer();
        }

        private void SwitchActivePlayer()
        {
            if (ActivePlayerIndex == 0)
            {
                ActivePlayerIndex = 1;
            }
            else if (ActivePlayerIndex == 1)
            {
                ActivePlayerIndex = 0;
            }
        }

        public Player GetActivePlayer()
        {
            return PlayerList[ActivePlayerIndex];
        }

        public Player GetDefendingPlayer()
        {
            var defendingIndex = (ActivePlayerIndex + 1) % 2;
            return PlayerList[defendingIndex];
        }

        private int DecideActivePlayer()
        {
            return random.Next(PlayerList.Count);
        }

    }
}
