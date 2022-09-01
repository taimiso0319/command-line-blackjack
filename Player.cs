namespace Taimiso
{
    public class Player
    {
        private List<Card> m_Hand;
        public int Credit { get; private set; }

        private int InitialBet = 0;

        public int TotalBet { get; private set; } = 0;

        public bool IsBlackJack => HandTotal() == 21;

        public bool IsBurst => HandTotal() > 21;
        public Player(int credit = 1000)
        {
            Credit = credit;
            m_Hand = new List<Card>();
        }

        public void Clear()
        {
            ClearHand();
            ClearBet();
        }

        private void ClearBet()
        {
            InitialBet = 0;
            TotalBet = 0;
        }

        private void ClearHand()
        {
            m_Hand.Clear();
        }

        public void AddHand(Card card)
        {
            m_Hand.Add(card);
        }

        public void SetPlayerInitialBet(int bet)
        {
            InitialBet = bet;
            TotalBet = InitialBet;
        }

        public void DoubleBet()
        {
            TotalBet *= 2;
        }

        public void AddCredit(int credit)
        {
            Credit += credit;
        }

        public int HandCount()
        {
            return m_Hand.Count();
        }
        public int HandTotal()
        {
            int total = 0;
            bool containsAce = false;
            foreach (var card in m_Hand)
            {
                if (card.Number == 1)
                {
                    containsAce = true;
                    continue;
                }
                int addingNumber = card.Number;
                if (addingNumber > 10)
                {
                    addingNumber = 10;
                }
                total += addingNumber;
            }
            if (containsAce)
            {
                int addingNumber = 11;
                if (total + 11 > 21)
                {
                    addingNumber = 1;
                }
                total += addingNumber;
            }
            return total;
        }
    }
}