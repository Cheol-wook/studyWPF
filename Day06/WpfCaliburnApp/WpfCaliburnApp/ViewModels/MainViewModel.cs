using Caliburn.Micro;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using WpfCaliburnApp.Models;

namespace WpfCaliburnApp.ViewModels
{
    public class MainViewModel : Screen
    {
        private EmployeesModel employee;
        public List<EmployeesModel> Listemployees { get; set; }
        string connstring = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True";

        public MainViewModel()
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string strQuery = "SELECT * FROM TblEmployees";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Listemployees = new List<EmployeesModel>();

                while (reader.Read())
                {
                    var temp = new EmployeesModel
                    {
                        id = (int)reader["id"],
                        EmpName = reader["EmpName"].ToString(),
                        Salary = (decimal)reader["Salary"],
                        DeptName = reader["DeptName"].ToString(),
                        Destination = reader["Destination"].ToString()
                    };

                    Listemployees.Add(temp);
                }
            }
        }

        int Id;
        public int id
        {
            get { return Id; }
            set
            {
                Id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }
        string empName;
        public string EmpName
        {
            get { return empName; }
            set
            {
                empName = value;
                NotifyOfPropertyChange(() => empName);
            }
        }
        decimal salary;
        public decimal Salary
        {
            get { return salary; }
            set
            {
                salary = value;
                NotifyOfPropertyChange(() => salary);
            }
        }
        string deptName;
        public string DeptName
        {
            get { return deptName; }
            set
            {
                deptName = value;
                NotifyOfPropertyChange(() => deptName);
            }
        }
        string destination;
        public string Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                NotifyOfPropertyChange(() => destination);
            }
        }

        private EmployeesModel selectedEmployee;
        public EmployeesModel SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;

                if (value != null)
                {
                    id = value.id;
                    EmpName = value.EmpName;
                    Salary = value.Salary;
                    DeptName = value.DeptName;
                    Destination = value.Destination;
                }
                NotifyOfPropertyChange(() => SelectedEmployee);
            }
        }
    }
}
