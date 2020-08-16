using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone
{
    public static class PlayerFactory
    {
        private static int Id = 1;

        public static Player CreatePlayer(string name)
        {
            Player player = new Player(Id, name);
            Id++;
            return player;
        }
    }
}
