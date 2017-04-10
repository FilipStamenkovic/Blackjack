using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackjack.ObjectModel;

namespace Blackjack.Game
{
    public class Agent
    {
        private List<Card> agentCards;
        private Policy policy;
        private Card dealersCard;
        public Agent(Policy policy, Card dealersCard)
        {
            agentCards = new List<Card>();
            this.policy = policy;
            this.dealersCard = dealersCard;
        }

        public Action Play(Card receivedCard)
        {
            int sum = 0;

            agentCards.Add(receivedCard);
            sum = agentCards.Sum(c => c.BlackjackValue);
            int numOfAces = agentCards.Count(s => s.BlackjackValue == 1);
            if (sum < 12 && numOfAces > 0)
            {
                sum += 10;
                numOfAces--;
            }
            if (sum < 12)
                return Action.Hit;
            else if (sum > 21)
                return Action.Stick;

            return policy.GetAction(sum, numOfAces > 0, dealersCard.BlackjackValue);
        }
    }
}