using System;
using System.Collections.Generic;
using System.Text;

namespace TcgClone.Entities
{
    public class Card
    {
        public int Point { get; private set; }

        public Card(int point)
        {
            Point = point;
        }
    }
}
