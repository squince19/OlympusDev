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
            string sqlSelect = "SELECT username, userPassword, employeeID FROM users WHERE username=@idValue and userPassword=@passValue";
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
                Session["userID"] = sqlDt.Rows[0]["employeeID"];
                success = true;
            }

            return success;

        }

        [WebMethod(EnableSession = true)]
        public List<Employee> GetNames()
        { 
            //WEB METHOD IN PROGRESS
            List<Employee> Employees = new List<Employee>();
            string userID = Session["userID"].ToString();

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
            string sqlSelect = "SELECT DISTINCT FName, LName, employeeID, ManagerID from employee_data WHERE ManagerID = " + userID;
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);

            int count = sqlDt.Rows.Count;
            int mgrID = 0;


            foreach(DataRow row in sqlDt.Rows)
            {
                Employee tempEmployee = new Employee();
                tempEmployee.fname = row["FName"].ToString();
                tempEmployee.lname = row["LName"].ToString();
                tempEmployee.employeeId = Convert.ToInt32(row["employeeID"]);
                tempEmployee.ManagerID = Convert.ToInt32(row["ManagerID"]);
                mgrID = tempEmployee.ManagerID;
                string sqlConnectString2 = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
                string sqlSelect2 = "SELECT FName, LName from employee_data WHERE employeeID = " + mgrID;
                MySqlConnection sqlConnection2 = new MySqlConnection(sqlConnectString2);
                MySqlCommand sqlCommand2 = new MySqlCommand(sqlSelect2, sqlConnection2);

                MySqlDataAdapter sqlDa2 = new MySqlDataAdapter(sqlCommand2);
                DataTable sqlDt2 = new DataTable();
                sqlDa2.Fill(sqlDt2);

                string managerFirstName = sqlDt2.Rows[0]["FName"].ToString();
                string managerLastName = sqlDt2.Rows[0]["LName"].ToString();
                tempEmployee.ManagerName = managerFirstName + " " + managerLastName;

                Employees.Add(tempEmployee);
            }
            return Employees;

        }

        [WebMethod(EnableSession = true)]
        public Employee GetEmployeeInformation(string employeeID)
        {
            int id = Convert.ToInt32(employeeID);
            Employee emp = new Employee();
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
            string sqlSelect = "SELECT * FROM employee_data WHERE employeeID = " + id;
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);

            emp.fname = sqlDt.Rows[0]["FName"].ToString();
            emp.lname = sqlDt.Rows[0]["LName"].ToString();
            emp.employeeId = Convert.ToInt32(sqlDt.Rows[0]["employeeID"]);
            emp.ManagerID = Convert.ToInt32(sqlDt.Rows[0]["ManagerID"]);
            int mgrID = emp.ManagerID;
            string sqlConnectString2 = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
            string sqlSelect2 = "SELECT FName, LName from employee_data WHERE employeeID = " + mgrID;
            MySqlConnection sqlConnection2 = new MySqlConnection(sqlConnectString2);
            MySqlCommand sqlCommand2 = new MySqlCommand(sqlSelect2, sqlConnection2);

            MySqlDataAdapter sqlDa2 = new MySqlDataAdapter(sqlCommand2);
            DataTable sqlDt2 = new DataTable();
            sqlDa2.Fill(sqlDt2);

            string managerFirstName = sqlDt2.Rows[0]["FName"].ToString();
            string managerLastName = sqlDt2.Rows[0]["LName"].ToString();
            emp.ManagerName = managerFirstName + " " + managerLastName;

            return emp;
        }

        [WebMethod(EnableSession = true)]
        public List<Employee> SearchEmployee(string name)
        {
            List<Employee> searchResults = new List<Employee>();
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
            string sqlSelect = "SELECT DISTINCT FName, LName from employee_data where FName LIKE '%" + name + "%' or LName LIKE '%" + name + "%'";
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@name", HttpUtility.UrlDecode(name));


            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);

            foreach(DataRow row in sqlDt.Rows)
            {
                Employee tempEmployee = new Employee();
                tempEmployee.fname = row["FName"].ToString();
                tempEmployee.lname = row["LName"].ToString();
                searchResults.Add(tempEmployee);
            }

            return searchResults;

        }

        
    }
}
