using LibraryApi.Models;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public interface IMapBooks
    {
        Task<GetBooksResponse> GetBooks(string genre);
    }
}