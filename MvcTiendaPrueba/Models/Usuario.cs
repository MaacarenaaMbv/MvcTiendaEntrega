using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcTiendaPrueba.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }

        [Column("NOMBREUSUARIO")]
        public string NombreUsuario { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("APELLIDO")]
        public string Apellido { get; set; }
        [Column("CORREO")]
        public string Correo { get; set; }

        /*[Column("CONTRASENIA")]
        public string Contrasenia { get; set; }*/

        [Column("DIRECCION")]
        public string Direccion { get; set; }

        [Column("SALT")]
        public string Salt { get; set; }

        [Column("IDPROVINCIA")]
        public int IdProvincia { get; set; }

        [Column("PASSENCRIPT")]
        public byte[] PassEncript { get; set; }

        [Column("TELEFONO")]
        public string Telefono { get; set; }

    }
}
