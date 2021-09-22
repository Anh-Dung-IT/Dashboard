using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Entities
{
    public class RefreshTokens
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Username { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        [ForeignKey(nameof(Username))]
        public Accounts Account { get; set; }
    }
}
