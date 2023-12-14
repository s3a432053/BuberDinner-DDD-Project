namespace BuberDinner.Contracts.Authentication;

// ���Ҭ��� API ���^�Ǹ�Ʈ榡
public record AuthenticationResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Token);