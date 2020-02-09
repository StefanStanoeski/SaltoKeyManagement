using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SaltoKeyManagement.Models.Domain
{
    public class Entrance
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime TimeOfEntry { get; set; }
        public string UserId { get; set; }
        public Guid DoorId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

        [ForeignKey(nameof(DoorId))]
        public Door Door { get; set; }
    }
}
