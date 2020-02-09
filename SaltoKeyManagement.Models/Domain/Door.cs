using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SaltoKeyManagement.Models.Domain
{
    public class Door
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
