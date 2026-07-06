namespace NemoUtility
{
    public static class TimeUtility
    {
        public static string GetTime(int second)
        {
            int hour = second / 3600;
            int min = second / 60;
            int sec = second % 60;
            return ((hour > 0) ? hour.ToString("00") + ":" : "") + min.ToString("00") + ":" + sec.ToString("00");
        }
    }
}

