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
            Player player1 = PlayerMock.CreateMockPlayerWithName(1, "Player 1");
            Player player2 = PlayerMock.CreateMockPlayerWithName(2, "Player 2");

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
            Player player1 = PlayerMock.CreateMockPlayerWithName(1, "Active");
            Player player2 = PlayerMock.CreateMockPlayerWithName(2, "Defending");

            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 1, 0);

            // Act
            game.StartPhase(player1, player2);

            // Assert
            Player activePlayer = game.GetActivePlayer();

            Assert.Equal(1, activePlayer.Id);
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

            Player player1 = PlayerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);
            Player player2 = PlayerMock.CreateMockPlayerWithName(2, "Defending");
            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 2, 0);

            // Act
            game.StartPhase(player1, player2);

            // Assert
            Player activePlayer = game.GetActivePlayer();

            Assert.Equal(1, activePlayer.Id);
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

            Player player1 = PlayerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);
            Player player2 = PlayerMock.CreateMockPlayerWithName(2, "Defending");
            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 2, 0);

            // Act
            game.StartPhase(player1, player2);

            // Assert
            Player activePlayer = game.GetActivePlayer();

            Assert.Equal(1, activePlayer.Id);
            Assert.Equal(2, activePlayer.Hand.Count);
            Assert.Equal(2, activePlayer.Deck.Count);
        }

        [Fact]
        public void StartTurn_PlayerCouldntDrawCard_BleedsOut_Test()
        {
            // Arrange
            var name = "Active";
            var health = 30;
            var mana = 2;
            var manaCapacity = 4;

            var deck = new List<Card>
            {
            };

            var hand = new List<Card>() {
                new Card(3)
            };

            Player player1 = PlayerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);
            Player player2 = PlayerMock.CreateMockPlayerWithName(2, "Defending");
            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 2, 0);

            // Act
            game.StartPhase(player1, player2);

            // Assert
            Player activePlayer = game.GetActivePlayer();

            Assert.Equal(1, activePlayer.Id);
            Assert.Single(activePlayer.Hand);
            Assert.Empty(activePlayer.Deck);
            Assert.Equal(29, activePlayer.Health);
        }

        [Fact]
        public void StartTurn_PlayerLosesFromBleedingOut_Test()
        {

            // Arrange
            var name = "Active";
            var health = 1;
            var mana = 2;
            var manaCapacity = 4;

            var deck = new List<Card>
            {
            };

            var hand = new List<Card>() {
                new Card(3)
            };

            Player player1 = PlayerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);
            Player player2 = PlayerMock.CreateMockPlayerWithName(2, "Defending");
            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 2, 0);

            // Act
            game.StartPhase(player1, player2);

            // Assert
            Player activePlayer = game.GetActivePlayer();
            Player defendingPlayer = game.GetDefendingPlayer();

            Assert.Equal(1, activePlayer.Id);
            Assert.Single(activePlayer.Hand);
            Assert.Empty(activePlayer.Deck);
            Assert.Equal(0, activePlayer.Health);
            Assert.Equal(defendingPlayer.Id, game.WinnerPlayer.Id);
        }

        [Fact]
        public void EndTurn_ActivePlayerSwitches_Test()
        {
            // Arrange
            Player player1 = PlayerMock.CreateMockPlayerWithName(1,"Player A");
            Player player2 = PlayerMock.CreateMockPlayerWithName(2,"Player B");

            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 1, 0);

            Player initialActivePlayer = game.GetActivePlayer();

            // Act
            game.EndPhase(initialActivePlayer);

            // Assert
            Player activePlayer = game.GetActivePlayer();

            Assert.Equal(1, initialActivePlayer.Id);
            Assert.Equal(2, activePlayer.Id);
        }

        [Fact]
        public void UseCard_OpponentHPBelowZero_Test()
        {
            // Arrange
            var name = "Active";
            var health = 30;
            var mana = 80;
            var manaCapacity = 80;

            var deck = new List<Card>
            {
                new Card(1),
                new Card(2),
                new Card(4)
            };

            var card20 = new Card(20);
            var card10 = new Card(10);

            var hand = new List<Card>() {
                card20,
                card10
            };

            Player player1 = PlayerMock.CreateMockPlayer(1, name, health, mana, manaCapacity, deck, hand);
            Player player2 = PlayerMock.CreateMockPlayerWithName(2, "Defending");
            Gameplay game = GameplayMock.CreateMockGameplayWithDecidedSides(player1, player2, 2, 0);

            // Act
            game.UseCard(card20);
            game.UseCard(card10);

            // Assert
            Assert.Equal(50, player1.Mana);
            Assert.Empty(player1.Hand);
            Assert.Equal(0, player2.Health);
            Assert.NotNull(game.WinnerPlayer);
            Assert.Equal(player1.Id, game.WinnerPlayer.Id);
        }



    }
}
