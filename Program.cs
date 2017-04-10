using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackjack.Game;
using Blackjack.ObjectModel;
using Blackjack.ObjectModel.Interfaces;
using System.Threading;

namespace Blackjack
{
    public enum Mode {Train, Play};
    public class Program
    {
        public const string FileName = "Blackjack.txt";
        public static Mode Mode;
        public static double reward = 0;

        public static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.Unicode;


            Policy.Initialize();
            System.Console.WriteLine("Train or Play?");
            string read = System.Console.ReadLine();
            if (read.ToLower() == "t")
            {
                Mode = Mode.Train;
                System.DateTime then = System.DateTime.Now;
                for (int i = 0; i < 10000000; i++)
                {
                    Episode e = new Episode();                    
                    e.Play();
                }

                System.Console.WriteLine((System.DateTime.Now - then).TotalSeconds);
            }
            else if (read.ToLower() == "p")
            {
                Mode = Mode.Play;
                while (read.ToLower() == "p")
                {
                    Episode e = new Episode();
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
