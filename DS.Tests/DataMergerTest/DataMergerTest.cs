using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DS.DataImporter;
using DS.Interfaces;
using DS.AsciiImport;

namespace UnitTestProject
{
    [TestFixture]
    public class DataMergerTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [TestCaseSource(typeof(DataMergerSource), "SomeCommonTimestamps")]
        public void LoadAll_SomeCommonTimestamps_ReturnMergedDataExcludedRepeats(IEnumerable<AsciiSettings> settingsSet, int mergedTimestampsNum, int samplesNum, int columnsNum)
        {
            // arrange
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var mergedData = dataMerger.LoadAll();
            var timestamps = mergedData.Select(arg => arg.Timestamp);
            DateTime datetime = new DateTime(2000, 1, 1, 0, 0, 0);

            // assert
            Assert.AreEqual(mergedTimestampsNum, mergedData.Count());
            Assert.AreEqual(1, mergedData.GroupBy(arg => arg.Samples.Count()).Count());
            Assert.AreEqual(samplesNum, mergedData.Select(arg => arg.Samples.Count()).Sum());

            foreach (var timestamp in timestamps)
            {
                Assert.LessOrEqual(datetime, timestamp);
                datetime = timestamp;
            }
        }

        [TestCaseSource(typeof(DataMergerSource), "InvalidSamples")]
        public void LoadAll_InvalidSamples_ReturnInvalidSamplesAsNaN(IEnumerable<AsciiSettings> settingsSet, int mergedTimestampsNum, int invalidSamplesNum)
        {
            // arrange
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var mergedData = dataMerger.LoadAll();
            int NaN_num = mergedData.Select(arg => arg.Samples.Count(sample => double.IsNaN(sample))).Sum();

            // assert
            Assert.AreEqual(invalidSamplesNum, NaN_num);
        }

        [TestCaseSource(typeof(DataMergerSource), "DataWithEmptyLines")]
        public void LoadAll_DataWithEmptyLines_ReturnMergedDataWithoutEmpyLines(IEnumerable<AsciiSettings> settingsSet, int validMergedRowsNum)
        {
            // arrange 
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var mergedData = dataMerger.LoadAll();

            // assert
            Assert.AreEqual(validMergedRowsNum, mergedData.Count());
        }

        [TestCaseSource(typeof(DataMergerSource), "FileUsedMoreThanOnce")]
        public void LoadAll_FileUsedMoreThanOnce_ReturnDataExcludedDuplicatedData(IEnumerable<AsciiSettings> settingsSet, int validMergedRowsNum)
        {
            // arrange
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var mergedData = dataMerger.LoadAll();

            // assert
            Assert.AreEqual(validMergedRowsNum, mergedData.Count());
        }

        public void LoadAll_InvalidTimestamp_ReturnMergedDataWithout(IEnumerable<AsciiSettings> settingsSet, int validMergedRowsNum)
        {
            // arrange
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var mergedData = dataMerger.LoadAll();

            // assert
            Assert.AreEqual(validMergedRowsNum, mergedData.Count());
        }

        [TestCaseSource(typeof(DataMergerSource), "InvalidConfiguration")]
        public void LoadAll_InvalidConfiguration_ThrownException(IEnumerable<AsciiSettings> settingsSet)
        {
            // arrange
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            TestDelegate action = () => dataMerger.LoadAll();

            // assert
            Assert.Throws<FormatException>(action);
        }

        [TestCaseSource(typeof(DataMergerSource), "FilesWithDifferentConfigurations")]
        public void LoadAll_FilesWithDifferentConfigurations_ReturnValidMergedData(IEnumerable<AsciiSettings> settingsSet, int validMergedRowsNum)
        {
            // arrange
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var mergedData = dataMerger.LoadAll();

            // assert
            Assert.AreEqual(validMergedRowsNum, mergedData.Count());
        }

        [TestCaseSource(typeof(DataMergerSource), "SpecifiedRange")]
        public void Load_SpecifiedRange_ReturnSpecifiedRangeRows(IEnumerable<AsciiSettings> settingsSet, int skip, int take, int validMergedRowsNum)
        {
            // arrange
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var mergedData = dataMerger.Load(skip, take);

            // assert
            Assert.AreEqual(validMergedRowsNum, mergedData.Count());
        }

        [TestCaseSource(typeof(DataMergerSource), "ValidHeadersFiles")]
        public void GetHeaders_ValidHeadersFiles_ReturnValidHeadersCollection(IEnumerable<AsciiSettings> settingsSet, string[] validHeaders)
        {
            // arrange
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var headers = dataMerger.GetHeaders().ToList();

            // assert
            Assert.AreEqual(headers.Count(), validHeaders.Count());
            CollectionAssert.AreEqual(validHeaders, headers);
        }

        [TestCaseSource(typeof(DataMergerSource), "GenericHeaders")]
        public void GetHeaders_GenericHeaders_ReturnMergedGenericHeaders(IEnumerable<AsciiSettings> settingsSet, string[] genericHeaders)
        {
            // arrange
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var headers = dataMerger.GetHeaders().ToList();

            // assert
            Assert.AreEqual(headers.Count(), genericHeaders.Count());
            CollectionAssert.AreEqual(genericHeaders, headers);
        }

        [TestCase(0, TestName = "Get headers when there is no data")]
        public void GetHeaders_NoData_ReturnEmptyHeaders(int headersNum)
        {
            // arrange
            List<IDataImport> importers = new List<IDataImport>();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var headers = dataMerger.GetHeaders().ToList();

            // assert
            Assert.AreEqual(headersNum, headers.Count);
        }
 
        [TestCaseSource(typeof(DataMergerSource), "MixedHeaders")]
        public void GetHeaders_MixedHeadersType_ReturnValidMixedHeaders(IEnumerable<AsciiSettings> settingsSet, string[] mixedHeaders)
        {
            // arrange
            List<IDataImport> importers = settingsSet.Select(arg => (IDataImport)new AsciiDataImport(arg)).ToList();
            DataMerger dataMerger = new DataMerger(importers);

            // act
            var headers = dataMerger.GetHeaders().ToList();

            // assert
            Assert.AreEqual(headers.Count(), mixedHeaders.Count());
            CollectionAssert.AreEqual(mixedHeaders, headers);
        }
    }
}
