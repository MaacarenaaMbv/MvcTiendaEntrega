using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcTiendaPrueba.Models
{
    [Table("Provincias")]
    public class Provincia
    {
        [Key]
        [Column("IDPROVINCIA")]
        public int IdProvincia { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }
    }
}
