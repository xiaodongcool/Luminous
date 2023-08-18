using Luminous;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Luminous
{
    public class ColumnAccessor
    {
        private readonly ISheet _sheet;
        private readonly RowAccessor _rowAccesser;
        private readonly Dictionary<int, IColumnObject> _rows = new Dictionary<int, IColumnObject>();

        public ColumnAccessor(ISheet sheet, RowAccessor rowAccesser)
        {
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

                columnBuilder = new ColumnObject(_sheet, columnIndex, _rowAccesser);

                _rows[columnIndex] = columnBuilder;

                return columnBuilder;
            }
        }
    }
}
