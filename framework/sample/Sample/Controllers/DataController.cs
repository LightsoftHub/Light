using Light.Extensions;
using Microsoft.AspNetCore.Mvc;
using Sample.Data;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "Group1")]
    public class DataController(
        IUnitOfWork<AlphaDbContext> unitOfWork,
        IRepository<RetailLocation, AlphaDbContext> locationRepo,
        ILogger<DataController> logger) : ControllerBase
    {
        [HttpGet("locations")]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var data = await unitOfWork.Repository<RetailLocation>().ToListAsync(cancellationToken);
            return Ok(data);
        }

        [HttpGet("locations/{id}")]
        public async Task<IActionResult> GetAsync(string id, CancellationToken cancellationToken)
        {
            var data = await unitOfWork.Repository<RetailLocation>().FindByKeyAsync(id, cancellationToken);

            data.ThrowIfNull();

            return Ok(data);
        }

        [HttpPut("locations")]
        public async Task<IActionResult> PutAsync([FromBody] RetailLocation location, CancellationToken cancellationToken)
        {
            /*
            var data = await unitOfWork.Repository<RetailLocation>().FindByKeyAsync(location.Code, cancellationToken);
            data!.Name = location.Name;
            data.Phone = location.Phone;
            data.Address = location.Address;
            data.City = location.City;

            //var data2 = await _locationRepo.FindByKeyAsync("222", cancellationToken);
            //data2!.Name = "----222";

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            */
            await Task.Delay(10, cancellationToken);
            return Ok(location);
        }
    }
}
