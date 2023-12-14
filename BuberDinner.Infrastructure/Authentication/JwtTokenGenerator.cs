using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Services;
using BuberDinner.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BuberDinner.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        // IOption<T> 為 .NET Core 內建的組態設定注入方式，可從多個組態提供者(ConfigurationProvider) 中以強型別的方式繫結組態。
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        // 繼續使用 signingKey 創建 SigningCredentials 和 JWT 令牌
        var siginingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), // 使用新生成的256位金鑰
            SecurityAlgorithms.HmacSha256
        );

        // 創建一組聲明（claims），這些聲明將包含在JWT令牌中
        var claims = new[]
        {
        // 添加用戶的唯一標識符作為子（Subject）聲明
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        // 添加用戶的名字作為給予名稱（GivenName）聲明
        new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
        // 添加用戶的姓氏作為姓氏（FamilyName）聲明
        new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
        // 添加一個唯一識別符（JTI）聲明，通常用於防止重放攻擊
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

        // 創建一個JWT安全令牌（JwtSecurityToken）
        var securityToken = new JwtSecurityToken(
            // 指定JWT的發行者（issuer）
            issuer: _jwtSettings.Issuer,
            // audience 用於指定 JWT 令牌的「接收方」通常，audience 可以是應用程式的名稱、識別符或某些具體的值
            audience: _jwtSettings.Audience,
            // 設置令牌的過期時間為當前時間的 XX 分鐘後
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            // 將聲明（claims）添加到令牌中
            claims: claims,
            // 指定用於簽署令牌的簽署金鑰（SigningCredentials）
            signingCredentials: siginingCredentials
        );

        // 使用JWT安全令牌處理程序（JwtSecurityTokenHandler）將令牌轉換為字符串
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}