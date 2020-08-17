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
    public class Gameplay : IPhase
    {
        public List<Player> PlayerList { get; private set; }

        public int ActivePlayerIndex { get; set; }

        public int TurnCount { get; private set; }

        public bool IsGameFinished { get; private set; }

        public Player WinnerPlayer { get; private set; }

        private readonly Random random = new Random();

        public Gameplay(Player firstPlayer, Player secondPlayer, int turnCount, int activePlayerIndex)
        {
            PlayerList = new List<Player>()
            {
                firstPlayer,
                secondPlayer
            };

            InitializeGameplayProperties(turnCount, activePlayerIndex);
        }

        public Gameplay(Player firstPlayer, Player secondPlayer)
        {
            PlayerList = new List<Player>()
            {
                firstPlayer,
                secondPlayer
            };

            InitializeGameplayProperties();
        }

        public Gameplay(List<Player> players)
        {
            PlayerList = players;

            InitializeGameplayProperties();
        }

        /// <summary>
        /// Initializes gameplay properties, subscribes health below zero event. If turn count is 1, every player draws their starting hand.
        /// </summary>
        /// <param name="turnCount">Current turn count</param>
        /// <param name="activePlayerIndex">Index of player that owns current turn</param>
        private void InitializeGameplayProperties(int turnCount = 1, int activePlayerIndex = -1)
        {
            TurnCount = turnCount;
            ActivePlayerIndex = activePlayerIndex != -1 ? activePlayerIndex : DetermineStartingPlayerIndex();

            foreach (var player in PlayerList)
            {
                player.HealthHandler += Player_HealthBelowZero;
                if (turnCount == 1)
                {
                    player.GetStartingHand();
                }
            }
        }

        /// <summary>
        /// Executes the game till a player's health goes down to zero.
        /// Each turn consists of 3 phases: Start, Action and End
        /// </summary>
        public void RunGame()
        {
            while (!IsGameFinished)
            {
                Player activePlayer = GetActivePlayer();
                Player defendingPlayer = GetDefendingPlayer();

                StartPhase(activePlayer, defendingPlayer);
                ActionPhase(activePlayer, defendingPlayer);
                EndPhase(activePlayer);
            }
        }
        /// <summary>
        /// Start phase of the turn. Mana capacity of active palyer is increased and mana is refilled.
        /// If this is not the first turn, player draws a card.
        /// </summary>
        /// <param name="activePlayer">Player that owns the turn</param>
        /// <param name="defendingPlayer">Player that receives the consequences of the turn</param>
        public void StartPhase(Player activePlayer, Player defendingPlayer)
        {
            Console.WriteLine($"Now it's {activePlayer.Name}'s turn");
            Console.WriteLine("START PHASE");
            Console.WriteLine();

            activePlayer.IncrementManaCapacity();
            Console.WriteLine($"{activePlayer.Name}'s mana capacity is increased to : {activePlayer.ManaCapacity}");

            activePlayer.RefillMana();
            Console.WriteLine($"{activePlayer.Name}'s mana is refilled");

            if (TurnCount != 1)
            {
                activePlayer.DrawCard();
            }

            PrintTurnBeginningStats(activePlayer, defendingPlayer);
        }

        /// <summary>
        /// Action phase of the turn. If player has any card to play in the hand, player is asked to decide either play or not.
        /// If player has no available card in the hand, turn is passed.
        /// </summary>
        /// <param name="activePlayer">Player that owns the turn</param>
        /// <param name="defendingPlayer">Player that receives the consequences of the turn</param>
        public void ActionPhase(Player activePlayer, Player defendingPlayer)
        {
            Console.WriteLine("ACTION PHASE");
            Console.WriteLine();

            PrintHand(activePlayer);

            if (!activePlayer.CanPlayAnyMove())
            {
                Console.WriteLine($"No available card to play in {activePlayer.Name}'s hand.");
                Console.WriteLine();
            }

            bool shouldEndPhase = false;

            while (activePlayer.CanPlayAnyMove() && !shouldEndPhase && !IsGameFinished)
            {
                // take input from console
                var (decidedCard, isPassed) = activePlayer.DecideOnCard();
                if (decidedCard != null)
                {
                    activePlayer.PlayCard(decidedCard, defendingPlayer);
                }

                if (isPassed)
                {
                    shouldEndPhase = true;
                }
            }
        }

        /// <summary>
        /// End phase of the turn. Active player is switched inside
        /// </summary>
        /// <param name="activePlayer"></param>
        public void EndPhase(Player activePlayer)
        {
            Console.WriteLine("END PHASE");
            Console.WriteLine();
            SwitchActivePlayer();
            TurnCount++;
        }

        /// <summary>
        /// Method for using card by attackingPlayer towards to defendingPlayer
        /// </summary>
        /// <param name="card">Card is wanted to use</param>
        /// <param name="attackingPlayer">Attacking player</param>
        /// <param name="defendingPlayer">Defending player</param>
        public void UseCard(Card card, Player attackingPlayer, Player defendingPlayer)
        {
            attackingPlayer.PlayCard(card, defendingPlayer);
        }

        /// <summary>
        /// Switching active player, since, for now, there are 2 players in the game.
        /// </summary>
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

        /// <summary>
        /// Determining which player starts first. Each player has equal chance to start
        /// </summary>
        /// <returns>Active player index</returns>
        private int DetermineStartingPlayerIndex()
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
            Console.WriteLine($"Health: {defendingPlayer.Health} Mana: {defendingPlayer.Mana}");
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

            Console.WriteLine();
            Console.WriteLine($"Game is over! Winner is {WinnerPlayer.Name} and {loserPlayer.Name} has lost the game.");
            Console.WriteLine($"Game lasted for {TurnCount} turns");
        }
    }
}
