using Luminous;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Luminous
{
    public class RowAccessor
    {
        private readonly ISheet _sheet;
        private readonly ISheetObject _sheetObject;
        private readonly Dictionary<int, IRowObject> _rows = new Dictionary<int, IRowObject>();

        public RowAccessor(ISheet sheet, ISheetObject sheetObject)
        {
            _sheet = sheet;
            _sheetObject = sheetObject;
        }

        public IRowObject this[int rowIndex]
        {
            get
            {
                if (_rows.TryGetValue(rowIndex, out var rowBuilder))
                {
                    return rowBuilder;
                }

                var row = _sheet.GetRow(rowIndex);
                if (row == null)
                {
                    row = _sheet.CreateRow(rowIndex);
                }

                rowBuilder = new RowObject(_sheetObject, row, rowIndex);

                _rows[rowIndex] = rowBuilder;

                return rowBuilder;
            }
        }

        public IRowObject[] GetRows()
        {
            var rowBuilders = new IRowObject[_sheet.LastRowNum + 1];

            for (int i = 0; i <= _sheet.LastRowNum; i++)
            {
                rowBuilders[i] = this[i];
            }

            return rowBuilders;
        }
    }
}
