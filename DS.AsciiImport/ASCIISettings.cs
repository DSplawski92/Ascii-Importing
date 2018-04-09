namespace DS.AsciiImport
{
    public class AsciiSettings
    {
        public string FileName { get; set; }
        public string ColumnDelimiter { get; set; }
        public string DateTimeFormat { get; set; }
        public string NumberDelimiter { get; set; }
        public bool UseFirstRowAsHeader { get; set; }
        public int SkipFirstRowsNum { get; set; }

        public AsciiSettings()
        {
            FileName = string.Empty;
            ColumnDelimiter = string.Empty;
            DateTimeFormat = string.Empty;
            NumberDelimiter = string.Empty;
            UseFirstRowAsHeader = true;
            SkipFirstRowsNum = 0;
        }

        public override string ToString()
        {
            return string.Format("Excel Setings:\n" + "Filename: {0}\nColDelimiter: {1}\nNumberDelimiter: {2}\nDateTimeFormat: {3}\nUseFirstRowAsHeader: {4}\nSkipFirstRowsNum: {5}", 
                FileName, ColumnDelimiter, NumberDelimiter, DateTimeFormat, UseFirstRowAsHeader, SkipFirstRowsNum);
        }
    }
}