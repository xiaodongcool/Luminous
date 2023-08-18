using System;
using Luminous;

namespace Luminous
{
    public class DefaultCellValueReader : ICellValueReader
    {
        public virtual bool? Boolean(int rowIndex, int columnIndex, string value)
        {
            if (bool.TryParse(value, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public virtual byte? Byte(int rowIndex, int columnIndex, string value)
        {
            if (byte.TryParse(value, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public virtual decimal? Decimal(int rowIndex, int columnIndex, string value)
        {
            if (decimal.TryParse(value, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public virtual double? Double(int rowIndex, int columnIndex, string value)
        {
            if (double.TryParse(value, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public virtual float? Single(int rowIndex, int columnIndex, string value)
        {
            if (float.TryParse(value, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public virtual int? Int32(int rowIndex, int columnIndex, string value)
        {
            if (int.TryParse(value, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public virtual long? Int64(int rowIndex, int columnIndex, string value)
        {
            if (long.TryParse(value, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public virtual string ToString(int rowIndex, int columnIndex, string value)
        {
            return value;
        }

        public virtual DateTime? DateTime(int rowIndex, int columnIndex, string value)
        {
            if (System.DateTime.TryParse(value, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public virtual string String(int rowIndex, int columnIndex, string value)
        {
            return value;
        }
    }
}
