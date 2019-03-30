using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace Project2
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public int NumberOfAccounts()
        {
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
            string sqlSelect = "SELECT * from users";
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);
            return sqlDt.Rows.Count;
        }

        [WebMethod(EnableSession = true)]
        public bool LogOn(string username, string userPassword)
        {
            bool success = false;
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
            string sqlSelect = "SELECT username, userPassword FROM users WHERE username=@idValue and userPassword=@passValue";
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idValue", HttpUtility.UrlDecode(username));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(userPassword));
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);
            
            if(sqlDt.Rows.Count > 0)
            {
                Session["loggedIn"] = "true";
                success = true;
            }

            return success;
        }

        [WebMethod(EnableSession = true)]
        public List<Employee> GetNames()
        { 
            //WEB METHOD IN PROGRESS
            List<Employee> Employees = new List<Employee>();

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
            string sqlSelect = "SELECT DISTINCT FName, LName from employee_data";
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);

            int count = sqlDt.Rows.Count;

            foreach(DataRow row in sqlDt.Rows)
            {
                Employee tempEmployee = new Employee();
                tempEmployee.fname = row["FName"].ToString();
                tempEmployee.lname = row["LName"].ToString();
                Employees.Add(tempEmployee);
            }
            return Employees;

        }

        [WebMethod(EnableSession = true)]
        public List<Employee> SearchEmployee(string name)
        {
            List<Employee> searchResults = new List<Employee>();
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;



        }
    }
}
