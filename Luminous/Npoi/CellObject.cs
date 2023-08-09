using Luminous.Npoi;
using NPOI.SS.UserModel;
using System;

namespace Luminous.Npoi
{
    public class CellObject : ICellObject
    {
        private readonly NpoiObject _npoi;
        private readonly ICell _cell;

        public CellObject(NpoiObject npoi, ICell cell)
        {
            _npoi = npoi;
            _cell = cell;
        }

        public string Value
        {
            get
            {
                return ToString();
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
                return Convert.ToByte(Value);
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

        public int? IntValue
        {
            get
            {
                return Convert.ToInt32(Value);
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

        public long? LongValue
        {
            get
            {
                return Convert.ToInt64(Value);
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
                return Convert.ToDouble(Value);
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

        public float? FloatValue
        {
            get
            {
                return Convert.ToSingle(Value);
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
                return Convert.ToDecimal(Value);
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

        public bool? BoolValue
        {
            get
            {
                return Convert.ToBoolean(Value);
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

        public IRichTextString RichStringCellValue
        {
            get
            {
                return _cell.RichStringCellValue;
            }
            set
            {
                _cell.SetCellValue(value);
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
                _cell.CellStyle = value;
            }
        }

        public bool HasStyle { get; set; }

        public short Color
        {
            get
            {
                return Font.Color;
            }
            set
            {
                UpdateFontStyle(x =>
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
                UpdateFontStyle(x =>
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
                UpdateFontStyle(x =>
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
                UpdateFontStyle(x =>
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
                UpdateFontStyle(x =>
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
                UpdateFontStyle(x =>
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
                UpdateFontStyle(x =>
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
                UpdateStyle(x =>
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
                UpdateStyle(x =>
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
                UpdateStyle(x =>
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
                UpdateStyle(x => { x.Alignment = value; });
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
                UpdateStyle(x => { x.VerticalAlignment = value; });
            }
        }

        private IFont Font => Style.GetFont(_npoi.Book);

        private void UpdateFontStyle(Action<IFont> updateFunc)
        {
            var style = _npoi.CreateDefaultStyle();
            style.CloneStyleFrom(Style);
            var font = _npoi.Book.CreateFont();

            font.FontName = FontName;
            font.FontHeightInPoints = FontHeightInPoints;
            font.IsBold = IsBold;
            font.Color = Color;
            font.IsItalic = IsItalic;
            font.IsStrikeout = IsStrikeout;
            font.Underline = Underline;

            if (updateFunc != null)
            {
                HasStyle = true;

                updateFunc(font);
            }

            style.SetFont(font);
            Style = style;
        }

        private void UpdateStyle(Action<ICellStyle> updateFunc)
        {
            if (updateFunc != null)
            {
                HasStyle = true;

                updateFunc(Style);
            }
        }

        public override string ToString()
        {
            return _cell.ToString();
        }
    }
}
