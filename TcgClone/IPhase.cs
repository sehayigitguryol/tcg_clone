using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone
{
    /// <summary>
    /// Interface that gives implementing classes to different phases
    /// </summary>
    public interface IPhase
    {
        /// <summary>
        /// Start phase of the turn
        /// </summary>
        /// <param name="attackingPlayer">Player that owns the phase</param>
        /// <param name="defendingPlayer">Player that waits</param>
        void StartPhase(Player attackingPlayer, Player defendingPlayer);

        /// <summary>
        /// Action phase of the turn. Attacking player can choose to play or not.
        /// </summary>
        /// <param name="attackingPlayer">Player that owns the phase</param>
        /// <param name="defendingPlayer">Player that waits</param>
        void ActionPhase(Player attackingPlayer, Player defendingPlayer);

        /// <summary>
        /// End phase of the turn
        /// </summary>
        /// <param name="attackingPlayer">Player that owns the phase</param>
        void EndPhase(Player attackingPlayer);
    }
}
