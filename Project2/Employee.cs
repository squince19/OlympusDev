using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project2
{
    public class Employee
    {
        //DO NOT PUT GETTERS AND SETTERS
        public int employeeId;
        public string fname;
        public string lname;
        public int[] productivityLevel;
        public string SLAColor;
        public string ManagerName;
        public int ManagerID;
        public string Department;

        public override string ToString()
        {
            return fname + " " + lname;
        }




    }
}