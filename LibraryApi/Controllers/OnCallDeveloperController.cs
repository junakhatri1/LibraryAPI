using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class OnCallDeveloperController: ControllerBase
    {
        ILookupOnCallDevelopers OnCallLookup;

        public OnCallDeveloperController(ILookupOnCallDevelopers onCallLookup)
        {
            OnCallLookup = onCallLookup;
        }

        [HttpGet("/oncalldeveloper")]
        public async Task<ActionResult> GetOnCallDeveloper()
        {
            OnCallDeveloperResponse response = await OnCallLookup.GetDeveloper();
            return Ok(response);
        }
    }
}
