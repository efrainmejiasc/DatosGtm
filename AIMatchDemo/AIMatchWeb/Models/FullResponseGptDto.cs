using System.Collections.Generic;

namespace AIMatchWeb.Models
{
    public class FullResponseGptDto
    {
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public List<FullChoice> choices { get; set; }
        public FullUsage usage { get; set; }
        public object system_fingerprint { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class FullChoice
    {
        public int index { get; set; }
        public FullMessage message { get; set; }
        public object logprobs { get; set; }
        public string finish_reason { get; set; }
    }

    public class FullMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

 
    public class FullUsage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }
}
