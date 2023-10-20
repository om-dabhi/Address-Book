using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook.Areas.State.Models;
using AddressBook.Areas.Country.Models;

namespace AddressBook.Areas.State.Controllers
{
    [Area("State")]
    [Route("State/{controller}/{action}")]
    public class LOC_StateController : Controller
    {

            private IConfiguration Configuration;

            public LOC_StateController(IConfiguration _configuration)
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
                    objCmd.CommandText = "PR_State_SelectAll";
                    DataTable dt = new DataTable();
                    SqlDataReader objSDR = objCmd.ExecuteReader();
                    if (objSDR.HasRows)
                    {
                        dt.Load(objSDR);
                    }
                    con.Close();
                    return View("LOC_StateList", dt);
                }
            }
            public IActionResult Delete(int StateID)
            {
                string constr = Configuration.GetConnectionString("myConnectionString");
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    SqlCommand objCmd = con.CreateCommand();
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_State_DeleteByPK";
                    objCmd.Parameters.AddWithValue("@StateID", StateID);
                    objCmd.ExecuteNonQuery();
                    con.Close();
                    return RedirectToAction("Index");
                }
            }
            public IActionResult LOC_StateList()
            {
                return View();
            }

        public IActionResult Save(LOC_StateModel modelLOC_State)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand objCmd = con.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;

                if (modelLOC_State.StateID == 0)
                {
                    objCmd.CommandText = "PR_State_Insert";
                }
                else
                {
                    objCmd.CommandText = "PR_State_UpdateByPK";
                    objCmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelLOC_State.StateID;
                }
                objCmd.Parameters.Add("@StateName", SqlDbType.VarChar).Value = modelLOC_State.StateName;
                objCmd.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = modelLOC_State.StateCode;
                objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_State.CountryID;

                if (Convert.ToBoolean(objCmd.ExecuteNonQuery()))
                {
                    if (modelLOC_State.StateID == null)
                    {
                        TempData["StateInsertMsg"] = "Record Inserted Successfully";
                    }
                    else
                    {
                        TempData["StateInsertMsg"] = "Record Updated Successfully";
                    }
                }
                con.Close();
                return RedirectToAction("Index");
            }
        }
        public IActionResult Add(int? StateID)
        {
            FillCountryDDL();
            if (StateID != null)
            {
                string str = Configuration.GetConnectionString("myConnectionString");
                using (SqlConnection con = new SqlConnection(str))
                {
                    con.Open();
                    SqlCommand objCmd = con.CreateCommand();
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_State_SelectByPK";
                    objCmd.Parameters.Add("StateID", SqlDbType.Int).Value = StateID;
                    DataTable dt = new DataTable();
                    SqlDataReader objSDR = objCmd.ExecuteReader();
                    LOC_StateModel state = new LOC_StateModel();
                    if (objSDR.HasRows)
                    {
                        while (objSDR.Read())
                        {
                            state.StateName = objSDR["StateName"].ToString();
                            state.StateCode = objSDR["StateCode"].ToString();
                        }

                    }
                    return View("LOC_StateAddEdit", state);
                }
            }
            return View("LOC_StateAddEdit");
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
        public IActionResult StateSearch(LOC_StateModel lOC_StateModel)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand objCmd = con.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_State_SelectByStateName";
                objCmd.Parameters.AddWithValue("@StateName", lOC_StateModel.StateName);
                objCmd.Parameters.AddWithValue("@StateCode", lOC_StateModel.StateCode);
                objCmd.Parameters.AddWithValue("@CountryName", lOC_StateModel.CountryName);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                if (objSDR.HasRows)
                {
                    dt.Load(objSDR);
                }
                con.Close();
                return View("LOC_StateList", dt);
            }
        }
    }
}
