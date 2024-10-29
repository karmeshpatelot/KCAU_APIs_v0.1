namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{
    public class ExceptionForView
    {
        public string? name { get; set; }
        public string? code { get; set; }
    }

    public class Record
    {
        public string? datetime { get; set; }
        public string? userName { get; set; }
        public string? userId { get; set; }
        public string? userGroupName { get; set; }
        public string? shift { get; set; }
        public bool? overtimeRuleShift { get; set; }
        public string? leave { get; set; }
        public string? inTime { get; set; }
        public string? outTime { get; set; }
        public bool? isInTimeNextDay { get; set; }
        public bool? isOutTimeNextDay { get; set; }
        public string? exception { get; set; }
        public List<ExceptionForView>? exceptionForView { get; set; }
        public string? normalRegular { get; set; }
        public string? normalOvertime { get; set; }
        public string? punchBreak { get; set; }
        public string? overBreak { get; set; }
        public string? mealTime { get; set; }
        public string? totalWorkTime { get; set; }
        public string? regularByTimeRate { get; set; }
        public string? overtimeByTimeRate { get; set; }
    }

    public class ReportResponse
    {
        public string? message { get; set; }
        public string? message_key { get; set; }
        public string? language { get; set; }
        public string? status_code { get; set; }
        public List<Record> records { get; set; }
        public int? total { get; set; }
    }
}
