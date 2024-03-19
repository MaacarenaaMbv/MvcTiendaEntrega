using Microsoft.AspNetCore.Mvc;
using MvcTiendaPrueba.Extensions;
using MvcTiendaPrueba.Models;
using MvcTiendaPrueba.Repositories;

namespace MvcTiendaPrueba.Controllers
{
    public class CestaController : Controller
    {
        private RepositoryTienda repo;
        public CestaController(RepositoryTienda repo)
        {
            this.repo = repo;
        }


        public IActionResult Index()
        {

            return View();
        }



    }
}
