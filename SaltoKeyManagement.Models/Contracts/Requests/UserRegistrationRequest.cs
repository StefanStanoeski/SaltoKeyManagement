using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SaltoKeyManagement.Models.Contracts.Requests
{
    public class UserRegistrationRequest : UserRequestBase
    {
        public string Clearance { get; set; }
    }
}
