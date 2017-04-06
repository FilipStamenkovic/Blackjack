using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack.ObjectModel
{
    public class Deck
    {
    }

    public class Card
    {
        public readonly Suit CardSuit;
        private readonly byte NumericalRepresentation;

        //Base value of Ace, represented by this property, is 1
        public readonly byte BaseValue;

        //Blackjack value of Ace, represented by this property, is 1
        public readonly byte BlackjackValue;

        public readonly bool IsAce;

        private static Dictionary<int, string> cardLetter;

        public Card(byte numericalRepresentation)
        {
            if (cardLetter == null)
            {
                cardLetter = new Dictionary<int, string>();
                cardLetter.Add(11, "J");
                cardLetter.Add(12, "Q");
                cardLetter.Add(13, "K");
                cardLetter.Add(1, "A");
            }
            
            if (numericalRepresentation < 1 || numericalRepresentation > 52)
            {
                throw new Exception("Invalid numberical representation of a card! Supported range is [1, 52]");
            }

            NumericalRepresentation = numericalRepresentation;

            //Number 1 - 13 represent Hearts
            //Number 14 - 26 represent Diamonds
            //Number 27 - 39 represent Clubs
            //Number 40 - 52 represent Spades
            CardSuit = (Suit) ((numericalRepresentation - 1) / 13);

            byte reminder = (byte) (numericalRepresentation % 13);

            if (reminder >= 1 && reminder <= 10)
            {
                BaseValue = reminder;
            }
            else if (reminder == 0)
            {
                BaseValue = 14;
            }
            else
            {
                BaseValue = (byte)(reminder + 1);
            }
             
            BlackjackValue = BaseValue >= 10 ? (byte)10 : BaseValue;
            IsAce = BaseValue == 1;
        }

        public override string ToString()
        {
            char suitSymbol;

            switch (CardSuit)
            {
                case Suit.Hearts:
                    suitSymbol = '\u2665';
                    break;
                case Suit.Diamonds:
                    suitSymbol = '\u2666';
                    break;
                case Suit.Clubs:
                    suitSymbol = '\u2663';
                    break;
                case Suit.Spades:
                    suitSymbol = '\u2660';
                    break;
                default:
                    throw new Exception("Unsuported suit! Only values [0, 3] are supported!");
            }

            return suitSymbol + (cardLetter.ContainsKey(BaseValue) ? cardLetter[BaseValue] : BaseValue.ToString());
        }
    }

    public enum Suit : byte
    {
        Hearts = 0,
        Diamonds = 1,
        Clubs = 2,
        Spades = 3
    }
}
