using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.ExcelImport
{
    public class ExcelSettings
    {
        public string FileName { get; set; }
        public string DateTimeFormat { get; set; }
        public string NumberDelimiter { get; set; }
        public bool UseFirstRowAsHeader { get; set; }
        public int SkipFirstRowsNum { get; set; }

        public override string ToString()
        {
            return "Excel Setings:\n" + $"Filename: {FileName}\n" + $"DateTimeFormat: {DateTimeFormat}\n" +
                $"UseFirstRowAsHeader: {UseFirstRowAsHeader}\n" + $"SkipFirstRowsNum: {SkipFirstRowsNum}";
        }
    }
}
