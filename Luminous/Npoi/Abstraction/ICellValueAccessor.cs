namespace Luminous.Npoi
{
    public interface ICellValueAccessor
    {
        string Value { get; set; }
        byte? ByteValue { get; set; }
        int? IntValue { get; set; }
        long? LongValue { get; set; }
        double? DoubleValue { get; set; }
        float? FloatValue { get; set; }
        decimal? DecimalValue { get; set; }
        bool? BoolValue { get; set; }
    }
}
