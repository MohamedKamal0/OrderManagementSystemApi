using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystemApplication.Dtos.Address
{
    public class AddressDeleteDto
    {
        [Required(ErrorMessage = "CustomerId is Required")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "AddressId is Required")]
        public int AddressId { get; set; }
    }
}
