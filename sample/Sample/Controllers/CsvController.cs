using CsvHelper.Configuration.Attributes;
using Light.File.Csv;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CsvController(ICsvService csvService) : ControllerBase
    {
        private readonly string _path = Path.Combine(@"D:\\", "Files", "Adobe_aswDM50210_20250311182339.csv");

        [HttpGet("read")]
        public IActionResult Get(string fileName)
        {
            var path = Path.Combine(@"D:\\", "Files", $"{fileName}.csv");

            var stream = new StreamReader(path);

            var dt = csvService.Read(stream);

            return Ok(dt);
        }

        [HttpGet("read_as")]
        public IActionResult ReadAs(string fileName)
        {
            var path = Path.Combine(@"D:\\", "Files", $"{fileName}.csv");

            var stream = new StreamReader(path);

            var dt = csvService.Read<CsvObject>(stream);

            return Ok(dt);
        }

        [HttpGet("export")]
        public IActionResult Write()
        {
            var stream = new StreamReader(_path);

            var dt = csvService.ReadAs<CsvObject>(stream);

            var write = csvService.Write(dt);

            return File(write, "application/octet-stream", "DataExport.csv"); // returns a FileStreamResult
        }
    }

    public class CsvObject
    {
        [Index(0)]
        public long BroadLogRcpId { get; set; }

        [Index(1)]
        public long Visible_Card { get; set; }

        [Index(2)]
        public string C_MOBILE { get; set; } = null!;

        [Index(3)]
        public string C_NAME { get; set; } = null!;

        public object? Test { get; set; }
    }
}


