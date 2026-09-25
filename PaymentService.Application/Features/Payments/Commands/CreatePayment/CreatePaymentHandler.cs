using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using PaymentService.Application.Contracts.Payments;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities.Payment;
using PaymentService.Domain.Entities.Payment.Errors;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentHandler
    : IRequestHandler<
        CreatePaymentCommand,
        Result<CreatePaymentResponse>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentProvider _paymentProvider;
        private readonly ICurrentUserService _currentUserService;


        public CreatePaymentHandler(
            IPaymentRepository paymentRepository,
            IPaymentProvider paymentProvider, ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _paymentProvider = paymentProvider;
            _currentUserService = currentUserService;
        }

        public async Task<Result<CreatePaymentResponse>> Handle(CreatePaymentCommand request,
            CancellationToken cancellationToken)
        {
            Guid userId = _currentUserService.UserId;

            var existingPayment = await _paymentRepository.GetLatestByOrderIdAsync(request.OrderId, userId,
                    cancellationToken);

            if (existingPayment is not null)
            {
                if (existingPayment.Status == PaymentStatus.Paid)
                {
                    return Result<CreatePaymentResponse>.Failure(PaymentErrors.AlreadyPaid);
                }
                if (existingPayment.Status == PaymentStatus.Pending)
                {
                    return Result<CreatePaymentResponse>.Success(
                        new CreatePaymentResponse(existingPayment.Id, existingPayment.CheckoutUrl!));
                }
            }

            //if (existingPayment.Status == PaymentStatus.Pending)
            //{
            //    if (existingPayment.CheckoutExpiresAt > DateTime.UtcNow)
            //    {
            //        return Result<CreatePaymentResponse>.Success(
            //            new CreatePaymentResponse(existingPayment.Id, existingPayment.CheckoutUrl));
            //    }
            //}

            var payment = Payment.Create(
                request.OrderId,
                request.CustomerId,
                request.Amount);

            if (payment.IsFailure)
                return Result<CreatePaymentResponse>
                    .Failure(payment.Error);

            var providerResult =
                await _paymentProvider.CreatePaymentAsync(
                    new PaymentProviderRequest(
                   payment.Value.Id, request.OrderId,
                        request.Amount,
                        "EGP"),
                    cancellationToken);


            if (!providerResult.IsSuccess)
            {
                return Result<CreatePaymentResponse>.Failure(
                    new Error(
                        "Payment.ProviderFailed",
                        providerResult.ErrorMessage ?? "Unknown Paymob error.",
                        ErrorType.Failure));
            }

            payment.Value!.SetProviderPayment(
         providerResult.ProviderPaymentId, providerResult.CheckoutUrl);

            await _paymentRepository.AddAsync(
                payment.Value);

            await _paymentRepository.CompleteAsync(
                cancellationToken);

            return Result<CreatePaymentResponse>.Success(
                new CreatePaymentResponse(
                    payment.Value.Id,
              providerResult.CheckoutUrl!));
        }
    }
}

