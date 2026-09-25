using Microsoft.Extensions.Options;
using PaymentService.Application.Contracts.Payments;
using PaymentService.Application.Interfaces;
using System.Net.Http.Json;

namespace PaymentService.Infrastructure.Services.Paymob
{
    public class PaymobPaymentProvider : IPaymentProvider
    {
        private readonly HttpClient _httpClient;
        private readonly PaymobOptions _options;

        public PaymobPaymentProvider(
            HttpClient httpClient,
            IOptions<PaymobOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<PaymentProviderResult> CreatePaymentAsync(
            PaymentProviderRequest request,
            CancellationToken cancellationToken)
        {
            var amountInCents = (int)(request.Amount * 100);

            var intentionRequest = new PaymobCreateIntentionRequest
            {
                Amount = amountInCents,

                Currency = request.Currency,

                PaymentMethods = new List<int>
            {
                int.Parse(_options.IntegrationId)
            },

                BillingData = new PaymobBillingData(),

                NotificationUrl = _options.WebhookUrl,
                SpecialReference = request.PaymentId.ToString()
            };

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "v1/intention/");

            httpRequest.Headers.Add(
                "Authorization",
                $"Token {_options.SecretKey}");

            httpRequest.Content =
                JsonContent.Create(intentionRequest);

            var response = await _httpClient.SendAsync(
                httpRequest,
                cancellationToken);


            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(
                        cancellationToken);

                return new PaymentProviderResult(
                    false, null, null, error);
            }

            var paymobResponse =
                await response.Content.ReadFromJsonAsync<PaymobCreateIntentionResponse>(
                    cancellationToken);
            if (string.IsNullOrWhiteSpace(paymobResponse.ClientSecret))
            {
                return new PaymentProviderResult(
                    false,
                    null,
                    null,
                    "Paymob returned an empty client_secret.");
            }
            if (paymobResponse is null)
            {
                return new PaymentProviderResult(false, null, null,
                    "Invalid response from Paymob.");
            }

            var checkoutUrl =
            $"{_options.BaseUrl}unifiedcheckout" +
            $"?publicKey={_options.PublicKey}" +
            $"&clientSecret={paymobResponse.ClientSecret}";

            return new PaymentProviderResult(
                true, paymobResponse.Id,
                checkoutUrl, null);
        }
    }
}
