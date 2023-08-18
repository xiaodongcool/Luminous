using System;

namespace Luminous
{
    public interface ICellValueReader
    {
        string? String(int rowIndex, int columnIndex, string value);
        byte? Byte(int rowIndex, int columnIndex, string value);
        int? Int32(int rowIndex, int columnIndex, string value);
        long? Int64(int rowIndex, int columnIndex, string value);
        float? Single(int rowIndex, int columnIndex, string value);
        double? Double(int rowIndex, int columnIndex, string value);
        decimal? Decimal(int rowIndex, int columnIndex, string value);
        bool? Boolean(int rowIndex, int columnIndex, string value);
        DateTime? DateTime(int rowIndex, int columnIndex, string value);
    }
}
