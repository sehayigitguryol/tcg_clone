using System;
using System.Collections.Generic;
using System.Text;
using TcgClone.Entities;

namespace TcgClone.Interfaces
{
    public interface IPlayerActions
    {
        void DrawCard();

        void GetStartingHand();

        void IncrementManaCapacity();

        void RefillMana();

        void InflictDamage(int damage);

        void DealDamage(int damage, Player opponent);

        void PlayCard(Card card, Player opponent);

        bool CanPlayAnyMove();

        Card DecideOnCard();

        string GetPlayerInput();
    }
}
