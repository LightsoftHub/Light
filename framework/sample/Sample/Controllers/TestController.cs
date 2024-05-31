using Light.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sample.Data;
using Sample.Services;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController(
        IMemoryCache memoryCache,
        AlphaDbContext context,
        IDateTimeService dateTimeService,
        DataService dataService,
        IData1Service data1Service,
        IData2Service data2Service,
        IData3Service data3Service) : ControllerBase
    {
        private readonly string _cacheKey = "Key";

        [HttpGet("mem")]
        public async Task<IActionResult> GetAsync()
        {
            var memObj = await memoryCache.GetOrCreateAsync(_cacheKey,
                async factory =>
                {
                    var data = await context.RetailCategories.FirstAsync();
                    Console.WriteLine("Get data from DB");
                    return data;
                });

            return Ok(memObj);
        }

        [HttpGet("dis")]
        public IActionResult GetDisAsync()
        {
            return Ok();
        }

        [HttpGet("throw")]
        public IActionResult ThrowAsync()
        {
            throw new InternalServerErrorException("Server error");
        }

        [HttpGet("now")]
        public IActionResult GetNowAsync()
        {
            return Ok(dateTimeService.Now);
        }

        [HttpGet("new_id")]
        public IActionResult GetNewIdAsync()
        {
            var obj = new
            {
                Id1 = data1Service.NewId,
                Id2 = data2Service.NewId,
                Id3 = data3Service.NewId,
            };

            return Ok(obj);
        }
    }
}
