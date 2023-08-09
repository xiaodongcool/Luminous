using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Luminous.Npoi
{
    public interface INpoiObject
    {
        XSSFWorkbook Book { get; }

        ICellStyle CreateDefaultStyle();
        ISheetObject CreateSheet();
        ISheetObject GetOrCreateSheet(string sheetName);
        void Save(string physicalFilePath);
        void Save(string physicalFilePath, bool applyStyleToMergedCells, bool setDefaultCellStyle);
        void SetDefaultCellStyle();
    }
}
