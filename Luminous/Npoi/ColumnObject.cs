using Luminous;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Luminous
{

    public class ColumnObject : IColumnObject
    {
        private readonly ISheet _sheet;
        private readonly int _columnIndex;
        private readonly RowAccessor _rowAccesser;
        private readonly Dictionary<int, ICellObject> _cells = new Dictionary<int, ICellObject>();
        private readonly Dictionary<int, IRow> _rows = new Dictionary<int, IRow>();

        public ColumnObject(ISheet sheet, int columnIndex, RowAccessor rowAccesser)
        {
            _sheet = sheet;
            _columnIndex = columnIndex;
            _rowAccesser = rowAccesser;
        }

        public ICellObject this[int rowIndex]
        {
            get
            {
                return _rowAccesser[rowIndex][_columnIndex];
            }
        }

        public string[] GetValues()
        {
            var rowBuilders = _rowAccesser.GetRows();

            var values = new string[rowBuilders.Length];

            for (int i = 0; i < rowBuilders.Length; i++)
            {
                values[i] = rowBuilders[i][_columnIndex].ToString();
            }

            return values;
        }
    }
}
