using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackjack.Game;
using Blackjack.ObjectModel;
using Blackjack.ObjectModel.Interfaces;

namespace Blackjack
{
    public class Program
    {
        public const string FileName = "Blackjack.txt";
        public static double reward = 0;

        public static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.Unicode;


            Policy.Initialize();
            System.Console.WriteLine("Train or Play?");
            string read = System.Console.ReadLine();
            if (read == "T")
            {
                System.DateTime then = System.DateTime.Now;
                for (int i = 0; i < 500000000; i++)
                {
                    Episode e = new Episode();
                    e.Play();
                }

                System.Console.WriteLine((System.DateTime.Now - then).TotalSeconds);
            }
            else if (read == "P")
            {
                while (read == "P")
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
