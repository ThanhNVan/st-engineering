using Microsoft.IdentityModel.Tokens;
using Napoleon.Fos.Contract.Customers;
using Napoleon.Fos.Core.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SHA512CryptoServiceProvider = System.Security.Cryptography.SHA512;

namespace Napoleon.Fos.Application.Extensions;

public static class HelperExtension
{
    public static string ComputeSHA512(this string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashedInputBytes = SHA512CryptoServiceProvider.HashData(bytes);

        // Convert to text  
        // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte   
        var hashedInputStringBuilder = new StringBuilder(128);

        foreach (var b in hashedInputBytes)
            hashedInputStringBuilder.Append(b.ToString("X2"));

        return hashedInputStringBuilder.ToString();
    }

    public static AuthenticationTokenKeyValue GetCustomerAccessToken(this CustomerDto model, JwtSettings jwtSettings)
    {
        var stringToken = string.Empty;
        var id = Ulid.NewUlid().ToGuid();
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var secretKeyBytes = Encoding.UTF8.GetBytes(jwtSettings.Secret.ComputeSHA512());

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("CustomerId", model.Id.ToString() ?? ""),
                new Claim("Name", model.Name),
                new Claim("Email", model.Email),
                new Claim("Age", model.Age.ToString()),
                new Claim("Id", id.ToString()),
            ]),
            Expires = DateTime.UtcNow.AddDays(jwtSettings.ValidationLengthInDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
        };
        var securityToken = jwtTokenHandler.CreateToken(tokenDescriptor);
        stringToken = jwtTokenHandler.WriteToken(securityToken);

        var result = new AuthenticationTokenKeyValue
        {
            Id = id,
            Token = stringToken
        };

        return result;
    }
}
