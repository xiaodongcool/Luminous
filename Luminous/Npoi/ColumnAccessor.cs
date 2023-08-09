using Luminous.Npoi;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Luminous.Npoi
{
    public class ColumnAccessor
    {
        private readonly NpoiObject _npoi;
        private readonly ISheet _sheet;
        private readonly RowAccessor _rowAccesser;
        private readonly Dictionary<int, IColumnObject> _rows = new Dictionary<int, IColumnObject>();

        public ColumnAccessor(NpoiObject npoi, ISheet sheet, RowAccessor rowAccesser)
        {
            _npoi = npoi;
            _sheet = sheet;
            _rowAccesser = rowAccesser;
        }

        public IColumnObject this[int columnIndex]
        {
            get
            {
                if (_rows.TryGetValue(columnIndex, out var columnBuilder))
                {
                    return columnBuilder;
                }

                columnBuilder = new ColumnObject(_npoi, columnIndex, _rowAccesser);

                _rows[columnIndex] = columnBuilder;

                return columnBuilder;
            }
        }
    }
}
