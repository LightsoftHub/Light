using CsvHelper.Configuration.Attributes;
using Light.Extensions;
using Light.File.Csv;
using Light.File.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CsvController(ICsvService csvService) : ControllerBase
    {
        private readonly string _path = Path.Combine(@"D:\\", "Files", "Adobe_aswDM50210_20250311182339.csv");

        [HttpGet("headers")]
        public IActionResult Headers()
        {
            var stream = new StreamReader(_path);

            var dt = csvService.ReadHeaders(stream);

            return Ok(dt);
        }

        [HttpGet("read")]
        public IActionResult Read()
        {
            var stream = new StreamReader(_path);

            var dt = csvService.ReadAs<CsvObject>(stream);

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

        public string C_NAME { get; set; } = null!;

        [Name("Member_Point_Balance (1)")]
        public int Member_Point_Balance { get; set; }
    }
}


