using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.BLL.Models
{
    public class DepartmentDetailsDto
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }

        public string Code { get; set; } = null!;
        public String Name { get; set; } = null!;
        public String? Description { get; set; }
        public DateOnly CreationDate { get; set; }

       

    }
}
