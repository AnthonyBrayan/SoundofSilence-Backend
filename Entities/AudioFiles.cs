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
    public class AudioFiles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_AudioFiles { get; set; }
        public string Title { get; set; }
        public string Video { get; set; }
        public string Description { get; set; }
        public string Audio { get; set; }   
        // Relación con Usuer (muchos a uno)
        [ForeignKey("Category")]
        public int Id_category { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        [JsonIgnore]
        public ICollection<UserAudio> UserAudio { get; set; }

    }
}
