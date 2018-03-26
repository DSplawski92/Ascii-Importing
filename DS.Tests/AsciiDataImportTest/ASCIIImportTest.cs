using DS.DataImporter;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using DS.AsciiImport;

namespace UnitTestProject
{
    [TestFixture]
    public class ASCIIImportTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        #region GetHeaders Tests
        [TestCaseSource(typeof(AsciiDataImportSource), "FirstRowAsHeaders")]
        public void GetHeaders_FirstRowAsHeaders_ReturnHeadersFromFirstRow(AsciiSettings ValidAsciiSettings, IEnumerable<object> validHeaders)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(ValidAsciiSettings);

            // act
            var headers = asciiImport.GetHeaders();

            // assert
            CollectionAssert.AreEqual(validHeaders, headers);
            Assert.AreEqual(validHeaders.Count(), headers.Count());
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "GenericHeaders")]
        public void GetHeaders_FileWithoutHeadersinFirstRow_ReturnGenericHeaders(AsciiSettings GenericHeadersAsciiSettings, IEnumerable<object> genericHeaders)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(GenericHeadersAsciiSettings);
            
            // act
            var headers = asciiImport.GetHeaders().ToList();

            // assert
            CollectionAssert.AreEqual(genericHeaders, headers);
            Assert.AreEqual(genericHeaders.Count(), headers.Count());
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "EmptyFilesAsciiSettings")]
        public void GetHeaders_EmptyFileAndUseFirstRowAsHeaders_ReturnEmptyHeaders(AsciiSettings EmptyFilesAsciiSettings, int rowsNum)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(EmptyFilesAsciiSettings);

            // act
            var headers = asciiImport.GetHeaders();

            // assert
            Assert.AreEqual(rowsNum, headers.Count(), "The returned headers are not empty.");
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "NotExisitngFileAsciiSettings")]
        public void GetHeaders_FileDoesNotExist_ThrowsException(AsciiSettings NotExisitngFileAsciiSettings)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(NotExisitngFileAsciiSettings);

            // act
            TestDelegate action = () => asciiImport.GetHeaders();

            // assert
            Assert.Throws<FileNotFoundException>(action, "Expected exception was not thrown.");
        }
        #endregion

        #region LoadAll Tests
        [TestCaseSource(typeof(AsciiDataImportSource), "ValidAsciiSettings")]
        public void LoadAll_ValidFile_ReturnAllRowsFromFile(AsciiSettings ValidAsciiSettings, IEnumerable<object> validHeaders)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(ValidAsciiSettings);
            int headersNum, samplesNum;

            // act
            var rows = asciiImport.LoadAll();
            var headers = asciiImport.GetHeaders();
            samplesNum = rows.First().Samples.Count();
            headersNum = headers.Count() - 1; // Timestamp is not sample

            CollectionAssert.AreEqual(validHeaders, headers);

            // assert
            Assert.AreEqual(1, rows.GroupBy(arg => arg.Samples.Count()).Count(), "Rows have a different number of columns.");
            Assert.AreEqual(samplesNum, headersNum, $"The number of samples ({samplesNum}) is different from the number of headers ({headersNum}).");
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "EmptyFilesAsciiSettings")]
        public void LoadAll_EmptyFile_ReturnEmpty(AsciiSettings EmptyFilesAsciiSettings, int rowsNum)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(EmptyFilesAsciiSettings);

            // act
            var rows = asciiImport.LoadAll();

            // assert
            Assert.AreEqual(rowsNum, rows.Count());
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "SamplesWithEmptyLinesAsciiSettings")]
        public void LoadAll_SamplesFileWithEmptyLines_SkipEmptyLines(AsciiSettings SamplesWithEmptyLinesAsciiSettings, int rowsWithoutEmpyLinesNum)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(SamplesWithEmptyLinesAsciiSettings);

            // act
            var rows = asciiImport.LoadAll().ToList();

            // assert
            Assert.AreEqual(rowsWithoutEmpyLinesNum, rows.Count);
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "NotExisitngFileAsciiSettings")]
        // [ExpectedException(typeof(FileNotFoundException), "Expected exception was not thrown.")]
        public void LoadAll_FileDoesNotExist_ThrowsException(AsciiSettings NotExisitngFileAsciiSettings)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(NotExisitngFileAsciiSettings);

            // act
            TestDelegate action = () => asciiImport.LoadAll();

            // assert
            Assert.Throws<FileNotFoundException>(action);
        }
        #endregion

        #region Load Tests
        [TestCaseSource(typeof(AsciiDataImportSource), "ReturnSpecificRowsRangeFromFile")]
        public void Load_ValidFile_ReturnSpecificRowsRangeFromFile(AsciiSettings ReturnSpecificRowsRangeFromFile, int rowsNum)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(ReturnSpecificRowsRangeFromFile);

            // act
            var rows = asciiImport.Load(0, rowsNum);

            // assert
            Assert.AreEqual(rowsNum, rows.Count());
            //Check if all rows have the same columns number
            Assert.AreEqual(1, rows.GroupBy(arg => arg.Samples.Count()).Count());
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "ColumnDelimitersAsciiSettings")]
        public void Load_ValidColumnDelimiter_ReturnValidRows(AsciiSettings ColumnDelimitersAsciiSettings, int rowsNum, int columnsNum)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(ColumnDelimitersAsciiSettings);

            // act
            var rows = asciiImport.Load(0, rowsNum);

            // assert
            Assert.AreEqual(rowsNum, rows.Count());
            Assert.AreEqual(1, rows.GroupBy(arg => arg.Samples.Count()).Count());
            Assert.AreEqual(columnsNum, rows.ElementAt(0).Samples.Count());
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "ColumnDelimiterTheSameAsNumberDelimiterAsciiSettings")]
        public void Load_SettingsWithColumnDelimiterTheSameAsNumberDelimiter_ThrowsException(AsciiSettings ColumnDelimiterTheSameAsNumberDelimiterAsciiSettings, int rows)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(ColumnDelimiterTheSameAsNumberDelimiterAsciiSettings);

            // act
            TestDelegate action = () => asciiImport.Load(0, rows);

            // assert
            Assert.Throws<ArgumentException>(action, "Expected exception was not thrown.");
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "FileWithColumnDelimiterTheSameAsNumberDelimiterAsciiSettings")]
        public void Load_FileWithColumnDelimiterTheSameAsNumberDelimiter_ThrowsException(AsciiSettings FileWithColumnDelimiterTheSameAsNumberDelimiterAsciiSettings, int rows)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(FileWithColumnDelimiterTheSameAsNumberDelimiterAsciiSettings);

            // act
            TestDelegate action = () => asciiImport.Load(0, rows).ToList().ForEach(arg => arg.Samples.ToList());

            // assert
            Assert.Throws<ArgumentException>(action, "Expected exception was not thrown.");
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "InvalidColumnDelimitersAsciiSettings")]
        public void Load_InvalidColumnDelimiter_ThrowsException(AsciiSettings InvalidColumnDelimitersAsciiSettings, int rows)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(InvalidColumnDelimitersAsciiSettings);

            // act
            TestDelegate action = () => asciiImport.Load(0, rows).ToList();

            // assert
            Assert.Throws<FormatException>(action, "Expected exception was not thrown.");
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "InvalidColumnDelimitersAsciiSettings")]
        public void Load_InvalidNumberDelimiter_ThrowsException(AsciiSettings InvalidNumberDelimitertAsciiSettings, int rows)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(InvalidNumberDelimitertAsciiSettings);

            // act
            TestDelegate action = () => asciiImport.Load(0, rows).ToList();

            // assert
            Assert.Throws<FormatException>(action, "Expected exception was not thrown.");
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "InvalidNumberFormatAsciiSettings")]
        public void Load_InvalidNumberFormat_ThrowsException(AsciiSettings InvalidNumberFormatAsciiSettings, int rows)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(InvalidNumberFormatAsciiSettings);

            // act
            TestDelegate action = () => asciiImport.Load(0, rows).ToList().ForEach(arg => arg.Samples.ToList());

            // assert
            Assert.Throws<FormatException>(action, "Expected exception was not thrown.");
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "InvalidDateTimeFormatAsciiSettings")]
        public void Load_InvalidDateTimeFormat_ThrowsException(AsciiSettings InvalidDateTimeFormatAsciiSettings, int rows)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(InvalidDateTimeFormatAsciiSettings);

            // act
            TestDelegate action = () => asciiImport.Load(0, rows).ToList();

            // assert
            Assert.Throws<FormatException>(action, "Expected exception was not thrown.");
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "ColumnDelimiterAsElementofDateTimeFormatAsciiSettings")]
        public void Load_ColumnDelimiterAsElementofDateTimeFormat_ThrowsException(AsciiSettings ColumnDelimiterAsElementofDateTimeFormatAsciiSettings, int rows)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(ColumnDelimiterAsElementofDateTimeFormatAsciiSettings);

            // act
            TestDelegate action = () => asciiImport.Load(0, rows).ToList();

            // assert
            Assert.Throws<ArgumentException>(action, "Expected exception was not thrown.");
        }

        [TestCaseSource(typeof(AsciiDataImportSource), "SkipAllRowsAsciiSettings")]
        public void Load_SkipAllRows_ReturnEmpty(AsciiSettings SkipAllRowsAsciiSettings, int skip, int take)
        {
            // arrange
            AsciiDataImport asciiImport = new AsciiDataImport(SkipAllRowsAsciiSettings);
            string failInfo = "Returned collection are invalid.";

            // act
            var rows = asciiImport.Load(skip, take);

            // assert
            Assert.AreEqual(0, rows.Count(), failInfo);
        }
        #endregion

        //[TestCaseSource(typeof(AsciiDataImportSource), "ValidAsciiSettings")]
        //public void GetRow_ValidLine_ReturnValidRow(AsciiSettings ValidAsciiSettings)
        //{
        //    // arrange
        //    string timestamp = "2018-02-10 10:12:23";
        //    string line = timestamp + ";0,12431;0,345345;0,461233";
        //    Row validRow = new Row()
        //    {
        //        Timestamp = new DateTime(2018, 2, 10, 10, 12, 23),
        //        Samples = new[] { 0.12431, 0.345345, 0.461233 }
        //    };

        //    // act
        //    PrivateObject obj = new PrivateObject(new AsciiDataImport(ValidAsciiSettings));
        //    var row = (Row)obj.Invoke("GetRow", line);

        //    // assert
        //    Assert.AreEqual(validRow.Timestamp, row.Timestamp, "Actual data are different from expected.");
        //    Assert.AreEqual(validRow.Samples.Count(), row.Samples.Count(), "Actual number of samples are different from expected.");
        //    CollectionAssert.AreEqual(validRow.Samples, row.Samples);
        //}
    }
}
