using System.ComponentModel.DataAnnotations.Schema;

namespace MvcTiendaPrueba.Models
{
    [Table("DetallesPedidoView")]
    public class DetallePedidoView
    {
        [Column("IDDETALLEPEDIDO")]
        public int IdDetallePedido { get; set; }

        [Column("IDPEDIDO")]
        public int IdPedido { get; set; }

        [Column("IDPRODUCTO")]
        public int IdProducto { get; set; }

        [Column("CANTIDAD")]
        public int Cantidad { get; set; }

        [Column("PRECIOUNITARIO")]
        public decimal PrecioUnitario { get; set; }

        [Column("NOMBRE_PRODUCTO")]
        public string NombreProducto { get; set; }

        [Column("TOTAL_DETALLE")]
        public decimal TotalDetalle { get; set; }

        [Column("TOTAL_PEDIDO")]
        public decimal TotalPedido { get; set; }
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }

    }
}
