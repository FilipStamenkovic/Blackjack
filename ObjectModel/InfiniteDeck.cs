
using System;
using Blackjack.ObjectModel.Interfaces;

namespace Blackjack.ObjectModel
{
    public class InfiniteDeck : IDeck
    {
        private static Random _generator = new Random();
        
        public InfiniteDeck()
        {
            //Initialize();
        }

        public Card GetNextCard()
        {
            byte numbericalRepresentation = (byte)_generator.Next(1, 53);

            return new Card(numbericalRepresentation);
        }

        public bool HasCards()
        {
            return true;
        }

        public void Initialize()
        {
            _generator = new Random();
        }
    }
}