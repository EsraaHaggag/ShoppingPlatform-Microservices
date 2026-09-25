using System.Text.Json.Serialization;

namespace PaymentService.Infrastructure.Services.Paymob
{
    public class PaymobCreateIntentionRequest
    {
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "EGP";

        [JsonPropertyName("payment_methods")]
        public List<int> PaymentMethods { get; set; } = new();

        //[JsonPropertyName("items")]
        //public List<PaymobItem> Items { get; set; } = new();

        [JsonPropertyName("billing_data")]
        public PaymobBillingData BillingData { get; set; } = new();

        [JsonPropertyName("special_reference")]
        public string SpecialReference { get; set; } = string.Empty;

        [JsonPropertyName("notification_url")]
        public string NotificationUrl { get; set; } = string.Empty;

        //[JsonPropertyName("redirection_url")]
        //public string RedirectionUrl { get; set; } = string.Empty;
    }

    public class PaymobItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public class PaymobBillingData
    {
        [JsonPropertyName("apartment")]
        public string Apartment { get; set; } = "NA";

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = "Test";

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = "Customer";

        [JsonPropertyName("street")]
        public string Street { get; set; } = "Test Street";

        [JsonPropertyName("building")]
        public string Building { get; set; } = "1";

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; } = "01000000000";

        [JsonPropertyName("city")]
        public string City { get; set; } = "Cairo";

        [JsonPropertyName("country")]
        public string Country { get; set; } = "EG";

        [JsonPropertyName("email")]
        public string Email { get; set; } = "test@example.com";

        [JsonPropertyName("floor")]
        public string Floor { get; set; } = "1";

        [JsonPropertyName("state")]
        public string State { get; set; } = "Cairo";
    }
}
