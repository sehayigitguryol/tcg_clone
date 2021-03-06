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
        private readonly PlayerMock playerMock = new PlayerMock();

        [Fact]
        public void InitializeDefault_Test()
        {
            // Arrange
            var playerName = "Test Player";

            // Act
            Player player = new Player(1, playerName);

            // Assert
            Assert.Equal(1, player.Id);
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


            Player player = playerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);

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

            Player player = playerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);

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

            Player player = playerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);

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
            Player player = playerMock.CreateMockPlayerWithName(1, playerName);

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
            var name = "Test Player";
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

            Player player = playerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);

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
            var name = "Test Player";
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

            Player player = playerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);

            // Act
            player.IncrementManaCapacity();

            // Assert
            Assert.Equal(2, player.ManaCapacity);
        }

        [Fact]
        public void IncrementManaCapacity_EqualsMaxManaCapacity_Test()
        {
            // Arrange
            var name = "Test Player";
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

            Player player = playerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);

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
            var name = "Test Player";
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

            Player player = playerMock.CreateMockPlayer(1, name, health, currentMana, manaCapacity, deck, hand);

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
            var name = "Test Player";
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

            Player player = playerMock.CreateMockPlayer(1, name, health, currentMana, manaCapacity, deck, hand);

            // Act
            player.InflictDamage(damage);

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

            Player currentPlayer = playerMock.CreateMockPlayerWithName(1, "Player 1");
            Player opponent = playerMock.CreateMockPlayerWithName(2, "Player 2");
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

            Player player = playerMock.CreateMockPlayerWithName(1, "player");
            Player opponent = playerMock.CreateMockPlayerWithName(2, "opponent");

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
            Player player = playerMock.CreateMockPlayerWithName(1, "player");
            player.Mana = playerMana;

            List<Card> hand = handCardPoints.Select((x) => new Card(x)).ToList();
            player.Hand = new List<Card>(hand);

            Player opponent = playerMock.CreateMockPlayerWithName(2, "opponent");

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

        [Fact]
        public void CanPlayAnyCard_EmptyHand_Test()
        {
            // Arrange
            Player player = playerMock.CreateMockPlayerWithName(1, "player");

            // Act
            bool canPlay = player.CanPlayAnyMove();

            // Assert
            Assert.False(canPlay);
        }

        [Fact]
        public void CanPlayAnyCard_InsufficentMana_Test()
        {
            // Arrange
            Player player = playerMock.CreateMockPlayerWithName(4, "player");
            player.Mana = 2;
            player.Hand = new List<Card>()
            {
                new Card(4),
                new Card(5),
                new Card(3)
            };

            // Act
            bool canPlay = player.CanPlayAnyMove();

            // Assert
            Assert.False(canPlay);
        }

        [Fact]
        public void CanPlayAnyCard_EnoughMana_Test()
        {
            // Arrange
            Player player = playerMock.CreateMockPlayerWithName(4, "player");
            player.Mana = 8;
            player.Hand = new List<Card>()
            {
                new Card(4),
                new Card(5),
                new Card(3)
            };

            // Act
            bool canPlay = player.CanPlayAnyMove();

            // Assert
            Assert.True(canPlay);
        }

        [Fact]
        public void DecideOnCard_InvalidInputsTillCorrectOne_Test()
        {
            // Arrange
            var playerName = "Test Player";
            var health = 30;
            var currentMana = 4;
            var manaCapacity = 5;

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

            var inputs = new List<string>() { "a", "b", "2" };
            PlayerWithInput player = playerMock.CreateMockInputPlayer(2, playerName, health, currentMana, manaCapacity, deck, hand, inputs);

            // Act
            var (selectedCard, _) = player.DecideOnCard();

            // Assert 
            Assert.Equal(2, selectedCard.Point);
        }

        [Fact]
        public void DecideOnCard_InsufficentCardsTillSuffiicent_Test()
        {
            // Arrange
            var playerName = "Test Player";
            var health = 30;
            var currentMana = 4;
            var manaCapacity = 5;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2)
            };

            var hand = new List<Card>
            {
                new Card(3),
                new Card(6),
                new Card(7),
                new Card(8),
            };

            var inputs = new List<string>() { "8", "7", "3" };
            PlayerWithInput player = playerMock.CreateMockInputPlayer(4, playerName, health, currentMana, manaCapacity, deck, hand, inputs);

            // Act
            var (selectedCard, _) = player.DecideOnCard();

            // Assert 
            Assert.Equal(3, selectedCard.Point);
        }

        [Fact]
        public void DecideOnCard_UnavaiilableCardsTillAvailable_Test()
        {
            // Arrange
            var playerName = "Test Player";
            var health = 30;
            var currentMana = 4;
            var manaCapacity = 5;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2)
            };

            var hand = new List<Card>
            {
                new Card(3),
                new Card(6),
                new Card(7),
                new Card(8),
            };

            var inputs = new List<string>() { "1", "4", "3" };
            PlayerWithInput player = playerMock.CreateMockInputPlayer(4, playerName, health, currentMana, manaCapacity, deck, hand, inputs);

            // Act
            var (selectedCard, _) = player.DecideOnCard();

            // Assert 
            Assert.Equal(3, selectedCard.Point);
        }

        [Fact]
        public void DecideOnCard_PlayerSkipsTurn_Test()
        {
            // Arrange
            var playerName = "Test Player";
            var health = 30;
            var currentMana = 4;
            var manaCapacity = 5;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2)
            };

            var hand = new List<Card>
            {
                new Card(3),
                new Card(6),
                new Card(7),
                new Card(8),
            };

            var inputs = new List<string>() { "P" };
            PlayerWithInput player = playerMock.CreateMockInputPlayer(4, playerName, health, currentMana, manaCapacity, deck, hand, inputs);

            // Act
            var (selectedCard, _) = player.DecideOnCard();

            // Assert 
            Assert.Null(selectedCard);
            Assert.Equal(4, player.Hand.Count);
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
