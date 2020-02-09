using System;
using System.Collections.Generic;
using System.Text;

namespace SaltoKeyManagement.Models.Contracts.Responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
