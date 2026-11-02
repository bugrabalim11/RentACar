using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.ContactMessageDtos;

namespace RentACar.API.Controllers
{
    [Route("api/contact-messages")]
    [ApiController]
    [AllowAnonymous]
    public class ContactMessagesController : ControllerBase
    {
        private readonly IContactMessageService _contactMessageService;

        public ContactMessagesController(IContactMessageService contactMessageService)
        {
            _contactMessageService = contactMessageService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(ContactMessageCreateDto contactMessageAddDto)
        {
            var result = await _contactMessageService.AddAsync(contactMessageAddDto);
            return Ok(result);
        }
    }
}
