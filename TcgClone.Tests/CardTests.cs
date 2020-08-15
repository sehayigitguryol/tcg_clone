using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;
using Xunit;

namespace TcgClone.Tests
{
    public class CardTests
    {
        [Fact]
        public void Card_Initialize_Test()
        {
            // Arrange
            var cardValue = 3;

            // Act
            Card card = new Card(cardValue);

            // Assert
            Assert.Equal(cardValue, card.Point);

        }
    }
}
