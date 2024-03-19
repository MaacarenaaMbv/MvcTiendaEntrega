using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcTiendaPrueba.Data;
using MvcTiendaPrueba.Helpers;
using MvcTiendaPrueba.Models;
using System.Data;

namespace MvcTiendaPrueba.Repositories
{

    #region PROCEDIMIENTOS
    /*
     create procedure SP_REGISTRO_PRODUCTO_CATEGORIA
(@posicion int, @categoria int 
, @registros int out) 
as 
select @registros = count(IDPRODUCTO) from Productos 
where IDCATEGORIA=@categoria 
select IDPRODUCTO, NOMBRE, DESCRIPCION, PRECIO, INVENTARIO,IDCATEGORIA,IDSUBCATEGORIA from  
    (select cast( 
    ROW_NUMBER() OVER (ORDER BY NOMBRE) as int) AS POSICION 
    , IDPRODUCTO, NOMBRE, DESCRIPCION, PRECIO, INVENTARIO,IDCATEGORIA,IDSUBCATEGORIA 
    from Productos 
    where IDCATEGORIA=@categoria) as QUERY 
    where QUERY.POSICION = @posicion 
go 

create view V_GRUPO_PRODUCTOS
as
	select cast(
		row_number() over (order by NOMBRE) as int) as posicion,
		ISNULL(IDPRODUCTO, 0) AS IDPRODUCTO, NOMBRE, DESCRIPCION, PRECIO, INVENTARIO, IDCATEGORIA, IDSUBCATEGORIA FROM Productos
go

create procedure SP_GRUPO_PRODUCTOS(@posicion int)
as
	select IDPRODUCTO, NOMBRE, DESCRIPCION, PRECIO, INVENTARIO, IDCATEGORIA, IDSUBCATEGORIA FROM V_GRUPO_PRODUCTOS
	where posicion >= @posicion and posicion < (@posicion + 16)
go

create procedure SP_GRUPO_PRODUCTOS_CATEGORIA(@posicion int, @idcategoria int)
as
	select IDPRODUCTO, NOMBRE, DESCRIPCION, PRECIO, INVENTARIO, IDCATEGORIA, IDSUBCATEGORIA FROM 
	(select cast(
		row_number() over (order by NOMBRE) as int) as posicion,
		IDPRODUCTO, NOMBRE, DESCRIPCION, PRECIO, INVENTARIO, IDCATEGORIA, IDSUBCATEGORIA FROM Productos
	where IDCATEGORIA = @idcategoria) as QUERY
	where QUERY.posicion >= @posicion and posicion < (@posicion + 16)
go

    	SELECT * FROM DetallesPedidoView;

alter VIEW DetallesPedidoView AS
SELECT 
    dp.IDDETALLEPEDIDO,
    dp.IDPEDIDO,
    dp.IDPRODUCTO,
    dp.CANTIDAD,
    dp.PRECIOUNITARIO,
    p.NOMBRE AS NOMBRE_PRODUCTO,
    dp.CANTIDAD * dp.PRECIOUNITARIO AS TOTAL_DETALLE,
    ped.TOTAL AS TOTAL_PEDIDO,
    ped.IDUSUARIO  -- Agregamos el IdUsuario
FROM 
    DETALLESPEDIDO dp
JOIN 
    PRODUCTOS p ON dp.IDPRODUCTO = p.IDPRODUCTO
JOIN 
    PEDIDOS ped ON dp.IDPEDIDO = ped.IDPEDIDO;



     */
    #endregion
    public class RepositoryTienda
    {
        private TiendaContext context;

        public RepositoryTienda(TiendaContext context)
        {
            this.context = context;
        }   

        public List<Provincia> GetProvincias()
        {
            var consulta = from datos in this.context.Provincias
                           select datos;
            return consulta.ToList();
        }

        public List<Comentario> GetComentarios()
        {
            var consulta = from datos in this.context.Comentarios
                           select datos;
            return consulta.ToList();
        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            return await this.context.Productos.ToListAsync();
        }

        public async Task<List<Comentario>> FindComentariosAsync(int idproducto)
        {
            return await this.context.Comentarios
                .Where(x => x.IdProducto == idproducto)
                .ToListAsync();
        }


        public async Task<Producto> FindProductosAsync(int id)
        {
            return await this.context.Productos
                .FirstOrDefaultAsync(x => x.IdProducto == id);
        }

        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            return await this.context.Categorias.ToListAsync();
        }
        public async Task<Categoria> FindCategoriasAsync(int id)
        {
            return await this.context.Categorias
                .FirstOrDefaultAsync(x => x.IdCategoria == id);
        }

