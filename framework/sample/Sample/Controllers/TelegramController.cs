/*
namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly ITelegramService _botService;

        public TelegramController(ITelegramService botService)
        {
            _botService = botService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _botService.SendMessage("123");

            return Ok();
        }
    }
}
*/