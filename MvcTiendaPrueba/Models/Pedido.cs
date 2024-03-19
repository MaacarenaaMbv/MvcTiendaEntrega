using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcTiendaPrueba.Models
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        [Column("IDPEDIDO")]
        public int IdPedido { get; set; }

        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }


        [Column("FECHA")]
        public DateTime Fecha { get; set; }

        [Column("TOTAL")]
        public decimal Total { get; set; }
    }
}
