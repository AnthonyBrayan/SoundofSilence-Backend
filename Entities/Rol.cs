using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities
{
    public class Rol
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_rol { get; set; }
        public string Name_rol { get; set; }

        [JsonIgnore]
        public ICollection<Users> Users { get; set; }

    }
}
