using System.Text.Json.Serialization;


namespace PaymentService.Infrastructure.Services.Paymob
{

    public class PaymobCreateIntentionResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = string.Empty;

        [JsonPropertyName("intention_order_id")]
        public int IntentionOrderId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
