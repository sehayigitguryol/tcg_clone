using System;
using System.Collections.Generic;
using System.Text;

namespace TcgClone.Entities
{
    public static class PlayerConstants
    {
        public static int INITIAL_HAND_SIZE = 3;

        public static int MAX_HAND_SIZE = 5;

        public static int MAX_MANA_CAPACITY = 10;

        public static int MAX_HEALTH = 30;

        public static List<int> INITIAL_CARD_COSTS = new List<int>() { 0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7, 8 };
    }
}
