using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcTiendaPrueba.Models
{
    [Table("DetallesPedido")]
    public class DetallePedido
    {
        [Key]
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
    }
}
