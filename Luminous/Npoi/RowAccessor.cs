using Luminous.Npoi;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Luminous.Npoi
{
    public class RowAccessor
    {
        private readonly NpoiObject _npoi;
        private readonly ISheet _sheet;
        private readonly Dictionary<int, IRowObject> _rows = new Dictionary<int, IRowObject>();

        public RowAccessor(NpoiObject npoi, ISheet sheet)
        {
            _npoi = npoi;
            _sheet = sheet;
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

                rowBuilder = new RowObject(_npoi, row, rowIndex);

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
