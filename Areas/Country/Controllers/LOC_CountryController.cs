using AddressBook.Areas.Country.Models;
using AddressBook.Areas.State.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace AddressBook.Areas.Country.Controllers
{
    [Area("Country")]
    [Route("Country/{controller}/{action}")]
    public class LOC_CountryController : Controller
    {
        private IConfiguration Configuration;

        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }


        public IActionResult Index()
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand objCmd = con.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Country_SelectAll";
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                if (objSDR.HasRows)
                {
                    dt.Load(objSDR);
                }
                con.Close();
                return View("LOC_CountryList", dt);
            }
        }

        public IActionResult Delete(int CountryID)
        {
            string constr = Configuration.GetConnectionString("myConnectionString");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand objCmd = con.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Country_DeleteByPK";
                objCmd.Parameters.AddWithValue("@CountryID", CountryID);
                objCmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("Index");
            }
        }

        public IActionResult LOC_CountryList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(LOC_CountryModel modelLOC_Country)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand objCmd = con.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;

                if (modelLOC_Country.CountryID == null)
                {
                    objCmd.CommandText = "PR_Country_Insert";
                }
                else
                {
                    objCmd.CommandText = "PR_Country_UpdateByPK";
                    objCmd.Parameters.Add("CountryID", SqlDbType.Int).Value = modelLOC_Country.CountryID;
                }
                objCmd.Parameters.Add("CountryName", SqlDbType.VarChar).Value = modelLOC_Country.CountryName;
                objCmd.Parameters.Add("CountryCode", SqlDbType.VarChar).Value = modelLOC_Country.CountryCode;

                if (Convert.ToBoolean(objCmd.ExecuteNonQuery()))
                {
                    if (modelLOC_Country.CountryID == null)
                    {
                        TempData["CountryInsertMsg"] = "Record Inserted Successfully";
                    }
                    else
                    {
                        TempData["CountryInsertMsg"] = "Record Updated Successfully";
                    }
                }
                con.Close();
                return View("LOC_CountryAddEdit");
            }
        }

        public IActionResult Add(int? CountryID)
        {
            if (CountryID != null)
            {
                string str = Configuration.GetConnectionString("myConnectionString");
                using (SqlConnection con = new SqlConnection(str))
                {
                    con.Open();
                    SqlCommand objCmd = con.CreateCommand();
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_Country_SelectByPK";
                    objCmd.Parameters.Add("CountryID", SqlDbType.Int).Value = CountryID;
                    DataTable dt = new DataTable();
                    SqlDataReader objSDR = objCmd.ExecuteReader();
                    LOC_CountryModel country = new LOC_CountryModel();
                    if (objSDR.HasRows)
                    {
                        while (objSDR.Read())
                        {
                            country.CountryName = objSDR["CountryName"].ToString();
                            country.CountryCode = objSDR["CountryCode"].ToString();
                        }

                    }
                    return View("LOC_CountryAddEdit", country);
                }
            }
            return View("LOC_CountryAddEdit");
        }
        public IActionResult CountrySearch(LOC_CountryModel lOC_CountryModel)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand objCmd = con.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Country_SelectByCountryName";
                objCmd.Parameters.AddWithValue("@CountryName", lOC_CountryModel.CountryName);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                if (objSDR.HasRows)
                {
                    dt.Load(objSDR);
                }
                con.Close();
                return View("LOC_CountryList", dt);
            }
        }
    }
}