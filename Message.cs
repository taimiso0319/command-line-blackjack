namespace Taimiso
{
    public static class Message
    {
        public static string GameOver = "Game Over!";
        public static string NoCredit = "賭けられるCreditがありません。";
        public static string AskBetAmount = "幾らBetしますか？";
        public static string TellHowMuchBet = "{0} creditsをBetしました。";
        public static string TellHowMuchBetTotal = "あなたは合計 {0} creditsをBetしています。";
        public static string TellHoldingCredit = "あなたは {0} credits持っています。";
        public static string TellMinimumBet = "最低Bet額は {0} です。";
        public static string TellMinimumBetUnit = "Bet単位は {0} ごとです。";
        public static string ErrorBetHasToBeDigit = "Bet額は半角数字で入力してください。";
        public static string ErrorBetNotEnough = "Bet額が足りません。最低額以上をBetしてください。";
        public static string ErrorBetTooMuch = "所持しているCredits以上のBetはできません。";
        public static string ErrorBetInvalidUnit = "Bet単位は {0} ごとです。正しい値を入力してください。";
        public static string DealPlayerCard = "プレイヤーにカードが配られます。";
        public static string DealDealerCard = "ディーラーにカードが配られます。";
        public static string DealtCard = "{0} 枚目のカードが配られました。";
        public static string ShowDealtCard = "カードは {0} の {1} です。";
        public static string ShowPlayerCardTotal = "現在のプレイヤーの合計は {0} です。";
        public static string ShowDealerCardTotal = "現在のディーラーの合計は {0} です。";
        public static string ShowBlackJack = "Black Jack!!";
        public static string ShowBurst = "Burst!!";
        public static string ShowNaturalBlackJack = "Natural Black Jack!!";
        public static string AskContinue = "続けますか？(yes/no):";
        public static string PaybackWin = "勝利したため {0} creditsを得ました。";
        public static string PaybackLose = "敗北したため {0} creditsを失いました。";
        public static string PaybackDraw = "引き分けしたため {0} creditsが返還されました";
        public static string PaybackBlackJack = "BlackJackで勝利したため {0} creditsを得ました。";
        public static string ErrorAskContinue = "yesかnoを入力して下さい。";
        public static string AskPlayerAction = "どうしますか？ (0:hit, 1:double, 2:stand)";
        public static string PlayerActionStand = "Standを選択しました。";
        public static string ErrorAskPlayerAction = "入力された文字が不正です。半角数字で入力してください。";
        public static string ErrorAskPlayerActionDouble = "Creditsが足りないためDoubleできません。";
        public static string DealerStopDraw = "ディーラーの合計が17以上に達したため、ドローを終了します。";
        public static string DealerContinueDraw = "ディーラーの合計が17に達していないため、ドローを継続します。";
        public static string GameOverResult = "{0} creditsで終了!";
        public static string CountAceAsOne = "Aを1としてカウントしました。";
        public static string ListingCards = "カード一覧: {0}";

        public static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}