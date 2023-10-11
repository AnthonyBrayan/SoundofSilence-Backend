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
        public string title { get; set; }
        public string videoSrc { get; set; }
        public string description { get; set; }
        public string audioSrc { get; set; }   
        // Relación con Usuer (muchos a uno)
        [ForeignKey("Category")]
        public int Id_category { get; set; }

        //[JsonIgnore]
        //public virtual Category Category { get; set; }

        //[JsonIgnore]
        //public ICollection<UserAudio> UserAudio { get; set; }

    }
}
