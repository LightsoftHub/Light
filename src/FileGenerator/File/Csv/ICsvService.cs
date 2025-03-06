using System.Collections.Generic;
using System.IO;

namespace Light.File.Csv
{
    public interface ICsvService
    {
        IEnumerable<T> ReadAs<T>(StreamReader streamReader);

        IEnumerable<T> ReadAs<T>(Stream streamData);

        Stream Write<T>(IEnumerable<T> records);
    }
}
