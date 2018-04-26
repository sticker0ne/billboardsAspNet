using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillboardsMVC5.Models
{
    public class Billboard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Description { get; set; }
        public string TenantId { get; set; }
        public Single Lat { get; set; }
        public Single Long { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Cost { get; set; }
        public string ImageName { get; set; }

        [NotMapped]
        public DateTime EndRent { get; set; }
    }
}