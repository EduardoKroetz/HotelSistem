using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.RoomEntity;

public partial class Room
{
    public void ChangeStatus(ERoomStatus status)
    {
        if (status == ERoomStatus.Available)
        {
            AvailableStatus();
            return;
        }

        Status = status;
    }

    private void AvailableStatus()
    {
        if (Status != ERoomStatus.OutOfService)
            throw new InvalidOperationException("Só é possível alterar o status para 'Disponível' se o quarto estiver com o status 'Fora de serviço'");

        Status = ERoomStatus.Available;
    }

    public void ChangeCategory(Guid categoryId)
    => CategoryId = categoryId;

    public void ChangeNumber(int number)
    {
        ValidateNumber(number);
        Number = number;
    }

    public void ChangeDescription(string description)
    {
        ValidateDescription(description);
        Description = description;
    }


    public void ChangePrice(decimal price)
    {
        ValidatePrice(price);
        Price = price;
    }

    public void ChangeCapacity(int capacity)
    {
        ValidateCapacity(capacity);
        Capacity = capacity;
    }

    public void Enable()
    => IsActive = true;

    public void Disable()
  => IsActive = false;
}