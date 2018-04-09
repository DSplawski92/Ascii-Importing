using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS.DataImporter;
using DS.AsciiImport;

namespace UnitTestProject
{
    public class DataMergerSource
    {
        static AsciiSettings CreateAsciiSettings(string fileName, string colDelimiter = ";", string dateTimeFormat = "dd.MM.yyyy HH:mm:ss", string numberDelimiter = ",", int skipFirstRowsNum = 0, bool useFirstRowAsHeader = true)
        {
            return new AsciiSettings()
            {
                ColumnDelimiter = colDelimiter,
                DateTimeFormat = dateTimeFormat,
                FileName = @"DataMergerTest/TestFiles/" + fileName,
                NumberDelimiter = numberDelimiter,
                SkipFirstRowsNum = skipFirstRowsNum,
                UseFirstRowAsHeader = useFirstRowAsHeader
            };
        }

        public static IEnumerable SomeCommonTimestamps
        {
            get
            {
                {
                    var settings = new List<AsciiSettings> { CreateAsciiSettings("samples.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss") };
                    int mergedTimestampsNum = 7, samplesNum = 21, columnsNum = 1;
                    yield return new TestCaseData(settings, mergedTimestampsNum, samplesNum, columnsNum).SetName("Merging Valid data with valid configuration");
                }
                {
                    var settings = new List<AsciiSettings>
                    {
                        CreateAsciiSettings("samples.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                        CreateAsciiSettings("samples2.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss" )
                    };
                    yield return new TestCaseData(settings, 14, 70, 5).SetName("Merging Valid data without common timestamps");
                }
                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("samples.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                        CreateAsciiSettings("samples3.csv", dateTimeFormat: "yyyy.MM.dd HH:mm:ss")
                    };
                    yield return new TestCaseData(settings, 11, 55, 5); 
                }

                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("samples2.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                        CreateAsciiSettings("samples2.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss")
                    };
                    yield return new TestCaseData(settings, 7, 28, 2);
                }
                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("samples.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                        CreateAsciiSettings("samples2.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                        CreateAsciiSettings("samples3.csv", dateTimeFormat: "yyyy.MM.dd HH:mm:ss")
                    };
                    yield return new TestCaseData(settings, 14, 98, 7);
                }
                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("samples2.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                        CreateAsciiSettings("samples3.csv", dateTimeFormat: "yyyy.MM.dd HH:mm:ss"),
                    };
                    yield return new TestCaseData(settings, 7, 28, 4);
                }
            }
        }
        public static IEnumerable InvalidConfiguration
        {
            get
            {
                var settings = new List<AsciiSettings>()
                {
                    CreateAsciiSettings("shortValidSamples.csv", colDelimiter: "/"),
                    CreateAsciiSettings("shortValidSamples2.csv", colDelimiter: "/")
                };
                yield return new TestCaseData(settings);
            }
        }
        public static IEnumerable DataWithEmptyLines
        {
            get
            {
                var settings = new List<AsciiSettings>()
                {
                    CreateAsciiSettings("shortValidSamples.csv", skipFirstRowsNum: 1),
                    CreateAsciiSettings("DataWithEmptyLines.csv", skipFirstRowsNum: 1)
                };
                yield return new TestCaseData(settings, 12);
            }
        }
        public static IEnumerable InvalidTimestamp
        {
            get
            {
                var settings = new List<AsciiSettings>()
                {
                    CreateAsciiSettings("shortValidSamples.csv", skipFirstRowsNum: 1),
                    CreateAsciiSettings("invalidTimestamps.csv", skipFirstRowsNum: 1)
                };
                yield return new TestCaseData(settings, 10);
            }
        }
        public static IEnumerable InvalidSamples
        {
            get
            {
                var settings = new List<AsciiSettings>()
                {
                    CreateAsciiSettings("invalidSamples.csv", skipFirstRowsNum: 1),
                    CreateAsciiSettings("shortValidSamples.csv", skipFirstRowsNum: 1)
                };
                yield return new TestCaseData(settings, 12, 3);
            }
        }
        public static IEnumerable FileUsedMoreThanOnce
        {
            get
            {
                var settings = new List<AsciiSettings>()
                {
                    CreateAsciiSettings("shortValidSamples.csv", skipFirstRowsNum: 1),
                    CreateAsciiSettings("shortValidSamples.csv", skipFirstRowsNum: 1)
                };
                yield return new TestCaseData(settings, 10);
            }
        }
        public static IEnumerable FilesWithDifferentConfigurations
        {
            get
            {
                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("shortValidSamples.csv", skipFirstRowsNum: 1),
                        CreateAsciiSettings("onlySamples.csv", colDelimiter: "|", dateTimeFormat: "dd-MM-yyyy HH:mm:ss", numberDelimiter: ".", skipFirstRowsNum: 0, useFirstRowAsHeader: false)
                    };
                    yield return new TestCaseData(settings, 15);
                }
            }
        }
        public static IEnumerable SpecifiedRange
        {
            get
            {
                var settings = new List<AsciiSettings>()
                {
                    CreateAsciiSettings("samples.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                    CreateAsciiSettings("samples2.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss")
                };
                yield return new TestCaseData(settings, 7, 5, 5);
            }
        }
        public static IEnumerable ValidHeadersFiles
        {
            get
            {
                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("samples.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                    };
                    var headers = new string[] { "timestamp", "variable 1", "variable 2", "variable 3" };
                    yield return new TestCaseData(settings, headers);
                }
                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("samples.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                        CreateAsciiSettings("samples2.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                    };
                    var headers = new string[] { "timestamp", "variable 1", "variable 2", "variable 3", "sin", "cos" };
                    yield return new TestCaseData(settings, headers);
                }

            }
        }
        public static IEnumerable GenericHeaders
        {
            get
            {
                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("noheaders1.csv", useFirstRowAsHeader: false, skipFirstRowsNum: 1),
                    };
                    var headers = new string[] { "timestamp", "C1" };
                    yield return new TestCaseData(settings, headers);
                }
                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("noheaders1.csv", useFirstRowAsHeader: false, skipFirstRowsNum: 1),
                        CreateAsciiSettings("noheaders2.csv", useFirstRowAsHeader: false, skipFirstRowsNum: 1)
                    };
                    var headers = new string[] { "timestamp", "C1", "C1" };
                    yield return new TestCaseData(settings, headers);
                }
            }
        }
        public static IEnumerable MixedHeaders
        {
            get
            {
                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("noheaders1.csv", useFirstRowAsHeader: false, skipFirstRowsNum: 1),
                        CreateAsciiSettings("samples.csv", dateTimeFormat: "yyyy-MM-dd HH:mm:ss"),
                    };
                    var headers = new string[] { "timestamp", "C1", "variable 1", "variable 2", "variable 3" };
                    yield return new TestCaseData(settings, headers);
                }
                {
                    var settings = new List<AsciiSettings>()
                    {
                        CreateAsciiSettings("noheaders1.csv", useFirstRowAsHeader: false, skipFirstRowsNum: 1),
                        CreateAsciiSettings("noheaders2.csv", useFirstRowAsHeader: false, skipFirstRowsNum: 1)
                    };
                    var headers = new string[] { "timestamp", "C1", "C1" };
                    yield return new TestCaseData(settings, headers);
                }
            }
        }
    }
}
