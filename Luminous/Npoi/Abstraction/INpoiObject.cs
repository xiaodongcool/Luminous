using Luminous;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Luminous
{
    public interface INpoiObject
    {
        ICellValueReader Reader { get; set; }

        IDefaultCellStyle DefaultCellStyle { get; set; }

        ISheetObject CreateSheet();

        ISheetObject GetSheet(int sheetIndex);

        ISheetObject GetOrCreateSheet(string sheetName);

        ICellStyle CreateDefaultStyle(IDefaultCellStyle? defaultCellStyle = null);

        IFont CreateFont();

        IFont GetFont(ICellStyle style);

        void TraversalAllCellsSetDefaultStyle();

        void Save(string physicalFilePath);

        void Save(string physicalFilePath, bool applyStyleToMergedCells, bool setDefaultCellStyle);
    }
}
