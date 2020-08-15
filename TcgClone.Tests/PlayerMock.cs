using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone.Tests
{
    public class PlayerMock
    {
        public Player CreateMockPlayerWithName(string name)
        {
            Player player = new Player(name);
            return player;
        }

        public Player CreateMockPlayer(string name, int health, int mana, int manaCapacity, List<Card> deck, List<Card> hand)
        {
            Player player = new Player(name, health, mana, manaCapacity, deck, hand);
            return player;
        }

        public PlayerWithInput CreateMockPlayerWithInputAndName(string name, List<string> input)
        {
            PlayerWithInput player = new PlayerWithInput(name, input);
            return player;
        }

        public PlayerWithInput CreateMockInputPlayer(string name, int health, int mana, int manaCapacity, List<Card> deck, List<Card> hand, List<string> input)
        {
            PlayerWithInput player = new PlayerWithInput(name, health, mana, manaCapacity, deck, hand, input);
            return player;
        }
    }

}
