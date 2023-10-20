namespace AddressBook.Areas.State.Models
{
    public class LOC_StateModel
    {
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string? StateName { get; set; }
        public string? CountryName { get; set; }
        public string? StateCode { get; set; }
        public string Created { get; set; }    
        public string Modified { get; set; } 
    }
    public class LOC_StateDropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
}
