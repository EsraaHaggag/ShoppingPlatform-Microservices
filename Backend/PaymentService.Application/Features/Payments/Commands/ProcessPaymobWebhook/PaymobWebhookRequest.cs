using System.Text.Json.Serialization;

namespace PaymentService.Application.Features.Payments.Commands.ProcessPaymobWebhook
{


    public class PaymobWebhookRequest
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("obj")]
        public PaymobTransaction Obj { get; set; } = new();
    }

    public class PaymobTransaction
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("pending")]
        public bool Pending { get; set; }

        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        [JsonPropertyName("order")]
        public PaymobOrder Order { get; set; } = new();
    }

    public class PaymobOrder
    {
        [JsonPropertyName("merchant_order_id")]
        public string MerchantOrderId { get; set; } = string.Empty;
    }
}
