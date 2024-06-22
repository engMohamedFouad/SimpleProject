namespace SimpleProject.Helpers
{
    public static class CultureHelper
    {
        public static bool IsRightToLeft()
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
        }
    }
}
