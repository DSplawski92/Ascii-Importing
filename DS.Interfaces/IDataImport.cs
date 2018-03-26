using System.Collections.Generic;

namespace DS.Interfaces
{
    public interface IDataImport
    {
        IEnumerable<object> GetHeaders();
        IEnumerable<Row> LoadAll();
        IEnumerable<Row> Load(int skip, int take);
    }
}
