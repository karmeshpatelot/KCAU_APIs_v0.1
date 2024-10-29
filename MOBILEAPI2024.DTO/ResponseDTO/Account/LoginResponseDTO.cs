namespace MOBILEAPI2024.DTO.ResponseDTO.Account
{
    public class LoginResponseDTO
    {
        public LoginData? LoginData { get; set; }
        public List<Detail>? Details { get; set; }
        public string? Token { get; set; }
    }

    public class LoginData
    {
        public int Login_ID { get; set; }
        public string Login_Name { get; set; }
        public int Cmp_ID { get; set; }
        public int Emp_ID { get; set; }
        public string Alpha_Emp_Code { get; set; }
        public string Emp_Full_Name { get; set; }
        public string Emp_Left { get; set; }
        public string Image_Name { get; set; }
        public string Dept_Name { get; set; }
        public string Desig_Name { get; set; }
        public string LoginDate { get; set; }
        public int Is_Geofence_enable { get; set; }
        public int Is_Camera_enable { get; set; }
        public string Cmp_Logo { get; set; }
        public int Is_Route { get; set; }
        public int IsVertical { get; set; }
        public int Is_MobileWorkplan_Enable { get; set; }
        public int Is_MobileStock_Enable { get; set; }
        public int Is_VBA { get; set; }
        public int Store_ID { get; set; }
        public string Store_Name { get; set; }
        public string Emp_Sort_Name { get; set; }
        public int DEPT_Id { get; set; }
        public int Desig_Id { get; set; }
        public int Is_GuestPrivilege { get; set; }
    }

    public class Detail
    {
        public int Trans_Id { get; set; }
        public int Privilage_ID { get; set; }
        public int Cmp_Id { get; set; }
        public int Form_Id { get; set; }
        public int Is_View { get; set; }
        public int Is_Edit { get; set; }
        public int Is_Save { get; set; }
        public int Is_Delete { get; set; }
        public int Is_Print { get; set; }
        public string FORM_NAME { get; set; }
        public string Privilege_Name { get; set; }
        public string Alias { get; set; }
        public int Is_Active_For_menu { get; set; }
    }
}
