using LibraryApi.Models;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public interface ILookupOnCallDevelopers
    {
        Task<OnCallDeveloperResponse> GetDeveloper();
    }
}