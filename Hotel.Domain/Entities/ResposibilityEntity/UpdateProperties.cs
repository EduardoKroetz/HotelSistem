using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.ResponsibilityEntity;

public partial class Responsibility
{
    public void ChangeName(string name)
    {
        ValidateName(name);
        Name = name;
    }


    public void ChangeDescription(string description)
    {
        ValidateDescription(description);
        Description = description;
    }

    public void ChangePriority(EPriority priority)
    {
        ValidatePriority(priority);
        Priority = priority;
    }


}