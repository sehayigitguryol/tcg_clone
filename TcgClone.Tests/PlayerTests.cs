using System;
using System.Collections.Generic;
using System.Linq;
using TcgClone.Entities;
using Xunit;
using Xunit.Extensions;

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

            var hand = new List<Card>()
            {

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
        [InlineData(0, 5)]
        [InlineData(5, 7)]
        [InlineData(7, 7)]
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

        [Theory]
        [InlineData(30, 5)]
        [InlineData(5, 5)]
        [InlineData(10, 4)]
        [InlineData(3, 4)]
        public void TakeDamage_Test(int health, int damage)
        {
            // Arrange
            var playerName = "Test Player";
            var currentMana = 1;
            var manaCapacity = 3;
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
            player.TakeDamage(damage);

            // Assert
            Assert.Equal(health - damage, player.Health);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(1)]
        [InlineData(30)]
        [InlineData(60)]

        public void DealDamage_Test(int damage)
        {
            // Arrange

            Player currentPlayer = CreateMockPlayerWithName("Player 1");
            Player opponent = CreateMockPlayerWithName("Player 2");
            // Current opponent health = 30

            var opponentCurrentHealth = opponent.Health;

            // Act

            currentPlayer.DealDamage(damage, opponent);

            // Assert

            Assert.Equal(opponentCurrentHealth - damage, opponent.Health);
        }

        [Fact]
        public void PlayCard_InsufficentMana_Test()
        {
            // Arrange

            Player player = CreateMockPlayerWithName("player");
            Player opponent = CreateMockPlayerWithName("opponent");

            player.Mana = 4;
            player.Hand.Add(new Card(7));

            // Act
            player.PlayCard(player.Hand.First(), opponent);

            // Assert
            Assert.Equal(4, player.Mana);
            Assert.Equal(1, player.Hand.Count);
            Assert.Equal(30, opponent.Health);
        }

        [Theory, MemberData(nameof(PlayerHandAndDecisionData))]
        public void PlayCard_EnoughMana_Test(List<int> handCardPoints, int decidedCard, int playerMana)
        {
            // Arrange
            Player player = CreateMockPlayerWithName("player");
            player.Mana = playerMana;

            List<Card> hand = handCardPoints.Select((x) => new Card(x)).ToList();
            player.Hand = new List<Card>(hand);

            Player opponent = CreateMockPlayerWithName("opponent");

            // Act
            Card selectedCard = player.Hand.Where((x) => x.Point == decidedCard).First();

            player.PlayCard(selectedCard, opponent);

            // Assert
            Assert.Equal(hand.Count - 1, player.Hand.Count);

            int decidedCardCountInBeginning = hand.Where((x) => x.Point == decidedCard).ToList().Count;
            int decidedCardCountAfterPlaying = player.Hand.Where((x) => x.Point == decidedCard).ToList().Count;

            Assert.Equal(1, decidedCardCountInBeginning - decidedCardCountAfterPlaying);
            Assert.Equal(playerMana - decidedCard, player.Mana);
            Assert.Equal(30 - decidedCard, opponent.Health);
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

        public static IEnumerable<object[]> PlayerHandAndDecisionData
        {
            get
            {
                return new[]
                {
                    new object[] { new List<int>() { 1, 2, 3 }, 1, 8 },
                    new object[] { new List<int>() { 1, 2, 2, 4 }, 2 , 8},
                    new object[] { new List<int>() { 4 , 2, 5, 1 }, 5, 8},
                 };
            }
        }
    }
}
