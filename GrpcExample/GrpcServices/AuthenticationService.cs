using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcAuthenticaion;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrpcExample
{
    public class AuthenticationService : AuthService.AuthServiceBase
    {
        public IConfiguration Configuration { get; set; }

        public AuthenticationService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override Task<CreateIdentityResponse> GenerateToken(Empty request, ServerCallContext context)
        {
            var expiration = DateTime.UtcNow.AddDays(1);
            Claim[] claims = [new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );
            string _token = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(new CreateIdentityResponse { Token = _token });
        }
    }
}
