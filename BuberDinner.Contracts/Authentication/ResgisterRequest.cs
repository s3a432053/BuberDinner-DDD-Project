namespace BuberDinner.Contracts.Authentication;

// ���U API ��Ʈ榡
public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);