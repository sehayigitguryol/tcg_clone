using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone.Interfaces
{
    /// <summary>
    /// Interface to determine basic player actions
    /// </summary>
    public interface IPlayerActions
    {
        /// <summary>
        /// Player draws a card from deck
        /// </summary>
        void DrawCard();

        /// <summary>
        /// Player draws cards till its hand reaches to initial hand size
        /// </summary>
        void GetStartingHand();

        /// <summary>
        /// Increments mana capacity of the player
        /// </summary>
        void IncrementManaCapacity();

        /// <summary>
        /// Refills mana up to mana capacity
        /// </summary>
        void RefillMana();

        /// <summary>
        /// Player's health reduces by amount of damage
        /// </summary>
        /// <param name="damage">Amount of damage has been taken</param>
        void InflictDamage(int damage);

        /// <summary>
        /// Using player deals amount of damage to opponent player
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        /// <param name="opponent">Player that receives damage</param>
        void DealDamage(int damage, Player opponent);

        /// <summary>
        /// Using player uses card towards to opponent
        /// </summary>
        /// <param name="card">Card desired to be used</param>
        /// <param name="opponent">Player that would affected by card</param>
        void PlayCard(Card card, Player opponent);

        /// <summary>
        /// Checks if player can do any move with its hand
        /// </summary>
        /// <returns>true if player can do any move, false if can't</returns>
        bool CanPlayAnyMove();

        /// <summary>
        /// Decision of which card is played next
        /// </summary>
        /// <returns>Decided card</returns>
        Card DecideOnCard();

        /// <summary>
        /// Reading player input from console
        /// </summary>
        /// <returns>Typed string from console</returns>
        string GetPlayerInput();
    }
}
