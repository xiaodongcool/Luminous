using Luminous;
using NPOI.SS.UserModel;

namespace Luminous
{
    public class DefaultCellStyle : IDefaultCellStyle
    {
        public void SetDefaultCellStyle(ICellStyle style)
        {
            style.WrapText = true;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
