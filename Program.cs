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

        public static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.Unicode;

            System.DateTime then = System.DateTime.Now;
            Policy.Initialize();

            for (int i = 0; i < 10000000; i++)
            {
                Episode e = new Episode();
                e.Play();
            }

            Policy.FlushToDisk();
            System.Console.WriteLine((System.DateTime.Now - then).TotalSeconds);
        }
    }
}
