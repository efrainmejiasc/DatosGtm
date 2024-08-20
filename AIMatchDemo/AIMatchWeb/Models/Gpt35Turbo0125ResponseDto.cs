using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AIMatchWeb.Models
{
    public class Gpt35Turbo0125ResponseDto : CommonPropertyResponseDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }

        [JsonPropertyName("usage")]
        public Usage Usage { get; set; }

        [JsonPropertyName("system_fingerprint")]
        public string SystemFingerprint { get; set; }
    }
    public class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; }

        [JsonPropertyName("logprobs")]
        public object Logprobs { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
    }

    public class MessageResponse
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("comparacion")]
        public List<Comparacion> Comparacion { get; set; }
    }

    public class Comparacion
    {
        [JsonPropertyName("nombreCampo")]
        public string NombreCampo { get; set; }

        [JsonPropertyName("valor")]
        public string Valor { get; set; }

        [JsonPropertyName("numeroRepF1")]
        public int NumeroRepF1 { get; set; }

        [JsonPropertyName("numeroRepF2")]
        public int NumeroRepF2 { get; set; }
    }

    public class Usage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
