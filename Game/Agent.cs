using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackjack.ObjectModel;

namespace Blackjack.Game
{
    public class Agent
    {
        private List<Card> agentCards;
        public Agent()
        {
            agentCards = new List<Card>();
        }

        // public Action Play()
        // {
        //     int sum = 0;
        //     while (sum < 17)
        //     {
        //         agentCards.Add(deck.GetNextCard());
        //         sum = dealersCards.Sum(c => c.BlackjackValue);
        //         int numOfAces = dealersCards.Count(s => s.BlackjackValue == 1);
        //         if (sum < 12 && numOfAces > 0)
        //             sum += 10;
        //     }

        //     return sum;
        // }

    }
}