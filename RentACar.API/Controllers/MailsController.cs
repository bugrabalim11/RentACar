
using Microsoft.AspNetCore.Mvc;
using RentACar.Core.Utilities.Mailing;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailsController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] MailRequest mailRequest)
        {
            var result = await _mailService.SendEmailAsync(mailRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
