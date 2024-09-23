using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Route.IKEA.DAL.Common.Enums;
using Route.IKEA.DAL.Entities.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.DAL.Presistence.Data.Configurations
{
    internal class EmployeeConfigurations : IEntityTypeConfiguration<Employee>  
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Name)
                   .HasColumnType("varchar(50)")
                   .IsRequired();

            builder.Property(e => e.Address)
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Salary)
                .HasColumnType("decimal(8, 2)");

            builder.Property(e => e.Gender)
                .HasConversion(
                    gender => gender.ToString(),
                    gender => (Gender)Enum.Parse(typeof(Gender), gender)
                );

            builder.Property(e => e.EmployeeType)
                .HasConversion(
                    type => type.ToString(),
                    type => (EmployeeType)Enum.Parse(typeof(EmployeeType), type)
                );
        }
    }
}
