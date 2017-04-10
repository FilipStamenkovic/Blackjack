using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack.ObjectModel
{
    public abstract class Policy
    {
        protected static Action[] _actions = new Action[200];
        protected static double[] q = new double[400];
        protected static int[] timesVisited = new int[400];
        protected Dictionary<State, Action> _history = new Dictionary<State, Action>();
        protected const double epsilon = 0.1;
        protected static Random random = new Random();

        public Policy()
        {
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

                string s = "";
                string unvisited = "";
                for (int i = 0; i < 200; i++)
                {
                    if (timesVisited[i] + timesVisited[i + 200] == 0)
                        unvisited += string.Format("Unvisited State: {0}" + Environment.NewLine, i);
                    s += string.Format("State: {0}, visited: {1}" + Environment.NewLine, i, timesVisited[i] + timesVisited[i + 200]);
                }

                File.WriteAllText("TimesVisited.txt", s + unvisited);

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

        public void ClearHistory()
        {
            _history.Clear();            
        }

        public virtual Action GetAction(int currentSum, bool hasUsableAce, int dealerCard)
        {
            int ace = hasUsableAce ? 1 : 0;
            int index = ace * 100 + (dealerCard % 10) * 10 + currentSum % 10;
            Action a = _actions[index];

            if (Program.Mode == Mode.Train && random.NextDouble() < epsilon)//_history.Count == 0)
            {
                a = (Action)Math.Round(random.NextDouble());                
                //a = random.NextDouble() > 0.5 ? Action.Hit : Action.Stick;
            }

            timesVisited[(int)a * 200 + index]++;

            _history.Add(new State(currentSum, hasUsableAce, dealerCard), a);

            return a;
        }

        ///returns true if best action for any state is changed
        public abstract bool EvaluateAndImprovePolicy(double reward);

        //evaluates policy after each taken action
        public abstract void EvaluateAndImprovePolicy();

        public static void FlushToDisk()
        {
            string text = string.Join(Environment.NewLine, timesVisited);

            text += Environment.NewLine + string.Join(Environment.NewLine, q);
            File.WriteAllText(Program.FileName, text);
        }
    }
}