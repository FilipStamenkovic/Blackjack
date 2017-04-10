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

        public State(int sum, bool usableAce, int dealerCard)
        {
            CurrentSum = sum;
            HasUsableAce = usableAce;
            DealerCard = dealerCard;
        }
    }

//action * 200 + HasUsableAce * 100 + ( DealerCard % 10) * 10 +  CurrentSum % 10

    public enum Action : byte
    {
        Stick = 0,
        Hit = 1
    }
}
