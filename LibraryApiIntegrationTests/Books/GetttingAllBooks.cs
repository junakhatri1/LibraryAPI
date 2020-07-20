
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests.Books
{
    public class GetttingAllBooks : IClassFixture<WebTestFixture>
    {
        private HttpClient _client;

        public GetttingAllBooks(WebTestFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CorrectStatusCode()
        {
            var response = await _client.GetAsync("/books");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UsesCorrectMediaType()
        {
            var response = await _client.GetAsync("/books");
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task HasCorrectDataRepresentation()
        {
            var response = await _client.GetAsync("/books");
            var content = await response.Content.ReadAsAsync<GetBooksResponse>();
            Assert.Equal(2, content.numberOfBooks);
            Assert.Null(content.genreFilter);
        }


    }


    public class GetBooksResponse
    {
        public BookItem[] books { get; set; }
        public string genreFilter { get; set; }
        public int numberOfBooks { get; set; }
    }

    public class BookItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public int numberOfPages { get; set; }
    }

}
