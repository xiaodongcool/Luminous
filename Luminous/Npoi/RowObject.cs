using Luminous;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Luminous
{
    public class RowObject : IRowObject
    {
        private readonly ISheetObject _sheetObject;
        private readonly IRow _row;
        private readonly int _rowIndex;
        private readonly Dictionary<int, ICellObject> _cells = new Dictionary<int, ICellObject>();

        public RowObject(ISheetObject sheetObject, IRow row, int rowIndex)
        {
            _sheetObject = sheetObject;
            _row = row;
            _rowIndex = rowIndex;
        }

        public ICellObject this[int columnIndex]
        {
            get
            {
                if (_cells.TryGetValue(columnIndex, out var cellBuilder))
                {
                    return cellBuilder;
                }

                var cell = _row.GetCell(columnIndex);

                if (cell == null)
                {
                    cell = _row.CreateCell(columnIndex);
                }

                cellBuilder = new CellObject(_sheetObject, cell);

                _cells[columnIndex] = cellBuilder;

                return cellBuilder;
            }
        }

        public string[] GetValues()
        {
            var values = new string[_row.LastCellNum];
            for (var i = 0; i < _row.LastCellNum; i++)
            {
                var cell = _row.GetCell(i);

                if (cell != null)
                {
                    values[i] = cell.ToString();
                }
            }

            return values;
        }
    }
}
