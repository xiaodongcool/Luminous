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

namespace Luminous.Npoi
{
    public class NpoiObject : INpoiObject
    {
        private readonly XSSFWorkbook _workBook;
        private readonly Dictionary<string, ISheetObject> _sheets = new Dictionary<string, ISheetObject>();

        public NpoiObject()
        {
            _workBook = new XSSFWorkbook();
        }

        public NpoiObject(Stream stream)
        {
            _workBook = new XSSFWorkbook(stream);
        }

        public ISheetObject GetSheet(int sheetIndex)
        {
            var sheet = _workBook.GetSheetAt(sheetIndex);

            if (sheet == null)
            {
                throw new ArgumentOutOfRangeException(nameof(sheetIndex));
            }

            if (_sheets.TryGetValue(sheet.SheetName, out var sheetBuilder))
            {
                return sheetBuilder;
            }

            sheetBuilder = new SheetObject(this, sheet);

            _sheets[sheet.SheetName] = sheetBuilder;

            return sheetBuilder;
        }

        public ISheetObject CreateSheet()
        {
            var sheet = _workBook.CreateSheet();
            var sheetBuilder = new SheetObject(this, sheet);
            _sheets.Add(sheet.SheetName, sheetBuilder);
            return sheetBuilder;
        }

        public ISheetObject GetOrCreateSheet(string sheetName)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                throw new ArgumentNullException(nameof(sheetName));
            }

            if (_sheets.TryGetValue(sheetName, out var sheetBuilder))
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

            _sheets[sheetName] = sheetBuilder;

            return sheetBuilder;
        }

        public void Save(string physicalFilePath)
        {
            Save(physicalFilePath, true, true);
        }

        public void Save(string physicalFilePath, bool applyStyleToMergedCells, bool setDefaultCellStyle)
        {
            if (applyStyleToMergedCells)
            {
                foreach (var (_, sheet) in _sheets)
                {
                    sheet.ApplyStyleToMergedCells();
                }
            }

            if (setDefaultCellStyle)
            {
                SetDefaultCellStyle();
            }

            using var ms = new FileStream(physicalFilePath, FileMode.CreateNew);
            _workBook.Write(ms);
        }

        /// <summary>
        ///     会覆盖原有样式，请先执行该方法，再对需要特殊样式单元格做修改
        /// </summary>
        public void SetDefaultCellStyle()
        {
            foreach (var (_, sheet) in _sheets)
            {
                for (var i = 0; i <= sheet.GetMaxDataRow(); i++)
                {
                    for (int j = 0; j <= sheet.GetMaxDataColumn(); j++)
                    {
                        var cell = sheet[i][j];
                        var code = cell.GetHashCode();

                        if (!sheet[i][j].HasStyle)
                        {
                            sheet[i][j].Style = CreateDefaultStyle();
                        }
                    }
                }
            }
        }

        public XSSFWorkbook Book => _workBook;

        public ICellStyle CreateDefaultStyle()
        {
            var newStyle = _workBook.CreateCellStyle();

            newStyle.WrapText = true;
            newStyle.BorderBottom = BorderStyle.Thin;
            newStyle.BorderTop = BorderStyle.Thin;
            newStyle.BorderRight = BorderStyle.Thin;
            newStyle.BorderLeft = BorderStyle.Thin;
            newStyle.Alignment = HorizontalAlignment.Center;
            newStyle.VerticalAlignment = VerticalAlignment.Center;

            return newStyle;
        }
    }
}
