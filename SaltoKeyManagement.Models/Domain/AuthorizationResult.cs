using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaltoKeyManagement.Models.Domain
{
    public class AuthorizationResult
    {
        public string Token { get; set; }
        public string DoorName { get; set; }
        public bool Success { get { return ErrorMessages == null || !ErrorMessages.Any(); } }
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
