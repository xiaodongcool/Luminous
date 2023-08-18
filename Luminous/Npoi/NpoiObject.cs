using Luminous;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming.Values;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Luminous
{
    public class NpoiObject : INpoiObject
    {
        private readonly XSSFWorkbook _workBook;
        private readonly Dictionary<string, ISheetObject> _sheetObjects = new Dictionary<string, ISheetObject>();

        private NpoiObject(Stream stream, ICellValueReader cellValueReader, IDefaultCellStyle defaultCellStyle)
        {
            if (stream == null)
            {
                _workBook = new XSSFWorkbook();
            }
            else
            {
                _workBook = new XSSFWorkbook(stream);
            }

            Reader = cellValueReader ?? new DefaultCellValueReader();
            DefaultCellStyle = defaultCellStyle ?? new DefaultCellStyle();
        }

        public ICellValueReader Reader { get; set; }

        public IDefaultCellStyle DefaultCellStyle { get; set; }

        public ISheetObject CreateSheet()
        {
            var sheet = _workBook.CreateSheet();
            var sheetBuilder = new SheetObject(this, sheet);
            _sheetObjects.Add(sheet.SheetName, sheetBuilder);
            return sheetBuilder;
        }

        public ISheetObject GetSheet(int sheetIndex)
        {
            var sheet = _workBook.GetSheetAt(sheetIndex);

            if (sheet == null)
            {
                throw new ArgumentOutOfRangeException(nameof(sheetIndex));
            }

            if (_sheetObjects.TryGetValue(sheet.SheetName, out var sheetBuilder))
            {
                return sheetBuilder;
            }

            sheetBuilder = new SheetObject(this, sheet);

            _sheetObjects[sheet.SheetName] = sheetBuilder;

            return sheetBuilder;
        }

        public ISheetObject GetOrCreateSheet(string sheetName)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                throw new ArgumentNullException(nameof(sheetName));
            }

            if (_sheetObjects.TryGetValue(sheetName, out var sheetBuilder))
            {
                return sheetBuilder;
            }

            ISheet sheet = null;

            if (!string.IsNullOrEmpty(sheetName))
            {
                sheet = _workBook.GetSheet(sheetName);
            }

            if (sheet == null)
            {
                sheet = _workBook.CreateSheet(sheetName);
            }

            sheetBuilder = new SheetObject(this, sheet);

            _sheetObjects[sheetName] = sheetBuilder;

            return sheetBuilder;
        }

        public ICellStyle CreateDefaultStyle(IDefaultCellStyle? defaultCellStyle = null)
        {
            var newStyle = _workBook.CreateCellStyle();

            (defaultCellStyle ?? DefaultCellStyle).SetDefaultCellStyle(newStyle);

            return newStyle;
        }

        public IFont CreateFont()
        {
            return _workBook.CreateFont();
        }

        public IFont GetFont(ICellStyle style)
        {
            return style.GetFont(_workBook);
        }

        public void TraversalAllCellsSetDefaultStyle()
        {
            foreach (var (_, sheet) in _sheetObjects)
            {
                for (var i = 0; i <= sheet.GetMaxDataRow(); i++)
                {
                    for (int j = 0; j <= sheet.GetMaxDataColumn(); j++)
                    {
                        var cell = sheet[i][j];
                        var code = cell.GetHashCode();

                        if (!sheet[i][j].HasStyle)
                        {
                            sheet[i][j].Style = CreateDefaultStyle(sheet.DefaultCellStyle);
                        }
                    }
                }
            }
        }

        public void Save(string physicalFilePath)
        {
            Save(physicalFilePath, true, true);
        }

        public void Save(string physicalFilePath, bool applyStyleToMergedCells, bool setDefaultCellStyle)
        {
            if (applyStyleToMergedCells)
            {
                foreach (var (_, sheet) in _sheetObjects)
                {
                    sheet.ApplyStyleToMergedCells();
                }
            }

            if (setDefaultCellStyle)
            {
                TraversalAllCellsSetDefaultStyle();
            }

            using var ms = new FileStream(physicalFilePath, FileMode.CreateNew);
            _workBook.Write(ms);
        }

        public static INpoiObject Load(Stream stream, ICellValueReader? convert = null, IDefaultCellStyle? defaultCellStyle = null)
        {
            return new NpoiObject(stream, convert, defaultCellStyle);
        }

        public static INpoiObject Create(ICellValueReader? convert = null, IDefaultCellStyle? defaultCellStyle = null)
        {
            return new NpoiObject(null, convert, defaultCellStyle);
        }
    }
}
