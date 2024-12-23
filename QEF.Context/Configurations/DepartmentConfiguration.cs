using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QEF.Model;

namespace QEF.Context.Configurations
{
    /// <summary>
    /// Configuration for <see cref="Department"/>.
    /// </summary>
    public class DepartmentConfiguration(string tableName, string? schema) :
        EntityTypeConfigurationBase(tableName, schema), IEntityTypeConfiguration<Department>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.HasKey(e => e.Id);

            builder.Property(x => x.Number);
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.CompanyId);

            builder.
                HasOne(x => x.Company).
                WithMany(x => x.Departments).
                HasForeignKey(x => x.CompanyId);
        }
    }
}
