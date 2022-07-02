namespace HighlightsParser.Infrastructure.Services
{
    public interface ITimeProvider
    {
        DateTime GetCurrentDate();
    }

    public class TimeProvider : ITimeProvider
    {
        public DateTime GetCurrentDate()
        {
            return DateTime.Today;
        }
    }
}
