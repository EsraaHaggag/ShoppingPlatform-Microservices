using BuildingBlocks.Common;
using OrderService.Application.Contracts.Payments;
using OrderService.Application.Interfaces;
using System.Net.Http.Json;

namespace OrderService.Infrastructure.Services
{
    public class PaymentServiceClient : IPaymentServiceClient
    {
        private readonly HttpClient _httpClient;

        public PaymentServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<InitiatePaymentResponse>> InitiatePaymentAsync(Guid orderId,
            Guid customerId, decimal amount,
            CancellationToken cancellationToken)
        {
            var request = new InitiatePaymentRequest(
                orderId, customerId, amount);

            var response = await _httpClient.PostAsJsonAsync(
                "api/payments",
                request,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return Result<InitiatePaymentResponse>.Failure(
                    new Error(
                        "Payment.InitiationFailed",
                        "Failed to initiate payment.",
                        ErrorType.Failure));
            }

            var paymentResponse =
                await response.Content.ReadFromJsonAsync<InitiatePaymentResponse>(
                    cancellationToken);

            if (paymentResponse is null)
            {
                return Result<InitiatePaymentResponse>.Failure(
                    new Error(
                        "Payment.InvalidResponse",
                        "Invalid response from Payment Service.",
                        ErrorType.Failure));
            }

            return Result<InitiatePaymentResponse>.Success(
                paymentResponse);
        }
    }
}
