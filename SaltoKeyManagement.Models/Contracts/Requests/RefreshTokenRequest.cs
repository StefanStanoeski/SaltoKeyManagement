using System;
using System.Collections.Generic;
using System.Text;

namespace SaltoKeyManagement.Models.Contracts.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
