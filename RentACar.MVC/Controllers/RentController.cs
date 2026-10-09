using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.OfficeDtos;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.Responses;
using RentACar.MVC.Models.UIRentalDtos;
using RentACar.MVC.Models.UIRentalViewModels;

namespace RentACar.MVC.Controllers
{
    //[Authorize]
    public class RentController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut(int id)
        {
            var viewModel = new UIRentalCreateViewModel();
            viewModel.UIRentalCreateDto = new UIRentalCreateDto();
            viewModel.UIRentalCreateDto.CarId = id;
            await PopulateDropdowns(viewModel);
            return View(viewModel);
        }

        private async Task PopulateDropdowns(IRentalDropdownsViewModel dropdownsViewModel)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var officeResponseMessage = await client.GetAsync("api/Offices");
            if (officeResponseMessage.IsSuccessStatusCode)
            {
                var officeJsonData = await officeResponseMessage.Content.ReadAsStringAsync();

                var officeResponseBox = JsonConvert.DeserializeObject<ResponseModel<List<OfficeResultDto>>>(officeJsonData);
                if (officeResponseBox != null && officeResponseBox.Data != null)
                {
                    dropdownsViewModel.Offices = officeResponseBox.Data;
                }
            }
        }
    }
}
