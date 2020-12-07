using System;

namespace SoundMastery.Application.Common
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset GetUtcNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}
