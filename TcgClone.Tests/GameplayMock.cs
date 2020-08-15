using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone.Tests
{
    public class GameplayMock
    {
        public Gameplay CreateMockGameplay(Player player1, Player player2)
        {
            return new Gameplay(player1, player2);
        }
    }
}
