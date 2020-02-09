using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SaltoKeyManagement.Models.Contracts.Requests
{
    public class UserRequestBase
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
