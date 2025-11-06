namespace Domain.DTO.Requests;

public record AddClientRequest(
    string Name,
    string Surname,
    string Email,
    string? PhoneNumber);

