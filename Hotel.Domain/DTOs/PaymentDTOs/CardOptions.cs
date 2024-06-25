namespace Hotel.Domain.DTOs.PaymentDTOs;

public record CardOptions(string Number, int ExpMonth, int ExpYear, string Cvc);
