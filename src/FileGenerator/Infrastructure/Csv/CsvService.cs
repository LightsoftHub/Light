using CsvHelper;
using Light.File.Csv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Light.Infrastructure.Csv
{
    public class CsvService : ICsvService
    {
        public string[]? ReadHeaders(StreamReader streamReader)
        {
            using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);
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
            using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

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

        public CsvData<T> Read<T>(StreamReader streamReader)
        {
            using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<T>().ToList();
            var headers = csv.HeaderRecord;

            return new CsvData<T>
            {
                Headers = headers,
                Rows = records
            };
        }

        public CsvData<T> Read<T>(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return Read<T>(reader);
        }

        public IList<IDictionary<string, object?>> ReadAsDictionary(StreamReader streamReader)
        {
            using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            csv.Read();          // Read first row
            csv.ReadHeader();    // Read headers

            var rows = new List<IDictionary<string, object?>>();

            var headers = csv.HeaderRecord; // Get header names

            if (headers != null)
            {
                while (csv.Read()) // Read each row
                {
                    var row = new Dictionary<string, object?>();

                    foreach (var header in headers)
                    {
                        row[header] = csv.GetField(header); // Get value by column name
                    }

                    rows.Add(row);
                }
            }

            return rows;
        }

        public IList<IDictionary<string, object?>> ReadAsDictionary(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return ReadAsDictionary(reader);
        }

        public Stream Write<T>(IEnumerable<T> records)
        {
            var memoryStream = new MemoryStream();

            var writer = new StreamWriter(memoryStream, Encoding.UTF8);

            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(records);

            writer.Flush(); // Ensure all data is written to the stream

            memoryStream.Seek(0, SeekOrigin.Begin); // Alternative to memoryStream.Position = 0, Reset stream position for reading

            return memoryStream;
        }
    }
}
