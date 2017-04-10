using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackjack.ObjectModel;
using Blackjack.ObjectModel.Interfaces;

namespace Blackjack.Game
{
    public class Environment
    {
        private IDeck deck;
        private int numberOfPlayes;
        private List<Card> dealersCards;
        public int Sum { get; private set; }

        public Environment(IDeck deck, int numberOfPlayes = 1)
        {
            this.deck = deck;
            this.numberOfPlayes = numberOfPlayes;
            this.dealersCards = new List<Card>();
        }
        
        public Card Deal(bool toDealer)
        {
            Card nextCard = deck.GetNextCard();
            if (toDealer)
                dealersCards.Add(nextCard);
            return nextCard;
        }

        //finishes the game at the end by dealing cards to the dealer
        public int FinishGame()
        {
            int sum = 0;
            while (sum < 17)
            {
                dealersCards.Add(deck.GetNextCard());
                sum = dealersCards.Sum(c => c.BlackjackValue);
                int numOfAces = dealersCards.Count(s => s.BlackjackValue == 1);

                if (sum < 12 && numOfAces > 0)
                    sum += 10;
            }

            return (Sum = sum);
        }

        public override string ToString()
        {
            return string.Format("Dealer Sum: {0}" + System.Environment.NewLine + "Dealer Cards: [{1}]", Sum, string.Join(", ", dealersCards.Select(c => c.ToString())));
        }
    }
}