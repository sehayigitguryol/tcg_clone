using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone.Events
{
    /// <summary>
    ///  Event arguments for player health below zero event
    /// </summary>
    public class PlayerHealthBelowZeroEventArgs : EventArgs
    {
        public Player LoserPlayer { get; set; }
    }
}