        public List<Categoria> GetCategorias()
        {
            var consulta = from datos in this.context.Categorias
                           select datos;
            return consulta.ToList();
        }

        /*public async Task<List<Producto>> FindProductoAsync(int idcategoria)
        {
            return await this.context.Productos.FirstOrDefaultAsync(e => e.IdCategoria == idcategoria);
        }*/

        /* public async Task<List<Producto>> FindProductoCategoriaAsync(int idcategoria)
         {

         }*/

        public async Task<ModelProductoPaginacion>
            GetProductoCategoriaAsync
            (int? posicion, int idcategoria)
        {
            string sql = "SP_REGISTRO_PRODUCTO_CATEGORIA @posicion, @categoria, "
                + " @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamDepartamento =
                new SqlParameter("@categoria", idcategoria);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = ParameterDirection.Output;
            var consulta =
                this.context.Productos.FromSqlRaw
                (sql, pamPosicion, pamDepartamento, pamRegistros);
            //PRIMERO DEBEMOS EJECUTAR LA CONSULTA PARA PODER RECUPERAR  
            //LOS PARAMETROS DE SALIDA 
            var datos = await consulta.ToListAsync();
            Producto producto = datos.FirstOrDefault();
            int registros = (int)pamRegistros.Value;
            return new ModelProductoPaginacion
            {
                Registros = registros,
                Producto = producto
            };
        }

        public async Task<int> GetNumeroProductosAsync()
        {
            return await this.context.Productos.CountAsync();
        }

        public async Task<List<Producto>> GetGrupoProductosAsync(int posicion)
        {
            string sql = "SP_GRUPO_PRODUCTOS @posicion";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            var consulta = this.context.Productos.FromSqlRaw(sql, pamPosicion);
            return await consulta.ToListAsync();
        }

        public async Task<List<Producto>> GetProductosSessionAsync(List<int> productos)
        {
            return await this.context.Productos
                .Where(c => productos.Contains(c.IdProducto))
                .ToListAsync();
        }

        /**********PAGINAR POR CATEGORIA *************/
        public async Task<int> GetNumeroProductosCategoriaAsync(int idcategoria)
        {
            return await this.context.Productos.Where(z => z.IdCategoria == idcategoria).CountAsync();
        }

        public async Task<List<Producto>> GetGrupoProductosCategoriaAsync
            (int posicion, int idcategoria)
        {
            string sql = "SP_GRUPO_PRODUCTOS_CATEGORIA @posicion, @idcategoria";
            SqlParameter pamPosicion =
                new SqlParameter("@posicion", posicion);
            SqlParameter pamIdcategoria =
                new SqlParameter("@idcategoria", idcategoria);
            var consulta = this.context.Productos.FromSqlRaw
                (sql, pamPosicion, pamIdcategoria);
            return await consulta.ToListAsync();
        }

