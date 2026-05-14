namespace CareerLens.Shared.DTOs.User;

public record UserProfileDto(Guid Id, string Email, string? FirstName, string? LastName, string Plan, DateTime CreatedAt);
