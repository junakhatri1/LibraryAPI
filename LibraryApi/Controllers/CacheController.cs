using System;

using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    public class CacheController : ControllerBase
    {
        ISystemTime Clock;

        public CacheController(ISystemTime clock)
        {
            Clock = clock;
        }

        [HttpGet("/time1")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 10)]
        public ActionResult GetTheTime()
        {
            return Ok(Clock.GetCurrent().ToLongTimeString());
        }
    }
}
