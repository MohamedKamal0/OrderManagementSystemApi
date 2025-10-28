using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderManagementSystemApplication.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RefundMethod
    {
        Original,   // Refund back to the original payment method
        PayPal,
        Stripe,
        BankTransfer,
        Manual
    }
}
