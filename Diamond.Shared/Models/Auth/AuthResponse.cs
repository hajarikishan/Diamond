﻿namespace Diamond.Share.Models.Auth
{
    public class AuthResponse
    {

        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";

    }
}
