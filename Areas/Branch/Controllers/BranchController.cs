using Country_State_City_Final.Areas.Branch.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Country_State_City_Final.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Route("Branch/[controller]/[action]")]
    public class BranchController : Controller
    {
        private readonly IConfiguration _configuration;
        public BranchController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public IActionResult BranchList()
        {
            string connection = this._configuration.GetConnectionString("myconnectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();

            SqlCommand command = sqlConnection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_BRANCH_SELECT_All";
            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            sqlConnection.Close();
            return View(dt);
        }

        public IActionResult BranchSave(Branchmodel branchmodel) 
        {
            string connection = this._configuration.GetConnectionString("myconnectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;



            if(branchmodel.BranchId == 0) 
            {
                cmd.CommandText = "PR_MST_BRANCH_INSERT";
            }
            else
            {
                cmd.CommandText = "PR_MST_BRANCH_Update";
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = branchmodel.BranchId;

            }


            cmd.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = branchmodel.BranchName;
            cmd.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = branchmodel.BranchCode;
            cmd.ExecuteNonQuery();
            return RedirectToAction("BranchList", "Branch", new { area = "Branch" });
        }
        public IActionResult BranchAddEdit(int? BranchId)
        {
            if (BranchId != null)
            {
                string connection = this._configuration.GetConnectionString("myconnectionString");
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();

                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Branch_SelectByPK";
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sqlDataReader);
                Branchmodel branchmodel = new Branchmodel();
                foreach (DataRow dataRow in dt.Rows)
                {
                    branchmodel.BranchId = Convert.ToInt32(dataRow["BranchId"]);
                    branchmodel.BranchName = dataRow["BranchName"].ToString();
                    branchmodel.BranchCode = dataRow["BranchCode"].ToString();
                }

                sqlConnection.Close();
                return View("BranchAddEdit", branchmodel);
            }
            return View("BranchAddEdit");
        }
        public IActionResult BranchDelete(int BranchId)
        {
            string connection = this._configuration.GetConnectionString("myconnectionString");
            SqlConnection sqlConnection= new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = sqlConnection.CreateCommand();
            command.CommandType= CommandType.StoredProcedure;
            command.CommandText = "PR_MST_BRANCH_DELETE";
            command.Parameters.AddWithValue("@BranchId", BranchId);
            command.ExecuteNonQuery();
            sqlConnection.Close();
            return RedirectToAction("BranchList");
        }
    }
}
