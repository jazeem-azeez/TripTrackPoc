using DomainServices.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Shared.DomainModels;

using Shared.ViewModels;

namespace TripTrackerApi.Controllers.v1
{

    [ApiController]
    [Route("[controller]")]
    public class DriversController : BaseController
    {
        private readonly ILogger<DriversController> _logger;
        private readonly IBizService<DriverDOM> bizService;

        public DriversController(ILogger<DriversController> logger, IBizService<DriverDOM> bizService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.bizService = bizService ?? throw new ArgumentNullException(nameof(bizService));
        }

        [HttpPost]
        public ViewModel<DriverDOM> Create(ViewModel<DriverDOM> viewModel)
        {
            return new ViewModel<DriverDOM>(bizService.Add(viewModel.Data));
        }
    }
}