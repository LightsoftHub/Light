using System.Collections.Generic;
using System.IO;

namespace Light.File.Csv
{
    public interface ICsvService
    {
        string[]? ReadHeaders(StreamReader streamReader);

        string[]? ReadHeaders(Stream stream);

        IEnumerable<T> ReadAs<T>(StreamReader streamReader);

        IEnumerable<T> ReadAs<T>(Stream stream);

        CsvData<T> Read<T>(StreamReader streamReader);

        CsvData<T> Read<T>(Stream stream);

        IList<IDictionary<string, object?>> ReadAsDictionary(StreamReader streamReader);

        IList<IDictionary<string, object?>> ReadAsDictionary(Stream stream);

        Stream Write<T>(IEnumerable<T> records);
    }
}
