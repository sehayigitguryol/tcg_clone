using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone.Tests
{
    public class PlayerMock
    {
        public Player CreateMockPlayerWithName(int id, string name)
        {
            Player player = new Player(id, name);
            return player;
        }

        public Player CreateMockPlayer(int id, string name, int health, int mana, int manaCapacity, List<Card> deck, List<Card> hand)
        {
            Player player = new Player(id, name, health, mana, manaCapacity, deck, hand);
            return player;
        }

        public PlayerWithInput CreateMockPlayerWithInputAndName(int id, string name, List<string> input)
        {
            PlayerWithInput player = new PlayerWithInput(id, name, input);
            return player;
        }

        public PlayerWithInput CreateMockInputPlayer(int id, string name, int health, int mana, int manaCapacity, List<Card> deck, List<Card> hand, List<string> input)
        {
            PlayerWithInput player = new PlayerWithInput(id, name, health, mana, manaCapacity, deck, hand, input);
            return player;
        }
    }

}
