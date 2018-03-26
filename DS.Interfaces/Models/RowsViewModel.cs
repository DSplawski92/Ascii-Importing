using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Interfaces
{
    public class RowsViewModel
    {
        private IEnumerable<Row> rows;
        private IEnumerable<object> headers;
        public DataView RowsView
        {
            get
            {
                DataTable rowsView = new DataTable();
                rowsView.Columns.Add(new DataColumn(headers.First().ToString(), typeof(DateTime)));
                foreach (var item in headers.Skip(1))
                {
                    rowsView.Columns.Add(new DataColumn(headers.ToString(), typeof(double)));
                }
                //rowsView.Columns.AddRange(headers.Skip(1).Select(arg => new DataColumn(arg.ToString())).ToArray());
                
                foreach (var item in rows)
                {
                    //DataRow dataRow = new DataRow;

                    rowsView.Rows.Add(item.Timestamp, item.Samples.ToArray());
                    //foreach (var sample in item.Samples)
                    //{
                    //    rowsView.Rows.Add(0);
                    //}
                    //rowsView.Rows.Add(item.Samples);
                }
                return rowsView.DefaultView;
            }
        }
        public RowsViewModel(IEnumerable<object> headers, IEnumerable<Row> rows)
        {
            this.headers = headers;
            this.rows = rows;
        }
    }
}
