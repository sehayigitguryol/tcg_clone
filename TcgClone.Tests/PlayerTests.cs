using System;
using TcgClone.Entities;
using Xunit;

namespace TcgClone.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_Initialize_Test()
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
        }
    }
}
