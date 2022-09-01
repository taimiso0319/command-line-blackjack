using System.Collections;
namespace Taimiso
{
    public class Deck
    {
        private int m_DeckAmount;
        private Queue<Card> m_Cards = new Queue<Card>();
        public Deck(int deckAmount = 1)
        {
            m_DeckAmount = deckAmount;
            InitializeDeck(m_DeckAmount);
        }

        public void InitializeDeck()
        {
            InitializeDeck(m_DeckAmount);
        }

        public void InitializeDeck(int deckAmount)
        {
            m_DeckAmount = deckAmount;
            m_Cards.Clear();
            List<Card> tempCards = new List<Card>();
            for (int i = 0; i < m_DeckAmount; i++)
            {
                AddCards(tempCards);
            }
            tempCards.Shuffle();
            m_Cards = new Queue<Card>(tempCards);

        }

        private void AddCards(List<Card> cards)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 13; j++)
                {
                    cards.Add(new Card((Card.SuitType)i, j));
                }
            }
        }

        public Card Deal()
        {
            if (!m_Cards.Any())
            {
                InitializeDeck();
            }
            return m_Cards.Dequeue();
        }
    }
}