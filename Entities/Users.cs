using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Entities
{
    public class Users
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_user { get; set; }
        public string Name_user { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Relación con Usuer (muchos a uno)
        [ForeignKey("Rol")]
        public int Id_rol { get; set; }

        [JsonIgnore]
        public virtual Rol Rol { get; set; }

        // Relación con Custumer (uno a muchos)
        //public ICollection<User> User { get; set; }

    }
}
