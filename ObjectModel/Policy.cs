using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.ObjectModel
{
    public abstract class Policy
    {
        protected static Action[] _actions = new Action[200];
        protected static double[] q = new double[400];
        protected static int[] timesVisited = new int[400];
        protected List<KeyValuePair<State, Action>> _history = new List<KeyValuePair<State, Action>>();
        protected const double epsilon = 0.1;
        protected static double alpha = 0.01;
        protected const double discount = 0.7;
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

        public virtual void ClearHistory()
        {
            _history.Clear();
            if (Game.Episode.EpisodeNumber % 1000000 == 0)
                alpha = alpha / 2;
        }

        public virtual Action GetAction(int currentSum, bool hasUsableAce, int dealerCard)
        {
            int ace = hasUsableAce ? 1 : 0;
            int index = ace * 100 + (dealerCard % 10) * 10 + currentSum % 10;
            Action a = _actions[index];

            if (Program.Mode == Mode.Train && random.NextDouble() < epsilon)
            {
                a = (Action)Math.Round(random.NextDouble());
            }

            timesVisited[(int)a * 200 + index]++;

            _history.Add(new KeyValuePair<State, Action>(new State(currentSum, hasUsableAce, dealerCard), a));

            return a;
        }

        ///returns true if best action for any state is changed
        public abstract bool EvaluateAndImprovePolicy(double reward, bool isFinal = true);

        public static void FlushToDisk()
        {
            string text = string.Join(Environment.NewLine, timesVisited);

            text += Environment.NewLine + string.Join(Environment.NewLine, q);
            File.WriteAllText(Program.FileName, text);
        }

        public void Print(string fileName = "")
        {
            if(string.IsNullOrWhiteSpace(fileName))
            {
                fileName = this.GetType().ToString() + "Policy.txt";
            }

            StringBuilder sb = new StringBuilder();
            
            //with usable ace
            for (int currentSum = 21; currentSum >= 12; currentSum--)
            {
                for (int dealerCard = 1; dealerCard <= 10; dealerCard++)
                {
                    int index = 100 + dealerCard % 10 * 10 + currentSum % 10;
                    int action = (int)_actions[index];

                    sb.Append(action + "\t");
                }
                sb.AppendLine();
            }

            sb.AppendLine();

            //no usable ace
            for (int currentSum = 21; currentSum >= 12; currentSum--)
            {
                for (int dealerCard = 1; dealerCard <= 10; dealerCard++)
                {
                    int index = dealerCard % 10 * 10 + currentSum % 10;
                    int action = (int) _actions[index];

                    sb.Append(action + "\t");
                }
                sb.AppendLine();
            }

            File.WriteAllText(fileName, sb.ToString());
        }
    }
}