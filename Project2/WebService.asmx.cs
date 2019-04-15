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

        //FINISHED
        //DUMMY WEB SERVICE
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

        //FINSIHED
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

        //FINISHED
        //THIS RUNS WHEN THEPAGE IS LOADED, AND RETURNS A LIST OF EMPLOYEES FROM A SPECIFIC MANAGER
        [WebMethod(EnableSession = true)]
        public List<Employee> GetNames()
        { 
            List<Employee> Employees = new List<Employee>();

            string sqlSelect = "";
            string sqlSelect2 = "";
            string userID = "";

            int mgrID = 0;


            if (Session["userID"] == null)
            {
                sqlSelect = "SELECT DISTINCT FName, LName, employeeID, ManagerID from employee_data;";
            }
            else if (Session["userID"] != null)
            {
                userID = Session["userID"].ToString();
                sqlSelect = "SELECT DISTINCT FName, LName, employeeID, ManagerID from employee_data WHERE ManagerID = " + userID;
                sqlSelect2 = "SELECT FName, LName from employee_data WHERE employeeID = " + mgrID;

            }

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
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
                tempEmployee.ManagerID = Convert.ToInt32(row["ManagerID"]);
                mgrID = tempEmployee.ManagerID;


                if (Session["userID"] == null)
                {

                    sqlSelect2 = "SELECT FName, LName from employee_data where employeeID = " + tempEmployee.ManagerID;

                }
                else if (Session["userID"] != null)
                {
                    sqlSelect2 = "SELECT FName, LName from employee_data WHERE employeeID = " + mgrID;

                }
                tempEmployee.employeeId = Convert.ToInt32(row["employeeID"]);
                string sqlConnectString2 = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
                MySqlConnection sqlConnection2 = new MySqlConnection(sqlConnectString2);
                MySqlCommand sqlCommand2 = new MySqlCommand(sqlSelect2, sqlConnection2);

                MySqlDataAdapter sqlDa2 = new MySqlDataAdapter(sqlCommand2);
                DataTable sqlDt2 = new DataTable();
                sqlDa2.Fill(sqlDt2);

                try
                {
                    string managerFirstName = sqlDt2.Rows[0]["FName"].ToString();
                    string managerLastName = sqlDt2.Rows[0]["LName"].ToString();
                    tempEmployee.ManagerName = managerFirstName + " " + managerLastName;
                }
                catch (Exception e)
                {

                }

                Employees.Add(tempEmployee);
            }
            return Employees;

        }

        //FINISHED 
        //GETS INFORMATION FOR EVERY EMPLOYEE FOR PUTTING IN THE NOTES BOX
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
            emp.Department = sqlDt.Rows[0]["dept"].ToString();
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

        //FINISHED
        //NEED TO REVIEW DATABASE TO CHECK FOR INVALID DATA
        //INVALID DATA FOR JOSEPH LOCK
        [WebMethod(EnableSession = true)]
        public List<Employee> EmployeeGraph()
        {
            //accepts bool
            //if it accepts true, then it will get a list of employees for just a manager
            //if it accepts false, then it will get ALL employees
            bool success = false;
            List<Employee> employeeInfo = new List<Employee>();
            int date = 7;
            int employeeCount;
            int mgrid = Convert.ToInt32(Session["userID"]);
            string firstName = "";
            string lastName = "";

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;

            string sqlSelect = "";

            if (success == true)
            {
                sqlSelect =
                    "select cd.EmployeeID, cd.FirstName, cd.LastName, ed.ManagerID, " +
                    "ROUND((SUM(cd.CallLengthHrs)/400)) as \"Time Worked\", hw.HoursWorked, ROUND((SUM(cd.CallLengthHrs)/400)/(hw.HoursWorked)*100) as \"Productivity Level\" " +
                        "FROM call_data_v2 cd, hours_worked hw, employee_data ed" +
                            " where Day(Call_Date) = " + date + " and ed.employeeID = cd.employeeID and ManagerID = " + mgrid +
                            " group by cd.EmployeeID; ";
            }
            else if (success == false)
            {
                sqlSelect =
                    "select cd.EmployeeID, cd.FirstName, cd.LastName, ed.ManagerID, " +
                    "ROUND((SUM(cd.CallLengthHrs)/400)) as \"Time Worked\", hw.HoursWorked, ROUND((SUM(cd.CallLengthHrs)/400)/(hw.HoursWorked)*100) as \"Productivity Level\" " +
                        "FROM call_data_v2 cd, hours_worked hw, employee_data ed" +
                            " where Day(Call_Date) = " + date + " and ed.employeeID = cd.employeeID " +
                            " group by cd.EmployeeID; ";

            }//end if/else



            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);

            employeeCount = sqlDt.Rows.Count;

            int rowposition = 0;
            foreach (DataRow row in sqlDt.Rows)
            {
                int position = 0;
                Employee temp = new Employee();
                temp.fname = sqlDt.Rows[rowposition]["FirstName"].ToString();
                temp.lname = sqlDt.Rows[rowposition]["LastName"].ToString();
                temp.employeeId = Convert.ToInt32(sqlDt.Rows[rowposition]["employeeID"]);
                temp.productivityLevel = new int[5];

                //declare employee properties
                //start for loop, date starting at 7;
                for (int i = 7; i <= 11; i++)
                {
                    firstName = temp.fname;
                    lastName = temp.lname;

                    string sqlConnectString2 = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
                    string sqlSelect2 = "";
                    if (success == true)
                    {
                        sqlSelect2 =
                            "select cd.EmployeeID, cd.FirstName, cd.LastName, ed.ManagerID, ROUND((SUM(cd.CallLengthHrs)/400)) as \"Time Worked\", " +
                            "hw.HoursWorked, ROUND((SUM(cd.CallLengthHrs)/400)/(hw.HoursWorked)*100) as \"Productivity Level\" " +
                                "FROM call_data_v2 cd, hours_worked hw, employee_data ed " +
                                    " where Day(Call_Date) = " + i + " and ed.employeeID = cd.employeeID and ManagerID = " + mgrid +
                                    " and ed.FName = \"" + firstName + "\" and ed.LName = \"" + lastName + "\";";
                    }
                    else if (success == false)
                    {
                        sqlSelect2 =
                            "select cd.EmployeeID, cd.FirstName, cd.LastName, ed.ManagerID, ROUND((SUM(cd.CallLengthHrs)/400)) as \"Time Worked\", " +
                            "hw.HoursWorked, ROUND((SUM(cd.CallLengthHrs)/400)/(hw.HoursWorked)*100) as \"Productivity Level\" " +
                                "FROM call_data_v2 cd, hours_worked hw, employee_data ed " +
                                    " where Day(Call_Date) = " + i + " and ed.employeeID = cd.employeeID " +
                                    " and ed.FName = \"" + firstName + "\" and ed.LName = \"" + lastName + "\";";
                    }

                    MySqlConnection sqlConnetion2 = new MySqlConnection(sqlConnectString2);
                    MySqlCommand sqlCommand2 = new MySqlCommand(sqlSelect2, sqlConnetion2);

                    MySqlDataAdapter sqlDa2 = new MySqlDataAdapter(sqlCommand2);
                    DataTable sqlDt2 = new DataTable();
                    sqlDa2.Fill(sqlDt2);

                    int prodLevel = 0;

                    try
                    {
                        prodLevel = Convert.ToInt32(sqlDt2.Rows[0]["Productivity Level"]);
                    }
                    catch(Exception e)
                    {

                    }


                    temp.productivityLevel[position] = prodLevel;
                    position++;
                }//end for loop

                
                employeeInfo.Add(temp);
                rowposition++;
                
            }//end foreach
            return employeeInfo;
        }//end EmployeeGraph webmethod

        //FINISHED
        [WebMethod(EnableSession = true)]
        public List<Employee> SearchEmployee(string name)
        {
            List<Employee> searchResults = new List<Employee>();
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
            string sqlSelect = "SELECT DISTINCT FName, LName from employee_data where FName LIKE '%" + name + "%' or LName LIKE '%" + name + "%'";
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);


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

        }//end SearchEmployee web method

        [WebMethod(EnableSession = true)]
        public List<Note> LoadNotes(string employeeID)
        {
            List<Note> noteList = new List<Note>();
            Convert.ToInt32(employeeID);

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
            //plug in sql statement later
            string sqlSelect =
                "select  mn.noteid, mn.EmployeeID, mn.ManagerID, dt.Fname, dt.Lname, mn.note_subject, mn.note_body " +
                    "from employee_data ed LEFT JOIN manager_notes mn ON ed.employeeID = mn.employeeID" +
                    " LEFT JOIN employee_data dt ON mn.ManagerID = dt.employeeID;";
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);


            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();

            sqlDa.Fill(sqlDt);
            

            foreach (DataRow row in sqlDt.Rows)
            {
                try
                {
                    if (Convert.ToInt32(row["employeeID"]) == Convert.ToInt32(employeeID))
                    {
                        Note tempNote = new Note();

                        tempNote.ManagerID = row["ManagerID"].ToString();

                        tempNote.Subject = row["note_subject"].ToString();
                        tempNote.Body = row["note_body"].ToString();
                        string firstName = row["FName"].ToString();
                        string lastName = row["LName"].ToString();
                        tempNote.ManagerName = firstName + " " + lastName;
                        tempNote.ManagerID = row["ManagerID"].ToString();
                        tempNote.NoteID = row["noteID"].ToString();
                        tempNote.EmployeeID = row["EmployeeID"].ToString();
                        noteList.Add(tempNote);
                    }
                }
                catch(Exception e)
                {

                }
                
            }

                return noteList;
        }


        [WebMethod]
        public Note GetNoteInfo(int id)
        {
            Note tempNote = new Note();
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["olympusDB"].ConnectionString;
            string sqlSelect = "SELECT * FROM manager_notes WHERE noteID = " + id;
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);


            tempNote.NoteID = sqlDt.Rows[0]["noteid"].ToString();
            


            return tempNote;
        }

    }//end class

}//end namespace*
