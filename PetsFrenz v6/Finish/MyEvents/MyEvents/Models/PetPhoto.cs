using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvents.Models
{
    public class PetPhoto
    {

        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "owneremail")]
        public string owneremail { get; set; }

        [JsonProperty(PropertyName = "petname")]
        public string petname { get; set; }

        [JsonProperty(PropertyName = "petimage")]
        public string petimage { get; set; }

        [JsonProperty(PropertyName= "CreateDate")]
        public DateTime datecreated { get; set; }

        [JsonProperty(PropertyName = "nolike")]
        public string nolike { get; set; }
    }
}
