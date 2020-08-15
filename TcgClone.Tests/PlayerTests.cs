using System;
using System.Linq;
using TcgClone.Entities;
using Xunit;

namespace TcgClone.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_InitializeDefault_Test()
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


        private Player CreateMockPlayerWithName(string name)
        {
            Player player = new Player(name);
            return player;
        }

    }
}
