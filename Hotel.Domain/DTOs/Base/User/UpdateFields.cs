using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.Base.User;

public record UpdateDateOfBirth(DateTime? DateOfBirth) : IDataTransferObject;

public record UpdatePassword(string Password) : IDataTransferObject;