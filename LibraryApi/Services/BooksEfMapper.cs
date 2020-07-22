using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Controllers;
using LibraryApi.Domain;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class BooksEfMapper : IMapBooks
    {
        LibraryDataContext Context;
        IMapper BooksAutoMapper;
        MapperConfiguration MapperConfig;

        public BooksEfMapper(LibraryDataContext context, IMapper booksAutoMapper, MapperConfiguration mapperConfig)
        {
            Context = context;
            BooksAutoMapper = booksAutoMapper;
            MapperConfig = mapperConfig;
        }

        public async Task<GetBooksResponse> GetBooks(string genre)
        {
            var books = Context.Books
               .Where(b => b.InStock)
               .ProjectTo<GetBooksResponseItem>(MapperConfig);
              

            if (genre != null)
            {
                books = books.Where(b => b.Genre == genre);
            }

            var booksList = await books.ToListAsync();
            var response = new GetBooksResponse
            {
                Books = booksList,
                GenreFilter = genre,
                NumberOfBooks = booksList.Count
            };

            return response;
        }
    }
}

