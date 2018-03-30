using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace DS.Interfaces
{
    public class RowsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Row> rows;
        private IEnumerable<object> headers;
        public IEnumerable<object> Headers
        {
            get
            {
                return headers;
            }
            set
            {
                if (headers != value)
                {
                    headers = value;
                    OnPropertyChanged("Headers");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Row> _rows;
        public ObservableCollection<Row> Rows
        {
            get
            {
                return _rows;
            }
            set
            {
                if (_rows != value)
                {
                    _rows = value;
                    OnPropertyChanged("Rows");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DataView rowsView;
        public DataView RowsView
        {
            get
            {
                DataTable rowsTable = new DataTable();
                rowsTable.Clear();
                rowsTable.Columns.Add(new DataColumn(headers.First().ToString(), typeof(DateTime)));
                foreach (var item in headers.Skip(1))
                {
                    rowsTable.Columns.Add(new DataColumn(item.ToString(), typeof(double)));
                }
                
                var rowList = rows.ToList();

                foreach (var item in rowList)
                {
                    DataRow dataRow = rowsTable.NewRow();
                    dataRow[0] = item.Timestamp;

                    int i = 1;
                    foreach (var sample in item.Samples.ToList())
                    {
                        dataRow[i++] = sample;
                    }

                    rowsTable.Rows.Add(dataRow);

                }
                rowsView = rowsTable.DefaultView;
                return rowsView;
            }
        }
        public RowsViewModel(IEnumerable<object> headers, IEnumerable<Row> rows)
        {
            this.headers = headers;
            this.rows = rows;
        }
    }
}
