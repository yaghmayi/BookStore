using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Models;

namespace BookStore.Services
{
    public interface IBookstoreService
    {
        Task<IEnumerable<IBook>> GetBooksAsync(string searchString);
    }
}
