using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack.ObjectModel
{
    public class Policy
    {
        private static Action[] _actions = new Action[200];
        private static double[] q = new double[400];
        private static int[] timesVisited = new int[400];
        private Dictionary<State, Action> _history = new Dictionary<State, Action>();
        private const double epsilon = 0.1;
        private Random random;

        public Policy()
        {
            random = new Random();
        }

        public static void Initialize()
        {
            if (File.Exists(Program.FileName))
            {
                string[] lines = File.ReadAllLines(Program.FileName);

                for (int i = 0; i < 400; i++)
                {
                    timesVisited[i] = int.Parse(lines[i]);
                    q[i] = double.Parse(lines[i + 400]);
                }

                for (int i = 0; i < 200; i++)
                {
                    _actions[i] = q[i] > q[i + 200] ? Action.Stick : Action.Hit;
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
            int index = ace * 100 + (dealerCard % 10) * 10 + currentSum % 10;
            Action a;
            if (random.NextDouble() > epsilon)
                a = _actions[index];
            else
            {
                a = (Action)Math.Round(random.NextDouble());
                Console.WriteLine("Greedy exploration");
            }
            timesVisited[(int)a * 200 + index]++;

            _history.Add(new State(currentSum, hasUsableAce, dealerCard), a);

            return a;
        }

        // public Action GetAction(State s)
        // {
        //     int ace = s.HasUsableAce ? 1 : 0;
        //     return _actions[ace * 100 + (s.DealerCard % 10) * 10 + s.CurrentSum % 10];
        // }

        public void EvaluatePolicy(double reward)
        {
            foreach (var keyValue in _history)
            {
                int a = (int)keyValue.Value;
                int ace = keyValue.Key.HasUsableAce ? 1 : 0;
                int index = a * 200 + ace * 100 + (keyValue.Key.DealerCard % 10) * 10 + keyValue.Key.CurrentSum % 10;

                //double oldVal = q[index];
                q[index] = q[index] + 1.0 / timesVisited[index] * (reward - q[index]);
            }
        }

        public void ImprovePolicy(double[] qStar)
        {
            for (int i = 0; i < 200; i++)
            {
                _actions[i] = q[i] > q[i + 200] ? Action.Hit : Action.Stick;
            }
        }

        public void EvaluateAndImprovePolicy(double reward)
        {
            foreach (var keyValue in _history)
            {
                int a = (int)keyValue.Value;
                int ace = keyValue.Key.HasUsableAce ? 1 : 0;
                int actionIndex = ace * 100 + (keyValue.Key.DealerCard % 10) * 10 + keyValue.Key.CurrentSum % 10;
                int qIndex = a * 200 + actionIndex;

                //eval
                q[qIndex] = q[qIndex] + 1.0 / timesVisited[qIndex] * (reward - q[qIndex]);
                //improve
                _actions[actionIndex] = q[actionIndex] > q[actionIndex + 200] ? Action.Stick : Action.Hit;
            }
        }

        public static void FlushToDisk()
        {
            string text = string.Join(Environment.NewLine, timesVisited);

            text += Environment.NewLine + string.Join(Environment.NewLine, q);
            File.WriteAllText(Program.FileName, text);
        }
    }
}