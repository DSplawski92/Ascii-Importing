using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Interfaces
{
    public class Row
    {
        public DateTime Timestamp { get; set; }
        public IEnumerable<double> Samples { get; set; }
    }
}
