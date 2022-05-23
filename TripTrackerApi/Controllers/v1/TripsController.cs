using System.ComponentModel.DataAnnotations;

using DomainServices;
using DomainServices.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Shared.DomainModels;

using Shared.ViewModels;

namespace TripTrackerApi.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    public class TripPlansController : BaseController
    {
        private readonly ILogger<TripPlansController> _logger;
        private readonly ITripPlanService bizService;

        public TripPlansController(ILogger<TripPlansController> logger, ITripPlanService bizService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.bizService = bizService ?? throw new ArgumentNullException(nameof(bizService));
        }

        [HttpPost]
        public ViewModel<TripPlanDOM> Create(ViewModel<TripPlanDOM> viewModel)
        {
            return new ViewModel<TripPlanDOM>(bizService.Add(viewModel.Data));
        }

        [HttpGet("reports/getKMByAgeAndCountryOverAPeriod")]
        public string GetKMByAgeAndCountryOverAPeriod([FromQuery][Required] int age,
                                                [FromQuery][Required] string countryCode,
                                                [FromQuery][Required] DateTime lowerTimeStamp,
                                                [FromQuery][Required] DateTime upperTimeStamp)
        {
            return bizService.GetKMByAgeAndCountryOverAPeriod(age, countryCode, lowerTimeStamp, upperTimeStamp) + " Kilometers";
        }


        [HttpGet("reports/getDistanceDrivenForTripPlan")]
        public string GetDistanceDrivenForTruckPlan([FromQuery][Required] string triplPlanId)
        {
            return bizService.GetDistanceDrivenForTripPlanInKM(triplPlanId) + " Kilometers";
        }

        [HttpGet("reports/GetKMByAgeAndCountryOverAPeriodFromLogs")]
        public string GetKMByAgeAndCountryOverAPeriodFromLogs([FromQuery][Required] int age,
                                                [FromQuery][Required] string countryCode,
                                                [FromQuery][Required] DateTime lowerTimeStamp,
                                                [FromQuery][Required] DateTime upperTimeStamp)
        {
            return bizService.GetKMByAgeAndCountryOverAPeriodFromLogs(age, countryCode, lowerTimeStamp, upperTimeStamp) + " Kilometers";
        }

        [HttpPut]
        public ViewModel<TripPlanDOM> Update(ViewModel<TripPlanDOM> viewModel)
        {
            return new ViewModel<TripPlanDOM>(bizService.Update(viewModel.Data));
        }
    }
}