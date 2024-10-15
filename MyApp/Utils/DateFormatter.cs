namespace MyApp.Utils
{
    public static class DateFormatter
    {
        public static string FormatDate(DateOnly? date)
        {
            return String.Format($"{date:dd MMM yyyy}");
        }

        public static string FormatDate(DateTime? date)
        {
            return String.Format($"{date?.Date:dd MMM yyyy}");
        }
    }
}
