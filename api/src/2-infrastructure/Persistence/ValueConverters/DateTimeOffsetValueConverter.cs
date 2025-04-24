using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SplitTheBill.Persistence.ValueConverters;

/// <summary>
/// This value converter will convert localised DateTimeOffset values to UTC when writing and
/// return UTC DateTimeOffset values when reading
/// Since Postgres by default only supports storing UTC values, it's useful to apply this by
/// convention so you can still use DateTimeOffset throughout the application layer without
/// having to worry about conversions
/// </summary>
internal sealed class DateTimeOffsetValueConverter : ValueConverter<DateTimeOffset, DateTime>
{
    public DateTimeOffsetValueConverter()
        : base(
            dateTimeOffset => dateTimeOffset.UtcDateTime,
            dateTime => new DateTimeOffset(dateTime, TimeSpan.Zero)
        ) { }
}
