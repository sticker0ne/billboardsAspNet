using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillboardsMVC5.Models
{
    public class Application
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string BillboardId { get; set; }
        [Column("ContactPhoneNumber")]
        public string ContactPhone { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public string TenantMail { get; set; }
        [NotMapped]
        public string BillboardDesc { get; set; }
    }
}