﻿using SimpleBankAPI.Models;

namespace SimpleBankAPI.Contracts
{
    public class LoginUserResponse
    {
        public string AccessToken { get; set; }
        public string AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenExpiresAt { get; set; }
        public string SessionId { get; set; }

        public User User { get; set; }


    }
}