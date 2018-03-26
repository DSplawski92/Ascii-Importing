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

        public override string ToString()
        {
            return "Excel Setings:\n" + $"Filename: {FileName}\n" + $"ColDelimiter: {ColDelimiter}\n" + $"NumberDelimiter: {NumberDelimiter}\n" 
                + $"DateTimeFormat: {DateTimeFormat}\n" + $"UseFirstRowAsHeader: {UseFirstRowAsHeader}\n" + $"SkipFirstRowsNum: {SkipFirstRowsNum}";
        }
    }
}