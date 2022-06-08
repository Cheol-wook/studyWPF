using Caliburn.Micro;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using WpfCaliburnApp.Models;

namespace WpfCaliburnApp.ViewModels
{
    public class MainViewModel : Screen
    {
        //private EmployeesModel employee;
        private BindableCollection<EmployeesModel> listEmployees;       //니들은 List같은거 사용하지마라...
        public BindableCollection<EmployeesModel> Listemployees
        {
            get { return listEmployees; }
            set
            {
                listEmployees = value;
                NotifyOfPropertyChange(() => listEmployees);
            }
        }
        string connstring = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True";

        public MainViewModel()
        {
            GetEmployees();
        }

        public void GetEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string strQuery = "SELECT * FROM TblEmployees";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Listemployees = new BindableCollection<EmployeesModel>();

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
                NotifyOfPropertyChange(() => CanDelEmployee);
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
                NotifyOfPropertyChange(() => CanSaveEmployee);
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
                NotifyOfPropertyChange(() => CanSaveEmployee);
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
                NotifyOfPropertyChange(() => CanSaveEmployee);
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
                NotifyOfPropertyChange(() => CanSaveEmployee);
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

        public void NewEmployee()
        {
            id = 0;
            EmpName = string.Empty;
            Salary = 0;
            DeptName = Destination = string.Empty;
        }

        //버튼 활성/비활성 위한 속성
        public bool CanSaveEmployee
        {
            get {
                return !string.IsNullOrEmpty(EmpName) &&
                  !string.IsNullOrEmpty(DeptName) &&
                  !string.IsNullOrEmpty(Destination) &&
                  salary != 0;
            }
        }

        //버튼 이벤트 정의
        public void SaveEmployee()
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if (id == 0)     //insert
                    cmd.CommandText = @"INSERT INTO TblEmployees
                                                (EmpName
                                                , Salary
                                                , DeptName
                                                , Destination)
                                            VALUES
                                                (@EmpName
                                                , @Salary
                                                , @DeptName
                                                , @Destination)";
                else             //update
                    cmd.CommandText = @"UPDATE TblEmployees
                                       SET EmpName = @EmpName
                                          ,Salary = @Salary
                                          ,DeptName = @DeptName
                                          ,Destination = @Destination
                                      WHERE  id = @id ";

                SqlParameter parmEmpName = new SqlParameter("@EmpName", EmpName);
                SqlParameter parmSalary = new SqlParameter("@Salary", Salary);
                SqlParameter parmDeptName = new SqlParameter("@DeptName", DeptName);
                SqlParameter parmDestination = new SqlParameter("@Destination", Destination);

                cmd.Parameters.Add(parmEmpName);
                cmd.Parameters.Add(parmSalary);
                cmd.Parameters.Add(parmDeptName);
                cmd.Parameters.Add(parmDestination);

                if(id != 0)     //update일 경우 id값도 파라미터로 추가해야함
                {
                    SqlParameter parmid = new SqlParameter("@id", id);
                    cmd.Parameters.Add(parmid);
                }

                cmd.ExecuteNonQuery();

            }   // End of Using (SqlConnection ...)

            // 입력창 전부 초기화
            NewEmployee();

            // 데이터 다시 조회
            GetEmployees();
        }

        public bool CanDelEmployee
        {
            get { return (id != 0); }
        }
        public void DelEmployee()
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string strQuery = "DELETE FROM TblEmployees WHERE id = @id";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlParameter parmId = new SqlParameter("@id", id);
                cmd.Parameters.Add(parmId);

                cmd.ExecuteNonQuery();
            }

            NewEmployee();          // 입력창 초기화
            GetEmployees();         // 데이터 그리드 재호출
        }
    }
}
