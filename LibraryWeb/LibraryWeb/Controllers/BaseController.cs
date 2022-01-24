using LibraryWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected ApplicationDbContext DbContext;

        public BaseController(ApplicationDbContext context)
        {
            DbContext = context;
        }
    }
}
