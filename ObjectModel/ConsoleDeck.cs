
using System;
using Blackjack.ObjectModel.Interfaces;

namespace Blackjack.ObjectModel
{
    public class ConsoleDeck : IDeck
    {
        
        public ConsoleDeck()
        {
            Initialize();
        }

        public Card GetNextCard()
        {
            string cStr = Console.ReadLine();
            byte numbericalRepresentation = 0;

            if(cStr.ToLower() == "k")
            {
                numbericalRepresentation = (byte) 13;
            }
            else if(cStr.ToLower() == "q")
            {
                numbericalRepresentation = (byte) 12;
            }
            else if(cStr.ToLower() == "j")
            {
                numbericalRepresentation = (byte) 11;
            }
            else if(cStr.ToLower() == "a")
            {
                numbericalRepresentation = (byte) 1;
            }
            else
            {
                numbericalRepresentation = byte.Parse(cStr);
            }

            return new Card(numbericalRepresentation);
        }

        public bool HasCards()
        {
            return true;
        }

        public void Initialize()
        {
        }
    }
}