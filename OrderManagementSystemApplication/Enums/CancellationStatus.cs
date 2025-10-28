using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderManagementSystemApplication.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CancellationStatus
    {
        Pending = 1,
        Approved = 8,
        Rejected = 9
    }
}
