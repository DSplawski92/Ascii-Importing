using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using DS.Interfaces;
using NLog;

namespace DS.AsciiImport
{
    public class AsciiDataImport : IDataImport
    {       
        private readonly AsciiSettings settings;
        private IEnumerable<string> lines;
        private int? colsNum = null;    // valid number of columns in row

        public AsciiDataImport(AsciiSettings settings)
        {
            this.settings = settings;
        }

        public IEnumerable<Row> Load(int skip, int take)
        {
            ValidateImportSettings();

            if (settings.UseFirstRowAsHeader)
            {
                skip += 1;
            }

            lines = File.ReadLines(settings.FileName).Skip(skip).Take(take);

            if (lines.Count() == 0)
            {
                return new List<Row>();
            }
            // set valid number of column based on first row (headers in file or first row with samples)
            if (colsNum == null)
            {
                var firstRow = File.ReadLines(settings.FileName).First().Split(settings.ColDelimiter);
                colsNum = firstRow.Count();
            }

            return lines.Select(arg => GetRow(arg)).Where(arg => arg != null);
        }

        public IEnumerable<Row> LoadAll()
        {
            return Load(settings.SkipFirstRowsNum, Int32.MaxValue);
        }

        public IEnumerable<object> GetHeaders()
        {
            ValidateImportSettings();
            var lines = File.ReadLines(settings.FileName, Encoding.UTF8);

            if (lines.Count() == 0)
                return lines;

            IEnumerable<object> headers = null;
            if (settings.UseFirstRowAsHeader)
            {
                foreach (var line in lines)
                {
                    headers = line.Split(settings.ColDelimiter);
                    if(headers.Count() > 1)
                    {
                        break;
                    }
                }
            }
            else
            {
                List<object> genericHeaders = new List<object>();
                int genericHeaderSize = lines.Skip(settings.SkipFirstRowsNum).First().Split(settings.ColDelimiter).Skip(1).Count();
                genericHeaders.Add("timestamp");
                for (int i = 1; i <= genericHeaderSize; i++)
                {
                    genericHeaders.Add($"C{i}");
                }
                headers = genericHeaders;
            }
            colsNum = headers.Count();
            return headers;
        }

        private void ValidateImportSettings()
        {
            if (!File.Exists(settings.FileName))
            {
                throw new FileNotFoundException("File does not exist.");
            }
            if (settings.ColDelimiter == settings.NumberDelimiter.First())
            {
                throw new ArgumentException("The column delimiter can not be the same as decimal delimiter.");
            }
            if (settings.DateTimeFormat.Contains(settings.ColDelimiter))
            {
                throw new ArgumentException("The column delimiter must not be contained in date time format.");
            }
        }

        private Row GetRow(string line)
        {
            if (String.IsNullOrWhiteSpace(line))
                return null;

            var cells = line.Split(settings.ColDelimiter).Take(colsNum.Value).ToList();

            if (cells.Count() <= 1)
                throw new FormatException("Invalid column delimiter");

            if (settings.UseFirstRowAsHeader && cells.Count() != colsNum)
            {
                for (int i = 0; i < colsNum - cells.Count(); i++)
                {
                    cells.Add("NaN");
                }
            }
                //throw new FormatException("Number of samples are different from number of columns.");

            NumberFormatInfo numberFormat = new NumberFormatInfo
            {
                NumberDecimalSeparator = settings.NumberDelimiter
            };

            return new Row
            {
                Timestamp = DateTime.ParseExact(cells.First(), settings.DateTimeFormat, CultureInfo.InvariantCulture),
                Samples = cells.Skip(1).Select(arg => double.TryParse(arg, NumberStyles.Any, numberFormat, out double sample) ? sample : double.NaN)
            };
        }
    }
}
