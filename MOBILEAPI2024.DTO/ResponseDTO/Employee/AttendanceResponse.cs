using System;
using System.Collections.Generic;

namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{
    public class AttendanceResponse
    {
        public EventCollection EventCollection { get; set; }
        public Response Response { get; set; }
    }

    public class EventCollection
    {
        public List<Row>? rows { get; set; } // Changed from Rows to rows
    }

    public class Row
    {
        public string id { get; set; } // Changed from Id to id
        public DateTime server_datetime { get; set; } // Changed from ServerDateTime to server_datetime
        public DateTime datetime { get; set; } // Changed from DateTime to datetime
        public string parameter { get; set; } // Changed from Parameter to parameter
        public string index { get; set; } // Changed from Index to index
        public string user_id_name { get; set; } // Changed from UserIdName to user_id_name
        public UserId user_id { get; set; } // Changed from UserId to user_id
        public UserGroupId user_group_id { get; set; } // Changed from UserGroupId to user_group_id
        public DeviceId device_id { get; set; } // Changed from DeviceId to device_id
        public EventTypeId event_type_id { get; set; } // Changed from EventTypeId to event_type_id
        public string user_update_by_device { get; set; } // Changed from UserUpdateByDevice to user_update_by_device
        public string hint { get; set; } // Changed from Hint to hint
    }

    public class UserId
    {
        public string user_id { get; set; } // Changed from UserIdValue to user_id
        public string name { get; set; } // Changed from Name to name
        public string photo_exists { get; set; } // Changed from PhotoExists to photo_exists
    }

    public class UserGroupId
    {
        public string id { get; set; } // Changed from Id to id
        public string name { get; set; } // Changed from Name to name
    }

    public class DeviceId
    {
        public string id { get; set; } // Changed from Id to id
        public string name { get; set; } // Changed from Name to name
    }

    public class EventTypeId
    {
        public string code { get; set; } // Changed from Code to code
    }

    public class Response
    {
        public string code { get; set; } // Changed from Code to code
        public string link { get; set; } // Changed from Link to link
        public string message { get; set; } // Changed from Message to message
    }
}
