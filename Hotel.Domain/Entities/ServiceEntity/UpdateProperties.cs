using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.ServiceEntity;

public partial class Service
{
    public void ChangeName(string name)
    {
        ValidateName(name);
        Name = name;
    }

    public void ChangePrice(decimal price)
    {
        ValidatePrice(price);
        Price = price;
    }

    public void Enable()
    => IsActive = true;

    public void Disable()
    => IsActive = false;

    public void ChangePriority(EPriority priority)
    {
        ValidatePriority((int)priority);
        Priority = priority;
    }


    public void ChangeTime(int timeInMinutes)
    {
        ValidateTimeInMinutes(timeInMinutes);
        TimeInMinutes = timeInMinutes;
    }




}