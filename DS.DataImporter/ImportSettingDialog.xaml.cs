using DS.AsciiImport;
using Microsoft.Win32;
using System.Linq;
using System.Windows;

namespace DS.DataImporter
{
    /// <summary>
    /// Logika interakcji dla klasy ImportSettingsDialog.xaml
    /// </summary>
    public partial class ImportSettingsDialog : Window
    {
        public AsciiSettings asciiSettings { get; private set; }

        public ImportSettingsDialog()
        {
            InitializeComponent();
            DataContext = new ImportSettingsDialogVievModel();
            dateTimeFormat.ItemsSource = new[] { "dd.MM.yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss" };
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            asciiSettings = new AsciiSettings();
            asciiSettings.FileName = fileName.Text;
            asciiSettings.ColDelimiter = columnDelimiter.Text.First();
            asciiSettings.DateTimeFormat = dateTimeFormat.SelectedValue as string;
            asciiSettings.NumberDelimiter = numberDelimiter.Text.First().ToString();
            asciiSettings.UseFirstRowAsHeader = isHeadersInFirstRow.IsChecked ?? false;
            asciiSettings.SkipFirstRowsNum = string.IsNullOrWhiteSpace(SkipFirstRowsNum.Text) ? 0 : int.Parse(SkipFirstRowsNum.Text);

            this.DialogResult = true;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Ascii files (*.txt; *.csv)|*.txt;*.csv|Binary files (*.xls, *.xlsx, *.ods) |*.xls;*.xlsx;*.ods";
            if (fileDialog.ShowDialog() == true)
            {
                fileName.Text = fileDialog.FileName;
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
