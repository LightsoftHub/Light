using CsvHelper;
using Light.File.Csv;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Light.Infrastructure.Csv
{
    public class CsvService : ICsvService
    {
        public IEnumerable<T> ReadAs<T>(StreamReader streamReader)
        {
            using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            foreach (var csvRecord in csv.GetRecords<T>())
            {
                yield return csvRecord;
            }
        }

        public IEnumerable<T> ReadAs<T>(Stream streamData)
        {
            using var reader = new StreamReader(streamData);
            return ReadAs<T>(reader);
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
