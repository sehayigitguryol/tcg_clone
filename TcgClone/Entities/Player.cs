using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcgClone.Events;
using TcgClone.Interfaces;

namespace TcgClone.Entities
{
    public class Player : IPlayerActions
    {
        private const int INITIAL_HAND_SIZE = 3;

        private const int MAX_HAND_SIZE = 5;

        private const int MAX_MANA_CAPACITY = 10;

        private const int MAX_HEALTH = 30;

        private readonly List<int> INITIAL_CARD_COSTS = new List<int>() { 0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7, 8 };

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

        private void InitializeDefaultPlayerProperties()
        {
            Health = MAX_HEALTH;
            Mana = 0;
            ManaCapacity = 0;
            Deck = new List<Card>();
            foreach (var item in INITIAL_CARD_COSTS)
            {
                Deck.Add(new Card(item));
            }

            Hand = new List<Card>();
        }

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
                if (Hand.Count < MAX_HAND_SIZE)
                {
                    Hand.Add(drawedCard);
                }
                else
                {
                    Console.WriteLine($"{Name} reached the hand limit.");
                }

            }
        }

        public void GetStartingHand()
        {
            for (int i = 0; i < INITIAL_HAND_SIZE; i++)
            {
                DrawCard();
            }
        }

        public void IncrementManaCapacity()
        {
            ManaCapacity = ManaCapacity < MAX_MANA_CAPACITY ? ManaCapacity + 1 : MAX_MANA_CAPACITY;
        }

        public void RefillMana()
        {
            Mana = ManaCapacity;
        }

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

        public void DealDamage(int damage, Player opponent)
        {
            opponent.InflictDamage(damage);
        }

        public Card DecideOnCard()
        {
            Card selectedCard = null;
            while (selectedCard == null)
            {
                Console.WriteLine("Please give an input");
                var input = GetPlayerInput();
                bool isValidInteger = Int32.TryParse(input, out int cardValue);
                if (!isValidInteger)
                {
                    Console.WriteLine($"{input} is invalid");
                }
                else
                {
                    Card retrievedCard = GetCardFromHand(cardValue);
                    if (retrievedCard != null)
                    {
                        if (cardValue <= Mana)
                        {
                            selectedCard = retrievedCard;
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

            return selectedCard;
        }

        private Card GetCardFromHand(int value)
        {
            return Hand.FirstOrDefault((x) => x.Point == value);
        }

        public void PlayCard(Card card, Player opponent)
        {
            if (Mana < card.Point)
            {
                Console.WriteLine($"{Name} doesn't have sufficient amount of mana to play card {card.Point}");
            }
            else
            {
                Hand.Remove(card);
                DealDamage(card.Point, opponent);
                Mana -= card.Point;
            }
        }

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

        public virtual string GetPlayerInput()
        {
            string input = Console.ReadLine();
            return input;
        }

        private void OnPlayerHealthBelowZero(PlayerHealthBelowZeroEventArgs e)
        {
            HealthHandler?.Invoke(this, e);
        }

        public event PlayerHealthBelowZeroEventHandler HealthHandler;
    }
}
