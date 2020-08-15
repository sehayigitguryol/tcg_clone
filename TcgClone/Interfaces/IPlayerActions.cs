using System;
using System.Collections.Generic;
using System.Text;

namespace TcgClone.Interfaces
{
    public interface IPlayerActions
    {
        void DrawCard();

        void GetStartingHand();

        void IncrementManaCapacity();

        void RefillMana();
    }
}
