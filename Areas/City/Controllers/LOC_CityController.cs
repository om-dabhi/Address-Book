using AddressBook.Areas.City.Models;
using AddressBook.Areas.Country.Models;
using AddressBook.Areas.State.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using LOC_CountryDropDownModel = AddressBook.Areas.City.Models.LOC_CountryDropDownModel;
using LOC_StateDropDownModel = AddressBook.Areas.City.Models.LOC_StateDropDownModel;

namespace AddressBook.Areas.City.Controllers
{
    [Area("City")]
    [Route("City/{controller}/{action}")]
    public class LOC_CityController : Controller
    {
        private IConfiguration Configuration;

        public LOC_CityController(IConfiguration _configuration)
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
                objCmd.CommandText = "PR_City_SelectAll";
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                if (objSDR.HasRows)
                {
                    dt.Load(objSDR);
                }
                con.Close();
                return View("LOC_CityList", dt);
            }
        }

        public IActionResult Delete(int CityID)
        {
            string constr = Configuration.GetConnectionString("myConnectionString");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand objCmd = con.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_City_DeleteByPK";
                objCmd.Parameters.AddWithValue("@CityID", CityID);
                objCmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("Index");
            }
        }

        public IActionResult LOC_CityList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand objCmd = con.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;

                if (modelLOC_City.CityID == null)
                {
                    objCmd.CommandText = "PR_City_Insert";
                }
                else
                {
                    objCmd.CommandText = "PR_City_UpdateByPK";
                    objCmd.Parameters.Add("CityID", SqlDbType.Int).Value = modelLOC_City.CityID;
                }
                objCmd.Parameters.Add("CityName", SqlDbType.VarChar).Value = modelLOC_City.CityName;
                objCmd.Parameters.Add("StateID", SqlDbType.VarChar).Value = modelLOC_City.StateID;
                objCmd.Parameters.Add("CountryID", SqlDbType.VarChar).Value = modelLOC_City.CountryID;
                objCmd.Parameters.Add("CityCode", SqlDbType.VarChar).Value = modelLOC_City.CityCode;

                if (Convert.ToBoolean(objCmd.ExecuteNonQuery()))
                {
                    if (modelLOC_City.CityID == null)
                    {
                        TempData["CityInsertMsg"] = "Record Inserted Successfully";
                    }
                    else
                    {
                        TempData["CityInsertMsg"] = "Record Updated Successfully";
                    }
                }
                con.Close();
                return View("LOC_CityAddEdit");
            }
        }

        public IActionResult Add(int? CityID)
        {
            FillStateDDL();
            FillCountryDDL();
            if (CityID != null)
            {
                string str = Configuration.GetConnectionString("myConnectionString");
                using (SqlConnection con = new SqlConnection(str))
                {
                    con.Open();
                    SqlCommand objCmd = con.CreateCommand();
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_City_SelectByPK";
                    objCmd.Parameters.Add("CityID", SqlDbType.Int).Value = CityID;
                    DataTable dt = new DataTable();
                    SqlDataReader objSDR = objCmd.ExecuteReader();
                    LOC_CityModel City = new LOC_CityModel();
                    if (objSDR.HasRows)
                    {
                        while (objSDR.Read())
                        {
                            City.CityName = objSDR["CityName"].ToString();
                            City.StateID = (int)objSDR["StateID"];
                            City.CountryID = (int)objSDR["CountryID"];
                            City.CityCode = objSDR["CityCode"].ToString();
                        }

                    }
                    return View("LOC_CityAddEdit", City);
                }
            }
            return View("LOC_CityAddEdit");
        }

        public IActionResult CitySearch(LOC_CityModel lOC_CityModel)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand objCmd = con.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_SelectByCityName";
                objCmd.Parameters.AddWithValue("@CityName", lOC_CityModel.CityName);
                objCmd.Parameters.AddWithValue("@CityCode", lOC_CityModel.CityCode);
                objCmd.Parameters.AddWithValue("@StateName", lOC_CityModel.StateName);
                objCmd.Parameters.AddWithValue("@CountryName", lOC_CityModel.CountryName);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                if (objSDR.HasRows)
                {
                    dt.Load(objSDR);
                }
                con.Close();
                return View("LOC_CityList", dt);
            }
        }
        public IActionResult DropDownByCountry(int CountryID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            DataTable dt = new DataTable();
            conn.Open();
            SqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_Country_SelectByCountryName";
            cmd1.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            dt.Load(sdr1);
            conn.Close();

            List<LOC_StateDropDownModel> list1 = new List<LOC_StateDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                LOC_StateDropDownModel vlst1 = new LOC_StateDropDownModel();
                vlst1.StateID = Convert.ToInt32(dr["StateID"]);
                vlst1.StateName = dr["StateName"].ToString();
                list1.Add(vlst1);
            }

            var vModel = list1;
            return Json(vModel);
        }
        public void FillStateDDL()
        {
            string str = this.Configuration.GetConnectionString("myconnectionString");
            SqlConnection sqlConnection = new SqlConnection(str);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_DropDown";

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);

            List<LOC_StateDropDownModel> Statelist = new List<LOC_StateDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                LOC_StateDropDownModel tempState = new LOC_StateDropDownModel();
                tempState.StateID = Convert.ToInt32(dr["StateID"]);
                tempState.StateName = dr["StateName"].ToString();
                Statelist.Add(tempState);
            }
            sqlConnection.Close();
            ViewBag.StateList = Statelist;
        }

        public void FillCountryDDL()
        {

            string constr = Configuration.GetConnectionString("myConnectionString");
            SqlConnection SqlConnection = new SqlConnection(constr);
            SqlConnection.Open();

            SqlCommand cmd = SqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_SelectAll";

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);

            List<LOC_CountryDropDownModel> countrylist = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                LOC_CountryDropDownModel tempcountry = new LOC_CountryDropDownModel();
                tempcountry.CountryID = Convert.ToInt32(dr["CountryId"]);
                tempcountry.CountryName = dr["CountryName"].ToString();
                countrylist.Add(tempcountry);
            }
            SqlConnection.Close();
            ViewBag.CountryList = countrylist;
        }
    }
}