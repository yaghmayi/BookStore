using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LightStore.DataAccess;
using Models;

namespace Web.Controllers
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