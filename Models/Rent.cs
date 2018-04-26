using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillboardsMVC5.Models
{
    public class Rent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string BillboardId { get; set; }
        public string ManagerId { get; set; }
        public string TenantId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
    }
}