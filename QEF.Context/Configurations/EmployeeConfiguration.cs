using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QEF.Model;

namespace QEF.Context.Configurations
{
    /// <summary>
    /// Configuration for <see cref="Employee"/>.
    /// </summary>
    public class EmployeeConfiguration(string tableName, string? schema) :
        EntityTypeConfigurationBase(tableName, schema), IEntityTypeConfiguration<Employee>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.HasKey(e => e.Id);

            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.IsActive);
            builder.Property(x => x.BirthDate);
            builder.Property(x => x.DepartmentId);

            builder.
                HasOne(x => x.Department).
                WithMany(x => x.Employees).
                HasForeignKey(x => x.DepartmentId);
        }
    }
}
