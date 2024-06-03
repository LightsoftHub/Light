using System.Collections.Generic;

namespace Light.Files.Excel
{
    public class ExportExcelDataRequest
    {
        public IList<object> Data { get; set; } = null!;

        public string? SheetName { get; set; }
    }
}
