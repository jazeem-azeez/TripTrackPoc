using Ardalis.GuardClauses;

using DomainServices;
using DomainServices.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Shared.DomainModels;
using Shared.ViewModels;

namespace TripTrackerApi.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    public class TrucksController : BaseController
    {
        private readonly ILogger<TrucksController> _logger;
        private readonly ITruckService bizService;

        public TrucksController(ILogger<TrucksController> logger,ITruckService bizService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.bizService = bizService ?? throw new ArgumentNullException(nameof(bizService));
        }

        [HttpPost]
        public ViewModel<TruckDOM> Create(ViewModel<TruckDOM> viewModel)
        {

            Guard.Against.Null(viewModel.Data);
            return new ViewModel<TruckDOM>(bizService.Add(viewModel.Data));
        }
        [HttpPut]
        public ViewModel<TruckDOM> Update(ViewModel<TruckDOM> viewModel)
        {
            Guard.Against.Null(viewModel.Data);
            return new ViewModel<TruckDOM>(bizService.Update(viewModel.Data));
        }
        [HttpPost("{truckId}/gps")]
        public async Task<ViewModel<GpsInfoDOM>> UpdateLocationAsync([FromBody] ViewModel<GpsInfoDOM> viewModel, [FromRoute] string truckId)
        {
            
            Guard.Against.NullOrEmpty(viewModel.Data.GPSLocationString);
            Guard.Against.NullOrEmpty(viewModel.Data.StatusMessage);
            Guard.Against.NullOrEmpty(viewModel.Data.TripPlanId);
            Guard.Against.NullOrEmpty(viewModel.Data.TruckId);
            Guard.Against.NullOrWhiteSpace(truckId);
            return new ViewModel<GpsInfoDOM>(await bizService.TruckMovementAsync(viewModel.Data));
        }
    }
}