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

        public Episode()
        {
            IsOver = false;
            ++Episode.EpisodeNumber;

            IDeck deck = new InfiniteDeck(); 
            policy = new Policy();
            environment = new Environment(deck);

            Card dealerCard = environment.Deal(true);
            agent = new Agent(policy, dealerCard);
        }

        public void Play()
        {
            while(agent.Play(environment.Deal(false)) != Action.Stick) {}

            IsOver = true;

            int reward = 0;
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

            policy.EvaluatePolicy(reward);
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