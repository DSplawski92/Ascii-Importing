using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DS.Interfaces;
using NPOI;
using NPOI.SS.UserModel;
//using Excel = Microsoft.Office.Interop.Excel;

namespace DS.ExcelImport
{
    public class ExcelDataImport : IDataImport
    {
        private readonly ExcelSettings settings;
        private List<Row> rows;
        private int? colsNum = null;

        public ExcelDataImport(ExcelSettings settings)
        {
            this.settings = settings;
            rows = new List<Row>();
        }

        public IEnumerable<object> GetHeaders()
        {
            var firstRow = GetFirstRow();
            IEnumerable<object> headers = new List<object>();

            if (firstRow != null)
            {
                var cells = firstRow.Cells.Select(arg => arg.ToString());
                if (settings.UseFirstRowAsHeader)
                {
                    headers = cells;
                }
                else
                {
                    headers = GenerateHeaders(cells.Count());
                }
            }
            colsNum = headers.Skip(1).Count();
            return headers;
        }

        public IEnumerable<Row> Load(int skip, int take)
        {
            ValidateImportSettings();

            if (settings.UseFirstRowAsHeader)
            {
                skip += 1;
            }

            using (FileStream fileStream = new FileStream(settings.FileName, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fileStream);
                ISheet worksheet = workbook.GetSheetAt(0);

                if (worksheet.LastRowNum == 0 && worksheet.GetRow(0) == null)
                {
                    return new List<Row>();
                }

                if(colsNum == null)
                {
                    colsNum = worksheet.GetRow(worksheet.FirstRowNum).Skip(1).Count();
                }

                int rowId = skip + worksheet.FirstRowNum;
                for (int i = 0; i < take && rowId <= worksheet.LastRowNum; i++)
                {
                    rowId = i + skip + worksheet.FirstRowNum;
                    var excelRow = worksheet.GetRow(rowId);
                    var row = GetRow(excelRow);
                    if (excelRow != null && row != null && !rows.Select(arg => arg.Timestamp).Contains(row.Timestamp))
                    {
                        rows.Add(row);
                    }
                }
            }
            return rows;
        }

        public IEnumerable<Row> LoadAll()
        {
            return Load(settings.SkipFirstRowsNum, Int32.MaxValue);
        }

        private IRow GetFirstRow()
        {
            ValidateImportSettings();
            IRow firstRow = null;
            using (FileStream fileStream = new FileStream(settings.FileName, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fileStream);
                ISheet worksheet = workbook.GetSheetAt(0);
                firstRow = worksheet.GetRow(worksheet.FirstRowNum);
            }
            return firstRow;
        }

        private int GetColumnsNumber(ISheet worksheet)
        {
            int AllColsNum = 0;
            foreach (var item in worksheet)
            {
                int rowColsNum = (item as IRow).LastCellNum;
                AllColsNum = AllColsNum < rowColsNum ? rowColsNum : AllColsNum;
            }
            return AllColsNum;
        }

        private Row GetRow(IRow excelRow)
        {
            if (excelRow == null)
                return null;

            NumberFormatInfo numberFormat = new NumberFormatInfo
            {
                NumberDecimalSeparator = settings.NumberDelimiter
            };

            DateTime timestamp;
            Row row = null;
            if(DateTime.TryParseExact(excelRow.Cells.First().ToString(), settings.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp))
            {
                double sample;
                var samples = excelRow.Cells.Skip(1).Select(arg => double.TryParse(arg.ToString(), NumberStyles.Any, numberFormat, out sample) ? sample : double.NaN).Take(colsNum.Value);

                int samplesNum = samples.Count();
                if (samplesNum < colsNum)
                {
                    List<double> missingCells = new List<double>();
                    for (int i = samplesNum; i < colsNum; i++)
                    {
                        missingCells.Add(double.NaN);
                    }
                    samples = samples.Concat(missingCells);
                }

                row = new Row
                {
                    Timestamp = timestamp,
                    Samples = samples
                };

            }
            return row;
        }

        private IEnumerable<object> GenerateHeaders(int number)
        {
            List<string> genericHeaders = new List<string>();
            int genericHeaderSize = number;
            genericHeaders.Add("timestamp");
            for (int i = 1; i < genericHeaderSize; i++)
            {
                genericHeaders.Add("C" + i);
            }
            return genericHeaders;
        }

        private void ValidateImportSettings()
        {
            if (!File.Exists(settings.FileName))
            {
                throw new FileNotFoundException();
            }
        }
    }
}
