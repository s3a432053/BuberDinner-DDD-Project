namespace BuberDinner.Contracts.Authentication;

// 登入 API 資料格式
public record LoginRequest(
    string Email,
    string Password);