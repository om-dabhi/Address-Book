using System.ComponentModel.DataAnnotations;

namespace AddressBook.Areas.City.Models
{
    public class LOC_CityModel
    {
        public int? CityID { get; set; }

        [Required(ErrorMessage = "Please Enter City Name")]

        public string? CountryName { get; set; }

        public string? StateName { get; set; }
        [Required(ErrorMessage = ("Please Select City Name"))]

        public string? CityName { get; set; }

        [Required(ErrorMessage = ("Please Select Country"))]
        public int CountryID { get; set; }

        [Required(ErrorMessage = ("Please Select State"))]

        public int StateID { get; set; }

        [Required(ErrorMessage = ("Please Enter State Code"))]

        public string? CityCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public string? searchstring { get; set; }

    }
    public class LOC_CityDropDownModel
    {
        public int CityID { get; set; }
        public string? CityName { get; set; }
    }

    public class LOC_CountryDropDownModel
    {
        public int CountryID { get; set; }
        public string? CountryName { get; set; }
    }
    public class LOC_StateDropDownModel
    {
        public int StateID { get; set; }
        public string? StateName { get; set; }

    }
}
