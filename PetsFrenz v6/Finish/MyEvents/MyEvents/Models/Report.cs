using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvents.Models
{
    public class Report
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "ReportUser")]
        public string ReportUser { get; set; }

        [JsonProperty(PropertyName = "ReportDescription")]
        public string ReportDescription { get; set; }
    }
}
