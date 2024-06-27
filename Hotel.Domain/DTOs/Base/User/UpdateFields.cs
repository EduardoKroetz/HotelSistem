namespace Hotel.Domain.DTOs.Base.User;

public record UpdateDateOfBirth(DateTime? DateOfBirth) : IDataTransferObject;

public record UpdatePassword(string Password) : IDataTransferObject;
public record UpdateGender(int Gender);