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
    public class UserAudio
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_UserAudio { get; set; }
        public string State { get; set; }

        [ForeignKey("Users")]
        public int Id_user { get; set; }

        [JsonIgnore]
        public virtual Users Users { get; set; }

        [ForeignKey("AudioFiles")]
        public int Id_AudioFiles { get; set; }

        [JsonIgnore]
        public virtual Users AudioFiles { get; set; }
    }
}
