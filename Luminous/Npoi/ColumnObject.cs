using Luminous.Npoi;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Luminous.Npoi
{

    public class ColumnObject : IColumnObject
    {
        private readonly NpoiObject _npoi;
        private readonly int _columnIndex;
        private readonly RowAccessor _rowAccesser;
        private readonly Dictionary<int, ICellObject> _cells = new Dictionary<int, ICellObject>();
        private readonly Dictionary<int, IRow> _rows = new Dictionary<int, IRow>();

        public ColumnObject(NpoiObject npoi, int columnIndex, RowAccessor rowAccesser)
        {
            _npoi = npoi;
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