        /********************** USUARIOS ***********************************/
        private async Task<int> GetMaxIdUsuarioAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await
                    this.context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }
        }


        public async Task RegisterUserAsync(string nombreUsuario, string nombre, string apellido, string email,
            string direccion, int idprovincia, string passEncript, string telefono)
        {
            Usuario user = new Usuario();
            user.IdUsuario = await this.GetMaxIdUsuarioAsync();
            user.NombreUsuario = nombreUsuario;
            user.Nombre = nombre;
            user.Apellido = apellido;
            user.Correo = email;
            user.Direccion = direccion;
            user.IdProvincia = idprovincia;
            user.Telefono = telefono;
            //CADA USUARIO TENDRA UN SALT DISTINTO 
            user.Salt = HelperCryptography.GenerateSalt();
            //GUARDAMOS EL PASSWORD EN BYTE[] 
            user.PassEncript =
                HelperCryptography.EncryptPassword(passEncript, user.Salt);
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }

        //NECESITAMOS UN METODO PARA VALIDAR AL USUARIO 
        //DICHO METODO DEVOLVERA EL PROPIO USUARIO 
        //COMO COMPARAMOS?? email CAMPO UNICO 
        //password (12345) 
        //1) RECUPERAR EL USER POR SU EMAIL 
        //2) RECUPERAMOS EL SALT DEL USUARIO 
        //3) CONVERTIMOS DE NUEVO EL PASSWORD CON EL SALT 
        //4) RECUPERAMOS EL BYTE[] DE PASSWORD DE LA BBDD 
        //5) COMPARAMOS LOS DOS ARRAYS (BBDD) Y EL GENERADO EN EL CODIGO 

        public async Task<Usuario> LogInUserAsync(string correo, string passEncript)
        {
            Usuario user = await
                this.context.Usuarios.FirstOrDefaultAsync(x => x.Correo == correo);
            if (user == null)
            {
                return null;
            }
            else
            {
                string salt = user.Salt;
                byte[] temp =
                    HelperCryptography.EncryptPassword(passEncript, salt);
                byte[] passUser = user.PassEncript;
                bool response =
                    HelperCryptography.CompareArrays(temp, passUser);
                if (response == true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        /********************* PEDIDOS *******************************/
        public async Task<int> GetMaxIdPedidoAsync()
        {
            if (this.context.Pedidos.Count() == 0) return 1;
            return await this.context.Pedidos.MaxAsync(x => x.IdPedido) + 1;
        }
        public async Task<int> GetMaxIdDetallePedidoAsync()
        {
            if (this.context.DetallePedidos.Count() == 0) return 1;
            return await this.context.DetallePedidos.MaxAsync(x => x.IdDetallePedido) + 1;
        }


        public async Task<List<Producto>> GetProductosEnCestaAsync(List<int> idsProductos)
        {
            List<Producto> productosEnCesta = await context.Productos
                                                        .Where(p => idsProductos.Contains(p.IdProducto))
                                                        .ToListAsync();
            return productosEnCesta;
        }


        public async Task<Pedido> CreatePedidoAsync(int idusuario, List<Producto> cesta)
        {
            var total = 0.0m;
            foreach(Producto pro in cesta)
            {
                total = pro.Precio + total;
            }
            Pedido pedido = new Pedido
            {
                IdPedido = await GetMaxIdPedidoAsync(),
                IdUsuario = idusuario,
                Fecha = DateTime.Now,
                Total = total
            };
            await this.context.Pedidos.AddAsync(pedido);
            await this.context.SaveChangesAsync();

            foreach (Producto p in cesta)
            {
                DetallePedido detalle = new DetallePedido
                {
                    IdDetallePedido = await GetMaxIdDetallePedidoAsync(),
                    IdPedido = pedido.IdPedido,
                    IdProducto = p.IdProducto,
                    Cantidad = 1,
                    PrecioUnitario = p.Precio
                };

                // Verificar si ya existe un DetallePedido con el mismo IdDetallePedido
                DetallePedido existingDetalle = await this.context.DetallePedidos.FindAsync(detalle.IdDetallePedido);
                if (existingDetalle != null)
                {
                    // Actualizar el DetallePedido existente si es necesario
                    existingDetalle.IdPedido = detalle.IdPedido;
                    existingDetalle.IdProducto = detalle.IdProducto;
                    existingDetalle.Cantidad = detalle.Cantidad;
                    existingDetalle.PrecioUnitario = detalle.PrecioUnitario;
                }
                else
                {
                    // Agregar el nuevo DetallePedido al contexto si no existe
                    await this.context.AddAsync(detalle);
                    await this.context.SaveChangesAsync();

                }
            }

            await this.context.SaveChangesAsync();
            return pedido;
        }

        public async Task<List<DetallePedidoView>> GetProductosPedidoAsync(List<int> idpedidos)
        {
            return await this.context.DetallePedidoViews.Where(x => idpedidos.Contains(x.IdPedido)).ToListAsync();
        }

        public async Task<List<DetallePedidoView>> GetProductosPedidoUsuarioAsync(int idUsuario)
        {
            return await context.DetallePedidoViews
                .Where(d => d.IdUsuario == idUsuario)
                .ToListAsync();
        }

        public async Task<Usuario> ModificarUsuario(Usuario usuarioModificado)
        {
            Usuario usuarioActual = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == usuarioModificado.IdUsuario);

            if (usuarioActual != null)
            {
                // Actualizar los valores del usuario actual con los valores del usuario modificado
                usuarioActual.Nombre = usuarioModificado.Nombre;
                usuarioActual.Apellido = usuarioModificado.Apellido;
                usuarioActual.Correo = usuarioModificado.Correo;
                usuarioActual.Telefono = usuarioModificado.Telefono;
                usuarioActual.Direccion = usuarioModificado.Direccion;
                await this.context.SaveChangesAsync();
                return usuarioActual;
            }
            else
            {
                return null;
            }
        }

    }
}
