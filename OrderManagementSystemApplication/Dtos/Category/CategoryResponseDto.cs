using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystemApplication.Dtos.Category
{
    public class CategoryResponseDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        
    }
}
