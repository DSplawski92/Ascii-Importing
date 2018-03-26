using DS.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class RowComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var x1 = x as Row;
            var y1 = y as Row;
            int compareResult = x1.Timestamp.CompareTo(y1.Timestamp);
            if (compareResult == 1)
                return compareResult;
            else
                return CompareSamples(x1.Samples, y1.Samples);

        }
        private int CompareSamples(IEnumerable<double> x, IEnumerable<double> y)
        {
            var intersectCount = x.Intersect(y).Count();
            if (intersectCount == x.Count() && intersectCount == y.Count())
                return 0;
            else
                return 1;
        }
    }
}
