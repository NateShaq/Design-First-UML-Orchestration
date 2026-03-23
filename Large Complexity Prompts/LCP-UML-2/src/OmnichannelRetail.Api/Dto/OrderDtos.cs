using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmnichannelRetail.Api.Dto;

public record OrderLineDto(
    [property: Required] string ProductSku,
    [property: Range(1, int.MaxValue)] int Quantity,
    [property: Range(0, double.MaxValue)] decimal UnitPrice);

public record CreateOrderDto(
    [property: Required] Guid CustomerId,
    [property: Required, MaxLength(50)] string OrderNumber,
    [property: Required, MaxLength(50)] string Status,
    [property: Range(0, double.MaxValue)] decimal Total,
    [property: Required] IList<OrderLineDto> Lines);

public record PaymentRequestDto(
    [property: Required] Guid PaymentGatewayId,
    [property: Required] string OrderNumber,
    [property: Range(0, double.MaxValue)] decimal Amount);

public record RefundRequestDto(
    [property: Required] Guid PaymentGatewayId,
    [property: Required] string RmaNumber,
    [property: Range(0, double.MaxValue)] decimal Amount,
    [property: Required] string Method);
