using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;
using Xunit;

namespace TcgClone.Tests
{
    public class GameplayTests
    {
        private readonly GameplayMock GameplayMock = new GameplayMock();
        private readonly PlayerMock PlayerMock = new PlayerMock();

        [Fact]
        public void InitializeGameplay_Test()
        {
            // Arrange
            Player player1 = PlayerMock.CreateMockPlayerWithName("Player 1");
            Player player2 = PlayerMock.CreateMockPlayerWithName("Player 2");

            // Act
            Gameplay game = GameplayMock.CreateMockGameplay(player1, player2);

            // Assert
            Assert.Equal(2, game.PlayerList.Count);

            Assert.Equal(3, game.PlayerList[0].Hand.Count);
            Assert.Equal(3, game.PlayerList[1].Hand.Count);
        }

        [Fact]
        public void FirstTurn_PlayerDoesntDrawCard_Test()
        {
            // Arrange
            Player player1 = PlayerMock.CreateMockPlayerWithName("Active");
            Player player2 = PlayerMock.CreateMockPlayerWithName("Defending");

            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 1, 0);

            // Act
            game.StartTurn();

            // Assert
            Player activePlayer = game.GetActivePlayer();

            Assert.Equal("Active", activePlayer.Name);
            Assert.Equal(3, activePlayer.Hand.Count);
        }

        [Fact]
        public void StartTurn_PlayerReceivesManapointAndRefillMana_Test()
        {
            // Arrange
            var name = "Active";
            var health = 30;
            var mana = 2;
            var manaCapacity = 4;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2),
                new Card(4)
            };

            var hand = new List<Card>() {
                new Card(3)
            };

            Player player1 = PlayerMock.CreateMockPlayer(name, health, mana, manaCapacity, deck, hand);
            Player player2 = PlayerMock.CreateMockPlayerWithName("Defending");
            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 2, 0);

            // Act
            game.StartTurn();

            // Assert
            Player activePlayer = game.GetActivePlayer();

            Assert.Equal("Active", activePlayer.Name);
            Assert.Equal(5, activePlayer.ManaCapacity);
            Assert.Equal(5, activePlayer.Mana);
        }

        [Fact]
        public void StartTurn_PlayerDrawsCard_Test()
        {
            // Arrange
            var name = "Active";
            var health = 30;
            var mana = 2;
            var manaCapacity = 4;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2),
                new Card(4)
            };

            var hand = new List<Card>() {
                new Card(3)
            };

            Player player1 = PlayerMock.CreateMockPlayer(name, health, mana, manaCapacity, deck, hand);
            Player player2 = PlayerMock.CreateMockPlayerWithName("Defending");
            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 2, 0);

            // Act
            game.StartTurn();

            // Assert
            Player activePlayer = game.GetActivePlayer();

            Assert.Equal("Active", activePlayer.Name);
            Assert.Equal(2, activePlayer.Hand.Count);
            Assert.Equal(2, activePlayer.Deck.Count);
        }

        [Fact]
        public void EndTurn_ActivePlayerSwitches_Test()
        {
            // Arrange
            Player player1 = PlayerMock.CreateMockPlayerWithName("Player A");
            Player player2 = PlayerMock.CreateMockPlayerWithName("Player B");

            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 1, 0);

            Player initialActivePlayer = game.GetActivePlayer();

            // Act
            game.EndTurn();

            // Assert
            Player activePlayer = game.GetActivePlayer();

            Assert.Equal("Player A", initialActivePlayer.Name);
            Assert.Equal("Player B", activePlayer.Name);
        }
    }
}
