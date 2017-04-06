using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackjack.ObjectModel;
using Blackjack.ObjectModel.Interfaces;

namespace Blackjack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            IDeck deck = new InfiniteDeck(); 

            for (int i = 0; i < 52; i++)
            {
                Console.WriteLine(deck.GetNextCard());
            }
        }
    }
}
