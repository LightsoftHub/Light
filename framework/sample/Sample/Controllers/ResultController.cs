using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Light.Models;
using Light.Extensions;
using Light.AspNetCore.Hosting.Authorization;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly List<int> _list;

        public ResultController()
        {
            var list = new List<int>();
            for (int i = 0; i < 20; i++)
                list.Add(i);

            _list = list;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var auth = HttpContext.Request.ReadBasicAuthorization();
            Console.WriteLine(auth);

            return Ok(_list.ToPagedResult(6, 5));
        }

        [HttpGet("mapper-list")]
        public IActionResult GetPaged(int page, int size)
        {
            var paged = _list.ToPagedResult(page, size);
            var result = paged.Adapt<PagedResult<int>>();
            return Ok(result);
        }

        [HttpGet("deserialize-list")]
        public IActionResult DeserializePaged(int page, int size)
        {
            var list = _list.ToPagedResult(page, size);
            var json = JsonSerializer.Serialize(list);

            var result = JsonSerializer.Deserialize<PagedResult<int>>(json);

            return Ok(result);
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            var error = Result.Error("Error1", "Error2", "Error3", "Error4");
            var errorT = Result<string>.Error("Error1", "Error2", "Error3", "Error4");

            return Ok(new { error, errorT });
        }

        [HttpGet("success")]
        public IActionResult GetSuccess()
        {
            return Ok(Result.Success());
        }
    }
}
