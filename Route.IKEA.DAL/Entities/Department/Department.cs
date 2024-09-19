using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.DAL.Entities.Department
{
    public class Department : ModelBase
    {
        public string Code { get; set; } = null!;
        public String Name { get; set; } = null!;
        public String? Description { get; set; }
        public DateOnly CreationDate { get; set; }

    }
}
