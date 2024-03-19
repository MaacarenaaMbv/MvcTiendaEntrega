using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcTiendaPrueba.Models
{
    [Table("Subcategorias")]
    public class Subcategoria
    {
        [Key]
        [Column("IDSUBCATEGORIA")]
        public int IdSubcategoria { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("IDCATEGORIA")]
        public int IdCategoria { get; set; }

        [ForeignKey("IDCATEGORIA")]
        public Categoria Categoria { get; set; }
    }
}
