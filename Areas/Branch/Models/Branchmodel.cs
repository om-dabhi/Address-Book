using System.ComponentModel.DataAnnotations;

namespace Country_State_City_Final.Areas.Branch.Models
{
    public class Branchmodel
    {
        public int BranchId { get; set; }
        [Required]
        public string? BranchName { get; set; }
        [Required]

        public string? BranchCode { get; set;}

    }
}
