using Luminous.Npoi;

namespace Luminous.Npoi
{
    public interface ISheetObject
    {
        IRowObject this[int rowIndex] { get; }
        RowAccessor Row { get; }
        ColumnAccessor Column { get; }
        bool SelfAdaptionColumnWidth { get; set; }
        int MaxColumnMultiples { get; set; }
        string SheetName { get; }
        void ApplyStyleToMergedCells();
        int GetMaxDataColumn();
        int GetMaxDataRow();
        void Merge(int beginRow, int endRow, int beginColumn, int endColumn);
        void RecursivelyAddMutilHeaders(MutilHeader[] headers, int rowIndex = 0, int columnIndex = 0);
        void SetColumnWidthMultiples(int columnIndex, double multiples);
    }
}
