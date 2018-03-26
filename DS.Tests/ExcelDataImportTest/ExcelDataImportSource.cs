using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using DS.ExcelImport;
using DS.Interfaces;

namespace UnitTestProject
{
    public class ExcelDataImportSource
    {
        static ExcelSettings CreateExcelSettings(string fileName, string dateTimeFormat = "dd-MM-yyyy HH:mm:ss", string numberDelimiter = ",", int skipFirstRowsNum = 0, bool useFirstRowAsHeader = true)
        {
            return new ExcelSettings()
            {
                DateTimeFormat = dateTimeFormat,
                FileName = @"ExcelDataImportTest/TestFiles/" + fileName,
                NumberDelimiter = numberDelimiter,
                SkipFirstRowsNum = skipFirstRowsNum,
                UseFirstRowAsHeader = useFirstRowAsHeader
            };
        }

        public static IEnumerable HeadersInFile
        {
            get
            {
                {
                    var settings = CreateExcelSettings("samples.xls");
                    var headers = new List<object>() { "timestamp", "variable 1", "variable 2", "variable 3" };
                    yield return new TestCaseData(settings, headers);
                }
            }
        }
        public static IEnumerable GenericHeaders
        {
            get
            {
                {
                    var settings = CreateExcelSettings("samples.xls", useFirstRowAsHeader: false);
                    var headers = new List<object>() { "timestamp", "C1", "C2", "C3" };
                    yield return new TestCaseData(settings, headers);
                }
            }
        }
        public static IEnumerable EmptyFile
        {
            get
            {
                {
                    var settings = CreateExcelSettings("empty.xls");
                    int rowsNum = 0;
                    yield return new TestCaseData(settings, rowsNum);
                }
            }
        }

        public static IEnumerable ValidExcelFile
        {
            get
            {
                var validRows = new List<Row>()
                {
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 25), Samples = new double[] { 0.1011991198, 0.4766171544, 0.8710178677 }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 27), Samples = new double[] { 0.0994543063, 0.8352792030, 0.5027661952 }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 29), Samples = new double[] { 0.5390439995, 0.6669164386, 0.2144592430 }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 31), Samples = new double[] { 0.4302489003, 0.0161936665, 0.3082652711 }}
                };
                {
                    var settings = CreateExcelSettings("shortSamples.xls", dateTimeFormat: "yyyy-MM-dd HH:mm:ss");
                    #region samples and timestamps
                    var samples = new double[,] 
                    {
                        { 0,10119912,  0,476617154, 0,871017868 },
                        { 0,099454306, 0,835279203, 0,502766195 },
                        { 0,539044,    0,666916439, 0,214459243 },
                        { 0,4302489,   0,016193667, 0,308265271 }
                    };
                    var timestamps = new DateTime[]
                    {
                        new DateTime(2018, 02, 08, 13, 34, 25),
                        new DateTime(2018, 02, 08, 13, 34, 27),
                        new DateTime(2018, 02, 08, 13, 34, 29),
                        new DateTime(2018, 02, 08, 13, 34, 31),                      
                    };
                    #endregion
                   yield return new TestCaseData(settings, validRows).SetName("LoadAll_ValiSamples");
                }
                {
                    var settings = CreateExcelSettings("samplesWithEmptyLineAfterHeaders.xls", dateTimeFormat: "yyyy-MM-dd HH:mm:ss");
                    yield return new TestCaseData(settings, validRows).SetName("LoadAll Samples with empty line after headers");
                }
                {
                    var settings = CreateExcelSettings("samplesWithFirstEmptyLine.xls", dateTimeFormat: "yyyy-MM-dd HH:mm:ss");
                    yield return new TestCaseData(settings, validRows).SetName("LoadAll Samples with empty first line");
                }
            }
        }
        public static IEnumerable InvalidSamplesFiles
        {
            get
            {
                var validRows = new List<Row>()
                {
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 25), Samples = new double[] { double.NaN,   0.4766171544, 0.8710178677 }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 27), Samples = new double[] { 0.0994543063, double.NaN,   0.5027661952 }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 29), Samples = new double[] { 0.5390439995, 0.6669164386, double.NaN   }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 31), Samples = new double[] { double.NaN,   double.NaN,   double.NaN   }}
                };
                {
                    var settings = CreateExcelSettings("invalidSamples.xls", dateTimeFormat: "yyyy-MM-dd HH:mm:ss");
                    yield return new TestCaseData(settings, validRows).SetName("LoadAll invalid samples");
                }
                {
                    var settings = CreateExcelSettings("invalidSamples2.xls", dateTimeFormat: "yyyy-MM-dd HH:mm:ss");
                    yield return new TestCaseData(settings, validRows).SetName("LoadAll invalid samples");
                }
            }
        }
        public static IEnumerable InvalidTimestampsFiles
        {
            get
            {
                var validRows = new List<Row>()
                {
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 27), Samples = new double[] { 0.0994543063, 0.835279203, 0.5027661952 }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 29), Samples = new double[] { 0.5390439995, 0.6669164386, 0.214459243 }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 31), Samples = new double[] { 0.4302489003, 0.0161936665, 0.3082652711 }}
                };
                {
                    var settings = CreateExcelSettings("invalidTimestamps.xls", dateTimeFormat: "yyyy-MM-dd HH:mm:ss");
                    yield return new TestCaseData(settings, validRows).SetName("LoadAll invalid timestamps");
                }
            }
        }
        public static IEnumerable DuplicatedTimestampsFiles
        {
            get
            {
                var validRows = new List<Row>()
                {
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 25), Samples = new double[] { 0.1011991198, 0.4766171544, 0.8710178677 }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 27), Samples = new double[] { 0.0994543063, 0.8352792030, 0.5027661952 }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 29), Samples = new double[] { 0.5390439995, 0.6669164386, 0.2144592430 }},
                    new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 31), Samples = new double[] { 0.4302489003, 0.0161936665, 0.3082652711 }}
                };
                {
                    var settings = CreateExcelSettings("duplicatedTimestamps.xls", dateTimeFormat: "yyyy-MM-dd HH:mm:ss");
                    yield return new TestCaseData(settings, validRows).SetName("LoadAll duplicated timestamps");
                }
            }
        }
        public static IEnumerable SpecifiedRowsRangeWithValidFile
        {
            get
            {
                {
                    var validRows = new List<Row>()
                    {
                        new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 27), Samples = new double[] { 0.0994543063, 0.8352792030, 0.5027661952 }},
                        new Row() { Timestamp = new DateTime(2018, 02, 08, 13, 34, 29), Samples = new double[] { 0.5390439995, 0.6669164386, 0.2144592430 }},
                    };
                    var settings = CreateExcelSettings("shortSamples.xls", dateTimeFormat: "yyyy-MM-dd HH:mm:ss");
                    int skip = 1;
                    int take = 2;
                    yield return new TestCaseData(settings, validRows, skip, take);
                }
                {
                    var validRows = new List<Row>();
                    var settings = CreateExcelSettings("shortSamples.xls", dateTimeFormat: "yyyy-MM-dd HH:mm:ss");
                    int skip = 4;
                    int take = 2;
                    yield return new TestCaseData(settings, validRows, skip, take);
                }
            }
        }
        public static IEnumerable NotExistingFile
        {
            get
            {
                var settings = CreateExcelSettings("notExistingFile.xls");
                yield return new TestCaseData(settings);
            }
        }
    }
}
