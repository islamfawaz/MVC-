using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.BLL.Validations.Enums
{
    public class EnumNotDefaultAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false; 

            var enumValue = (Enum)value;
            return !enumValue.Equals(Activator.CreateInstance(enumValue.GetType()));
        }
    }
}
