using Microsoft.AspNetCore.Mvc;
using MvcTiendaPrueba.Data;
using MvcTiendaPrueba.Extensions;
using MvcTiendaPrueba.Models;
using MvcTiendaPrueba.Repositories;

namespace MvcTiendaPrueba.Controllers
{
    public class TiendaController : Controller
    {
        private RepositoryTienda repo;
        private TiendaContext context;

        public TiendaController(RepositoryTienda repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await repo.GetCategoriasAsync();
            ViewData["CATEGORIAS"] = categorias;
            var comentarios = repo.GetComentarios();
            ViewData["COMENTARIOS"] = comentarios;
            return View();
        }


        /*******************************/

        public async Task<IActionResult> DetailsProducto
            (int idproducto)
        {
            List<Comentario> comentarios = await this.repo.FindComentariosAsync(idproducto);
            HttpContext.Session.SetObject("COMENTARIOS", comentarios);

            Producto producto = await this.repo.FindProductosAsync(idproducto);
            return View(producto);
        }

        public async Task<IActionResult> PaginarGrupoProductos(int? posicion)
        {
            var categorias = await repo.GetCategoriasAsync();
            ViewData["CATEGORIAS"] = categorias;
            if (posicion == null)
            {
                posicion = 1;
            }
            int registros = await this.repo.GetNumeroProductosAsync();
            List<Producto> productos = await this.repo.GetGrupoProductosAsync(posicion.Value);
            ViewData["REGISTROS"] = registros;
            ViewData["NumeroPaginaActual"] = posicion; // Pasa el número de página actual a la vista
            return View(productos);
        }

        public async Task<IActionResult> ProductosCategoria(int? posicion, int idcategoria)
        {
            var categorias = await repo.GetCategoriasAsync();
            ViewData["CATEGORIAS"] = categorias;
            if (posicion == null)
            {
                posicion = 1;
            }
           
                List<Producto> productos = await this.repo.GetGrupoProductosCategoriaAsync(posicion.Value,idcategoria);
                int registros = await this.repo.GetNumeroProductosCategoriaAsync(idcategoria);
                ViewData["REGISTROS"] = registros;
                ViewData["IDCATEGORIA"] = idcategoria;
                ViewData["NumeroPaginaActual"] = posicion; // Pasa el número de página actual a la vista
                return View(productos);
        }


        public async Task<IActionResult> Categorias()
        {
            List<Categoria> categorias = await this.repo.GetCategoriasAsync();
            return View(categorias);
        }

        /************************CESTA*****************************************/

        public async Task<IActionResult> Cesta()
        {
            List<int> cesta = HttpContext.Session.GetObject<List<int>>("CESTA");
            if (cesta != null)
            {
                List<Producto> productos = await this.repo.GetProductosSessionAsync(cesta);
                return View(productos);
            }
            return View();
        }

        public IActionResult AnyadirProductoCesta(int? idproducto)
        {
            if (idproducto != null)
            {
                List<int> cesta;
                if (HttpContext.Session.GetString("CESTA") == null)
                {
                    cesta = new List<int>();
                }
                else
                {
                    cesta = HttpContext.Session.GetObject<List<int>>("CESTA");
                }
                cesta.Add(idproducto.Value);
                HttpContext.Session.SetObject("CESTA", cesta);
            }
            return RedirectToAction("PaginarGrupoProductos");
        }

        public async Task<IActionResult> EliminarProductoCesta(int? idproducto)
        {
            if (idproducto != null)
            {
                List<int> cesta =
                    HttpContext.Session.GetObject<List<int>>("CESTA");
                cesta.Remove(idproducto.Value);
                if (cesta.Count() == 0)
                {
                    HttpContext.Session.Remove("CESTA");
                }
                else
                {
                    HttpContext.Session.SetObject("CESTA", cesta);
                }
            }
            return RedirectToAction("CESTA");
        }

        /**************************FAVORITOS***************************************/
        public async Task<IActionResult> Favoritos()
        {
            List<int> favoritos = HttpContext.Session.GetObject<List<int>>("FAVORITOS");
            if (favoritos != null)
            {
                List<Producto> productos = await this.repo.GetProductosSessionAsync(favoritos);
                return View(productos);
            }
            return View();
        }

        public IActionResult AnyadirProductoFavoritos(int? idproducto)
        {
            if (idproducto != null)
            {
                List<int> favoritos;
                if (HttpContext.Session.GetString("FAVORITOS") == null)
                {
                    favoritos = new List<int>();
                }
                else
                {
                    favoritos = HttpContext.Session.GetObject<List<int>>("FAVORITOS");
                }
                favoritos.Add(idproducto.Value);
                HttpContext.Session.SetObject("FAVORITOS", favoritos);
            }
            return RedirectToAction("PAginarGrupoProductos");
        }

        public async Task<IActionResult> EliminarProductoFavoritos(int? idproducto)
        {
            if (idproducto != null)
            {
                List<int> favoritos =
                    HttpContext.Session.GetObject<List<int>>("FAVORITOS");
                favoritos.Remove(idproducto.Value);
                if (favoritos.Count() == 0)
                {
                    HttpContext.Session.Remove("FAVORITOS");
                }
                else
                {
                    HttpContext.Session.SetObject("FAVORITOS", favoritos);
                }
            }
            return RedirectToAction("FAVORITOS");
        }

        /******************** PEDIDOS ****************************/
        private int ObtenerIdUsuario()
        {
            // Verificamos si el usuario está presente en la sesión
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");

            // Si el usuario está presente en la sesión, devolvemos su ID
            // Si no está presente o es nulo, devolvemos algún valor predeterminado o manejamos la situación según sea necesario
            return usuario != null ? usuario.IdUsuario : 0;
        }
        public async Task<IActionResult> RealizarCompra()
        {
            // Obtenemos el ID del usuario desde la sesión
            int idUsuario = ObtenerIdUsuario();

            // Obtenemos la lista de productos en la cesta desde la sesión
            List<int> cesta = HttpContext.Session.GetObject<List<int>>("CESTA");

            // Obtenemos los detalles de los productos en la cesta desde la base de datos
            List<Producto> productosEnCesta = await this.repo.GetProductosEnCestaAsync(cesta);

            // Creamos el pedido
            Pedido pedido = await this.repo.CreatePedidoAsync(idUsuario, productosEnCesta);

            // Limpiamos la cesta después de crear el pedido
            HttpContext.Session.Remove("CESTA");

            // Redirigimos a alguna página de confirmación o cualquier otra acción que necesites
            return RedirectToAction("PaginarGrupoProductos");
        }

        public async Task<IActionResult> PedidosUsuario()
        {
            int idUsuario = ObtenerIdUsuario();
            List<DetallePedidoView> pedidosUsuarios = await this.repo.GetProductosPedidoUsuarioAsync(idUsuario);
            return View(pedidosUsuarios);
        }

    }
}
