namespace DS.ExcelImport
{
    public class ExcelSettings
    {
        public string FileName { get; set; }
        public string DateTimeFormat { get; set; }
        public string NumberDelimiter { get; set; }
        public bool UseFirstRowAsHeader { get; set; }
        public int SkipFirstRowsNum { get; set; }

        public ExcelSettings()
        {
            FileName = string.Empty;
            DateTimeFormat = string.Empty;
            NumberDelimiter = string.Empty;
            UseFirstRowAsHeader = true;
            SkipFirstRowsNum = 0;
        }

        public override string ToString()
        {
            return string.Format("Excel Setings:\nFilename: {0}\nDateTimeFormat: {1}\nUseFirstRowAsHeader: {2}\nSkipFirstRowsNum: {3}",
                FileName, DateTimeFormat, UseFirstRowAsHeader, SkipFirstRowsNum);
        }
    }
}
