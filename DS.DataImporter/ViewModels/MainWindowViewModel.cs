using DS.AsciiImport;
using DS.DataImporter.ViewModels;
using DS.ExcelImport;
using DS.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DS.DataImporter
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Row> rows;
        private IEnumerable<object> headers;
        public IEnumerable<object> Headers
        {
            get
            {
                if(headers == null)
                {
                    headers = new List<object>();
                }

                return headers;
            }
            set
            {
                if (headers != value)
                {
                    headers = value;
                    OnPropertyChanged();
                }
            }
        }
        private IDataImport dataImport;
        public ICommand OpenAsciiImportDialog { get { return new RelayCommand(OpenAsciiImportDialogExecute, () => true); } }
        public ICommand OpenExcelImportDialog { get { return new RelayCommand(OpenExcelImportDialogExecute, () => true); } }

        private void LoadCustomData(string parameter)
        {
            AsciiSettings asciiSettings = new AsciiSettings()
            {
                ColumnDelimiter = ";",
                DateTimeFormat = parameter == "long" ? "yyyy-MM-dd HH:mm:ss" : "dd.MM.yyyy HH:mm:ss",
                FileName = parameter == "long" ? "longValidSamples.csv" : "shortValidSamples.csv",
                NumberDelimiter = ",",
                SkipFirstRowsNum = 1,
                UseFirstRowAsHeader = true
            };
            ImportAsciiData(asciiSettings);
        }

        public void OpenAsciiImportDialogExecute(object parameter)
        {
            var importVM = new ImportAsciiSettingsDialogViewModel();
            var importView = new ImportAsciiSettingsDialog
            {
                DataContext = importVM
            };
            if (importView.ShowDialog() == true)
            {
                ImportAsciiData(importVM.AsciiSettings);
            }
            importView.Close();
        }

        private void ImportAsciiData(AsciiSettings asciiSettings)
        {
            dataImport = new AsciiDataImport(asciiSettings);
            Headers = dataImport.GetHeaders();
            rows = dataImport.LoadAll();
            RowsView = CreateDataView();
        }

        public void OpenExcelImportDialogExecute(object parameter)
        {
            var importVM = new ImportExcelDataDialogViewModel();
            var importView = new ImportExcelDataDialog
            {
                DataContext = importVM
            };

            if (importView.ShowDialog() == true)
            {
                ImportExcelData(importVM.ExcelSettings);
            }
            importView.Close();
        }

        private void ImportExcelData(ExcelSettings excelSettings)
        {
            dataImport = new ExcelDataImport(excelSettings);
            Headers = dataImport.GetHeaders();
            rows = dataImport.LoadAll();
            RowsView = CreateDataView();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private DataView rowsView;
        public DataView RowsView
        {
            get
            {
                if (rowsView == null)
                {
                    rowsView = new DataView();
                }
                return rowsView;
            }
            set
            {
                if (rowsView != value)
                {
                    rowsView = value;
                    OnPropertyChanged();
                }
            }
        }

        private DataView CreateDataView()
        {
            DataTable rowsTable = new DataTable();
            rowsTable.Columns.Add(new DataColumn(headers.First().ToString(), typeof(DateTime)));

            foreach (var item in headers.Skip(1))
            {
                rowsTable.Columns.Add(new DataColumn(item.ToString(), typeof(double)));
            }

            var rowList = rows;

            foreach (var item in rowList)
            {
                DataRow dataRow = rowsTable.NewRow();
                dataRow[0] = (item as Row).Timestamp;

                int i = 1;
                foreach (var sample in (item as Row).Samples)
                {
                    dataRow[i++] = sample;
                }

                rowsTable.Rows.Add(dataRow);
            }
            return rowsTable.AsDataView();
        }

        public MainWindowViewModel()
        {
        }
    }
}
