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
                    ShowMessage(Messages.NoCredit);
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
                    ShowMessage(Messages.ShowNaturalBlackJack);
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
            ShowMessage(Messages.GameOver);
        }

        private void AskBet()
        {
            bool betSuccess = false;
            while (!betSuccess)
            {
                ShowMessage(string.Format(Messages.TellHoldingCredit, m_Player.Credit));
                ShowMessage(Messages.AskBetAmount);
                ShowMessage(string.Format(Messages.TellMinimumBet, MINIMUM_BET));
                ShowMessage(string.Format(Messages.TellMinimumBetUnit, BET_UNIT));
                int amount = 0;
                if (!Int32.TryParse(Console.ReadLine(), out amount))
                {
                    ShowMessage(Messages.ErrorBetHasToBeDigit);
                    continue;
                }
                if (amount % BET_UNIT != 0)
                {
                    ShowMessage(string.Format(Messages.ErrorBetInvalidUnit, BET_UNIT));
                    continue;
                }
                if (amount < MINIMUM_BET)
                {
                    ShowMessage(Messages.ErrorBetNotEnough);
                    continue;
                }
                if (m_Player.Credit < amount)
                {
                    ShowMessage(Messages.ErrorBetTooMuch);
                    continue;
                }
                ShowMessage(string.Format(Messages.TellHowMuchBet, amount));
                m_Player.SetPlayerInitialBet(amount);
                betSuccess = true;
            }
            Console.ReadKey();
        }

        private void DealDealerCard()
        {
            ShowMessage(Messages.DealDealerCard);
            Card card = m_Deck.Deal();
            m_Dealer.AddHand(card);
            ShowMessage(string.Format(Messages.DealtCard, m_Dealer.HandCount()));
            ShowMessage(string.Format(Messages.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
            Console.ReadKey();
        }

        private void DealPlayerFirstCard()
        {
            ShowMessage(Messages.DealPlayerCard);
            Card card = m_Deck.Deal();
            m_Player.AddHand(card);
            ShowMessage(string.Format(Messages.DealtCard, m_Player.HandCount()));
            ShowMessage(string.Format(Messages.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
            Console.ReadKey();
        }

        private void DealDealerFirstCard()
        {
            DealDealerCard();
        }

        private void DealPlayerSecondCard()
        {
            ShowMessage(Messages.DealPlayerCard);
            Card card = m_Deck.Deal();
            m_Player.AddHand(card);
            ShowMessage(string.Format(Messages.DealtCard, m_Player.HandCount()));
            ShowMessage(string.Format(Messages.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
            Console.ReadKey();
        }

        private void ShowPlayerCardTotal()
        {
            ShowMessage(string.Format(Messages.ShowPlayerCardTotal, m_Player.HandTotal()));
        }

        private void ShowDealerCardTotal()
        {
            ShowMessage(string.Format(Messages.ShowDealerCardTotal, m_Dealer.HandTotal()));
        }

        private void PlayerDraw()
        {
            bool drawDone = false;
            while (!drawDone)
            {
                ShowMessage(Messages.AskPlayerAction);
                int action = 0;
                if (!Int32.TryParse(Console.ReadLine(), out action))
                {
                    ShowMessage(Messages.ErrorAskPlayerAction);
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
                        ShowMessage(Messages.ErrorAskPlayerActionDouble);
                        ShowMessage(string.Format(Messages.TellHoldingCredit, m_Player.Credit));
                        ShowMessage(string.Format(Messages.TellHowMuchBetTotal, m_Player.TotalBet));
                        continue;
                    }
                    ShowMessage(string.Format(Messages.TellHowMuchBet, m_Player.TotalBet));
                    m_Player.DoubleBet();
                    ShowMessage(string.Format(Messages.TellHowMuchBetTotal, m_Player.TotalBet));
                    HitCard();
                    break;
                case PlayerActionType.Stand:
                    ShowMessage(string.Format(Messages.PlayerActionStand, m_Player.HandTotal()));
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
            ShowMessage(Messages.DealPlayerCard);
            Card card = m_Deck.Deal();
            m_Player.AddHand(card);
            ShowMessage(string.Format(Messages.DealtCard, m_Player.HandCount() + 1));
            ShowMessage(string.Format(Messages.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
            Console.ReadKey();
            ShowPlayerCardTotal();
            if (m_Player.IsBlackJack)
            {
                ShowMessage(string.Format(Messages.ShowBlackJack));
            }
            if (m_Player.IsBurst)
            {
                ShowMessage(string.Format(Messages.ShowBurst));
            }
            Console.ReadKey();
        }

        private void DealerDraw()
        {
            ShowMessage(Messages.DealDealerCard);
            Card card = m_Deck.Deal();
            m_Dealer.AddHand(card);
            ShowMessage(string.Format(Messages.DealtCard, m_Dealer.HandCount() + 1));
            ShowMessage(string.Format(Messages.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
            if (m_Dealer.IsBlackJack)
            {
                ShowMessage(string.Format(Messages.ShowBlackJack));
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
                    ShowMessage(Messages.DealerStopDraw);
                    drawDone = true;
                    continue;
                }
                ShowMessage(Messages.DealDealerCard);
                card = m_Deck.Deal();
                m_Dealer.AddHand(card);
                ShowMessage(string.Format(Messages.DealtCard, m_Dealer.HandCount()));
                ShowMessage(string.Format(Messages.ShowDealtCard, card.Suit.ToString(), card.GetNumberString()));
                Console.ReadKey();
                ShowDealerCardTotal();
                if (m_Dealer.IsBlackJack)
                {
                    ShowMessage(string.Format(Messages.ShowBlackJack));
                }
                if (m_Dealer.IsBurst)
                {
                    ShowMessage(string.Format(Messages.ShowBurst));
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
                    ShowMessage(string.Format(Messages.PaybackWin, payback.ToString()));
                    m_Player.AddCredit(payback);
                }
                break;
            case ResultType.Lose:
                {
                    int payback = m_Player.TotalBet;
                    ShowMessage(string.Format(Messages.PaybackLose, payback.ToString()));
                    m_Player.AddCredit(-payback);
                }
                break;
            case ResultType.Draw:
                {
                    int payback = m_Player.TotalBet;
                    ShowMessage(string.Format(Messages.PaybackDraw, payback.ToString()));
                }
                break;
            case ResultType.BlackJack:
                {
                    int payback = (int)(m_Player.TotalBet * 1.5);
                    ShowMessage(string.Format(Messages.PaybackBlackJack, payback.ToString()));
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
                ShowMessage(Messages.AskContinue);
                input = Console.ReadLine();
                if (input == null || (input != "yes" && input != "no"))
                {
                    ShowMessage(Messages.ErrorAskContinue);
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

        private void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}