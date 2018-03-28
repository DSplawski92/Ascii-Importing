using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public RowsViewModel(IEnumerable<Row> rows, IEnumerable<object> headers)
        {
            Rows = new ObservableCollection<Row>(rows);
            Headers = headers;
        }

        public DataView RowsView
        {
            get
            {
                DataTable rowsView = new DataTable();
                rowsView.Columns.Clear();
                rowsView.Columns.Add(new DataColumn(headers.First().ToString(), typeof(DateTime)));
                foreach (var item in headers.Skip(1))
                {
                    rowsView.Columns.Add(new DataColumn(item.ToString(), typeof(double)));
                }
                //rowsView.Columns.AddRange(headers.Skip(1).Select(arg => new DataColumn(arg.ToString())).ToArray());
                rowsView.Rows.Clear();

                foreach (var item in rows)
                {
                    DataRow dataRow = rowsView.NewRow();
                    dataRow[0] = item.Timestamp;

                    int i = 1;
                    foreach (var sample in item.Samples)
                    {
                        dataRow[i++] = sample;
                    }

                    rowsView.Rows.Add(dataRow);
                    //rowsView.Rows.Add(item.Timestamp, item.Samples.ToArray());
                    //rowsView.Rows.Add(row); 
                }
                var temp = rowsView.AsDataView()[0].Row;
                return rowsView.AsDataView();
            }
        }
        public RowsViewModel(IEnumerable<object> headers, IEnumerable<Row> rows)
        {
            this.headers = headers;
            this.rows = rows;
        }
    }
}
