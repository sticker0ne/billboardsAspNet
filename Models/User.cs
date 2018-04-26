using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BillboardsMVC5.Models
{
    [Table("AspNetUsers")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Email { get; set; }

        [NotMapped]
        public bool IsManager { get; set; }
    }
}