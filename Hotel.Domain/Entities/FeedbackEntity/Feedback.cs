using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.DislikeEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.LikeEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.RoomEntity;

namespace Hotel.Domain.Entities.FeedbackEntity;

public partial class Feedback : Entity, IFeedback
{
    internal Feedback() { }


    public Feedback(string comment, int rate, Guid customerId, Guid reservationId, Guid roomId, Reservation? reservation = null)
    {
        Comment = comment;
        Rate = rate;
        RoomId = roomId;
        CustomerId = customerId;
        Reservation = reservation;
        ReservationId = reservationId;
        UpdatedAt = DateTime.Now;

        Validate();
    }

    public string Comment { get; private set; } = string.Empty;
    public int Rate { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer? Customer { get; private set; }
    public Guid ReservationId { get; private set; }
    public Reservation? Reservation { get; private set; }
    public Guid RoomId { get; private set; }
    public Room? Room { get; private set; }
    public ICollection<Like> Likes { get; private set; } = [];
    public ICollection<Dislike> Dislikes { get; private set; } = [];
}