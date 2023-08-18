using Luminous;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Text.RegularExpressions;

namespace Luminous
{
    public class SheetObject : ISheetObject
    {
        private readonly ISheet _sheet;
        private readonly RowAccessor _rowAccesser;
        private readonly ColumnAccessor _columnAccesser;
        private const int DEFAULT_COLUMN_WIDTH = 2048;

        private ICellValueReader _convert;

        public SheetObject(NpoiObject npoi, ISheet sheet)
        {
            Npoi = npoi;
            _sheet = sheet;
            _rowAccesser = new RowAccessor(sheet, this);
            _columnAccesser = new ColumnAccessor(sheet, _rowAccesser);
        }

        public RowAccessor Row => _rowAccesser;
        public ColumnAccessor Column => _columnAccesser;

        public IRowObject this[int rowIndex] => _rowAccesser[rowIndex];

        public void Merge(int beginRow, int endRow, int beginColumn, int endColumn)
        {
            _sheet.AddMergedRegion(new CellRangeAddress(beginRow, endRow, beginColumn, endColumn));
        }

        public void RecursivelyAddMutilHeaders(MutilHeader[] headers, int rowIndex = 0, int columnIndex = 0)
        {
            var currentRowIndex = rowIndex;
            var currentColumnIndex = columnIndex;

            foreach (var header in headers)
            {
                var count = GetOccupyColumnCount(header);

                this[currentRowIndex][currentColumnIndex].Value = header.Title;

                if (header.WidthMultiples > 1)
                {
                    _sheet.SetColumnWidth(currentColumnIndex, (int)Math.Ceiling((header.WidthMultiples * DEFAULT_COLUMN_WIDTH)));
                }
                else if (SelfAdaptionColumnWidth)
                {
                    _sheet.SetColumnWidth(currentColumnIndex, FigureUpWidthMultiples(header.Title) * DEFAULT_COLUMN_WIDTH);
                }

                header.CrossRowCount = header.CrossRowCount <= 0 ? 1 : header.CrossRowCount;

                if (count > 1 || header.CrossRowCount > 1)
                {
                    var begRowIndex = currentRowIndex;
                    var endRowIndex = currentRowIndex + header.CrossRowCount - 1;

                    var begColumnIndex = currentColumnIndex;
                    var endColumnIndex = currentColumnIndex + count - 1;

                    Merge(begRowIndex, endRowIndex, begColumnIndex, endColumnIndex);
                }

                RecursivelyAddMutilHeaders(header.Children.ToArray(), currentRowIndex + header.CrossRowCount, currentColumnIndex);

                currentColumnIndex += count;
            }
        }

        public void ApplyStyleToMergedCells()
        {
            for (var i = 0; i < _sheet.NumMergedRegions; i++)
            {
                var mergedRegion = _sheet.GetMergedRegion(i);
                var startRowIndex = mergedRegion.FirstRow;
                var endRowIndex = mergedRegion.LastRow; var beginColumnIndex = mergedRegion.FirstColumn;
                var endColumnIndex = mergedRegion.LastColumn;

                for (var rowIndex = startRowIndex; rowIndex <= endRowIndex; rowIndex++)
                {
                    var currentRow = _sheet.GetRow(rowIndex) ?? _sheet.CreateRow(rowIndex);

                    for (var columnIndex = beginColumnIndex; columnIndex <= endColumnIndex; columnIndex++)
                    {
                        var currentCell = currentRow.GetCell(columnIndex) ?? currentRow.CreateCell(columnIndex);
                        //currentCell.CellStyle = _npoi.CreateDefaultStyle();
                    }
                }
            }
        }

        public void SetColumnWidthMultiples(int columnIndex, double multiples)
        {
            _sheet.SetColumnWidth(columnIndex, (int)Math.Ceiling(multiples * DEFAULT_COLUMN_WIDTH));
        }

        public bool SelfAdaptionColumnWidth { get; set; } = true;
        public int MaxColumnWidthMultiples { get; set; } = 2;

        public string SheetName => _sheet.SheetName;

        public ICellValueReader Reader
        {
            get
            {
                if (_convert == null)
                {
                    _convert = Npoi.Reader;
                }

                return _convert;
            }
            set
            {
                _convert = value;
            }
        }

        public INpoiObject Npoi { get; }
        public IDefaultCellStyle DefaultCellStyle { get; set; }

        public int GetMaxDataRow()
        {
            var lastRowNum = _sheet.LastRowNum;

            for (var row = lastRowNum; row >= 0; row--)
            {
                var currentRow = _sheet.GetRow(row);
                if (currentRow != null)
                {
                    foreach (var cell in currentRow)
                    {
                        if (!string.IsNullOrEmpty(cell.ToString()))
                        {
                            return row;
                        }
                    }
                }
            }

            return -1;
        }

        public int GetMaxDataColumn()
        {
            var maxDataColumn = 0;

            foreach (IRow row in _sheet)
            {
                var lastCellNum = row.LastCellNum;

                for (var col = lastCellNum - 1; col >= 0; col--)
                {
                    var cell = row.GetCell(col, MissingCellPolicy.RETURN_BLANK_AS_NULL);
                    if (cell != null)
                    {
                        if (!string.IsNullOrEmpty(cell.ToString()))
                        {
                            maxDataColumn = Math.Max(maxDataColumn, col);
                            break;
                        }
                    }
                }
            }

            return maxDataColumn;
        }

        private int FigureUpWidthMultiples(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return 1;
            }

            float cellCharCount;

            if (ContainsChineseCharacters(content))
            {
                cellCharCount = 4.0f;
            }
            else
            {
                cellCharCount = 8.0f;
            }

            return Math.Min(MaxColumnWidthMultiples, (int)Math.Ceiling(content.Length / cellCharCount));
        }
        private int GetOccupyColumnCount(MutilHeader header)
        {
            if (header == null)
            {
                return 0;
            }

            if (header.Children.Count == 0)
            {
                return 1;
            }

            var sum = 0;

            foreach (var child in header.Children)
            {
                sum += GetOccupyColumnCount(child);
            }

            return sum;
        }
        private bool ContainsChineseCharacters(string input)
        {
            var pattern = @"[\u4e00-\u9fa5]+";
            return Regex.IsMatch(input, pattern);
        }
    }
}
