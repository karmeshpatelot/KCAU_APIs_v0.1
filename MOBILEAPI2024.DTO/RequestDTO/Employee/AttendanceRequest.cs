using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Employee
{
    public class AttendanceRequest
    {
        public Query Query { get; set; }
    }

    public class Query
    {
        public int limit { get; set; }
        public List<Condition> conditions { get; set; }
        public List<Order> orders { get; set; }
    }

    public class Condition
    {
        public string column { get; set; }
        [JsonProperty("operator")]
        public int Operator { get; set; }
        public List<string> values { get; set; }
    }

    public class Order
    {
        public string column { get; set; }
        public bool descending { get; set; }
    }

}
