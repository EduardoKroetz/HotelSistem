using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomEntity;

public partial class Room
{
    public void AddService(Service service)
    {
        if (Services.Contains(service))
            throw new ValidationException("Esse serviço já foi adicionado.");
        Services.Add(service);
    }

    public void RemoveService(Service service)
    {
        if (!Services.Remove(service))
            throw new ValidationException("Esse serviço não está associado a essa hospedagem.");
    }


}