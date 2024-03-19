using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MvcTiendaPrueba.Data;
using MvcTiendaPrueba.Extensions;
using MvcTiendaPrueba.Models;
using MvcTiendaPrueba.Repositories;
using Newtonsoft.Json;

namespace MvcTiendaPrueba.Controllers
{
    public class AccountController : Controller
    {
        private RepositoryTienda repo;
        private TiendaContext context;

        public AccountController(RepositoryTienda repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Register()
        {
            var provincias = repo.GetProvincias();
            ViewData["PROVINCIAS"] = provincias;
            return View();
        }

        public IActionResult MiPerfil()
        {
            // Obtener el correo electrónico del usuario de la sesión
            string correo = HttpContext.Session.GetString("USUARIO");

            // Deserializar la cadena JSON a un objeto Usuario
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(correo);

            // Pasar el objeto usuario como modelo a la vista
            return View(usuario);
        }
        public IActionResult EditarPerfil()
        {
            string correo = HttpContext.Session.GetString("USUARIO");
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(correo);

            return View(usuario);
        }
        [HttpPost]
        public async Task<IActionResult> EditarPerfil(Usuario usuarioModificado)
        {
            string correo = HttpContext.Session.GetString("USUARIO");
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(correo);

            usuarioModificado.Salt = usuario.Salt;
            usuarioModificado.PassEncript = usuario.PassEncript;

            await this.repo.ModificarUsuario(usuarioModificado);
 
            HttpContext.Session.SetObject("USUARIO", usuarioModificado);

            // Redirige a una página de perfil actualizado o a donde sea adecuado
            return RedirectToAction("MiPerfil", "Account");
        }


        [HttpPost]
        public async Task<IActionResult> Register
            (string nombreUsuario, string nombre, string apellido, string correo, string direccion, int idprovincia, string passEncript, string telefono)
        {
            await this.repo.RegisterUserAsync(nombreUsuario, nombre,apellido,correo, direccion, idprovincia, passEncript, telefono);
            ViewData["MENSAJE"] = "Usuario registrado correctamente";
            return RedirectToAction("Login");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string contrasenia)
        {
            Usuario user = await this.repo.LogInUserAsync(correo, contrasenia);
            if (user == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
            else
            {
                HttpContext.Session.SetObject("USUARIO", user);
                return View(user);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("USUARIO");
            return RedirectToAction("Login");
        }


    }
}
