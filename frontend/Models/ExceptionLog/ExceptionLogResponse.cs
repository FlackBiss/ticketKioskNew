using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models.ExceptionLog
{
    public class ExceptionLogResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("terminal")]
        public string Terminal { get; set; }

        [JsonProperty("log")]
        public string Log { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
