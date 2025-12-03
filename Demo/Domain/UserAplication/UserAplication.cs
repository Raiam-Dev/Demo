using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Domain.User
{
    public class UserAplication : IdentityUser
    {
        [Column("USR_RG")]
        public string Rg { get; set; } = null!;
    }
}
