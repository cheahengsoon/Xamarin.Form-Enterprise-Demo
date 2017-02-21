using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvents.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string email { get; set; }


        [JsonProperty(PropertyName = "password")]
        public string password { get; set; }


        [JsonProperty(PropertyName = "petname")]
        public string petname { get; set; }



        [JsonProperty(PropertyName = "petage")]
        public string petage { get; set; }



        [JsonProperty(PropertyName = "petgender")]
        public string petgender { get; set; }



        [JsonProperty(PropertyName = "petimage")]
      public string petimage { get; set; }

        [JsonProperty(PropertyName = "pettype")]
        public string pettype { get; set; }


        [JsonProperty(PropertyName = "nolike")]
        public string nolike { get; set; }

        [JsonProperty(PropertyName = "ownername")]
        public string ownername { get; set; }


    }
}