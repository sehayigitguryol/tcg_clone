using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;
using Xunit;

namespace TcgClone.Tests
{
    public class PlayerFactoryTests
    {
        [Fact]
        public void InitializePlayerFromFactory_Test()
        {
            // Arrange
            var playerName = "Test Player";

            // Act
            Player player = PlayerFactory.CreatePlayer(playerName);

            // Assert
            Assert.Equal(1, player.Id);
            Assert.Equal(playerName, player.Name);
            Assert.Equal(30, player.Health);
            Assert.Equal(0, player.Mana);
            Assert.Equal(0, player.ManaCapacity);

            Assert.Empty(player.Hand);
            Assert.Equal(20, player.Deck.Count);
        }
    }
}
