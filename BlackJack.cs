namespace Taimiso
{
    public class BlackJack
    {
        private enum ResultType
        {
            Win,
            Lose,
            Draw,
            BlackJack
        }

        private enum PlayerActionType
        {
            Hit = 0,
            Double,
            Stand
        }
        private Player m_Player;
        private Dealer m_Dealer;
        private Deck m_Deck;
        private const int MINIMUM_BET = 100;
        private const int BET_UNIT = 10;
        public BlackJack()
        {
            m_Player = new Player();
            m_Dealer = new Dealer();
            m_Deck = new Deck(8);
        }

        public void Begin()
        {
            Main();
        }

        private void Main()
        {
            bool playLoop = true;
            while (playLoop)
            {
                if (CheckPlayerLose())
                {
                    Message.ShowMessage(Message.NoCredit);
                    playLoop = false;
                    continue;
                }
                m_Player.Clear();
                m_Dealer.Clear();
                ResultType result = ResultType.Lose;
                AskBet();
                DealPlayerFirstCard();
                DealDealerFirstCard();
                DealPlayerSecondCard();
                ShowDealerCardTotal();
                ShowPlayerCardTotal();
                if (m_Player.IsBlackJack)
                {
                    Message.ShowMessage(Message.ShowNaturalBlackJack);
                }
                else
                {
                    PlayerDraw();
                }
                if (!m_Player.IsBurst)
                {
                    DealerDraw();
                }
                result = CheckResult();
                PayCredit(result);
                if (!AskContinue())
                {
                    playLoop = false;
                }
            }
            Message.ShowMessage(string.Format(Message.GameOverResult, m_Player.Credit.ToString()));
            Message.ShowMessage(Message.GameOver);
        }

        private void AskBet()
        {
            bool betSuccess = false;
            while (!betSuccess)
            {
                Message.ShowMessage(string.Format(Message.TellHoldingCredit, m_Player.Credit));
                Message.ShowMessage(Message.AskBetAmount);
                Message.ShowMessage(string.Format(Message.TellMinimumBet, MINIMUM_BET));
                Message.ShowMessage(string.Format(Message.TellMinimumBetUnit, BET_UNIT));
                int amount = 0;
                if (!Int32.TryParse(Console.ReadLine(), out amount))
                {
                    Message.ShowMessage(Message.ErrorBetHasToBeDigit);
                    continue;
                }
                if (amount % BET_UNIT != 0)
                {
                    Message.ShowMessage(string.Format(Message.ErrorBetInvalidUnit, BET_UNIT));
                    continue;
                }
                if (amount < MINIMUM_BET)
                {
                    Message.ShowMessage(Message.ErrorBetNotEnough);
                    continue;
                }
                if (m_Player.Credit < amount)
                {
                    Message.ShowMessage(Message.ErrorBetTooMuch);
                    continue;
                }
                Message.ShowMessage(string.Format(Message.TellHowMuchBet, amount));
                m_Player.SetPlayerInitialBet(amount);
                betSuccess = true;
            }
            Console.ReadKey();
        }

        private void DealDealerCard()
        {
            Message.ShowMessage(Message.DealDealerCard);
            Card card = m_Deck.Deal();
            m_Dealer.AddHand(card);
            Message.ShowMessage(string.Format(Message.DealtCard, m_Dealer.HandCount()));
            Message.ShowMessage(string.Format(Message.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
            Console.ReadKey();
        }

        private void DealPlayerFirstCard()
        {
            Message.ShowMessage(Message.DealPlayerCard);
            Card card = m_Deck.Deal();
            m_Player.AddHand(card);
            Message.ShowMessage(string.Format(Message.DealtCard, m_Player.HandCount()));
            Message.ShowMessage(string.Format(Message.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
            Console.ReadKey();
        }

        private void DealDealerFirstCard()
        {
            DealDealerCard();
        }

        private void DealPlayerSecondCard()
        {
            Message.ShowMessage(Message.DealPlayerCard);
            Card card = m_Deck.Deal();
            m_Player.AddHand(card);
            Message.ShowMessage(string.Format(Message.DealtCard, m_Player.HandCount()));
            Message.ShowMessage(string.Format(Message.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
            Console.ReadKey();
        }

        private void ShowPlayerCardTotal()
        {
            Message.ShowMessage(string.Format(Message.ShowPlayerCardTotal, m_Player.HandTotal()));
        }

        private void ShowDealerCardTotal()
        {
            Message.ShowMessage(string.Format(Message.ShowDealerCardTotal, m_Dealer.HandTotal()));
        }

        private void PlayerDraw()
        {
            bool drawDone = false;
            while (!drawDone)
            {
                Message.ShowMessage(Message.AskPlayerAction);
                int action = 0;
                if (!Int32.TryParse(Console.ReadLine(), out action))
                {
                    Message.ShowMessage(Message.ErrorAskPlayerAction);
                    continue;
                }
                switch ((PlayerActionType)action)
                {
                case PlayerActionType.Hit:
                    HitCard();
                    break;
                case PlayerActionType.Double:
                    if (m_Player.Credit < m_Player.TotalBet * 2)
                    {
                        Message.ShowMessage(Message.ErrorAskPlayerActionDouble);
                        Message.ShowMessage(string.Format(Message.TellHoldingCredit, m_Player.Credit));
                        Message.ShowMessage(string.Format(Message.TellHowMuchBetTotal, m_Player.TotalBet));
                        continue;
                    }
                    Message.ShowMessage(string.Format(Message.TellHowMuchBet, m_Player.TotalBet));
                    m_Player.DoubleBet();
                    Message.ShowMessage(string.Format(Message.TellHowMuchBetTotal, m_Player.TotalBet));
                    HitCard();
                    break;
                case PlayerActionType.Stand:
                    Message.ShowMessage(string.Format(Message.PlayerActionStand, m_Player.HandTotal()));
                    drawDone = true;
                    break;
                }
                if (m_Player.IsBlackJack || m_Player.IsBurst)
                {
                    drawDone = true;
                }
            }
        }

        private void HitCard()
        {
            Message.ShowMessage(Message.DealPlayerCard);
            Card card = m_Deck.Deal();
            m_Player.AddHand(card);
            Message.ShowMessage(string.Format(Message.DealtCard, m_Player.HandCount() + 1));
            Message.ShowMessage(string.Format(Message.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
            Console.ReadKey();
            ShowPlayerCardTotal();
            if (m_Player.IsBlackJack)
            {
                Message.ShowMessage(string.Format(Message.ShowBlackJack));
            }
            if (m_Player.IsBurst)
            {
                Message.ShowMessage(string.Format(Message.ShowBurst));
            }
            Console.ReadKey();
        }

        private void DealerDraw()
        {
            Message.ShowMessage(Message.DealDealerCard);
            Card card = m_Deck.Deal();
            m_Dealer.AddHand(card);
            Message.ShowMessage(string.Format(Message.DealtCard, m_Dealer.HandCount() + 1));
            Message.ShowMessage(string.Format(Message.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
            if (m_Dealer.IsBlackJack)
            {
                Message.ShowMessage(string.Format(Message.ShowBlackJack));
                return;
            }
            Console.ReadKey();
            ShowDealerCardTotal();
            Console.ReadKey();
            bool drawDone = false;
            while (!drawDone)
            {
                if (m_Dealer.HandTotal() >= 17)
                {
                    Message.ShowMessage(Message.DealerStopDraw);
                    drawDone = true;
                    continue;
                }
                Message.ShowMessage(Message.DealDealerCard);
                card = m_Deck.Deal();
                m_Dealer.AddHand(card);
                Message.ShowMessage(string.Format(Message.DealtCard, m_Dealer.HandCount()));
                Message.ShowMessage(string.Format(Message.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
                Console.ReadKey();
                ShowDealerCardTotal();
                if (m_Dealer.IsBlackJack)
                {
                    Message.ShowMessage(string.Format(Message.ShowBlackJack));
                }
                if (m_Dealer.IsBurst)
                {
                    Message.ShowMessage(string.Format(Message.ShowBurst));
                }
                Console.ReadKey();
            }
        }

        private ResultType CheckResult()
        {
            if (m_Player.IsBurst)
            {
                return ResultType.Lose;
            }
            if (m_Dealer.IsBurst)
            {
                return ResultType.Win;
            }
            if (m_Dealer.HandTotal() == m_Player.HandTotal())
            {
                return ResultType.Draw;
            }
            if (m_Dealer.HandTotal() > m_Player.HandTotal())
            {
                return ResultType.Lose;
            }
            if (m_Player.IsBlackJack)
            {
                return ResultType.BlackJack;
            }
            return ResultType.Win;
        }

        private void PayCredit(ResultType result)
        {
            // Bet単位を10ごとにしているけど、変更する場合はBlackJackのCaseの修正必要あり
            switch (result)
            {
            case ResultType.Win:
                {
                    int payback = m_Player.TotalBet;
                    Message.ShowMessage(string.Format(Message.PaybackWin, payback.ToString()));
                    m_Player.AddCredit(payback);
                }
                break;
            case ResultType.Lose:
                {
                    int payback = m_Player.TotalBet;
                    Message.ShowMessage(string.Format(Message.PaybackLose, payback.ToString()));
                    m_Player.AddCredit(-payback);
                }
                break;
            case ResultType.Draw:
                {
                    int payback = m_Player.TotalBet;
                    Message.ShowMessage(string.Format(Message.PaybackDraw, payback.ToString()));
                }
                break;
            case ResultType.BlackJack:
                {
                    int payback = (int)(m_Player.TotalBet * 1.5);
                    Message.ShowMessage(string.Format(Message.PaybackBlackJack, payback.ToString()));
                    m_Player.AddCredit(payback);
                }
                break;
            }
        }

        private bool AskContinue()
        {
            bool successInput = false;
            string? input = "";
            while (!successInput)
            {
                Message.ShowMessage(Message.AskContinue);
                input = Console.ReadLine();
                if (input == null || (input != "yes" && input != "no"))
                {
                    Message.ShowMessage(Message.ErrorAskContinue);
                    continue;
                }
                successInput = true;
            }
            return input == "yes";
        }

        private bool CheckPlayerLose()
        {
            if (m_Player.Credit < 100) { return true; }
            return false;
        }
    }
}