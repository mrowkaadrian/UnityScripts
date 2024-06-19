using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamerBotIntegration
{
    [System.Serializable]
    public class UDPEventData
    {
        [JsonPropertyName("Event")]
        public string Event { get; set; } = string.Empty;

        [JsonPropertyName("User")]
        public string User { get; set; } = string.Empty;
        
        [JsonPropertyName("Message")]
        public string Message { get; set; } = string.Empty;
        
        [JsonPropertyName("Amount")]
        public int Amount { get; set; } = 0;
        
        public static UDPEventData DeserializeJson(string data)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            
            return JsonSerializer.Deserialize<UDPEventData>(data, options);
        }
    }
}