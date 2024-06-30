using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ReservationDTOs;

public class ReservationQueryParameters : QueryParameters
{
    public TimeSpan? TimeHosted { get; set; }
    public string? TimeHostedOperator { get; set; }
    public decimal? DailyRate { get; set; }
    public string? DailyRateOperator { get; set; }
    public DateTime? CheckIn { get; set; }
    public string? CheckInOperator { get; set; }
    public DateTime? CheckOut { get; set; }
    public string? CheckOutOperator { get; set; }
    public EReservationStatus? Status { get; set; }
    public int? Capacity { get; set; }
    public string? CapacityOperator { get; set; }
    public Guid? RoomId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? InvoiceId { get; set; }
    public Guid? ServiceId { get; set; }
    public TimeSpan? ExpectedTimeHosted { get; set; }
    public string? ExpectedTimeHostedOperator { get; set; }
    public DateTime? ExpectedCheckIn { get; set; }
    public string? ExpectedCheckInOperator { get; set; }
    public DateTime? ExpectedCheckOut { get; set; }
    public string? ExpectedCheckOutOperator { get; set; }

}
