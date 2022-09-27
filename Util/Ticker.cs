namespace GravityPlatform.Util
{
    public class Ticker
    {
        public ulong Tick { get; private set; } = 0;
        public Ticker()
        {
            
        }

        public void NextTick()
            => Tick++;

        public bool Before(ulong other, ulong dist)
            => Tick - dist < other;

        public bool After(ulong other, ulong dist)
            => Tick - dist > other;

        public void Reset() => Tick = 0;
    }
}