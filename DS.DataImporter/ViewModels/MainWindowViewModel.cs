using DS.AsciiImport;
using DS.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ICommand OpenImportDialog { get { return new RelayCommand(OpenImportDialogExecute, () => true); } }
        
        public void OpenImportDialogExecute(object parameter)
        {
            var importVM = new ImportSettingsDialogVievModel();
            var importView = new ImportSettingsDialog
            {
                DataContext = importVM
            };
            if (importView.ShowDialog() == true)
            {
                ImportData(importVM.AsciiSettings);
                rowsView = CreateDataView();
            }
        }

        private void ImportData(AsciiSettings asciiSettings)
        {
            dataImport = new AsciiDataImport(asciiSettings);
            Headers = dataImport.GetHeaders();
            rows = dataImport.LoadAll();
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
                if (RowsView != value)
                {
                    rowsView = CreateDataView();
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

        //public MainWindowViewModel(IEnumerable<object> headers, IEnumerable<Row> rows)
        //{
        //    this.headers = headers;
        //    this.rows = rows;
        //}

        public MainWindowViewModel()
        {
        }
    }
}
