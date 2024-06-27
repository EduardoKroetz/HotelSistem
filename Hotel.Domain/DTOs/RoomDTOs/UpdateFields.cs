namespace Hotel.Domain.DTOs.RoomDTOs;

public record UpdateRoomPrice(decimal Price);

public record UpdateRoomNumber(int Number);

public record UpdateRoomName(string Name);  

public record UpdateRoomDescription(string Description);
public record UpdateRoomCapacity(int Capacity);
public record UpdateRoomCategory(Guid CategoryId);