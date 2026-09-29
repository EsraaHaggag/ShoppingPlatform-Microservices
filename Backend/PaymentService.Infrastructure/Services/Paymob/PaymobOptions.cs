namespace PaymentService.Infrastructure.Services.Paymob
{
    public class PaymobOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
        public string WebhookUrl { get; set; } = string.Empty;
        public string IntegrationId { get; set; } = string.Empty;
    }
}
