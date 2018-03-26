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
    public class AsciiDataImportSource
    {
        static AsciiSettings CreateAsciiSettings(string fileName, char colDelimiter = ';', string dateTimeFormat = "yyyy-MM-dd HH:mm:ss", string numberDelimiter = ",", int skipFirstRowsNum = 1, bool useFirstRowAsHeader = true)
        {
            return new AsciiSettings()
            {
                ColDelimiter = colDelimiter,
                DateTimeFormat = dateTimeFormat,
                FileName = @"AsciiDataImportTest/TestFiles/" + fileName,
                NumberDelimiter = numberDelimiter,
                SkipFirstRowsNum = skipFirstRowsNum,
                UseFirstRowAsHeader = useFirstRowAsHeader
            };
        }

        public static IEnumerable ValidAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("samples.csv");
                    var headers = new List<object>() { "timestamp", "variable 1", "variable 2", "variable 3" };
                    yield return new TestCaseData(settings, headers);
                }
            }
        }
        public static IEnumerable NotExisitngFileAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("invalidFile.csv");
                    yield return new TestCaseData(settings);
                }
            }
        }
        public static IEnumerable FirstRowAsHeaders
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("samples.csv");
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
                    var settings = CreateAsciiSettings("samplesWithoutHeaders.csv", useFirstRowAsHeader: false );
                    var headers = new List<object>() { "timestamp", "C1", "C2", "C3" };
                    yield return new TestCaseData(settings, headers);
                }
            }
        }
        public static IEnumerable EmptyFilesAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("empty.csv");
                    yield return new TestCaseData(settings, 0);
                }
            }
        }
        public static IEnumerable ReturnSpecificRowsRangeFromFile
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("samples.csv");
                    yield return new TestCaseData(settings, 10);
                }
            }
        }
        public static IEnumerable SkipAllRows
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("samples.csv");
                    yield return new TestCaseData(settings, 100, 10);
                }
            }
        }
        public static IEnumerable ColumnDelimiterAsElementofDateTimeFormatAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("ColumnDelimiterTheSameAsNumberDelimiter.csv", colDelimiter: ':');
                    yield return new TestCaseData(settings, 10);
                }
            }
        }
        public static IEnumerable ColumnDelimiterTheSameAsNumberDelimiterAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("ColumnDelimiterTheSameAsNumberDelimiter.csv", colDelimiter: ',');
                    yield return new TestCaseData(settings, 10);
                }
            }
        }
        public static IEnumerable FileWithColumnDelimiterTheSameAsNumberDelimiterAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("ColumnDelimiterTheSameAsNumberDelimiter.csv", colDelimiter: ',');
                    yield return new TestCaseData(settings, 10);
                }
            }
        }

        public static IEnumerable InvalidDateTimeFormatAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("samples.csv", dateTimeFormat: "yyyy-MM-dd");
                    yield return new TestCaseData(settings, 10);
                }
            }
        }
        public static IEnumerable InvalidColumnDelimitersAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("samples.csv", colDelimiter: '/');
                    yield return new TestCaseData(settings, 10);
                }
            }
        }
        public static IEnumerable InvalidNumberDelimitertAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("samples.csv", colDelimiter: '.');
                    yield return new TestCaseData(settings, 10);
                }
            }
        }
        public static IEnumerable InvalidNumberFormatAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("InvalidSamples.csv", colDelimiter: '.');
                    yield return new TestCaseData(settings, 10);
                }
            }
        }

        public static IEnumerable SamplesWithEmptyLinesAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("SamplesWithEmptyLines.csv");
                    yield return new TestCaseData(settings, 93);
                }
            }
        }
        public static IEnumerable ColumnDelimitersAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("SemicolonDelimiter.csv", colDelimiter: ';');
                    yield return new TestCaseData(settings, 10, 3);
                }
                {
                    var settings = CreateAsciiSettings("PipeDelimiter.csv", colDelimiter: '|');
                    yield return new TestCaseData(settings, 10, 3);
                }
            }
        }
        public static IEnumerable SkipAllRowsAsciiSettings
        {
            get
            {
                {
                    var settings = CreateAsciiSettings("samples.csv");
                    yield return new TestCaseData(settings, 200, 10);
                }
            }
        }
    }
}
