using System;

namespace SoundMastery.Application.Common;

public interface IDateTimeProvider
{
    DateTimeOffset GetUtcNow();
}