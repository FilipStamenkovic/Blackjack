using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackjack.ObjectModel;

namespace Blackjack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Random generator = new Random(/*todo: specify seed*/);

            for (int i = 0; i < 52; i++)
            {
                byte numbericalRepresentation = (byte)generator.Next(1, 53);
                Console.WriteLine(numbericalRepresentation + " = " + new Card(numbericalRepresentation));
            }
        }
    }
}
