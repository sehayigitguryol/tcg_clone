﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcgClone.Entities;

namespace TcgClone.Tests
{
    public class PlayerWithInput : Player
    {
        public List<string> Inputs { get; set; }

        public PlayerWithInput(string name, List<string> inputs)
            : base(name)
        {
            Inputs = inputs;
        }

        public PlayerWithInput(string name, int health, int mana, int manaSlots, List<Card> deck, List<Card> hand, List<string> inputs)
         : base(name, health, mana, manaSlots, deck, hand)
        {
            Inputs = inputs;
        }


        public override string GetPlayerInput()
        {
            var input = Inputs.FirstOrDefault();
            Inputs.Remove(input);
            return input;
        }
    }
}
