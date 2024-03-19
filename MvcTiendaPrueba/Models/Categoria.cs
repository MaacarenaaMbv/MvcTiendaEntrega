using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcTiendaPrueba.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        [Column("IDCATEGORIA")]
        public int IdCategoria { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }
    }
}
