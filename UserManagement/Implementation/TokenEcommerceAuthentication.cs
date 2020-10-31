﻿using ECommerceDbContext;

using ECommerceDbContext.ECOMDBENTITIES;
using Microsoft.IdentityModel.Tokens;
using ShopModels.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UserManagement.Contracts;

namespace UserManagement.Implementation
{
    public class TokenEcommerceAuthentication : ITokenEcommerceAuthentication
    {
      
        private readonly ECOMDBContext _dbEcom;
        public TokenEcommerceAuthentication( ECOMDBContext dbEcom)
        {
     
            _dbEcom = dbEcom;
        }
        public string Authentication(UserDto model)
        {
            var user = _dbEcom.Users.FirstOrDefault(c => c.Email == model.Email && c.Password == model.Password);
            if (user != null)
            {
                var claim = new[]
                {
                    new Claim(ClaimTypes.Name,user.UserId.ToString())
                };

                var secretByte = Encoding.UTF8.GetBytes(Constants.Secret);
                var key = new SymmetricSecurityKey(secretByte);
                var algorithm = SecurityAlgorithms.HmacSha256;
                var signingCredetial = new SigningCredentials(key, algorithm);
                var acces_token = new JwtSecurityToken(
                    Constants.Issuer,
                    Constants.Audience,
                    claim,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddDays(1),
                    signingCredetial);
                var token = new JwtSecurityTokenHandler().WriteToken(acces_token);
                return token;


            }
            return null;
        }
    }
    
}
