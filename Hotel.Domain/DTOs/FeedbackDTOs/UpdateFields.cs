namespace Hotel.Domain.DTOs.FeedbackDTOs;

public record UpdateFeedback(string Comment, int Rate);

public record UpdateComment(string Comment);

public record UpdateRate(int Rate);