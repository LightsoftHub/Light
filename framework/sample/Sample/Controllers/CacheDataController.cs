using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Sample.Data;
using Sample.Data.Repository;

namespace Sample.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "Group1")]
    [Route("[controller]")]
    public class CacheDataController(
        ICacheRepository<RetailLocation, AlphaDbContext> cacheRepository) : ControllerBase
    {
        private readonly ICacheRepository<RetailLocation, AlphaDbContext> _cacheRepository = cacheRepository;

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var list = await _cacheRepository.ToListAsync(cancellationToken);
            return Ok(list);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(CancellationToken cancellationToken)
        {
            var list = await _cacheRepository.ToListAsync(cancellationToken);

            var bon = list.Where(x => x.Code == "BonGrocer").First();
            bon.Name = "______";

            _cacheRepository.Update(bon);
            await _cacheRepository.SaveChangesAsync(cancellationToken);

            return Ok(list);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(CancellationToken cancellationToken)
        {
            await _cacheRepository.RemoveCacheAsync(cancellationToken);
            return Ok();
        }
    }
}
