using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcgClone.Entities;
using TcgClone.Events;

namespace TcgClone
{
    /// <summary>
    /// Main gameplay logic of the game
    /// </summary>
    public class Gameplay
    {
        public List<Player> PlayerList { get; private set; }

        public int ActivePlayerIndex { get; set; }

        public int TurnCount { get; private set; }

        public bool IsGameFinished { get; private set; }

        public Player WinnerPlayer { get; private set; }

        private readonly Random random = new Random();

        public Gameplay(Player firstPlayer, Player secondPlayer, int turnCount, int activePlayerIndex)
        {
            firstPlayer.HealthHandler += Player_HealthBelowZero;
            secondPlayer.HealthHandler += Player_HealthBelowZero;

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
            firstPlayer.HealthHandler += Player_HealthBelowZero;
            secondPlayer.HealthHandler += Player_HealthBelowZero;

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
            while (!IsGameFinished)
            {
                Player activePlayer = GetActivePlayer();
                Player defendingPlayer = GetDefendingPlayer();

                StartTurn(activePlayer, defendingPlayer);
                GetPlayerAction(activePlayer, defendingPlayer);
                EndTurn();
            }
        }

        public void StartTurn(Player activePlayer, Player defendingPlayer)
        {
            Console.WriteLine($"Now it's {activePlayer.Name}'s turn");
            activePlayer.IncrementManaCapacity();
            activePlayer.RefillMana();
            if (TurnCount != 1)
            {
                activePlayer.DrawCard();
            }

            PrintTurnBeginningStats(activePlayer, defendingPlayer);
        }

        public void GetPlayerAction(Player activePlayer, Player defendingPlayer)
        {
            PrintHand(activePlayer);
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
            TurnCount++;
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

        private void PrintTurnBeginningStats(Player attackingPlayer, Player defendingPlayer)
        {
            Console.WriteLine();
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%");
            Console.WriteLine($"Attacking Player Name : {attackingPlayer.Name}");
            Console.WriteLine($"Health: {attackingPlayer.Health} Mana: {attackingPlayer.Mana}");
            Console.WriteLine($"Defending Player Name : {defendingPlayer.Name}");
            Console.WriteLine($"Health: {attackingPlayer.Health} Mana: {attackingPlayer.Mana}");
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%");
            Console.WriteLine();
        }

        private void PrintHand(Player attackingPlayer)
        {
            Console.Write("Cards in hand: ");
            for (int i = 0; i < attackingPlayer.Hand.Count; i++)
            {
                if (i != 0)
                {
                    Console.Write(" - ");
                }

                Card card = attackingPlayer.Hand[i];
                Console.Write($"{card.Point}");
            }

            Console.WriteLine();
        }

        private void Player_HealthBelowZero(object sender, PlayerHealthBelowZeroEventArgs e)
        {
            IsGameFinished = true;

            var loserPlayer = e.LoserPlayer;
            WinnerPlayer = PlayerList.Where((x) => x.Id != loserPlayer.Id).First();

            Console.WriteLine($"Game is over! Winner is {WinnerPlayer.Name} and {loserPlayer.Name} has lost the game.");
            Console.WriteLine($"Game lasted for {TurnCount} turns");
        }
    }
}
