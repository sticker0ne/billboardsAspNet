using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BillboardsMVC5.Models
{
    [Table("AspNetUserRoles")]
    public class UserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}