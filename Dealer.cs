namespace Taimiso
{
    public class Dealer : Player
    {
        public Dealer() : base(0) { }
        public bool StopDraw()
        {
            return HandTotal() < 17;
        }
    }
}