using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMModelo.DataModel
{
    [Table("Usuario")]
    public class Usuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "INT")]
        public int Id { get; set; }
       
        [Column(Order = 2, TypeName = "VARCHAR(50)")]
        public string Nombre { get; set; }

        [Column(Order = 3, TypeName = "VARCHAR(50)")]
        public string Apellido{ get; set; }

        [Column(Order = 4, TypeName = "VARCHAR(50)")]
        public string UserName { get; set; }
        [Key]
        [Column(Order = 5, TypeName = "VARCHAR(50)")]
        public string Email { get; set; }

        [Column(Order = 6, TypeName = "VARCHAR(50)")]
        public string Password { get; set; }

        [Column(Order = 7, TypeName = "VARCHAR(50)")]
        public string Password2 { get; set; }

        [Column(Order = 8, TypeName = "DATETIME")]
        public DateTime Fecha { get; set; }

        [Column(Order = 9, TypeName = "BIT")]
        public bool Estado { get; set; }

        [Column(Order = 10, TypeName = "INT")]
        public int RolId { get; set; }
    }
}
