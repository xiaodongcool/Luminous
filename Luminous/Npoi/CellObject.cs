using Luminous;
using NPOI.SS.UserModel;
using System;

namespace Luminous
{
    public class CellObject : ICellObject
    {
        private readonly INpoiObject _npoiObject;
        private readonly ISheetObject _sheetObject;
        private readonly ICell _cell;

        public CellObject(ISheetObject sheetObject, ICell cell)
        {
            _sheetObject = sheetObject;
            _npoiObject = sheetObject.Npoi;
            _cell = cell;
        }

        public string Value
        {
            get
            {
                return _sheetObject.Reader.String(_cell.RowIndex, _cell.ColumnIndex, _cell.ToString());
            }
            set
            {
                _cell.SetCellValue(value);
            }
        }

        public byte? ByteValue
        {
            get
            {
                return _sheetObject.Reader.Byte(_cell.RowIndex, _cell.ColumnIndex, Value);
            }
            set
            {
                if (value.HasValue)
                {
                    _cell.SetCellValue(value.Value);
                }
                else
                {
                    _cell.SetCellValue(string.Empty);
                }
            }
        }

        public int? Int32Value
        {
            get
            {
                return _sheetObject.Reader.Int32(_cell.RowIndex, _cell.ColumnIndex, Value);
            }
            set
            {
                if (value.HasValue)
                {
                    _cell.SetCellValue(value.Value);
                }
                else
                {
                    _cell.SetCellValue(string.Empty);
                }
            }
        }

        public long? Int64Value
        {
            get
            {
                return _sheetObject.Reader.Int64(_cell.RowIndex, _cell.ColumnIndex, Value);
            }
            set
            {
                if (value.HasValue)
                {
                    _cell.SetCellValue(value.Value);
                }
                else
                {
                    _cell.SetCellValue(string.Empty);
                }
            }
        }

        public double? DoubleValue
        {
            get
            {
                return _sheetObject.Reader.Double(_cell.RowIndex, _cell.ColumnIndex, Value);
            }
            set
            {
                if (value.HasValue)
                {
                    _cell.SetCellValue(value.Value);
                }
                else
                {
                    _cell.SetCellValue(string.Empty);
                }
            }
        }

        public float? SingleValue
        {
            get
            {
                return _sheetObject.Reader.Single(_cell.RowIndex, _cell.ColumnIndex, Value);
            }
            set
            {
                if (value.HasValue)
                {
                    _cell.SetCellValue(value.Value);
                }
                else
                {
                    _cell.SetCellValue(string.Empty);
                }
            }
        }

        public decimal? DecimalValue
        {
            get
            {
                return _sheetObject.Reader.Decimal(_cell.RowIndex, _cell.ColumnIndex, Value);
            }
            set
            {
                if (value.HasValue)
                {
                    _cell.SetCellValue((double)value.Value);
                }
                else
                {
                    _cell.SetCellValue(string.Empty);
                }
            }
        }

        public bool? BooleanValue
        {
            get
            {
                return _sheetObject.Reader.Boolean(_cell.RowIndex, _cell.ColumnIndex, Value);
            }
            set
            {
                if (value.HasValue)
                {
                    _cell.SetCellValue(value.Value);
                }
                else
                {
                    _cell.SetCellValue(string.Empty);
                }
            }
        }

        public DateTime? DateTimeValue
        {
            get
            {
                return _sheetObject.Reader.DateTime(_cell.RowIndex, _cell.ColumnIndex, Value);
            }
            set
            {
                if (value.HasValue)
                {
                    _cell.SetCellValue(value.Value);
                }
                else
                {
                    _cell.SetCellValue(string.Empty);
                }
            }
        }

        public ICellStyle Style
        {
            get
            {
                return _cell.CellStyle;
            }
            set
            {
                HasStyle = true;

                _cell.CellStyle = value;
            }
        }

        public bool HasStyle { get; private set; }

        public short Color
        {
            get
            {
                return Font.Color;
            }
            set
            {
                SetFontStyle(x =>
                {
                    x.Color = value;
                });
            }
        }

        public bool IsBold
        {
            get
            {
                return Font.IsBold;
            }
            set
            {
                SetFontStyle(x =>
                {
                    x.IsBold = value;
                });
            }
        }

        public bool IsItalic
        {
            get
            {
                return Font.IsItalic;
            }
            set
            {
                SetFontStyle(x =>
                {
                    x.IsItalic = value;
                });
            }
        }

        public bool IsStrikeout
        {
            get
            {
                return Font.IsStrikeout;
            }
            set
            {
                SetFontStyle(x =>
                {
                    x.IsStrikeout = value;
                });
            }
        }

        public FontUnderlineType Underline
        {
            get
            {
                return Font.Underline;
            }
            set
            {
                SetFontStyle(x =>
                {
                    x.Underline = value;
                });
            }
        }

        public double FontHeightInPoints
        {
            get
            {
                return Font.FontHeightInPoints;
            }
            set
            {
                SetFontStyle(x =>
                {
                    x.FontHeightInPoints = value;
                });
            }
        }

        public string FontName
        {
            get
            {
                return Font.FontName;
            }
            set
            {
                SetFontStyle(x =>
                {
                    x.FontName = value;
                });
            }
        }

        public short FillForegroundColor
        {
            get
            {
                return Style.FillForegroundColor;
            }
            set
            {
                SetStyle(x =>
                {
                    x.FillForegroundColor = value;
                    x.FillPattern = FillPattern.SolidForeground;
                });
            }
        }

        public BorderStyle BorderStyle
        {
            set
            {
                SetStyle(x =>
                {
                    x.BorderLeft = value;
                    x.BorderRight = value;
                    x.BorderTop = value;
                    x.BorderBottom = value;
                });
            }
        }

        public short BorderColor
        {
            set
            {
                SetStyle(x =>
                {
                    x.LeftBorderColor = value;
                    x.RightBorderColor = value;
                    x.TopBorderColor = value;
                    x.BottomBorderColor = value;
                });

            }
        }

        public HorizontalAlignment Alignment
        {
            get
            {
                return Style.Alignment;
            }
            set
            {
                SetStyle(x => { x.Alignment = value; });
            }
        }

        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return Style.VerticalAlignment;
            }
            set
            {
                SetStyle(x => { x.VerticalAlignment = value; });
            }
        }

        public IFont Font => _npoiObject.GetFont(Style);

        public void SetFontStyle(Action<IFont> setFontFunc)
        {
            var newStyle = _npoiObject.CreateDefaultStyle(_sheetObject.DefaultCellStyle);
            newStyle.CloneStyleFrom(Style);

            var newFont = _npoiObject.CreateFont();

            var previousFont = Font;

            newFont.FontName = previousFont.FontName;
            newFont.FontHeightInPoints = previousFont.FontHeightInPoints;
            newFont.IsBold = previousFont.IsBold;
            newFont.Color = previousFont.Color;
            newFont.IsItalic = previousFont.IsItalic;
            newFont.IsStrikeout = previousFont.IsStrikeout;
            newFont.Underline = previousFont.Underline;

            setFontFunc(newFont);

            newStyle.SetFont(newFont);
            Style = newStyle;

            HasStyle = true;
        }

        public void SetStyle(Action<ICellStyle> setStyleFunc)
        {
            setStyleFunc(Style);

            HasStyle = true;
        }

        public override string ToString()
        {
            return _cell.ToString();
        }
    }
}
