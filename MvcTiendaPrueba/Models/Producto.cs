using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcTiendaPrueba.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        [Column("IDPRODUCTO")]
        public int IdProducto { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("PRECIO")]
        public decimal Precio { get; set; }

        [Column("INVENTARIO")]
        public int Inventario { get; set; }

        [Column("IDCATEGORIA")]
        public int IdCategoria { get; set; }

        [Column("IDSUBCATEGORIA")]
        public int IdSubcategoria { get; set; }

    }
}
