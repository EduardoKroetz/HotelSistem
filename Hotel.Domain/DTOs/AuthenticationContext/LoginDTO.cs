namespace Hotel.Domain.DTOs.AuthenticationContext;

public class LoginDTO : IDataTransferObject
{
    public LoginDTO(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; private set; }
    public string Password { get; private set; }
}
