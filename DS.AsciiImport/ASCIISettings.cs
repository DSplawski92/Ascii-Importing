namespace DS.AsciiImport
{
    public class AsciiSettings
    {
        public string FileName { get; set; }
        public char ColDelimiter { get; set; }
        public string DateTimeFormat { get; set; }
        public string NumberDelimiter { get; set; }
        public bool UseFirstRowAsHeader { get; set; }
        public int SkipFirstRowsNum { get; set; }

        public AsciiSettings()
        {
            FileName = string.Empty;
            ColDelimiter = new char();
            DateTimeFormat = string.Empty;
            NumberDelimiter = string.Empty;
            UseFirstRowAsHeader = true;
            SkipFirstRowsNum = new int();
        }

        public override string ToString()
        {
            return string.Format("Excel Setings:\n" + "Filename: {0}\nColDelimiter: {1}\nNumberDelimiter: {2}\nDateTimeFormat: {3}\nUseFirstRowAsHeader: {4}\nSkipFirstRowsNum: {5}", 
                FileName, ColDelimiter, NumberDelimiter, DateTimeFormat, UseFirstRowAsHeader, SkipFirstRowsNum);
        }
    }
}