using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvents.Models
{
    public class Friend
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "UserPetName")]
        public string userpetname { get; set; }

        [JsonProperty(PropertyName = "FriendPetName")]
        public string friendpetname { get; set; }

        [JsonProperty(PropertyName = "Status")]
        public string status { get; set; }

        [JsonProperty(PropertyName = "petimage")]
        public string petimage { get; set; }

    }
}
