using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.DataAccess;
using BookStore.Models;

namespace BookStore.Services
{
    public class BookService : IBookstoreService
    {
        public Task<IEnumerable<IBook>> GetBooksAsync(string searchString)
        {
            Task<IEnumerable<IBook>>  task = Task<IEnumerable<IBook>>.Factory.StartNew(() => BookDAO.Search(searchString));

            return task;
        }
    }
}