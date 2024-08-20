using Newtonsoft.Json;
using System.Collections.Generic;

namespace AIMatchWeb.Models
{
    public class AuthGptRequestDto
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }
    }

    public class Message
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
