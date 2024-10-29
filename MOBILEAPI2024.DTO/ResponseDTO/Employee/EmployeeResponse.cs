namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{
    public class EmployeeResponse
    {
        public int Emp_ID { get; set; }
        public int Cmp_ID { get; set; }
        public string Alpha_Emp_Code { get; set; }
        public string Emp_Full_Name { get; set; }
        public string Emp_Full_Name_Superior { get; set; }
        public string Date_Of_Join { get; set; } // Changed to string
        public string Date_Of_Birth { get; set; } // Changed to string
        public string Dept_Name { get; set; }
        public string Desig_Name { get; set; }
        public string Branch_Name { get; set; }
        public string Vertical_Name { get; set; }
        public string Mobile_No { get; set; }
        public string Work_Email { get; set; }
        public string Gender { get; set; }
        public string Blood_Group { get; set; }
    }
}
