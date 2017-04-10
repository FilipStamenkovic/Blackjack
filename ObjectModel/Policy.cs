using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack.ObjectModel
{
    public class Policy
    {
        private Action[] _actions = new Action[200];
        private double[] q = new double[400];
        private int[] timesVisited = new int[400];
        private Dictionary<State, Action> _history = new Dictionary<State, Action>();

        public Policy()
        {
            if (File.Exists(Program.fileName))
            {
                string[] lines = File.ReadAllLines(Program.fileName);

                for (int i = 0; i < 400; i++)
                {
                    timesVisited[i] = int.Parse(lines[i]);
                    q[i] = double.Parse(lines[i + 400]);
                }

                for (int i = 0; i < 200; i++)
                {
                    _actions[i] = q[i] > q[i + 200] ? Action.Hit : Action.Stick;
                }
            }
            else
            {
                for (int i = 0; i < 200; ++i)
                {
                    _actions[i] = i % 10 > 1 ? Action.Hit : Action.Stick;
                }
            }
        }

        public Action GetAction(int currentSum, bool hasUsableAce, int dealerCard)
        {
            int ace = hasUsableAce ? 1 : 0;
            Action a = _actions[ace * 100 + (dealerCard % 10) * 10 + currentSum % 10];
            
            _history.Add(new State(currentSum, hasUsableAce, dealerCard), a);
            
            return a;
        }

        public Action GetAction(State s)
        {
            int ace = s.HasUsableAce ? 1 : 0;
            return _actions[ace * 100 + (s.DealerCard % 10) * 10 + s.CurrentSum % 10];
        }

        public void EvaluatePolicy(int reward)
        {
            double alpha = 0.01;

            foreach (var keyValue in _history)
            {
                int a = (int) keyValue.Value;
                int ace = keyValue.Key.HasUsableAce ? 1 : 0;
                int index = a * 200 + ace * 100 + (keyValue.Key.DealerCard % 10) * 10 +  keyValue.Key.CurrentSum % 10;
                
                //double oldVal = q[index];
                q[index] = q[index] + alpha * (reward - q [index]);
            }
        }

        public void ImprovePolicy(double[] qStar)
        {
            throw new System.NotImplementedException();

            string text = string.Join(Environment.NewLine, timesVisited);

            text += string.Join(Environment.NewLine, q);
            File.WriteAllText(Program.fileName, text);
        }
    }
}