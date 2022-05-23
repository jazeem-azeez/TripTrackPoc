using Microsoft.AspNetCore.Mvc;

namespace TripTrackerApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
    }
}