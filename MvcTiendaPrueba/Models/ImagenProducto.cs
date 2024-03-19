using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcTiendaPrueba.Models
{
    [Table("ImagenesProductos")]
    public class ImagenProducto
    {
        [Key]
        [Column("IDIMAGEN")]
        public int IdImagen { get; set; }

        [Column("IDPRODUCTO")]
        public int IdProducto { get; set; }


        [Column("RUTAIMAGEN")]
        public string RutaImagen { get; set; }
    }
}
