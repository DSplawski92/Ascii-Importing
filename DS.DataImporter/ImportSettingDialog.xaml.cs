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
        //public AsciiSettings AsciiSettings { get; private set; }

        public ImportSettingsDialog()
        {
            InitializeComponent();
            //dateTimeFormat.ItemsSource = new[] { "dd.MM.yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss" };
        }

        //private void Ok_Click(object sender, RoutedEventArgs e)
        //{
        //    AsciiSettings = new AsciiSettings();
        //    AsciiSettings.FileName = fileName.Text;
        //    AsciiSettings.ColDelimiter = columnDelimiter.Text.First();
        //    AsciiSettings.DateTimeFormat = dateTimeFormat.SelectedValue as string;
        //    AsciiSettings.NumberDelimiter = numberDelimiter.Text.First().ToString();
        //    AsciiSettings.UseFirstRowAsHeader = isHeadersInFirstRow.IsChecked ?? false;
        //    AsciiSettings.SkipFirstRowsNum = string.IsNullOrWhiteSpace(SkipFirstRowsNum.Text) ? 0 : int.Parse(SkipFirstRowsNum.Text);

        //    this.DialogResult = true;
        //}

        //private void BrowseButton_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog fileDialog = new OpenFileDialog();

        //    fileDialog.Filter = "Ascii files (*.txt; *.csv)|*.txt;*.csv|Binary files (*.xls, *.xlsx, *.ods)|*.xls;*.xlsx;*.ods";
        //    if (fileDialog.ShowDialog() == true)
        //    {
        //        fileName.Text = fileDialog.FileName;
        //    }
        //}

        //private void cancel_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = false;
        //}
    }
}
