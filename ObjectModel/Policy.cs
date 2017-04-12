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
        protected static double alpha = 0.001;
        protected const double discount = 0.9;
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

        private void PrintPlotValues(string fileName, bool hasUsableAce)
        {
            StringBuilder sb = new StringBuilder();
            int offset = hasUsableAce ? 100 : 0;
            //with usable ace
            for (int currentSum = 21; currentSum >= 12; currentSum--)
            {
                for (int dealerCard = 1; dealerCard <= 10; dealerCard++)
                {
                    int index = offset + dealerCard % 10 * 10 + currentSum % 10;
                    double difference = (q[index] - q[index + 200]) / 2.0 * 100;
                    sb.Append(currentSum % 10 + ", " + dealerCard % 10 + ", " + Math.Max(q[index], q[index + 200]).ToString("0.000"));
                    sb.AppendLine();
                }                
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        public void Print(string fileName = "")
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = this.GetType().Name + "Policy.txt";
            }

            PrintPlotValues("Plot" + this.GetType().Name + ".txt", false);
            PrintPlotValues("Plot" + this.GetType().Name + ".txt", true);
            StringBuilder sb = new StringBuilder();

            //with usable ace
            for (int currentSum = 21; currentSum >= 12; currentSum--)
            {
                for (int dealerCard = 1; dealerCard <= 10; dealerCard++)
                {
                    int index = 100 + dealerCard % 10 * 10 + currentSum % 10;
                    double difference = (q[index] - q[index + 200]) / 2.0 * 100;
                    sb.Append(difference.ToString("0.000") + "\t");
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
                    double difference = (q[index] - q[index + 200]) / 2.0 * 100;
                    sb.Append(difference.ToString("0.000") + "\t");
                }
                sb.AppendLine();
            }

            File.WriteAllText(fileName, sb.ToString());
        }
    }
}