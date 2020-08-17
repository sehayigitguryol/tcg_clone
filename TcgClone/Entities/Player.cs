using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcgClone.Events;
using TcgClone.Interfaces;

namespace TcgClone.Entities
{
    /// <summary>
    /// Player class which hold player props and player capabilities
    /// </summary>
    public class Player : IPlayerActions
    {
        private readonly Random random = new Random();

        public int Id { get; set; }

        public string Name { get; private set; }

        public int Health { get; set; }

        public int Mana { get; set; }

        public int ManaCapacity { get; set; }

        public List<Card> Deck { get; set; }

        public List<Card> Hand { get; set; }

        public Player(int id, string name)
        {
            Id = id;
            Name = name;
            InitializeDefaultPlayerProperties();
        }

        public Player(int id, string name, int health, int mana, int manaSlots, List<Card> deck, List<Card> hand)
        {
            Id = id;
            Name = name;
            Health = health;
            Mana = mana;
            ManaCapacity = manaSlots;
            Deck = deck;
            Hand = hand;
        }

        /// <summary>
        /// Initializing default properties of players.
        /// Like creating hand & deck and setting default params to player properties
        /// </summary>
        private void InitializeDefaultPlayerProperties()
        {
            Health = PlayerConstants.MAX_HEALTH;
            Mana = 0;
            ManaCapacity = 0;
            Deck = new List<Card>();
            foreach (var item in PlayerConstants.INITIAL_CARD_COSTS)
            {
                Deck.Add(new Card(item));
            }

            Hand = new List<Card>();
        }

        /// <summary>
        /// Player draws card from the deck.
        /// If player doesn't have any card in the deck, player Bleeds Out.
        /// And if player's health reaches at zero, PlayerHealthBelowZero event is triggered
        /// 
        /// If player has any card in the deck, a random card is retrieved from the deck.
        /// If player has maximum amount of card in the hand, that drawn card would discarded immediately.
        /// Otherwise, player adds drawn card to hand.
        /// </summary>
        public void DrawCard()
        {
            if (Deck.Count == 0)
            {
                Console.WriteLine($"{Name} doesn't have any card in deck. {Name} is bleeding out.");
                Health -= 1;

                if (Health <= 0)
                {
                    PlayerHealthBelowZeroEventArgs args = new PlayerHealthBelowZeroEventArgs()
                    {
                        LoserPlayer = this,
                    };

                    OnPlayerHealthBelowZero(args);
                }
            }
            else
            {
                int nextCardIndex = random.Next(Deck.Count - 1);
                Card drawedCard = Deck[nextCardIndex];

                Deck.RemoveAt(nextCardIndex);

                Console.WriteLine($"{Name} draws a card");
                if (Hand.Count < PlayerConstants.MAX_HAND_SIZE)
                {
                    Hand.Add(drawedCard);
                }
                else
                {
                    Console.WriteLine($"{Name} reached the hand limit. {drawedCard.Point} is discarded.");
                }

            }
        }

        /// <summary>
        /// Player draws card till the hand size reaches to initial hand size
        /// </summary>
        public void GetStartingHand()
        {
            for (int i = 0; i < PlayerConstants.INITIAL_HAND_SIZE; i++)
            {
                DrawCard();
            }
        }

        /// <summary>
        /// Incrementing player's mana capacity till the maximum mana capacity level.
        /// </summary>
        public void IncrementManaCapacity()
        {
            ManaCapacity = ManaCapacity < PlayerConstants.MAX_MANA_CAPACITY ? ManaCapacity + 1 : PlayerConstants.MAX_MANA_CAPACITY;
        }

        /// <summary>
        /// Refilling mana to mana capacity.
        /// </summary>
        public void RefillMana()
        {
            Mana = ManaCapacity;
        }

        /// <summary>
        /// Player's health drops by damage points.
        /// If player's health goes below zero, PlayerHealthBelowZeroEvent is triggered
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        public void InflictDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                PlayerHealthBelowZeroEventArgs args = new PlayerHealthBelowZeroEventArgs()
                {
                    LoserPlayer = this,
                };

