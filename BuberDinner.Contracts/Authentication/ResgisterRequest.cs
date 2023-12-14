namespace BuberDinner.Contracts.Authentication;

// 註冊 API 資料格式
public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);