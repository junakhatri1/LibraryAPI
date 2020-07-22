using LibraryApi.Domain;
using LibraryApi.Models;
using AutoMapper;

namespace LibraryApi.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Book, GetBooksResponseItem>();
        }
    }
}
