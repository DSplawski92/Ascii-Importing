using DS.AsciiImport;
using DS.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DS.DataImporter
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        AsciiSettings asciiSettings = new AsciiSettings()
        {
            ColDelimiter = ';',
            DateTimeFormat = "dd.MM.yyyy HH:mm:ss",
            NumberDelimiter = ",",
            SkipFirstRowsNum = 0,
            UseFirstRowAsHeader = true,
            FileName = "shortValidSamples.csv"
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
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

            //ImportSettingsDialog importDialog = new ImportSettingsDialog();
            //if (importDialog.ShowDialog() == true)
            {
                IDataImport asciiImport = new AsciiDataImport(asciiSettings);
                //if((sender as MenuItem).Tag.ToString() == "load ascii file")
                {
                    try
                    {
                        var headers = asciiImport.GetHeaders();
                        var rows = asciiImport.Load(1, 10);
                        var t = new RowsViewModel(headers, rows).RowsView;
                        
                        dataGrid.DataContext = new RowsViewModel(headers, rows);
                        
                        li.ItemsSource = t;
                        combo.ItemsSource = asciiImport.GetHeaders();
                        ((ListBox)combo.Template.FindName("listBox", combo)).SelectAll();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
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
            }
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboItems = ((ListBox)combo.Template.FindName("listBox", combo)).SelectedItems;

            foreach (var column in dataGrid.Columns.Skip(1))
            {
                column.Visibility = comboItems.Contains(column.Header) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
