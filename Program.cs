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
       public static double[] q = new double[400];

        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            IDeck deck = new InfiniteDeck(); 
            
            Card dealerCard = deck.GetNextCard();
            int dealerSum = dealerCard.BlackjackValue;
            int playerSum = 0;
            List<Card> playerCards = new List<Card>();
            int usableAce = 0;

            while(playerSum < 12)
            {
                var newCard = deck.GetNextCard();
                playerCards.Add(newCard);
                playerSum = playerCards.Sum(c => c.BlackjackValue);
                int numOfAces = playerCards.Where(c => c.BlackjackValue == 1).Count();

                if(playerSum <= 11 && numOfAces > 0)
                {
                    usableAce = 1;
                    playerSum += 10;
                } 
            }

            
        }
    }
}
