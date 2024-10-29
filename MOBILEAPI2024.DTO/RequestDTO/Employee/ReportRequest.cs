using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Employee
{
    public class ReportRequest
    {
        public int? limit { get; set; }
        public int? offset { get; set; }
        public string? type { get; set; }
        public string? start_datetime { get; set; }
        public string? end_datetime { get; set; }
        public List<string>? group_id_list { get; set; }
        public string? report_type { get; set; }
        public string? report_filter_type { get; set; }
        public string? language { get; set; }
        public bool? rebuild_time_card { get; set; }
        public List<Dictionary<string, string>>? columns { get; set; }
    }
}
