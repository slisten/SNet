namespace Common
{
    public static class UniqueIdHelper
    {
        private static int defaultId = 10000;
        public static int CreateId()
        {
            return defaultId++;
        }
    }
}