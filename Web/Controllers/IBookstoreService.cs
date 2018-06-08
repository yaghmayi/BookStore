using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Web.Controllers
{
    public interface IBookstoreService
    {
        Task<IEnumerable<IBook>> GetBooksAsync(string searchString);
    }
}
