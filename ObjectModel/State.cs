using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack.ObjectModel
{
    public class State
    {
        public int CurrentSum { get; set; }
        public bool HasUsableAce { get; set; }
        public int DealerCard { get; set; }
    }

    public enum Action : byte
    {
        Stick = 0,
        Hit = 1
    }
}
