using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackjack.ObjectModel;
using Blackjack.ObjectModel.Interfaces;

namespace Blackjack.Game
{
    public class Episode
    {
        private Policy policy;
        private Agent agent;
        private Environment environment;
        public bool IsOver { get; private set; }
        public int Sum { get; private set; }
        public static int EpisodeNumber = 0;

        public Episode(Policy policy)
        {
            IsOver = false;
            ++Episode.EpisodeNumber;

            IDeck deck = new InfiniteDeck(); 
            this.policy = policy;
            policy.ClearHistory();
            environment = new Environment(deck);
            //System.Console.WriteLine("Get dealer card:");
            Card dealerCard = environment.Deal(true);
            agent = new Agent(policy, dealerCard);
        }

        ///returns true if best action for any state is changed
        public bool Play()
        {
            //System.Console.WriteLine("Get agent cards:");
            while(agent.Play(environment.Deal(false)) != Action.Stick) 
            {
            }
            //System.Console.WriteLine("Get dealer cards:");
            environment.FinishGame();
            IsOver = true;

            double reward = 0;
            if(agent.Sum > 21)
            {
                reward = -1;
            }
            else if (agent.Sum > environment.Sum)
            {
                reward = 1;
            }
            else if (agent.Sum == environment.Sum)
            {
                reward = 0;
            }
            else if (environment.Sum > 21)
            {
                reward = 1;
            }
            else
            {
                reward = -1;
            }

            Program.reward += reward;
            return policy.EvaluateAndImprovePolicy(reward);
        }

        public void Print()
        {
            if(IsOver)
            {
                if(agent.Sum > 21)
                {
                    System.Console.WriteLine("Agent bust!");
                }
                else if (agent.Sum > environment.Sum)
                {
                    System.Console.WriteLine("Agent won!");
                }
                else if (agent.Sum == environment.Sum)
                {
                    System.Console.WriteLine("Push!");
                }
                else if (environment.Sum > 21)
                {
                    System.Console.WriteLine("Dealer bust!");
                }
                else
                {
                    System.Console.WriteLine("Dealer won!");
                }
            }

            System.Console.WriteLine(agent);
            System.Console.WriteLine(environment);
        }
    }
}