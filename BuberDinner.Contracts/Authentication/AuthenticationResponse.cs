namespace BuberDinner.Contracts.Authentication;

// ���Ҭ��� API ���^�Ǹ�Ʈ榡
public record AuthenticationResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string Token);