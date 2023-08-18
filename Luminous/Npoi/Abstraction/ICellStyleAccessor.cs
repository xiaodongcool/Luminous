using NPOI.SS.UserModel;

namespace Luminous
{
    public interface ICellStyleAccessor
    {
        ICellStyle Style { get; set; }
        bool HasStyle { get; }
        short Color { get; set; }
        bool IsBold { get; set; }
        bool IsItalic { get; set; }
        bool IsStrikeout { get; set; }
        FontUnderlineType Underline { get; set; }
        double FontHeightInPoints { get; set; }
        string FontName { get; set; }
        short FillForegroundColor { get; set; }
        BorderStyle BorderStyle { set; }
        short BorderColor { set; }
        HorizontalAlignment Alignment { get; set; }
        VerticalAlignment VerticalAlignment { get; set; }
    }
}
