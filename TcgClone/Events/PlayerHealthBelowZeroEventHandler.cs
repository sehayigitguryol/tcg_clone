using System;
using System.Collections.Generic;
using System.Text;

namespace TcgClone.Events
{
    /// <summary>
    ///  Event handler for player health below zero event
    /// </summary>
    /// <param name="sender">sender class</param>
    /// <param name="e"> event arguments</param>
    public delegate void PlayerHealthBelowZeroEventHandler(Object sender, PlayerHealthBelowZeroEventArgs e);
}
