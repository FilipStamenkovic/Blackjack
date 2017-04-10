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

        public int Sum { get; private set; }

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
            bool usableAce = false;
            
            if (sum < 12 && numOfAces > 0)
            {
                sum += 10;
                usableAce = true;
            }
            
            Sum = sum;

            if (sum < 12)
                return Action.Hit;
            else if (sum > 21)
                return Action.Stick;

            return policy.GetAction(sum, usableAce, dealersCard.BlackjackValue);
        }

        public override string ToString()
        {
            return string.Format("Agent Sum: {0}" + System.Environment.NewLine + "Agent Cards: [{1}]", Sum, string.Join(", ", agentCards.Select(c => c.ToString())));
        }
    }
}