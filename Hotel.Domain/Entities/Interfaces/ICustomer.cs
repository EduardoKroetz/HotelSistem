using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.DislikeEntity;
using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Entities.LikeEntity;
using Hotel.Domain.Entities.ReservationEntity;

namespace Hotel.Domain.Entities.Interfaces;

public interface ICustomer : IUser
{
    ICollection<Feedback> Feedbacks { get; } 
    ICollection<Reservation> Reservations { get; } 
    ICollection<Invoice> Invoices { get; } 
    ICollection<Like> Likes { get; } 
    ICollection<Dislike> Dislikes { get; } 
}
