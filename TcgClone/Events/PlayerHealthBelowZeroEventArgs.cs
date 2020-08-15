using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone.Events
{
    public class PlayerHealthBelowZeroEventArgs : EventArgs
    {
        public Player LoserPlayer { get; set; }
    }
}
