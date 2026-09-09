using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using backend.Database;

public class JwtService
{
    private readonly IConfiguration config;

    public JwtService(IConfiguration config)
    {
        this.config = config;
    }

    public string CreateToken(User user)
    {
        var claims = new[]
        {
            new Claim("user_id", user.user_id.ToString()),
            new Claim("user_name", user.user_name),
            new Claim("user_mail", user.user_mail)
        };

        if (user.musician_id != null)
        {
            claims.Append(
                new Claim("user_musician_id", user.musician_id.Value.ToString())
            );
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                config["Auth_token_config:key"]!
            )
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: config["Auth_token_config:Issuer"],
            audience: config["Auth_token_config:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}