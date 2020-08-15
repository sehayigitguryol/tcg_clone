using System;
using System.Collections.Generic;
using System.Linq;
using TcgClone.Entities;
using Xunit;

namespace TcgClone.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void InitializeDefault_Test()
        {
            // Arrange
            var playerName = "Test Player";

            // Act
            Player player = new Player(playerName);

            // Assert
            Assert.Equal(playerName, player.Name);
            Assert.Equal(30, player.Health);
            Assert.Equal(0, player.Mana);
            Assert.Equal(0, player.ManaSlots);

            Assert.Empty(player.Hand);
            Assert.Equal(20, player.Deck.Count);
        }

        [Fact]
        public void DrawCard_WhileDeckIsEmpty_Error()
        {
            //Arrange
            var name = "Player 1";
            var health = 30;
            var mana = 1;
            var manaSlot = 1;

            var deck = new List<Card>();
            var hand = new List<Card>();

            Player player = CreateMockPlayer(name, health, mana, manaSlot, deck, hand);

            //Act
            player.DrawCard();

            //Assert

            Assert.Empty(player.Deck);
            Assert.Empty(player.Hand);
        }

        [Fact]
        public void DrawCard_WhileDeckIsNotEmpty_Success()
        {
            //Arrange
            var name = "Player 1";
            var health = 30;
            var mana = 1;
            var manaSlot = 1;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2),
                new Card(4)
            };

            var hand = new List<Card>() {
                new Card(3)
            };

            Player player = CreateMockPlayer(name, health, mana, manaSlot, deck, hand);

            //Act
            player.DrawCard();

            //Assert
            Assert.Equal(2, player.Deck.Count);
            Assert.Equal(2, player.Hand.Count);
        }

        [Fact]
        public void DrawCard_WhileHandSizeAtLimit()
        {
            //Arrange
            var name = "Player 1";
            var health = 30;
            var mana = 1;
            var manaSlot = 1;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2),
                new Card(4)
            };

            var hand = new List<Card>() {
                new Card(3),
                new Card(1),
                new Card(2),
                new Card(4),
                new Card(1)
            };

            Player player = CreateMockPlayer(name, health, mana, manaSlot, deck, hand);

            //Act
            player.DrawCard();

            //Assert
            Assert.Equal(2, player.Deck.Count);
            Assert.Equal(5, player.Hand.Count);
        }

        private Player CreateMockPlayerWithName(string name)
        {
            Player player = new Player(name);
            return player;
        }

        private Player CreateMockPlayer(string name, int health, int mana, int manaSlots, List<Card> deck, List<Card> hand)
        {
            Player player = new Player(name, health, mana, manaSlots, deck, hand);
            return player;
        }

    }
}
