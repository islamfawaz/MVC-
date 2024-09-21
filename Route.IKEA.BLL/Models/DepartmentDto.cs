using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.BLL.Models
{
    public class DepartmentDto
    {
        public string Code { get; set; } = null!;
        public int Id { get; set; }
        public String Name { get; set; } = null!;
        public String Description { get; set; } = null!;
        public DateOnly CreationDate { get; set; }
    }
}
