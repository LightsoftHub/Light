using CsvHelper;
using CsvHelper.Configuration;
using Light.File.Csv;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Light.Infrastructure.Csv
{
    public class CsvService : ICsvService
    {
        private readonly CsvConfiguration _config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            HeaderValidated = null,   // Skips missing headers during mapping
            MissingFieldFound = null, // Skips missing *fields* in rows
        };

        public string[]? ReadHeaders(StreamReader streamReader)
        {
            using var csv = new CsvReader(streamReader, _config);

            csv.Read();
            csv.ReadHeader();

            return csv.HeaderRecord;
        }

        public string[]? ReadHeaders(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return ReadHeaders(reader);
        }

        public IEnumerable<T> ReadAs<T>(StreamReader streamReader)
        {
            using var csv = new CsvReader(streamReader, _config);

            csv.Context.TypeConverterCache.AddConverter<object>(new ObjectConverter());

            foreach (var csvRecord in csv.GetRecords<T>())
            {
                yield return csvRecord;
            }
        }

        public IEnumerable<T> ReadAs<T>(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return ReadAs<T>(reader);
        }

        public CsvData<T>? Read<T>(StreamReader streamReader)
        {
            using var csv = new CsvReader(streamReader, _config);

            csv.Context.TypeConverterCache.AddConverter<object>(new ObjectConverter());

            var records = csv.GetRecords<T>().ToList();
            var headers = csv.HeaderRecord;

            if (headers is null)
            {
                return null;
            }

            return new CsvData<T>
            {
                Headers = headers,
                Rows = records
            };
        }

        public CsvData<T>? Read<T>(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return Read<T>(reader);
        }

        public DictionaryData? Read(StreamReader streamReader)
        {
            using var csv = new CsvReader(streamReader, _config);

            csv.Context.TypeConverterCache.AddConverter<object>(new ObjectConverter());

            csv.Read();         // Read the first row to get the headers
            csv.ReadHeader();   // Read headers

            var headers = csv.HeaderRecord; // Get header names

            if (headers is null)
            {
                return null;
            }

            var rows = new List<IDictionary<string, object?>>();

            while (csv.Read()) // Read each row
            {
                var row = new Dictionary<string, object?>();

                foreach (var header in headers)
                {
                    row[header] = csv.GetField(header); // Get value by column name
                }

                rows.Add(row);
            }

            return new DictionaryData
            {
                Headers = headers,
                Rows = rows
            };
        }

        public DictionaryData? Read(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return Read(reader);
        }

        public Stream Write<T>(IEnumerable<T> records, bool excludeHeader = false)
        {
            var memoryStream = new MemoryStream();

            var writer = new StreamWriter(memoryStream, Encoding.UTF8);

            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            if (excludeHeader is false)
            {
                csv.WriteHeader<T>();
                csv.NextRecord();
            }

            foreach (var record in records)
            {
                csv.WriteRecord(record);
                csv.NextRecord();
            }

            writer.Flush(); // Ensure all data is written to the stream

            memoryStream.Seek(0, SeekOrigin.Begin); // Alternative to memoryStream.Position = 0, Reset stream position for reading

            return memoryStream;
        }
    }
}
