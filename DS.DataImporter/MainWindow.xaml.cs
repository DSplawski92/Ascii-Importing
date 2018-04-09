using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DS.DataImporter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ComboBox));
            if (dpd != null)
            {
                dpd.AddValueChanged(combo, OnSourceUpdated);
            }
        }

        private void DataLoaded(object sender, RoutedEventArgs e)
        {
            #region excel
            //ExcelSettings excelSettings = new ExcelSettings()
            //{

            //};
            //using (ExcelDataImport dataImport = new ExcelDataImport(excelSettings))
            //{
            //    try
            //    {
            //        //IDataImport dataImport = new ExcelDataImport(@"C:\Users\CodeConcept01\source\repos\Test- — kopia (2)\Test-\samplesMS1997.xls");
            //        var headers = dataImport.GetHeaders();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
            //return;
            #endregion             
            
            #region DataMerger
            //else if ((sender as MenuItem).Tag.ToString() == "merge ascii files")
            //{
            //    string directoryPath = System.IO.Path.GetDirectoryName(importDialog.asciiSettings.FileName);
            //    var files = Directory.EnumerateFiles(directoryPath);

            //    List<AsciiDataImport> dataImporters = new List<AsciiDataImport>();

            //    foreach (var file in files)
            //    {
            //        AsciiSettings settings = new AsciiSettings()
            //        {
            //            ColDelimiter = importDialog.asciiSettings.ColDelimiter,
            //            DateTimeFormat = importDialog.asciiSettings.DateTimeFormat,
            //            FileName = file,
            //            NumberDelimiter = importDialog.asciiSettings.NumberDelimiter,
            //            SkipFirstRowsNum = importDialog.asciiSettings.SkipFirstRowsNum,
            //            UseFirstRowAsHeader = importDialog.asciiSettings.UseFirstRowAsHeader
            //        };
            //        dataImporters.Add(new AsciiDataImport(settings));
            //    }

            //    DataMerger dataMerger = new DataMerger(dataImporters);
            //    var mergedFile = dataMerger.LoadAll();
            //    MessageBox.Show(mergedFile.Count() + "");

            //}
            #endregion
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboItems = ((ListBox)combo.Template.FindName("listBox", combo)).SelectedItems;

            foreach (var column in dataGrid.Columns)
            {
                column.Visibility = comboItems.Contains(column.Header) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OnSourceUpdated(object sender, EventArgs e)
        {
            ((ListBox)combo.Template.FindName("listBox", combo)).SelectAll();
        }
    }
}
