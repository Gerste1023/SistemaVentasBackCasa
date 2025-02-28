﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SistemaVentasBackCasa.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentasBackCasa.Utils
{
    public class JWTConfigurator
    {
        public static string GetToken(Usuario userInfo, IConfiguration config)
        {
            string SecretKey = config["Jwt:SecretKey"];
            string Issuer    = config["Jwt:Issuer"];
            string Audience  = config["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, (userInfo.Nombres + " " + userInfo.Apellidos) ),
                new Claim("identificacion", userInfo.Identificacion)
            };

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims,
                expires: DateTime.Now.AddMinutes(180),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static int GetTokenIdUsuario(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                foreach (var claim in claims)
                {
                    if (claim.Type == "identificacion")
                    {
                        return int.Parse(claim.Value);
                    }
                }
            }
            return 0;
        }
    }
}
