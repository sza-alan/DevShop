﻿using DevShop.Application.Interfaces;

namespace DevShop.Infrastructure.Security
{
    public class PasswordHashingService : IPasswordHashingService
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
