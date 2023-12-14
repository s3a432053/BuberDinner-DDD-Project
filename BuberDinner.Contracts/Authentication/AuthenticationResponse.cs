namespace BuberDinner.Contracts.Authentication;

// 驗證相關 API 的回傳資料格式
public record AuthenticationResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Token);