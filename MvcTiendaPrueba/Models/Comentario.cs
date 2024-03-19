using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcTiendaPrueba.Models
{
    [Table("Comentarios")]
    public class Comentario
    {
        [Key]
        [Column("IDCOMENTARIO")]
        public int IdComentario { get; set; }

        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }

        [Column("IDPRODUCTO")]
        public int IdProducto { get; set; }

        [Column("VALORACION")]
        public int Valoracion { get; set; }

        [Column("COMENTARIO")]
        public string ComentarioTexto { get; set; }

        [Column("FECHAPUBLICACION")]
        public DateTime FechaPublicacion { get; set; }
    }
}
