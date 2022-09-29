namespace GravityPlatform.Util
{
    public static class Extensions {
        public static void Disable(this ref ulong? a)
        {
            unsafe
            {
                fixed (ulong?* k = &a)
                {
                    *k = null;
                }
            }
        }
    }
}