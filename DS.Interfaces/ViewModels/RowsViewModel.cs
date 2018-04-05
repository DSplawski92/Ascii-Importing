using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DS.Interfaces
{
    public class RowsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Row> rows = null;
        private IEnumerable<object> headers = null;
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
        public IDataImport dataImport;
        public ICommand SayHi { get { return new RelayCommand(LoadData, CanLoadData); } }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public DataView RowsView
        {
            get
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
                return rowsTable.DefaultView;
            }
        }

        public RowsViewModel(IEnumerable<object> headers, IEnumerable<Row> rows)
        {
            this.headers = headers;
            this.rows = rows;
        }

        public RowsViewModel()
        {
        }
    }
}
