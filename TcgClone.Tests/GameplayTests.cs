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
            Assert.NotNull(game.ActivePlayer);
            Assert.NotNull(game.OpponentPlayer);

            Assert.Equal(3, game.ActivePlayer.Hand.Count);
            Assert.Equal(3, game.OpponentPlayer.Hand.Count);
        }

    }
}
