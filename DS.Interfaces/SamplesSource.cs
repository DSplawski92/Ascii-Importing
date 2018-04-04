using AlphaChiTech.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Interfaces
{
    class SamplesSource : IPagedSourceProvider<Row>
    {
        private IEnumerable<Row> rows;

        public SamplesSource(IEnumerable<Row> rows)
        {
            this.rows = rows;
        }

        public PagedSourceItemsPacket<Row> GetItemsAt(int pageoffset, int count, bool usePlaceholder)
        {
            return new PagedSourceItemsPacket<Row>()
            {
                LoadedAt = DateTime.Now,
                Items = rows.Skip(pageoffset).Take(count)
            };
        }

        public int Count
        {
            get { return rows.Count(); }
        }

        public int IndexOf(Row item)
        {
            return rows.TakeWhile(arg => arg.Timestamp == item.Timestamp).Count() - 1;
        }

        public void OnReset(int count)
        {

        }
    }
}
