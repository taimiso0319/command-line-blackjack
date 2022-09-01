using System;

namespace Taimiso
{
    public class Card
    {
        public enum SuitType
        {
            Club = 0,
            Heart,
            Diamond,
            Spade

        }
        public int Number { get; private set; }
        public SuitType Suit { get; private set; }
        public Card(SuitType suit, int number)
        {
            if (number < 1 || 13 < number)
            {
                throw new Exception("Invalid deck number error.");
            }
            Suit = suit;
            Number = number;
        }

        public string GetNumberString()
        {
            switch (Number)
            {
            case 1: return "A";
            case 11: return "J";
            case 12: return "Q";
            case 13: return "K";
            }
            return Number.ToString();
        }
    }
}