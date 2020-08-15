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
            Assert.Equal(0, player.ManaCapacity);

            Assert.Empty(player.Hand);
            Assert.Equal(20, player.Deck.Count);
        }

        [Fact]
        public void DrawCard_WhileDeckIsEmpty_Test()
        {
            //Arrange
            var name = "Player 1";
            var health = 30;
            var mana = 1;
            var manaCapacity = 1;

            var deck = new List<Card>();
            var hand = new List<Card>();

            Player player = CreateMockPlayer(name, health, mana, manaCapacity, deck, hand);

            //Act
            player.DrawCard();

            //Assert

            Assert.Empty(player.Deck);
            Assert.Empty(player.Hand);
        }

        [Fact]
        public void DrawCard_WhileDeckIsNotEmpty_Test()
        {
            //Arrange
            var name = "Player 1";
            var health = 30;
            var mana = 1;
            var manaCapacity = 1;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2),
                new Card(4)
            };

            var hand = new List<Card>() {
                new Card(3)
            };

            Player player = CreateMockPlayer(name, health, mana, manaCapacity, deck, hand);

            //Act
            player.DrawCard();

            //Assert
            Assert.Equal(2, player.Deck.Count);
            Assert.Equal(2, player.Hand.Count);
        }

        [Fact]
        public void DrawCard_WhileHandSizeAtLimit_Test()
        {
            //Arrange
            var name = "Player 1";
            var health = 30;
            var mana = 1;
            var manaCapacity = 1;

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

            Player player = CreateMockPlayer(name, health, mana, manaCapacity, deck, hand);

            //Act
            player.DrawCard();

            //Assert
            Assert.Equal(2, player.Deck.Count);
            Assert.Equal(5, player.Hand.Count);
        }

        [Fact]
        public void GetStartingHand_WithFullDeck_Test()
        {
            // Arrange
            var playerName = "Test Player";
            Player player = CreateMockPlayerWithName(playerName);

            // Act
            player.GetStartingHand();

            // Assert
            Assert.Equal(3, player.Hand.Count);
            Assert.Equal(17, player.Deck.Count);
        }

        [Fact]
        public void GetStartingHand_WithMissingDeck_Test()
        {
            // Arrange
            var playerName = "Test Player";
            var health = 30;
            var mana = 1;
            var manaCapacity = 1;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2)
            };

            var hand = new List<Card>() {

            };

            Player player = CreateMockPlayer(playerName, health, mana, manaCapacity, deck, hand);

            // Act
            player.GetStartingHand();

            // Assert
            Assert.Equal(2, player.Hand.Count);
            Assert.Empty(player.Deck);
        }

        [Fact]
        public void IncrementManaCapacity_LesserThanMaxManaCapacity_Test()
        {
            // Arrange
            var playerName = "Test Player";
            var health = 30;
            var mana = 1;
            var manaCapacity = 1;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2)
            };

            var hand = new List<Card>
            {
                new Card(1),
                new Card(2)
            };

            Player player = CreateMockPlayer(playerName, health, mana, manaCapacity, deck, hand);

            // Act
            player.IncrementManaCapacity();

            // Assert
            Assert.Equal(2, player.ManaCapacity);
        }

        [Fact]
        public void IncrementManaCapacity_EqualsMaxManaCapacity_Test()
        {
            // Arrange
            var playerName = "Test Player";
            var health = 30;
            var mana = 1;
            var manaCapacity = 10;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2)
            };

            var hand = new List<Card>
            {
                new Card(1),
                new Card(2)
            };

            Player player = CreateMockPlayer(playerName, health, mana, manaCapacity, deck, hand);

            // Act
            player.IncrementManaCapacity();

            // Assert
            Assert.Equal(10, player.ManaCapacity);
        }

        [Theory]
        [InlineData(0,5)]
        [InlineData(5,7)]
        [InlineData(7,7)]
        public void RefillMana_Test(int currentMana, int manaCapacity)
        {
            // Arrange
            var playerName = "Test Player";
            var health = 30;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2)
            };

            var hand = new List<Card>
            {
                new Card(1),
                new Card(2)
            };

            Player player = CreateMockPlayer(playerName, health, currentMana, manaCapacity, deck, hand);

            // Act
            player.RefillMana();

            // Assert
            Assert.Equal(manaCapacity, player.Mana);
        }


        private Player CreateMockPlayerWithName(string name)
        {
            Player player = new Player(name);
            return player;
        }

        private Player CreateMockPlayer(string name, int health, int mana, int manaCapacity, List<Card> deck, List<Card> hand)
        {
            Player player = new Player(name, health, mana, manaCapacity, deck, hand);
            return player;
        }

    }
}
