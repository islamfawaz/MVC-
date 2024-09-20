using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Route.IKEA.DAL.Entities.Department;

internal class DepartmentConfigurations : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(D => D.Id).UseIdentityColumn(10, 10);
        builder.Property(D => D.Code).HasColumnType("varchar(20)").IsRequired();
        builder.Property(D => D.Name).HasColumnType("varchar(50)").IsRequired();
        builder.Property(D => D.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(D => D.LastModifiedOn).HasComputedColumnSql("GETUTCDATE()");


    }
}
