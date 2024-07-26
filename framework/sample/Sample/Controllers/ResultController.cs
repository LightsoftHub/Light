using Light.AspNetCore.Hosting.Extensions;
using Light.Contracts;
using Light.Extensions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            //var res = Result.NotFound("Error message");
            var res = new Result { Code = "ABC", Message = "" };
            return res.ToActionResult();
        }

        [HttpGet("find")]
        public IActionResult FindValue()
        {
            var model = _list.Select(s => s.ToString()).FirstOrDefault(x => x == "A");
            return Result<string>.Success(model).ToActionResult();
        }


        [HttpGet("paged")]
        public IActionResult GetPaged(int page = 1, int pageSize = 10)
        {
            return _list.ToPagedResult(page, pageSize).ToActionResult();
        }

        [HttpGet("mapper-paged")]
        public IActionResult GetMapperPaged(int page, int size)
        {
            var paged = _list.ToPagedResult(page, size);
            var result = paged.Adapt<PagedResult<int>>();
            return result.ToActionResult();
        }

        [HttpGet("deserialize-paged")]
        public IActionResult DeserializePaged(int page, int size)
        {
            var list = _list.ToPagedResult(page, size);
            var json = JsonSerializer.Serialize(list);

            var result = JsonSerializer.Deserialize<PagedResult<int>>(json);

            return result!.ToActionResult();
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            var error = Result.Error("Error1");
            var errorT = Result<string>.Error("Error1");

            return Ok(new { error, errorT });
        }
    }
}
