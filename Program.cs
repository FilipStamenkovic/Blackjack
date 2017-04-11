using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackjack.Game;
using Blackjack.ObjectModel;
using Blackjack.ObjectModel.Interfaces;
using System.Threading;

namespace Blackjack
{
    public enum Mode { Train, Play };
    public class Program
    {
        public static string FileName = "Blackjack.txt";
        public const int Threshold = 1000000;
        public static Mode Mode;
        public static double reward = 0;

        public static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.Unicode;

            System.Console.WriteLine("Choose algorithm. Monte-Carlo or TD?");
            Policy policy;
            string read = System.Console.ReadLine();
            if (read.ToLower() == "m")
            {
                Program.FileName = "MC" + Program.FileName;
                policy = new MonteCarlo();
            }
            else if (read.ToLower() == "t")
            {
                Program.FileName = "TD" + Program.FileName;
                policy = new Sarsa();
            }
            else if (read.ToLower() == "b")
            {
                Program.FileName = "BS" + Program.FileName;
                policy = new BackwardSarsa();
            }
            else
                return;

            Policy.Initialize();
            System.Console.WriteLine("Train or Play?");
            read = System.Console.ReadLine();
            if (read.ToLower() == "t")
            {
                Mode = Mode.Train;
                System.DateTime then = System.DateTime.Now;
                int lastTimeActionUpdated = 0;
                for (int i = 0; i < 100000000; i++)
                {
                    Episode e = new Episode(policy);
                    if (e.Play())
                    {
                        lastTimeActionUpdated = i;
                    }
                    else if (i - lastTimeActionUpdated > Threshold)
                    {
                        System.Console.WriteLine("No need for more episodes, policy is optimal. Number of episode when action was updated last time: "
                            + lastTimeActionUpdated);
                        break;
                    }
                }
                System.Console.WriteLine((System.DateTime.Now - then).TotalSeconds);
            }
            else if (read.ToLower() == "p")
            {
                Mode = Mode.Play;
                while (read.ToLower() == "p")
                {
                    Episode e = new Episode(policy);
                    e.Play();
                    e.Print();
                    read = System.Console.ReadLine();
                }
            }
            Policy.FlushToDisk();
            System.Console.WriteLine("Total reward: " + reward);
        }
    }
}
