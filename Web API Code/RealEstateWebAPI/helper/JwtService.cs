using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstateWebAPI.Helper
{
	public class JwtService
	{
		private readonly string _key;
		private readonly string _issuer;
		private readonly string _audience;

		public JwtService(IConfiguration configuration)
		{
			_key = configuration["Jwt:Key"];
			_issuer = configuration["Jwt:Issuer"];
			_audience = configuration["Jwt:Audience"];
		}

		public string GenerateJwtToken(int userId, string userName, string userRole,string fullName)
		{
			var claims = new[]
			{
				new Claim("UserId", userId.ToString()),
				new Claim(ClaimTypes.Role, userRole),
				new Claim("UserName",userName),
				new Claim("UserRole",userRole),
				new Claim("FullName",fullName)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _issuer,
				audience: _audience,
				claims: claims,
				expires: DateTime.UtcNow.AddDays(4).ToUniversalTime(),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public Dictionary<string, string> DecodeJwt(string token)
		{
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);

			var payload = new Dictionary<string, string>();
			foreach (var claim in jwtToken.Claims)
			{
				payload[claim.Type] = claim.Value;
			}

			return payload;
		}
	}
}
