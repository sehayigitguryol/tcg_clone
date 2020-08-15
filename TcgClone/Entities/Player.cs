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

        private void InitializePlayerProperties()
        {
            Health = MAX_HEALTH;
            Mana = 0;
            ManaSlots = 0;
        }
    }
}
