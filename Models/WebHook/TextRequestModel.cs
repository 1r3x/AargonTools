using System.Text.Json.Serialization;

namespace AargonTools.Models.WebHook
{
    public class TextRequestModel
    {
        [JsonPropertyName("msisdn")]
        public string Msisdn { get; set; }  // Sender's phone 
        [JsonPropertyName("to")]
        public string To { get; set; }      // Recipient's phone 
        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }     // e.g., "text" or "unicode"
        [JsonPropertyName("keyword")]
        public string Keyword { get; set; }
        [JsonPropertyName("message-timestamp")]
        public string MessageTimestamp { get; set; }
        [JsonPropertyName("api-key")]
        public string ApiKey { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }     // Message content
    }
}
