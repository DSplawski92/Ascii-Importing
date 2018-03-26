using DS.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DS.DataImporter
{
    public class DataMerger : IDataImport
    {
        List<object> mergedHeaders = new List<object>();
        Dictionary<IDataImport, int> importerIndexPosition = new Dictionary<IDataImport, int>();

        Dictionary<DateTime, double[]> mergedDataFiles = new Dictionary<DateTime, double[]>();
        private readonly IEnumerable<IDataImport> dataImporters;

        public DataMerger(IEnumerable<IDataImport> dataImporters)
        {
            this.dataImporters = dataImporters;
        }

        public IEnumerable<object> GetHeaders()
        {
            CreateHeaders();
            return mergedHeaders;
        }

        public IEnumerable<Row> LoadAll()
        {
            Merge();
            return mergedDataFiles
                .OrderBy(arg => arg.Key)
                .Select(arg => CreateRow(arg));
        }

        private void CreateHeaders()
        {
            if (!mergedHeaders.Any())
            {
                mergedHeaders.Add("timestamp");
                foreach (var importer in dataImporters)
                {
                    ImportHeaders(importer);
                }
                if (mergedHeaders.Count == 1)
                {
                    mergedHeaders.Clear();
                }
            }
        }

        private Row CreateRow(KeyValuePair<DateTime, double[]> arg)
        {
            return new Row
            {
                Timestamp = arg.Key,
                Samples = arg.Value
            };
        }

        public IEnumerable<Row> Load(int skip, int take)
        {
            return LoadAll().Skip(skip).Take(take);
        }

        public void Save()
        {
            string file = "merged.csv";

            if (File.Exists(file))
                File.Delete(file);

            StringBuilder headers = new StringBuilder();
            mergedHeaders.ForEach(arg => headers.Append(";" + (string)arg));
            FileStream fs = new FileStream("merged.csv", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(headers.Remove(0, 1));

            var rows = LoadAll();

            foreach (var row in rows)
            {
                StringBuilder line = new StringBuilder();
                line.Append(row.Timestamp);
                foreach (var sample in row.Samples)
                {
                    line.Append(";" + sample);
                }
                sw.WriteLine(line);
            }
        }

        private void Merge()
        {
            if (!mergedDataFiles.Any())
            {
                CreateHeaders();

                foreach (var importer in dataImporters)
                {
                    ImportSingleFile(importer, importerIndexPosition[importer]);
                }
            }
        }

        private void ImportHeaders(IDataImport importer)
        {
            var headers = importer.GetHeaders().Skip(1);
            importerIndexPosition[importer] = mergedHeaders.Count - 1;
            mergedHeaders.AddRange(headers);
        }

        private void ImportSingleFile(IDataImport importer, int startingIndex)
        {
            try
            {
                var rows = importer.LoadAll();
            }
            catch (Exception ex)
            {
                return;

            }
            foreach (var row in importer.LoadAll())
            {
                var array = GetValuesArray(row);
                var samples = row.Samples.ToArray();
                for (int sampleIndex = 0; sampleIndex < samples.Length; sampleIndex++)
                {
                    array[startingIndex + sampleIndex] = samples[sampleIndex];
                }
            }
        }

        private double[] GetValuesArray(Row row)
        {
            if (!mergedDataFiles.ContainsKey(row.Timestamp))
            {
                mergedDataFiles[row.Timestamp] = new double[mergedHeaders.Count - 1];
            }
            return mergedDataFiles[row.Timestamp];
        }
    }
}