                OnPlayerHealthBelowZero(args);
            }
        }

        /// <summary>
        /// Using player deals amount of damage to opponent player
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        /// <param name="opponent">Defending player that receives damage</param>
        public void DealDamage(int damage, Player opponent)
        {
            opponent.InflictDamage(damage);
        }

        /// <summary>
        /// Decides which card that player wants to play by taking console input.
        /// If input equals to "P", player passes the turn.
        /// If input is not an integer, an error message pops up. Console requires player to try again.
        /// If input is an integer, but player's mana is insufficent to play that card, an error message pops up. Console requires player to try again.
        /// If input is an integer and player's mana is sufficent to play that card, that card is returned.
        /// </summary>
        /// <returns>Card that decided and true if turn is passed</returns>
        public (Card, bool) DecideOnCard()
        {
            Card selectedCard = null;
            bool shouldStopTry = false;
            bool isPassed = false;

            while (!shouldStopTry)
            {
                Console.WriteLine("Please enter cost of the card that you want to play. Enter P to pass your turn");
                var input = GetPlayerInput();

                if (string.Compare(input, "p", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Console.WriteLine($"{Name} passed turn");
                    shouldStopTry = true;
                    isPassed = true;
                    break;
                }

                bool isValidInteger = Int32.TryParse(input, out int cardValue);
                if (!isValidInteger)
                {
                    Console.WriteLine($"{input} is invalid.");
                }
                else
                {
                    Card retrievedCard = GetCardFromHand(cardValue);
                    if (retrievedCard != null)
                    {
                        if (cardValue <= Mana)
                        {
                            selectedCard = retrievedCard;
                            shouldStopTry = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"{Name} doesn't have sufficient amount of mana to play card {cardValue}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{cardValue} is not on your hand");
                    }
                }
            }

            return (selectedCard, isPassed);
        }

        /// <summary>
        /// Gets first card that has same value in params. 
        /// </summary>
        /// <param name="value">Value of card</param>
        /// <returns>null if value doesn't in the hand, Card if value is in the hand</returns>
        private Card GetCardFromHand(int value)
        {
            return Hand.FirstOrDefault((x) => x.Point == value);
        }

        /// <summary>
        /// Player uses the card towards to opponent.
        /// If player's mana is insufficent to play that card, an error message pops up.
        /// If player's mana is enough, the card is removed from the hand, inflict damage to opponent and reduce mana by card value
        /// </summary>
        /// <param name="card">Card is wanted to use</param>
        /// <param name="opponent">Opponent player would receive damage</param>
        public void PlayCard(Card card, Player opponent)
        {
            Console.WriteLine($"{Name} is playing card {card.Point}");
            if (Mana < card.Point)
            {
                Console.WriteLine($"{Name} doesn't have sufficient amount of mana to play card {card.Point}");
            }
            else
            {
                Hand.Remove(card);
                Console.WriteLine($"{card.Point} damage has given to {opponent.Name}");

                DealDamage(card.Point, opponent);
                Mana -= card.Point;

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Decides if player can do any move by the hand
        /// </summary>
        /// <returns>True if canplay, false if can not play</returns>
        public bool CanPlayAnyMove()
        {
            if (Hand.Count == 0)
            {
                return false;
            }

            var availableCardsByMana = Hand.Where(x => x.Point <= Mana).ToList();

            if (availableCardsByMana.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Taking player input from console
        /// </summary>
        /// <returns> input string</returns>
        public virtual string GetPlayerInput()
        {
            string input = Console.ReadLine();
            return input;
        }

        /// <summary>
        /// When player's health goes below zero, HealthHandler event is triggered
        /// </summary>
        /// <param name="e">Event arguments</param>
        private void OnPlayerHealthBelowZero(PlayerHealthBelowZeroEventArgs e)
        {
            HealthHandler?.Invoke(this, e);
        }

        public event PlayerHealthBelowZeroEventHandler HealthHandler;
    }
}
