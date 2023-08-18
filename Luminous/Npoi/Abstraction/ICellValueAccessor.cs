using System;

namespace Luminous
{
    public interface ICellValueAccessor
    {
        string Value { get; set; }
        byte? ByteValue { get; set; }
        int? Int32Value { get; set; }
        long? Int64Value { get; set; }
        double? DoubleValue { get; set; }
        float? SingleValue { get; set; }
        decimal? DecimalValue { get; set; }
        bool? BooleanValue { get; set; }
        DateTime? DateTimeValue { get; set; }
    }
}
