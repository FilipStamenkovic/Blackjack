
using System;
using Blackjack.ObjectModel.Interfaces;

namespace Blackjack.ObjectModel
{
    public class InfiniteDeck : IDeck
    {
        public Card GetNextCard()
        {
            throw new NotImplementedException();
        }

        public bool HasCards()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}