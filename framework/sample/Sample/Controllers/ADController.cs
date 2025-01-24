using Light.ActiveDirectory.Interfaces;
using Light.Contracts;
using Light.Exceptions;
using Light.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ADController(IActiveDirectoryService activeDirectoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(string user)
        {
            return Ok(await activeDirectoryService.GetByUserNameAsync(user));
        }

        [HttpGet("check_password")]
        public async Task<IActionResult> CheckPassword(string user, string password)
        {
            return Ok(await activeDirectoryService.CheckPasswordSignInAsync(user, password));
        }
    }
}
