using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.ReservationEntity;

public partial class Reservation
{
    public Invoice Finish()
    {
        //Validar Status
        if (Status == EReservationStatus.CheckedOut)
            throw new InvalidOperationException("Essa reserva já foi finalizada");

        if (Status == EReservationStatus.Canceled)
            throw new InvalidOperationException("Essa reserva foi cancelada, não é possível finaliza-la");

        if (Status != EReservationStatus.CheckedIn)
            throw new InvalidOperationException("Não foi possível finalizar a reserva pois ainda não foi feito Check-In");

        //Verificar se não é Check-In inválido
        if (CheckIn == null)
            throw new ArgumentNullException("Não foi possível finalizar a reserva pois o CheckIn ainda não foi realizado.");

        ValidateCheckIn(CheckIn);

        //Troca o CheckOut para a data atual, já que está finalizando
        UpdateCheckOut(DateTime.Now)
          .ToCheckOut(); // Muda o status para CheckedOut

        Invoice = new Invoice(this); //Gera uma instância de uma fatura

        Room?.ChangeStatus(ERoomStatus.OutOfService); //troca o status do quarto para 'OutOfService'(Fora de serviço)

        return Invoice;
    }

}