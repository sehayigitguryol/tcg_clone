using System;
using System.Collections.Generic;
using System.Text;

namespace TcgClone.Entities
{
    public class Player
    {
        private const int INITIAL_HAND_SIZE = 3;

        private const int MAX_HAND_SIZE = 5;

        private const int MAX_MANA_SLOTS = 10;

        private const int MAX_HEALTH = 30;

        private readonly List<int> INITIAL_CARD_COSTS = new List<int>() { 0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7, 8 };

        public string Name { get; private set; }

        public int Health { get; set; }

        public int Mana { get; set; }

        public int ManaSlots { get; set; }

        public List<Card> Deck { get; set; }

        public List<Card> Hand { get; set; }

        public Player(string name)
        {
            Name = name;
            InitializePlayerProperties();
        }

        public Player(string name, int health, int mana, int manaSlots, List<Card> deck, List<Card> hand)
        {
            Health = health;
            Mana = mana;
            ManaSlots = manaSlots;
            Deck = deck;
            Hand = hand;
        }

        private void InitializePlayerProperties()
        {
            Health = MAX_HEALTH;
            Mana = 0;
            ManaSlots = 0;

            Deck = new List<Card>();
            foreach (var item in INITIAL_CARD_COSTS)
            {
                Deck.Add(new Card(item));
            }

            Hand = new List<Card>();
        }



    }
}
