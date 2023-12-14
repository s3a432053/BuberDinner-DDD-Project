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
        // IOption<T> �� .NET Core ���ت��պA�]�w�`�J�覡�A�i�q�h�ӲպA���Ѫ�(ConfigurationProvider) ���H�j���O���覡ô���պA�C
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        // �~��ϥ� signingKey �Ы� SigningCredentials �M JWT �O�P
        var siginingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), // �ϥηs�ͦ���256����_
            SecurityAlgorithms.HmacSha256
        );

        // �Ыؤ@���n���]claims�^�A�o���n���N�]�t�bJWT�O�P��
        var claims = new[]
        {
        // �K�[�Τ᪺�ߤ@���Ѳŧ@���l�]Subject�^�n��
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        // �K�[�Τ᪺�W�r�@�������W�١]GivenName�^�n��
        new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
        // �K�[�Τ᪺�m��@���m��]FamilyName�^�n��
        new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
        // �K�[�@�Ӱߤ@�ѧO�š]JTI�^�n���A�q�`�Ω󨾤�����
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

        // �Ыؤ@��JWT�w���O�P�]JwtSecurityToken�^
        var securityToken = new JwtSecurityToken(
            // ���wJWT���o��̡]issuer�^
            issuer: _jwtSettings.Issuer,
            // audience �Ω���w JWT �O�P���u������v�q�`�Aaudience �i�H�O���ε{�����W�١B�ѧO�ũάY�Ǩ��骺��
            audience: _jwtSettings.Audience,
            // �]�m�O�P���L���ɶ�����e�ɶ��� XX ������
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            // �N�n���]claims�^�K�[��O�P��
            claims: claims,
            // ���w�Ω�ñ�p�O�P��ñ�p���_�]SigningCredentials�^
            signingCredentials: siginingCredentials
        );

        // �ϥ�JWT�w���O�P�B�z�{�ǡ]JwtSecurityTokenHandler�^�N�O�P�ഫ���r�Ŧ�
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}