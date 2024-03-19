using Microsoft.EntityFrameworkCore;
using MvcTiendaPrueba.Models;

namespace MvcTiendaPrueba.Data
{
    public class TiendaContext: DbContext
    {
        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) { }

        //POR CADA MODEL NECESITAMOS UNA COLECCION DbSet QUE SERA LA QUE UTILIZAREMOS PARA CONSULTAS LINQ
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Subcategoria> Subcategorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
        public DbSet<ImagenProducto> ImagenProductos { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<DetallePedidoView> DetallePedidoViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetallePedidoView>().HasNoKey(); // Indica que la entidad es keyless
        }

    }
}
