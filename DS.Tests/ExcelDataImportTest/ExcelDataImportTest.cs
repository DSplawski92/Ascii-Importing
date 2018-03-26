using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using DS.DataImporter;
using DS.ExcelImport;
using DS.Interfaces;

namespace UnitTestProject
{
    [TestFixture]
    class ExcelDataImportTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        #region GetHeaders() Tests
        [TestCaseSource(typeof(ExcelDataImportSource), "HeadersInFile")]
        public void GetHeaders_FileWithValidHeaders_ReturnValidHeaders(ExcelSettings settings, IEnumerable<object> validHeaders)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            var headers = importer.GetHeaders();

            // assert
            CollectionAssert.AreEqual(validHeaders, headers);
        }

        [TestCaseSource(typeof(ExcelDataImportSource), "GenericHeaders")]
        public void GetHeaders_NoHeaders_ReturnGenericHeaders(ExcelSettings settings, IEnumerable<object> validHeaders)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            var headers = importer.GetHeaders();

            // assert
            CollectionAssert.AreEqual(validHeaders, headers);
        }

        [TestCaseSource(typeof(ExcelDataImportSource), "EmptyFile")]
        public void GetHeaders_EmptyFile_ReturnEmptyFileHeaders(ExcelSettings settings, int headersNum)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            var headers = importer.GetHeaders();

            // assert
            Assert.AreEqual(headersNum, headers.Count());
        }

        [TestCaseSource(typeof(ExcelDataImportSource), "NotExistingFile")]
        public void GetHeaders_FileDoesNotExist_ThrowFileNotFoundException(ExcelSettings settings)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            TestDelegate action = () => importer.GetHeaders();

            // assert
            Assert.Throws<FileNotFoundException>(action);
        }
        #endregion

        #region LoadAll() Tests
        [TestCaseSource(typeof(ExcelDataImportSource), "ValidExcelFile")]
        public void LoadAll_ValidFile_ReturnRows(ExcelSettings settings, IEnumerable<Row> validRows)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            var rows = importer.LoadAll();

            // assert
            Assert.AreEqual(validRows.Count(), rows.Count());
            var validRowsEnumerator = validRows.GetEnumerator();
            var rowsEnumerator = rows.GetEnumerator();

            while (validRowsEnumerator.MoveNext() && rowsEnumerator.MoveNext())
            {
                Assert.AreEqual(validRowsEnumerator.Current.Timestamp, rowsEnumerator.Current.Timestamp);
                CollectionAssert.AreEqual(validRowsEnumerator.Current.Samples, rowsEnumerator.Current.Samples);
            }
        }

        [TestCaseSource(typeof(ExcelDataImportSource), "EmptyFile")]
        public void LoadAll_EmptyFile_ReturnEmptyRows(ExcelSettings settings, int rowsNum)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            var rows = importer.LoadAll();

            // assert
            Assert.AreEqual(rowsNum, rows.Count());
        }

        [TestCaseSource(typeof(ExcelDataImportSource), "InvalidSamplesFiles")]
        public void LoadAll_InvalidSamples_ReturnRowsWithInvalidSamplesAsNaN(ExcelSettings settings, IEnumerable<Row> validRows)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            var rows = importer.LoadAll();

            // assert
            Assert.AreEqual(validRows.Count(), rows.Count());
            var validRowsEnumerator = validRows.GetEnumerator();
            var rowsEnumerator = rows.GetEnumerator();

            while (validRowsEnumerator.MoveNext() && rowsEnumerator.MoveNext())
            {
                Assert.AreEqual(validRowsEnumerator.Current.Timestamp, rowsEnumerator.Current.Timestamp);
                CollectionAssert.AreEqual(validRowsEnumerator.Current.Samples, rowsEnumerator.Current.Samples);
            }
        }

        [TestCaseSource(typeof(ExcelDataImportSource), "InvalidTimestampsFiles")]
        public void LoadAll_InvalidTimestamps_ReturnRowsWithInvalidSamplesAsNaN(ExcelSettings settings, IEnumerable<Row> validRows)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            var rows = importer.LoadAll();

            // assert
            Assert.AreEqual(validRows.Count(), rows.Count());
            var validRowsEnumerator = validRows.GetEnumerator();
            var rowsEnumerator = rows.GetEnumerator();

            while (validRowsEnumerator.MoveNext() && rowsEnumerator.MoveNext())
            {
                Assert.AreEqual(validRowsEnumerator.Current.Timestamp, rowsEnumerator.Current.Timestamp);
                CollectionAssert.AreEqual(validRowsEnumerator.Current.Samples, rowsEnumerator.Current.Samples);
            }
        }

        [TestCaseSource(typeof(ExcelDataImportSource), "DuplicatedTimestampsFiles")]
        public void LoadAll_DuplicatedTimestamps_ReturnRowsWithInvalidSamplesAsNaN(ExcelSettings settings, IEnumerable<Row> validRows)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            var rows = importer.LoadAll();

            // assert
            Assert.AreEqual(validRows.Count(), rows.Count());
            var validRowsEnumerator = validRows.GetEnumerator();
            var rowsEnumerator = rows.GetEnumerator();

            while (validRowsEnumerator.MoveNext() && rowsEnumerator.MoveNext())
            {
                Assert.AreEqual(validRowsEnumerator.Current.Timestamp, rowsEnumerator.Current.Timestamp);
                CollectionAssert.AreEqual(validRowsEnumerator.Current.Samples, rowsEnumerator.Current.Samples);
            }
        }

        [TestCaseSource(typeof(ExcelDataImportSource), "NotExistingFile")]
        public void LoadAll_FileDoesNotExist_ThrowFileNotFoundException(ExcelSettings settings)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            TestDelegate action = () => importer.LoadAll();

            // assert
            Assert.Throws<FileNotFoundException>(action);
        }
        #endregion

        [TestCaseSource(typeof(ExcelDataImportSource), "SpecifiedRowsRangeWithValidFile")]
        public void Load_ValidFile_ReturnSpecifiedRowsRange(ExcelSettings settings, IEnumerable<Row> validRows, int skip, int take)
        {
            // arrange
            ExcelDataImport importer = new ExcelDataImport(settings);

            // act
            var rows = importer.Load(skip, take);

            // assert
            Assert.AreEqual(validRows.Count(), rows.Count());
            CollectionAssert.AreEqual(validRows, rows, new RowComparer());
            var validRowsEnumerator = validRows.GetEnumerator();
            var rowsEnumerator = rows.GetEnumerator();

            while (validRowsEnumerator.MoveNext() && rowsEnumerator.MoveNext())
            {
                Assert.AreEqual(validRowsEnumerator.Current.Timestamp, rowsEnumerator.Current.Timestamp);
                CollectionAssert.AreEqual(validRowsEnumerator.Current.Samples, rowsEnumerator.Current.Samples);
            }
        }
    }
}
